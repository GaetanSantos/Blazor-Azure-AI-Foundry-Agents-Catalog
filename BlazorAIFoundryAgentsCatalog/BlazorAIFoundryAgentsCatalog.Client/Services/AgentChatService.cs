using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using System.Net.Http.Json;

namespace BlazorAIFoundryAgentsCatalog.Client.Services
{
    public class AgentChatService : IAgentChatService
    {
        private readonly HttpClient _httpClient;

        public AgentChatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ChatMessageDto>> GetMessagesAsync(string agentId)
        {
            return await _httpClient.GetFromJsonAsync<List<ChatMessageDto>>($"api/agents/{agentId}/chat/history") ?? new();
        }

        public async Task<ChatMessageDto?> SendMessageAsync(string agentId, string content)
        {
            var message = new ChatMessageDto(agentId, "user", content, DateTime.UtcNow);
            var resp = await _httpClient.PostAsJsonAsync($"api/agents/{agentId}/chat/send", message);
            return await resp.Content.ReadFromJsonAsync<ChatMessageDto>();
        }

        public async Task<ChatThreadDto?> SendMessageAndGetHistoryAsync(string agentId, string userPrompt, string? threadId)
        {
            var request = new SendMessageRequest(agentId, userPrompt, threadId);
            var resp = await _httpClient.PostAsJsonAsync($"api/agents/{agentId}/chat/send", request);
            return await resp.Content.ReadFromJsonAsync<ChatThreadDto>();
        }
    }
}
