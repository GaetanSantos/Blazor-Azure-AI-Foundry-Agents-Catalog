using BlazorAIFoundryAgentsCatalog.Client.DTOs;
using BlazorAIFoundryAgentsCatalog.Client.Services;
using Markdig;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace BlazorAIFoundryAgentsCatalog.Client.Pages
{
    public class FoundryAgentChatModel : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        protected IAgentCatalogService AgentCatalogService { get; set; } = default!;

        [Inject]
        protected IAgentChatService AgentChatService { get; set; } = default!;

        [Parameter]
        public string AgentId { get; set; } = string.Empty;

        protected List<ChatMessageDto>? messages;
        protected string ThreadId = string.Empty;
        protected string CurrentText = string.Empty;
        protected bool IsLoading = false;
        protected string AgentName = string.Empty;
        protected string AgentDescription = string.Empty;
        protected bool IsAgentInfoLoading = true;

        protected override async Task OnParametersSetAsync()
        {
            IsAgentInfoLoading = true;
            var agent = await AgentCatalogService.GetAgentAsync(AgentId);
            AgentName = agent.Name;
            AgentDescription = agent.Description ?? string.Empty;
            IsAgentInfoLoading = false;

            await ResetAgentContext();
        }

        private async Task ResetAgentContext()
        {
            IsLoading = true;
            messages = new();
            ThreadId = string.Empty;
            IsLoading = false;
            await InvokeAsync(StateHasChanged);
        }

        protected async Task SendAndRefreshHistory()
        {
            if (string.IsNullOrWhiteSpace(CurrentText)) return;

            IsLoading = true;

            var userPrompt = CurrentText;

            // Optimistically add the user's message
            var userMsg = new ChatMessageDto(
                AgentId: AgentId,
                Role: "user",
                Content: userPrompt,
                Timestamp: DateTime.UtcNow
            );
            CurrentText = string.Empty;
            messages ??= new List<ChatMessageDto>();
            messages.Add(userMsg);
            StateHasChanged();

            var chatThread = await AgentChatService.SendMessageAndGetHistoryAsync(AgentId, userPrompt, ThreadId);
            if (chatThread is not null)
            {
                messages = chatThread.Messages;
                StateHasChanged();
                if (string.IsNullOrWhiteSpace(ThreadId))
                {
                    ThreadId = chatThread.ThreadId;
                }
            }

            IsLoading = false;
        }

        protected async Task OnInputKeyDown(KeyboardEventArgs e)
        {
            if (e.Key == "Enter" && !IsLoading)
            {
                await SendAndRefreshHistory();
            }
        }

        protected record MessageGroup(string Role, List<ChatMessageDto> Messages);

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (messages is not null && messages.Count > 0)
            {
                await JSRuntime.InvokeVoidAsync("eval", @"
                var el = document.getElementById('chatMessages');
                if (el) el.scrollTop = el.scrollHeight;
            ");
            }
        }

    }
}
