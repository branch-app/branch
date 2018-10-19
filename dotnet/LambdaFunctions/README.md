# Lambda functions

Branch uses several lambda functions to execute tasks that are scheduled. When changes have been made you can deploy them to AWS by running the following command:

## Getting started

To deploy lambda functions you will need to install the AWS lambda tools for dotnet. You can do that with the following command:

```bash
$ dotnet tool install -g Amazon.Lambda.Tools
```

## Deployment

When you have made the required changes, ensure the config is up to date in AWS, and then run the following command:

```bash
$ dotnet lambda deploy-function
```

All the settings are set in the `aws-lambda-tools-defaults.json` file. Configuration is set via the `CONFIG` environment variable as a JSON formatted string.
