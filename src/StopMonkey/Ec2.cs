using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;
using System.Net;
using System.Linq;

namespace StopMonkey
{
    public class Ec2 : IEc2
    {

        private int instanceCount;

        public async Task<List<Instance>> DescribeInstanceAsync()
        {

            using (IAmazonEC2 client = CreateAwsClient())
            {
                DescribeInstancesResponse resp = await client.DescribeInstancesAsync(
                    new DescribeInstancesRequest()
                    {
                        Filters = new List<Filter>()
                        {
                            new Filter
                            {
                                Name = "tag:stopmonkey",
                                Values = new List<string>
                                {
                                    "True"
                                }
                            }
                        }
                    }
                 );

                if (resp.HttpStatusCode == HttpStatusCode.OK)
                {
                    instanceCount = resp.Reservations.Sum(sum => sum.Instances.Count);
                    Console.WriteLine($"Processing {instanceCount} of instances");
                }
                List<Instance> instances = resp.Reservations.SelectMany(s => s.Instances).ToList();


                if (instanceCount == 0)
                {
                    return null;
                }

                return instances;

            };
        }

        public async Task<bool> StartInstanceAsync(string startInstanceId)
        {
            var startInstanceRequest = new StartInstancesRequest()
            {
                InstanceIds = new List<string> {
                        startInstanceId
                    }
            };
            using (IAmazonEC2 client = CreateAwsClient())
            {
                try
                {
                    StartInstancesResponse resp = await client.StartInstancesAsync(startInstanceRequest);

                    if (resp.HttpStatusCode == HttpStatusCode.OK)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
                
            }
            return false;
        }

        public async Task<bool> StopInstanceAsync(string stopInstanceId)
        {
            using (IAmazonEC2 client = CreateAwsClient())
            {

                StopInstancesResponse stopInstanceResp = await client.StopInstancesAsync(
                  new StopInstancesRequest()
                  {
                    InstanceIds = new List<string>
                    {
                        stopInstanceId
                    }
                  }
               );
               if (stopInstanceResp.HttpStatusCode == HttpStatusCode.OK)
               {
                    return true;
               }
               Console.WriteLine("stopping instance");
                
            }
            return false;
        }

        private AmazonEC2Client CreateAwsClient()
        {
            RegionEndpoint testRegionalEndpoint = RegionEndpoint.APSoutheast2;
            AmazonEC2Config ec2ClientConfig = new AmazonEC2Config
            {
                RegionEndpoint = testRegionalEndpoint
            };
            return new AmazonEC2Client(ec2ClientConfig);
        }
    }

}
