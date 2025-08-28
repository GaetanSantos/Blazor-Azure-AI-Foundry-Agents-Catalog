using Azure.AI.Projects;
using Azure.AI.Agents.Persistent;
using Azure.Identity;
using BlazorAIFoundryAgentsCatalog.Shared;
using BlazorAIFoundryAgentsCatalog.Options;
using Microsoft.Extensions.Options;

namespace BlazorAIFoundryAgentsCatalog.Services
{
    public class AzureAIFoundryService : IAzureAIFoundryService
    {
        private readonly AzureAIFoundryOptions _options;

        public AzureAIFoundryService(IOptions<AzureAIFoundryOptions> options)
        {
            _options = options.Value;
        }

        public async Task<IEnumerable<AgentDto>> GetAgents()
        {
            var endpointUri = new Uri(_options.EndpointUrl);
            var credential = new DefaultAzureCredential();
            var projectClient = new AIProjectClient(endpointUri, credential);
            var agentClient = projectClient.GetPersistentAgentsClient();
            var agents = new List<AgentDto>();

            await foreach (var foundryAgent in agentClient.Administration.GetAgentsAsync())
            {
                agents.Add(new AgentDto(foundryAgent.Id, foundryAgent.Name, foundryAgent.Description));
            }

            return agents;
        }
    }
}
