using Amazon;
using Amazon.AutoScaling;
using Amazon.AutoScaling.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StopMonkey
{
    public class Asg : IAsg
    {

        public async Task<bool> DisableSuspendProcessAsync(string asgName)
        {
            
            using (IAmazonAutoScaling asgClient =  CreateAsgAWSClient())
            {
                try
                {
                    var r = await asgClient.SuspendProcessesAsync(new SuspendProcessesRequest
                    {
                        AutoScalingGroupName = asgName,
                        ScalingProcesses = new List<string> {
                        "Terminate"
                    }
                    });

                    if (r.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }


                return false;
               
            };
            
        }

        public async Task<bool> ResumeSuspendProcessAsync(string asgName)
        {

            using (IAmazonAutoScaling asgClient = CreateAsgAWSClient())
            {
                try
                {
                    var r = await asgClient.ResumeProcessesAsync(new ResumeProcessesRequest
                    {
                        AutoScalingGroupName = asgName,
                        ScalingProcesses = new List<string> {
                        "Terminate"
                    }
                    });

                    if (r.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }


                return false;

            };

        }

        public async Task<bool> SetInstanceHealthyAsync(string instanceId)
        {
            using (IAmazonAutoScaling asgClient = CreateAsgAWSClient())
            {
                try
                {
                    SetInstanceHealthResponse healthResponse = await asgClient.SetInstanceHealthAsync(
                        new SetInstanceHealthRequest
                        {
                            HealthStatus = "Healthy",
                            InstanceId = instanceId
                        }
                    );
                    if (healthResponse.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {

                    Console.WriteLine(e);
                }
            }

            return false;
        }

        private AmazonAutoScalingClient CreateAsgAWSClient()
        {
            RegionEndpoint testRegionalEndpoint = RegionEndpoint.APSoutheast2;
            AmazonAutoScalingConfig asg = new AmazonAutoScalingConfig
            {
                RegionEndpoint = testRegionalEndpoint
            };
            return new AmazonAutoScalingClient(asg);
        }
    }
}
