using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using System.Net.Http.Json;

namespace BlazorAIFoundryAgentsCatalog.Client.Services
{
    public class AgentCatalogService : IAgentCatalogService
    {
        private readonly HttpClient _httpClient;

        public AgentCatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<AgentDto>> GetAgentsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<AgentDto>>("api/agents") ?? new();
        }

        public async Task<AgentDto> GetAgentAsync(string agentId)
        {
            return await _httpClient.GetFromJsonAsync<AgentDto>($"api/agents/{agentId}/") ?? new AgentDto("Unknown", "Unknown", string.Empty);
        }
    }
}
