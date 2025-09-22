using BlazorAIFoundryAgentsCatalog.Client.DTOs;

namespace BlazorAIFoundryAgentsCatalog.Services
{
    public interface IAzureAIFoundryAgentService
    {
        Task<List<ChatMessageDto>> GetChatHistoryAsync(string agentId);

        Task<(string? ThreadId, List<ChatMessageDto>? Messages)> SendMessageAndGetHistoryAsync(string agentId, string userPrompt, string? threadId);

    }
}
