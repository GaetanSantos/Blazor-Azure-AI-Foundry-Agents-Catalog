using Markdig;

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

        public static string ConvertMarkdownToHtml(string markdownContent)
        {
            if (string.IsNullOrWhiteSpace(markdownContent)) 
                return string.Empty;

            var pipeline = new MarkdownPipelineBuilder()
               .UsePipeTables()
               .Build();

            return Markdown.ToHtml(markdownContent, pipeline).Replace("<table>", "<table class=\"markdown-table\">");
        }
    }
}
