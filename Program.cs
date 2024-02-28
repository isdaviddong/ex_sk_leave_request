using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Microsoft.SemanticKernel.ChatCompletion;

internal class Program
{
    private static async global::System.Threading.Tasks.Task Main(string[] args)
    {
        //Azure OpenAI 
        var DeployName = "👉模型佈署名稱👈";
        var Endpoint = "https://👉API端點👈.openai.azure.com/";
        var ApiKey = "👉ApiKey👈";

        // Create a new kernel builder
        var builder = Kernel.CreateBuilder()
                    .AddAzureOpenAIChatCompletion(DeployName, Endpoint, ApiKey);
        builder.Plugins.AddFromType<LeaveRequestPlugin>(); // Add the LightPlugin to the kernel
        Kernel kernel = builder.Build();

        // Create chat history 物件，並且加入
        var history = new ChatHistory();
        history.AddSystemMessage(
            @"你是企業的請假助理，可以協助員工進行請假，或是查詢請假天數等功能。若員工需要請假，
                 你需要蒐集請假起始日期、天數、請假事由、代理人、請假者姓名等資訊。最後呼叫 LeaveRequest Method。
                 若員工需要查詢請假天數，你需要蒐集請假者姓名，最後呼叫 GetLeaveRecordAmount Method。
                ");

        // Get chat completion service
        var chatCompletionService = kernel.GetRequiredService<IChatCompletionService>();

        // 開始對談
        Console.Write("User > ");
        string? userInput;
        while (!string.IsNullOrEmpty(userInput = Console.ReadLine()))
        {
            // Add user input
            history.AddUserMessage(userInput);

            // Enable auto function calling
            OpenAIPromptExecutionSettings openAIPromptExecutionSettings = new()
            {
                ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions
            };

            // Get the response from the AI
            var result = await chatCompletionService.GetChatMessageContentAsync(
                history,
                executionSettings: openAIPromptExecutionSettings,
                kernel: kernel);

            // Print the results
            Console.WriteLine("Assistant > " + result);

            // Add the message from the agent to the chat history
            history.AddMessage(result.Role, result.Content ?? string.Empty);

            // Get user input again
            Console.Write("User > ");
        }
    }
}


// 
public class LeaveRequestPlugin
{
    [KernelFunction]
    [Description("取得今天日期")]
    public DateTime GetCurrentDate()
    {
        return DateTime.UtcNow.AddHours(8);
    }

    [KernelFunction]
    [Description("取得請假天數")]
    public int GetLeaveRecordAmount(string employeeName)
    {
        if (employeeName.ToLower() == "david")
            return 3;
        else
            return 5;
    }

    [KernelFunction]
    [Description("進行請假")]
    public bool LeaveRequest(DateTime 請假起始日期, string 天數, string 請假事由, string 代理人, string 請假者姓名)
    {

        // Print the state to the console
        Console.WriteLine($"建立假單:  {請假者姓名} 請假 {天數} 從 {請假起始日期} 開始，事由為 {請假事由}，代理人 {代理人}");

        return true;
    }
}