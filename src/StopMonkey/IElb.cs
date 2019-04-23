using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StopMonkey
{
    public interface IElb
    {
        Task<bool> RemoveandAddbackInstance();
    }
}
