# Semantic Kernel Gemini Connector Demo

This is a simple .NET 9 console application demonstrating how to use the Semantic Kernel with the Google Gemini connector. The application creates a chat-like interface in the console where a user can enter prompts and receive responses from the Gemini model.

## Features

- **Semantic Kernel Integration:** Shows the basic setup and usage of the Microsoft Semantic Kernel.
- **Gemini Connector:** Utilizes the `Microsoft.SemanticKernel.Connectors.Google` package to connect to the Gemini family of models.
- **Configuration Management:** Uses `Microsoft.Extensions.Configuration` to manage the API key securely via user secrets.
- **Interactive Console:** Provides a simple, interactive command-line interface for sending prompts to the Gemini model.
- **Error Handling:** Includes basic error handling for missing API keys and issues during API calls.

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- A Google Gemini API Key. You can obtain one from [Google AI Studio](https://aistudio.google.com/).

## Setup & Configuration

1.  **Clone the repository:**
    ```bash
    git clone <repository-url>
    cd SKDemoConsoleApp
    ```

2.  **Initialize User Secrets:**
    This project uses the .NET User Secrets Manager to store the Gemini API key securely. Initialize it with the following command:
    ```bash
    dotnet user-secrets init
    ```

3.  **Set the API Key:**
    Replace `"YOUR_ACTUAL_API_KEY"` with your actual Gemini API key and run the following command:
    ```bash
    dotnet user-secrets set "Gemini:ApiKey" "YOUR_ACTUAL_API_KEY"
    ```
    The application is configured to read this secret at runtime.

## How to Run

1.  **Restore Dependencies:**
    ```bash
    dotnet restore
    ```

2.  **Run the application:**
    ```bash
    dotnet run
    ```

Once the application starts, you will see a "User >" prompt. Type your message and press Enter. The application will send the prompt to the Gemini model and display the response.

To exit the application, type `exit` and press Enter.

## Code Overview

### Project File (`SKDemoConsoleApp.csproj`)

The project file includes the necessary NuGet packages:

-   `Microsoft.SemanticKernel`: The core Semantic Kernel library.
-   `Microsoft.SemanticKernel.Connectors.Google`: The specific connector for Google's AI services, including Gemini.
-   `Microsoft.Extensions.Configuration.*`: A set of packages for handling configuration from sources like JSON files, environment variables, and user secrets.

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Microsoft.Extensions.Configuration.EnvironmentVariables" Version="9.0.10" />
    <PackageReference Include="Microsoft.Extensions.Configuration.Json" Version="9.0.10" />
    <PackageReference Include="Microsoft.Extensions.Configuration.UserSecrets" Version="9.0.10" />
    <PackageReference Include="Microsoft.SemanticKernel" Version="1.66.0" />
    <PackageReference Include="Microsoft.SemanticKernel.Connectors.Google" Version="1.66.0-alpha" />
  </ItemGroup>

</Project>
```

### Main Application (`Program.cs`)

The main logic is in `Program.cs`:

1.  **Configuration Setup:** It builds a configuration object that reads from `appsettings.json`, user secrets, and environment variables.
2.  **API Key Retrieval:** It retrieves the `Gemini:ApiKey` from the configuration. If the key is not found, it displays an error and instructions on how to set it.
3.  **Kernel Initialization:** It creates a `Kernel` instance and configures it with the Google AI Gemini Chat Completion service using the `gemini-1.5-flash` model and the retrieved API key.
4.  **Main Loop:** It enters an infinite loop, prompting the user for input.
5.  **Invoke Prompt:** Inside the loop, it calls `kernel.InvokePromptAsync(userPrompt)` to send the user's input to the Gemini model.
6.  **Display Result:** It prints the model's response to the console.
7.  **Exit:** The loop breaks if the user types `exit`.
