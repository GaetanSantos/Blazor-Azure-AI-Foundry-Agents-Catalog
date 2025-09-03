using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using BlazorAIFoundryAgentsCatalog.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAIFoundryAgentsCatalog.Controllers
{
    [Route("api/agents/{agentId}/chat")]
    [ApiController]
    public class AgentChatController : ControllerBase
    {
        private readonly IFoundryAgentProvider _provider;

        public AgentChatController(IFoundryAgentProvider provider)
        {
            _provider = provider;
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<ChatMessageDto>>> GetHistory(string agentId)
        {
            var history = await _provider.GetChatHistoryAsync(agentId);
            return Ok(history);
        }

        [HttpPost("send")]
        public async Task<ActionResult<ChatMessageDto>> SendMessage([FromBody] SendMessageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.AgentId) || string.IsNullOrWhiteSpace(request.UserPrompt))
                return BadRequest();

            var (threadId, messages) = await _provider.SendMessageAndGetHistoryAsync(request.AgentId, request.UserPrompt, request.ThreadId);

            if (string.IsNullOrWhiteSpace(threadId) || messages is null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");

            return Ok(new ChatThreadDto(threadId, messages));
        }
    }
}
