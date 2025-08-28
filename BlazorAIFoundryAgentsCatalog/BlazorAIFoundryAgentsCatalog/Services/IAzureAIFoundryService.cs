using BlazorAIFoundryAgentsCatalog.Shared;

namespace BlazorAIFoundryAgentsCatalog.Services
{
    public interface IAzureAIFoundryService
    {
        Task<IEnumerable<AgentDto>> GetAgents();
    }
}
