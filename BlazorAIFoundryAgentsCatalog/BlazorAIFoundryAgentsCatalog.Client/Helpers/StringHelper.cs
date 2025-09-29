namespace BlazorAIFoundryAgentsCatalog.Client.Helpers
{
    public static class StringHelper
    {
        public static string GetInitials(string agentName)
        {
            if (string.IsNullOrWhiteSpace(agentName))
                return string.Empty;

            var initials = new string([.. agentName.Where(char.IsUpper)]);

            return initials;
        }

    }
}
