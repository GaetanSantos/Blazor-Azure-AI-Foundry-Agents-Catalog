using BlazorAIFoundryAgentsCatalog.Client.DTOs;

namespace BlazorAIFoundryAgentsCatalog.Client.Services
{
    public interface IAgentCatalogService
    {
        Task<List<AgentDto>> GetAgentsAsync();
    }
}
