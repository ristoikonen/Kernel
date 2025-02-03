// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Xunit;

namespace FxKernel;

#pragma warning disable SKEXP0070 // Chat completion connector is currently experimental.
#pragma warning disable SKEXP0001 // AsChatCompletionService

public sealed class BasicFxKernel //(ITestOutputHelper output) : BaseTest(output)
{
    [Fact]
    public async Task CreateKernelAsync()
    {
        Uri modelEndpoint = new("http://localhost:11434");
        //string modelName = "deepseek-r1:1.5b";
        string modelName = "llama3.2";

        //.AddOpenAIChatCompletion(
        //    modelId: TestConfiguration.OpenAI.ChatModelId,
        //    apiKey: TestConfiguration.OpenAI.ApiKey)

        // Create a kernel with OpenAI chat completion
        //Kernel kernel = Kernel.CreateBuilder()
        //    .AddOllamaChatCompletion(modelName, modelEndpoint)
        //    .Build();


        IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOllamaChatCompletion(modelName, modelEndpoint);

        kernelBuilder.Plugins.AddFromType<TimeInformation>();
        Kernel kernel = kernelBuilder.Build();

        OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

        // Example 2. Invoke the kernel with a templated prompt that invokes a plugin and display the result
        //Console.WriteLine(await kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}. How many days until Christmas?"));
        //Console.WriteLine();


        //OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
        //Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas? Explain your thinking.", new(settings)));

        // Example 4. Invoke the kernel with a prompt and allow the AI to automatically invoke functions that use enumerations
        Console.WriteLine(await kernel.InvokePromptAsync("Create a handy lime colored widget for me.", new(settings)));
        Console.WriteLine(await kernel.InvokePromptAsync("Create a beautiful scarlet colored widget for me.", new(settings)));
        Console.WriteLine(await kernel.InvokePromptAsync("Create an attractive maroon and navy colored widget for me.", new(settings)));


        //Invoke the kernel with a prompt and display the result
        //Console.WriteLine(await kernel.InvokePromptAsync("What color is the sky?"));
        //Console.WriteLine();
        // Console.WriteLine(string.Empty);
    }

}

/// <summary>
/// A plugin that returns the current time.
/// </summary>
public class TimeInformation
{
    [KernelFunction]
    [Description("Retrieves the current time in UTC.")]
    public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");
}
