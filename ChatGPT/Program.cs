using Newtonsoft.Json;
using System.Text;

DotNetEnv.Env.Load();
var chatGptKey = Environment.GetEnvironmentVariable("CHATGPTKEY");

do
{
    Console.Write("Digite uma pergunta: ");
    var question = Console.ReadLine();

    if (question?.Length <= 0)
    {
        Console.WriteLine("Você deve fazer uma pergunta!");
    }
    else
    {
        var client = new HttpClient();
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {chatGptKey}");

        var json = JsonConvert.SerializeObject(new { 
            model = "text-davinci-003", 
            prompt = question, 
            max_tokens = 1024, 
            temperature = 0.5 });

        var httpResponse = await client.PostAsync("https://api.openai.com/v1/completions", new StringContent(json, Encoding.Default, "application/json"));

        var data = await httpResponse.Content.ReadAsStringAsync();

        var response = JsonConvert.DeserializeObject<dynamic>(data);

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(response?.choices[0].text);
        Console.WriteLine();
        Console.ResetColor();
    }
} 
while (true);