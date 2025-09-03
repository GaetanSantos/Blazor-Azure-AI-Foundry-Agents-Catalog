namespace BlazorAIFoundryAgentsCatalog.Client.DTOs
{
    public record ChatThreadDto(
        string ThreadId,
        List<ChatMessageDto> Messages
    );
}
