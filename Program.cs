using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.Google;
using Microsoft.Extensions.Configuration;

// --- Configuration & Initialization ---

#region Configuration Setup
// 1. Build the Configuration object
IConfiguration config = new ConfigurationBuilder()
    // Looks for appsettings.json (good practice for non-secret settings)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    // IMPORTANT: Adds secrets stored using 'dotnet user-secrets'
    .AddUserSecrets<Program>()
    // Adds environment variables as a fallback
    .AddEnvironmentVariables()
    .Build();

#endregion

// 2. Define Model and API Key Retrieval
const string modelId = "gemini-2.5-flash";

// Attempt to read the key from configuration. 
// We are looking for a key named "Gemini:ApiKey"
string apiKey = config["Gemini:ApiKey"] ?? string.Empty;

if (string.IsNullOrEmpty(apiKey))
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine("--- ERROR: The 'Gemini:ApiKey' secret was not found. ---");
    Console.WriteLine("ACTION REQUIRED: Please set the key using the following command:");
    Console.WriteLine("   dotnet user-secrets set \"Gemini:ApiKey\" \"YOUR_ACTUAL_API_KEY\"");
    Console.ResetColor();
    return;
}

// 3. Initialize the Semantic Kernel with the Gemini connector
Console.WriteLine($"Initializing Semantic Kernel with Google Gemini ({modelId})...");

var builder = Kernel.CreateBuilder();

// Use the retrieved apiKey
builder.AddGoogleAIGeminiChatCompletion(
    modelId: modelId,
    apiKey: apiKey
);

var kernel = builder.Build();

Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("Gemini Console App Ready. Type 'exit' to quit.\n");
Console.ResetColor();

// --- Main Interaction Loop ---

while (true)
{
    Console.Write("User > ");
    string? userPrompt = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(userPrompt) || userPrompt.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    try
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Gemini > ");
        Console.ResetColor();

        // Call the service via the Kernel
        var result = await kernel.InvokePromptAsync(userPrompt);

        // Output the result content
        Console.WriteLine(result.GetValue<string>());
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"\nError during API call: {ex.Message}");
        Console.WriteLine("Please ensure your 'Gemini:ApiKey' is correct and valid.");
        Console.ResetColor();
    }
}

Console.WriteLine("\nExiting application.");
