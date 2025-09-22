using Azure.AI.Agents.Persistent;
using Azure.Identity;
using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using BlazorAIFoundryAgentsCatalog.Options;
using Microsoft.Extensions.Options;

namespace BlazorAIFoundryAgentsCatalog.Services
{
    public class AzureAIFoundryAgentService : IAzureAIFoundryAgentService
    {
        private readonly AzureAIFoundryOptions _options;
        private readonly string? _endpoint;
        private readonly DefaultAzureCredential _credential;

        public bool IsConfigured { get; private set; }
        public PersistentAgentsClient? Client { get; private set; }
        public string? ThreadId { get; private set; }
        public PersistentAgent? Agent { get; private set; }
        public string? AgentId { get; private set; }

        public AzureAIFoundryAgentService(IOptions<AzureAIFoundryOptions> options)
        {
            _options = options.Value;
            _endpoint = _options.EndpointUrl;
            _credential = new DefaultAzureCredential();
            IsConfigured = false;
        }

        private async Task InitializeAsync(string agentId, string? threadId = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_endpoint))
            {
                IsConfigured = false;
                return;
            }

            if (IsConfigured && AgentId == agentId && Client is not null)
            {
                return;
            }

            Client = new PersistentAgentsClient(_endpoint, _credential);

            if (string.IsNullOrWhiteSpace(threadId))
            {
                PersistentAgentThread agentThread = await Client.Threads.CreateThreadAsync(cancellationToken: cancellationToken);
                threadId = agentThread.Id;
            }

            ThreadId = threadId;

            // get agent by id (synchronous call per prior sample)
            Agent = await Client.Administration.GetAgentAsync(agentId, cancellationToken);

            AgentId = agentId;
            IsConfigured = Agent is not null && !string.IsNullOrWhiteSpace(ThreadId);
        }

        public async Task<List<ChatMessageDto>> GetChatHistoryAsync(string agentId)
        {
            // Ensure initialized for the requested agent
            if (!IsConfigured || AgentId != agentId || Client is null || ThreadId is null)
                await InitializeAsync(agentId);

            var result = new List<ChatMessageDto>();

            if (!IsConfigured || Client is null || ThreadId is null)
                return result;

            // Retrieve messages from the thread (assume ascending order)
            var messages = Client.Messages.GetMessages(
                threadId: ThreadId,
                order: ListSortOrder.Ascending);

            foreach (var msg in messages)
            {
                string content = "";
                if (msg.ContentItems is not null)
                {
                    foreach (var item in msg.ContentItems)
                    {
                        if (item is MessageTextContent text)
                            content += text.Text;
                    }
                }

                result.Add(new ChatMessageDto(
                    AgentId: agentId,
                    Role: msg.Role.ToString().ToLowerInvariant(),
                    Content: content,
                    Timestamp: msg.CreatedAt.UtcDateTime
                ));
            }

            return result;
        }

        public async Task<(string? ThreadId, List<ChatMessageDto>? Messages)> SendMessageAndGetHistoryAsync(string agentId, string userPrompt, string? threadId)
        {
            // Ensure initialized for the requested agent
            if (!IsConfigured || AgentId != agentId || Client is null || ThreadId is null)
                await InitializeAsync(agentId, threadId);

            if (!IsConfigured || Client is null || ThreadId is null || Agent is null)
                return (ThreadId: null, null);

            // If threadId is null or empty, create a new thread
            if (string.IsNullOrWhiteSpace(threadId))
            {
                threadId = ThreadId;
            }

            var resultMessages = new List<ChatMessageDto>();

            // 1. Add the user prompt to the thread
            await Client.Messages.CreateMessageAsync(
                threadId,
                MessageRole.User,
                userPrompt);

            // 2. Start a run to process the user prompt
            ThreadRun run = await Client.Runs.CreateRunAsync(
                threadId,
                Agent.Id);

            // 3. Poll for completion
            do
            {
                await Task.Delay(500);
                run = await Client.Runs.GetRunAsync(threadId, run.Id);
            }
            while (run.Status == RunStatus.Queued
                || run.Status == RunStatus.InProgress
                || run.Status == RunStatus.RequiresAction);

            // Retrieve messages from the thread
            var messagesFromThread = Client.Messages.GetMessages(
                threadId: threadId,
                order: ListSortOrder.Ascending);

            foreach (var msg in messagesFromThread)
            {
                string content = "";
                if (msg.ContentItems is not null)
                {
                    foreach (var item in msg.ContentItems)
                    {
                        if (item is MessageTextContent text)
                            content += text.Text;
                    }
                }

                resultMessages.Add(new ChatMessageDto(
                    AgentId: agentId,
                    Role: msg.Role.ToString().ToLowerInvariant(),
                    Content: content,
                    Timestamp: msg.CreatedAt.UtcDateTime
                ));
            }

            return (threadId, resultMessages);
        }
    }
}
