using Amazon.Lambda.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StopMonkey;
using System.Threading.Tasks;

namespace StopMonkeyUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1Async()
        {
            Ec2 ec2 = new Ec2();
            Asg asg = new Asg();
            StopMonkeyBaseLambda stopMonkeyBase = new StopMonkeyBaseLambda();
            await stopMonkeyBase.HandlerAsync(null, null);

        }
    }
}
