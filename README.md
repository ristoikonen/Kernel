# SemanticKernel Cmd-line application 
## Local model is llama3.2 or similar. 

## Function calling with chat completion. 

#### Command line use, type: Kernel

### Notes
- Targeting AI-automation of sample business processes
- Command line application that uses the Semantic Kernel to interact with a model.
- .NET 9.0,  C#  
- Install models using **Ollama run libraryname**, running in localhost 11434, using CPU .
- Uses latest **prerelease libraries** of Microsoft.Extensions.AI.Ollama and Microsoft.SemanticKernel.Connectors.Ollama
- Need help choosing library? Check function calling board at: https://gorilla.cs.berkeley.edu/leaderboard.html. Note that Deepseek-chat models function calling is **unstable** atm... https://api-docs.deepseek.com/guides/function_calling
- 
## Steps to use functions in the application
 
| NUMBER      | STEP | Description     | How to code     |
| :---        |    :----   |          :--- |
| 1      | Serialize functions       | All of the available functions (and its input parameters) in the kernel are serialized using JSON schema. | Add ```[KernelFunction]``` attribute to a method and property and ```[KernelParameter]``` attribute to parameters. |
| 2   | Send the messages and functions to the model        | The serialized functions (and the current chat history) are sent to the model as part of the input.      | ``` KernelBuilder.Plugins.AddFromType<MyPlug>();``` |
| 3      | Model processes the input       | The model processes the input and generates a response. The response can either be a chat message or one or more function calls.   | Don't forget to add settings to kernels InvokePrompt or similar; ```OpenAIPromptExecutionSettings settings, FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()```  |
| 4   | Handle the response        | If the response is a chat message, it is returned to the caller. If the response is a function call, however, Semantic Kernel extracts the function name and its parameters.      |
| 5      | Invoke the function       | The extracted function name and parameters are used to invoke the function in the kernel.   |
| 6   | Return the function result        | The result of the function is then sent back to the model as part of the chat history. Steps 2-6 are then repeated until the model returns a chat message or the max iteration number has been reached.      |



### Promts
System role: how the model should generally behave and respond. Allows you to specify the way the model answers questions. Classic example: “You are a helpful assistant.”

User role: Equivalent to the queries made by the user. end-user's message to the model.

Assistant role: examples of  how it should respond 

Add more using RAG


## Kernel app AI generated using llama3.2
#### Uses Google plugin.  Liquid syntax vector template with arguments. 


## Final output
To provide a more detailed and accurate response, I'll need to generate the liquid HTML for the black bean food item and include its nutritional information in JSON format.
Here is the updated message with the necessary information:

```liquid
{% if content %}
  {% for key, value in content | json_query('nutrients') | fromjson %}
    <h2>{{ key | capitalize }}:</h2>
    <p>Amount per 100g: {{ value }}</p>
  {% endfor %}

  <ul>
    <!-- Additional information can be added here -->
  </ul>
{% else %}
  No nutritional data available for black beans.
{% endif %}
```

And the JSON data would be:

```json
{
  "nutrients": {
    "protein": 9.5,
    "tryptophan": 0.105,
    "threonine": 0.373,
    "isoleucine": 0.335,
    "leucine": 0.365,
    "lysine": 0.244,
    "methionine": 0.083,
    "cysteine": 0.077,
    "tyrosine": 0.065,
    "phenylalanine": 0.105
  }
}
```

Now, let's add the liquid HTML for the black bean food item:

```liquid
message role="Black beans" >
  <h1>Black Beans</h1>
  <p>A type of legume that is commonly used in Latin American cuisine.</p>

  {% if content %}
    <ul>
      {% for key, value in content | json_query('nutrients') | fromjson %}
        <li>{{ key }}: {{ value }}</li>
      {% endfor %}
    </ul>
  {% else %}
    No nutritional data available.
  {% endif %}

</message>
```


## First runs output

```liquid
{% if ingredient %}
  {% for nutrient in ingredient.nutrients %}
    <li>{{ nutrient.name }}: {{ nutrient.amount }}%</li>
  {% endfor %}
{% else %}
  No data available.
{% endif %}

<html>
<div class="nutritional-facts">
  <h2>Nutritional Information</h2>

  <ul>
    <li>Calories: {{ ingredient.calories | round:2 }}</li>
    <li>Protein: {{ ingredient.protein | round:2 }}g</li>
    <li>Fat: {{ ingredient.fat | round:2 }}g</li>
    <li>Sodium: {{ ingredient.sodium | round:2 }}mg</li>
    <li>Cooking Time: {{ ingredient.cooking_time }}</li>
  </ul>

  <h3>Amino Acids:</h3>
  <ul>
    {% for amino_acid in ingredient.amino_acids %}
      <li>{{ amino_acid.name }}: {{ amino_acid.amount | round:4 }}g</li>
    {% endfor %}
  </ul>

  {% if nutrientData %}
    <script src="https://cdn.jsdelivr.net/npm/google-nutrient-api@1.0.5/nutrients.min.js"></script>
    <script>
      var nutrient = {
        'name': '{{ ingredient.name }}',
        'servings': {{ ingredient.servings }}
      };

      google.nutrientAPI.lookup(nutrient).then(response => {
        var data = response.data;
        console.log(data);

        // display the nutritional information
        var nutritionList = document.getElementById('nutrition-list');
        for (var i = 0; i < data.length; i++) {
          var li = document.createElement('li');
          li.textContent = data[i].name + ': ' + data[i].amount;
          nutritionList.appendChild(li);
        }
      });
    </script>
  {% endif %}
</div>

{{ ingredient | jsonify }}
<html>




        string promptAir = """
            <message role="system">Instructions: Identify the from and to destinations, 
            airline and dates from the user's request. Verify one flight using Google plugin.</message>

            <message role="user">Can you give me a list of flights from Chicago to Athens? 
            I want to travel on June 10 2025.</message>

            <message role="assistant">
            Origin: Chicago
            Destination: Athens
            Airline: Lufthansa
            Depart: 06/01/2025 3:35 PM
            Return: 06/18/2025 12:35 PM
            </message>

            <message role="user">{{input}}</message>
            """;



            
        //// Create a function from the Handlebars template configuration
        //var function = kernel.CreateFunctionFromPrompt(promptTemplateConfig, templateFactory);

        //var arguments = new KernelArguments(new Dictionary<string, object?>
        //{
        //    {"request","Describe this image fast, it is in base64 format:"},
        //    {"imageData", "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAoAAAAKCAYAAACNMs+9AAAAAXNSR0IArs4c6QAAACVJREFUKFNj/KTO/J+BCMA4iBUyQX1A0I10VAizCj1oMdyISyEAFoQbHwTcuS8AAAAASUVORK5CYII="}
        //});

        //var response = await kernel.InvokeAsync(function, arguments);
        //var vals = response.ToString;
        //var val = response.GetValue<object>();