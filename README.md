# SemanticKernel Cmd-line application 
## Local model is llama3.2 or similar. 

## Function calling with chat completion. 

#### Install models using **Ollama run libraryname**
#### Command line use, type: Kernel

### Notes
- Targeting AI-automation of sample business processes
- Command line application that uses the Semantic Kernel to interact with a model.
- .NET 9.0,  C#  
- Models installed using **Ollama run**, running in localhost PORT_NBR_HERE, using CPU .
- Uses latest **prerelease libraries** of Microsoft.Extensions.AI.Ollama and Microsoft.SemanticKernel.Connectors.Ollama

## Steps to use functions in the application
 
| NUMBER      | STEP | Description     | How to code     |
| :---        |    :----   |          :--- |
| 1      | Serialize functions       | All of the available functions (and its input parameters) in the kernel are serialized using JSON schema. | Add ```[KernelFunction]``` attribute to a method and property and ```[KernelParameter]``` attribute to parameters. |
| 2   | Send the messages and functions to the model        | The serialized functions (and the current chat history) are sent to the model as part of the input.      | ``` KernelBuilder.Plugins.AddFromType<MyPlug>();``` |
| 3      | Model processes the input       | The model processes the input and generates a response. The response can either be a chat message or one or more function calls.   | Don't forget to add settings to kernels InvokePrompt or similar; ```OpenAIPromptExecutionSettings settings, FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()```  |
| 4   | Handle the response        | If the response is a chat message, it is returned to the caller. If the response is a function call, however, Semantic Kernel extracts the function name and its parameters.      |
| 5      | Invoke the function       | The extracted function name and parameters are used to invoke the function in the kernel.   |
| 6   | Return the function result        | The result of the function is then sent back to the model as part of the chat history. Steps 2-6 are then repeated until the model returns a chat message or the max iteration number has been reached.      |







Takes you to visual multi-choice menu:
1. AI Chat with Phi-3.5
2. AI image analysis using Phi-3.5








#	Step	Description
1	Serialize functions	All of the available functions (and its input parameters) in the kernel are serialized using JSON schema.
2	Send the messages and functions to the model	The serialized functions (and the current chat history) are sent to the model as part of the input.
3	Model processes the input	The model processes the input and generates a response. The response can either be a chat message or one or more function calls.
4	Handle the response	If the response is a chat message, it is returned to the caller. If the response is a function call, however, Semantic Kernel extracts the function name and its parameters.
5	Invoke the function	The extracted function name and parameters are used to invoke the function in the kernel.
6	Return the function result	The result of the function is then sent back to the model as part of the chat history. Steps 2-6 are then repeated until the model returns a chat message or the max iteration number has been reached.

