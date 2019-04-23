using Amazon.EC2.Model;
using System.Collections;
using System.Collections.Generic;

namespace StopMonkey
{
    public class InstanceDetails 
    {
        public List<Instance> ec2Instances { get; set; }
        
        public enum InstanceState
        {
            running=16,
            stopped=80
        }

        public List<Instance> stopInstances { get; set; }
        public List<Instance> startInstances { get; set; }

    }
}