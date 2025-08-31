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
    }
}
