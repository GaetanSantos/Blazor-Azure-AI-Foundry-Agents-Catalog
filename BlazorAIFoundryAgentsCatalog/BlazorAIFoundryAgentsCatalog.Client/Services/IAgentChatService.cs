using BlazorAIFoundryAgentsCatalog.Client.DTOs;

namespace BlazorAIFoundryAgentsCatalog.Client.Services
{
    public interface IAgentChatService
    {
        Task<List<ChatMessageDto>> GetMessagesAsync(string agentId);

        Task<ChatMessageDto?> SendMessageAsync(string agentId, string content);

        Task<ChatThreadDto?> SendMessageAndGetHistoryAsync(string agentId, string userPrompt, string? threadId);
    }
}
