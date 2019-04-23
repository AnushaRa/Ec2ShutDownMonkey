using Amazon.Lambda.Core;
using System.Threading.Tasks;

namespace StopMonkey
{
    public interface IStopMonkeyBase<TInput>
    {
        Task HandlerAsync(TInput input, ILambdaContext context);
        Task ExecuteAsync(TInput input, ILambdaContext context);
    }
}