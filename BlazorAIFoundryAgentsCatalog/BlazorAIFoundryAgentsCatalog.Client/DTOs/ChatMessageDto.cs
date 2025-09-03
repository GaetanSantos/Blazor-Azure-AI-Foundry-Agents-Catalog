namespace BlazorAIFoundryAgentsCatalog.Client.DTOs
{
    public record ChatMessageDto(
        string AgentId,
        string Role, 
        string Content,
        DateTime Timestamp
    );
}
