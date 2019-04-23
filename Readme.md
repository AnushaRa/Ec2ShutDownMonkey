# AWS Lambda to stop/start ec2 instances based on schedule during off hours.

1. You need to tag your ec2 instances
2. Add a schedule tag. If left, then the lambda assigns a default schedule.
3. If the instance is part of an autoscaling group, then the lambda suspends Terminate process while stopping
4. While starting an instance, the lambda removes suspend process - Terminate and sets the instance health as healthy. Otherwise, autoscaling thinks the instance as unhealthy and terminates it.

## TODO
1. Add ELB support. If the instance is part of an elb, we need to remove from the elb and add it back when starting the instances. This is because the ELB wont automatically run the healthcheck if the instance is stopped and becomes unhealthy

## Here are some steps to follow from Visual Studio:

To deploy your function to AWS Lambda, right click the project in Solution Explorer and select *Publish to AWS Lambda*.

To view your deployed function open its Function View window by double-clicking the function name shown beneath the AWS Lambda node in the AWS Explorer tree.

To perform testing against your deployed function use the Test Invoke tab in the opened Function View window.

To configure event sources for your deployed function, for example to have your function invoked when an object is created in an Amazon S3 bucket, use the Event Sources tab in the opened Function View window.

To update the runtime configuration of your deployed function use the Configuration tab in the opened Function View window.

To view execution logs of invocations of your function use the Logs tab in the opened Function View window.

## Here are some steps to follow to get started from the command line:

Once you have edited your function you can use the following command lines to build, test and deploy your function to AWS Lambda from the command line (these examples assume the project name is *EmptyFunction*):

Restore dependencies
```
    cd "StopMonkey"
    dotnet restore
```

Execute unit tests
```
    cd "StopMonkey/test/StopMonkey.Tests"
    dotnet test
```

Deploy function to AWS Lambda
```
    cd "StopMonkey/src/StopMonkey"
    dotnet lambda deploy-function
```
