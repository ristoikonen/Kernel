// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.PromptTemplates.Liquid;
using Xunit;

namespace FxKernel;

#pragma warning disable SKEXP0070 // Chat completion connector is currently experimental.
#pragma warning disable SKEXP0001 // AsChatCompletionService

public sealed class BasicFxKernel
{
    private Uri ModelEndpoint { get; set; }
    private string ModelName { get; set; }

    public BasicFxKernel(Uri modelEndpoint, string modelName)
    {
        this.ModelEndpoint = modelEndpoint;
        this.ModelName = modelName;
    }

    [Fact]
    public async Task CreateKernelAsync()
    {
        IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOllamaChatCompletion(this.ModelName, this.ModelEndpoint);

        kernelBuilder.Plugins.AddFromType<TimeInformation>();
        Kernel kernel = kernelBuilder.Build();

        OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };

        // Prompt template using Liquid syntax
        string template = """
            <message role="system">
            You are an AI agent for the Food Chemistry website. As the agent, you generate liquid HTML for food items,  
                in a scientific manner using provided Website liquid HTML template and by generating nutritional information data of food item in JSON format and showing it. 
            Instructions: List the amounts of essential amino acids in messages food item.
             Verify amounts using Google plugin.</message>
            
            <message role="user">Can you tell me what amino acids do black beans have? 
            </message>
            
            <message role="assistant">
            Black beans, amino acids per 100g: Tryptophan 0.105g, Threonine 0.373 g.
            </message>
            

            # Website Context
            <message role="{{fooditem.name}}"
             <ul>
              {% for aminoacid in aminoacids %}
                <li>
                  <h2>{{ aminoacid.name }}</h2>
                  {{ aminoacid.amount | prettyprint | paragraph }}
                    </message>
                </li>
              {% endfor %}
            </ul>
            """;

        var arguments = new KernelArguments()
        {
            { "fooditem", new
                {
                    name = "Black beans",
                    aminoacids = "",
                }
            },
            { "history", new[]
                {
                    new { role = "user", content = "Can you tell me what amino acids do black beans have?" },
                }
            },
        };

        // Create the prompt template using liquid format
        var templateFactory = new LiquidPromptTemplateFactory();
        var promptTemplateConfig = new PromptTemplateConfig()
        {
            Template = template,
            TemplateFormat = "liquid",
            Name = "FoodChemistryChatPrompt",
        };

        // Render the prompt
        var promptTemplate = templateFactory.Create(promptTemplateConfig);
        var renderedPrompt = await promptTemplate.RenderAsync(kernel, arguments);
        Console.WriteLine($"Rendered Prompt:\n{renderedPrompt}\n");


        var function = kernel.CreateFunctionFromPrompt(promptTemplateConfig, templateFactory);
        var response = await kernel.InvokeAsync(function, arguments);
        Console.WriteLine(response);


        // Example 2. Invoke the kernel with a templated prompt that invokes a plugin and display the result
        //Console.WriteLine(await kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}. How many days until Christmas?"));
        //Console.WriteLine();


        //OpenAIPromptExecutionSettings settings = new() { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
        //Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas? Explain your thinking.", new(settings)));

        // Example 4. Invoke the kernel with a prompt and allow the AI to automatically invoke functions that use enumerations
        ////Console.WriteLine(await kernel.InvokePromptAsync("Create a handy lime colored widget for me.", new(settings)));
        // Console.WriteLine(await kernel.InvokePromptAsync("Create a beautiful scarlet colored widget for me.", new(settings)));



        //Console.WriteLine(await kernel.InvokePromptAsync("Create an attractive maroon and navy colored widget for me.", new(settings)));


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
