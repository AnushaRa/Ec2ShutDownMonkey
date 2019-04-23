using Amazon.EC2.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StopMonkey
{
    public interface IEc2
    {
        Task<List<Instance>> DescribeInstanceAsync();
        Task<bool> StopInstanceAsync(string stopInstanceId);
        Task<bool> StartInstanceAsync(string startInstanceId);
    }
}