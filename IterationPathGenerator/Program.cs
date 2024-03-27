// Azure DevOps organization URL
using System.Net.Http.Headers;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

string organizationUrl = "https://dev.azure.com";

//ask user for PAT and save it in a variable
Console.WriteLine("Enter your personal access token: ");
string personalAccessToken = Console.ReadLine();

string org = args[0];
string projectName = args[1];
int iterationStart = 0;

if (args.Length > 2)
{
    iterationStart = int.Parse(args[2]);
}

// Iteration count
int iterationCount = 10000;

// Create HttpClient
using (var client = new HttpClient())
{
    // Set up HttpClient
    client.BaseAddress = new Uri(organizationUrl);
    client.DefaultRequestHeaders.Accept.Clear();
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($":{personalAccessToken}")));

    // Iterate to create iterations
    for (int i = iterationStart; i < iterationCount; i++)
    {
        try
        {
            // Example: Create an iteration
            var iterationName = i.ToString("D5");

            var iteration = new
            {
                name = iterationName
            };

            var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(iteration), Encoding.UTF8, "application/json");

            var response = await client.PostAsync($"/{org}/{projectName}/_apis/wit/classificationnodes/iterations?api-version=7.0", content);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Iteration {iterationName} created successfully.");
            }
            else
            {
                Console.WriteLine($"Failed to create Iteration {iterationName}. StatusCode: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}

Console.WriteLine("All iterations created successfully.");