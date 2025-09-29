using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using BlazorAIFoundryAgentsCatalog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAIFoundryAgentsCatalog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly IAzureAIFoundryService _azureAIFoundryService;

        public AgentsController(IAzureAIFoundryService azureAIFoundryService)
        {
            _azureAIFoundryService = azureAIFoundryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgentDto>>> Get()
        {
            var agents = await _azureAIFoundryService.GetAgents();
            return Ok(agents);
        }


        [HttpGet("{agentId}")]
        public async Task<ActionResult<AgentDto>> GetAgentById(string agentId)
        {
            var agent = await _azureAIFoundryService.GetAgent(agentId);
            if (agent is null)
                return NotFound();
            return Ok(agent);
        }
    }
}
