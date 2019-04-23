using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace StopMonkey
{
    public abstract class StopMonkeyBase<TInput> : IStopMonkeyBase<TInput>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        protected IEc2 Ec2;
        protected IAsg Asg;
        protected StopMonkeyBase()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            IServiceProvider container = serviceCollection.BuildServiceProvider();
            _serviceScopeFactory = container.GetRequiredService<IServiceScopeFactory>();

        }

        protected StopMonkeyBase(IEc2 ec2, IAsg asg)
        {
            Ec2 = ec2;
            Asg = asg;
        }

        protected virtual void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(GetType());
            serviceCollection.TryAddScoped<IEc2, Ec2>();
            serviceCollection.TryAddScoped<IAsg, Asg>();
        }

        public async Task HandlerAsync(TInput input, ILambdaContext context)
        {
            using (IServiceScope scope = _serviceScopeFactory.CreateScope())
            {
                StopMonkeyBase<TInput> self = (StopMonkeyBase<TInput>)scope.ServiceProvider.GetService(GetType());
                await self.ExecuteAsync(input, context);
            }
            
        }

        public abstract Task ExecuteAsync(TInput input, ILambdaContext context);
    }

}
