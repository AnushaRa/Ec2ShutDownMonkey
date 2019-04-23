using Amazon.AutoScaling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StopMonkey
{
    public interface IAsg
    {
        Task<bool> DisableSuspendProcessAsync(string asgName);
        Task<bool> ResumeSuspendProcessAsync(string asgName);
        Task<bool> SetInstanceHealthyAsync(string instanceId);
    }
}
