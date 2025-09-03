namespace BlazorAIFoundryAgentsCatalog.Client.DTOs
{
    public record SendMessageRequest(
        string AgentId,
        string UserPrompt,
        string? ThreadId
    );

}
