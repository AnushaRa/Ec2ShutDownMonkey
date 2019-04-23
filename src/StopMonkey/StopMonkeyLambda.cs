using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using System.Collections;
using System.Linq;
using System.Collections.Concurrent;

namespace StopMonkey
{
    public class StopMonkeyBaseLambda : StopMonkeyBase<Ec2>
    {
        public StopMonkeyBaseLambda()
        {

        }

        public StopMonkeyBaseLambda(IEc2 ec2, IAsg asg)
            : base(ec2, asg)
        {

        }

        public override async Task ExecuteAsync(Ec2 input, ILambdaContext context)
        {

            List<Instance> instanceDetails = await Ec2.DescribeInstanceAsync();
            ConcurrentBag<Exception> exceptions = new ConcurrentBag<Exception>();
            Schedule getSchedule = new Schedule();


            Parallel.ForEach(instanceDetails, async instancedetail =>
            {
                try
                {
                    Action trigger = getSchedule.processSchedule(instancedetail.Tags.SingleOrDefault(t => t.Key == "schedule")?.Value);
                    var isasgKey = instancedetail.Tags.SingleOrDefault(asg => asg.Key == "aws:autoscaling:groupName");
                    if (trigger != Action.none)
                    { 
                        if (trigger == Action.start && (int)InstanceDetails.InstanceState.stopped == instancedetail.State.Code)
                        {
                            StartInstance(instancedetail.InstanceId, isasgKey?.Value).Wait();
                        } 
                        else if (trigger == Action.stop && (int)InstanceDetails.InstanceState.running == instancedetail.State.Code)
                        {
                            await StopInstance(instancedetail.InstanceId, isasgKey?.Value);
                        }
                                
                             
                    }
                }
                catch (Exception e)
                {
                    exceptions.Add(e);
                }
            });
        }

        private async Task StopInstance(string instanceId, string isasgKey)
        {
            if (!string.IsNullOrEmpty(isasgKey))
            {
                bool terminateSuspended = Asg.DisableSuspendProcessAsync(isasgKey).Result;
                if (terminateSuspended)
                {
                    Console.WriteLine($"{instanceId} - Terminate Suspended");
                }
            }
            bool instanceStopped = await Ec2.StopInstanceAsync(instanceId);
            if (instanceStopped)
            {
                Console.WriteLine($"Stopping - {instanceId}");
            }
            

        }

        private async Task StartInstance(string instanceId, string isasgKey)
        {
            bool suspendResumed = false;
            bool instanceStarted = await Ec2.StartInstanceAsync(instanceId);
            if (instanceStarted && !string.IsNullOrEmpty(isasgKey))
            {
                bool setHealthy = Asg.SetInstanceHealthyAsync(instanceId).Result;
                if (!setHealthy)
                {
                    Console.WriteLine($"Unable to set instance as healthy - {instanceId}. Leaving Terminate suspended");
                    
                }
                else
                {
                    suspendResumed = Asg.ResumeSuspendProcessAsync(isasgKey).Result;
                }
                if (suspendResumed)
                {
                    Console.WriteLine($"{instanceId} - Terminate Resumed and Set Healthy");
                }
            }
            Console.WriteLine($"Started - {instanceId}");
        }

    }
}
