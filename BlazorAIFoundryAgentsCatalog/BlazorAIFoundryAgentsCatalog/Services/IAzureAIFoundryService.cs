
using BlazorAIFoundryAgentsCatalog.Client.DTOs;

namespace BlazorAIFoundryAgentsCatalog.Services
{
    public interface IAzureAIFoundryService
    {
        Task<IEnumerable<AgentDto>> GetAgents();
    }
}
