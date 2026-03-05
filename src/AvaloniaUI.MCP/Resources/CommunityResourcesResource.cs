using System.ComponentModel;
using System.Text.Json;

using AvaloniaUI.MCP.Services;

using ModelContextProtocol.Server;

namespace AvaloniaUI.MCP.Resources;

[McpServerResourceType]
public static class CommunityResourcesResource
{
    [McpServerResource]
    [Description("Curated list of popular AvaloniaUI community libraries, themes, and open-source applications from awesome-avalonia and official sources")]
    public static async Task<string> GetCommunityResources()
    {
        return await ErrorHandlingService.SafeExecuteAsync("GetCommunityResources", async () =>
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "community-resources.json");
            string cacheKey = "formatted_community_resources";

            return await ResourceCacheService.GetOrLoadResourceAsync(cacheKey, async () =>
            {
                JsonElement resourcesData = await ResourceCacheService.GetOrLoadJsonResourceAsync(dataPath, TimeSpan.FromHours(1));
                return FormatCommunityResources(resourcesData);
            }, TimeSpan.FromMinutes(30));
        });
    }

    [McpServerResource]
    [Description("Get recommended MVVM frameworks for AvaloniaUI development")]
    public static async Task<string> GetMvvmFrameworks()
    {
        return await ErrorHandlingService.SafeExecuteAsync("GetMvvmFrameworks", async () =>
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "community-resources.json");
            string cacheKey = "formatted_mvvm_frameworks";

            return await ResourceCacheService.GetOrLoadResourceAsync(cacheKey, async () =>
            {
                JsonElement resourcesData = await ResourceCacheService.GetOrLoadJsonResourceAsync(dataPath, TimeSpan.FromHours(1));
                return FormatSection(resourcesData, "mvvm_frameworks", "MVVM Frameworks for AvaloniaUI");
            }, TimeSpan.FromMinutes(30));
        });
    }

    [McpServerResource]
    [Description("Get available UI themes and styling packages for AvaloniaUI")]
    public static async Task<string> GetUiThemes()
    {
        return await ErrorHandlingService.SafeExecuteAsync("GetUiThemes", async () =>
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "community-resources.json");
            string cacheKey = "formatted_ui_themes";

            return await ResourceCacheService.GetOrLoadResourceAsync(cacheKey, async () =>
            {
                JsonElement resourcesData = await ResourceCacheService.GetOrLoadJsonResourceAsync(dataPath, TimeSpan.FromHours(1));
                return FormatSection(resourcesData, "ui_themes", "UI Themes for AvaloniaUI");
            }, TimeSpan.FromMinutes(30));
        });
    }

    [McpServerResource]
    [Description("Get official AvaloniaUI learning resources including documentation, quick guides, and samples")]
    public static async Task<string> GetOfficialResources()
    {
        return await ErrorHandlingService.SafeExecuteAsync("GetOfficialResources", async () =>
        {
            string dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "community-resources.json");
            string cacheKey = "formatted_official_resources";

            return await ResourceCacheService.GetOrLoadResourceAsync(cacheKey, async () =>
            {
                JsonElement resourcesData = await ResourceCacheService.GetOrLoadJsonResourceAsync(dataPath, TimeSpan.FromHours(1));
                return FormatSection(resourcesData, "official_resources", "Official AvaloniaUI Resources");
            }, TimeSpan.FromMinutes(30));
        });
    }

    static string FormatCommunityResources(JsonElement resourcesData)
    {
        var result = "# AvaloniaUI Community Resources\n\n";
        result += "Curated from [awesome-avalonia](https://github.com/AvaloniaCommunity/awesome-avalonia) " +
                  "and official AvaloniaUI sources.\n\n";

        if (!resourcesData.TryGetProperty("avalonia_community_resources", out JsonElement resources))
            return result + "No community resources data found.";

        string[] sectionTitles =
        [
            "mvvm_frameworks:MVVM Frameworks",
            "ui_themes:UI Themes",
            "chart_libraries:Chart Libraries",
            "control_libraries:Control Libraries & Extensions",
            "notable_applications:Notable Open-Source Applications",
            "official_resources:Official Resources"
        ];

        foreach (string sectionEntry in sectionTitles)
        {
            string[] parts = sectionEntry.Split(':');
            string key = parts[0];
            string title = parts[1];

            if (resources.TryGetProperty(key, out JsonElement section))
            {
                result += $"## {title}\n\n";
                foreach (JsonProperty item in section.EnumerateObject())
                {
                    result += FormatResourceItem(item.Name, item.Value);
                }
            }
        }

        return result;
    }

    static string FormatSection(JsonElement resourcesData, string sectionKey, string sectionTitle)
    {
        var result = $"# {sectionTitle}\n\n";

        if (!resourcesData.TryGetProperty("avalonia_community_resources", out JsonElement resources))
            return result + "No data found.";

        if (!resources.TryGetProperty(sectionKey, out JsonElement section))
            return result + $"Section '{sectionKey}' not found.";

        foreach (JsonProperty item in section.EnumerateObject())
        {
            result += FormatResourceItem(item.Name, item.Value);
        }

        return result;
    }

    static string FormatResourceItem(string name, JsonElement item)
    {
        var result = $"### {name}\n\n";

        if (item.TryGetProperty("description", out JsonElement desc))
            result += $"{desc.GetString()}\n\n";

        if (item.TryGetProperty("nuget", out JsonElement nuget))
            result += $"**NuGet:** `{nuget.GetString()}`\n";

        if (item.TryGetProperty("github_url", out JsonElement github))
            result += $"**GitHub:** {github.GetString()}\n";

        if (item.TryGetProperty("docs_url", out JsonElement docs))
            result += $"**Docs:** {docs.GetString()}\n";

        if (item.TryGetProperty("url", out JsonElement url))
            result += $"**URL:** {url.GetString()}\n";

        if (item.TryGetProperty("usage", out JsonElement usage))
            result += $"\n**Usage:** {usage.GetString()}\n";

        if (item.TryGetProperty("key_features", out JsonElement features))
        {
            result += "\n**Key Features:**\n";
            foreach (JsonElement feat in features.EnumerateArray())
                result += $"- {feat.GetString()}\n";
        }

        if (item.TryGetProperty("sections", out JsonElement sections))
        {
            result += "\n**Sections:**\n";
            foreach (JsonElement sec in sections.EnumerateArray())
                result += $"- {sec.GetString()}\n";
        }

        if (item.TryGetProperty("topics", out JsonElement topics))
        {
            result += "\n**Topics:**\n";
            foreach (JsonElement topic in topics.EnumerateArray())
                result += $"- {topic.GetString()}\n";
        }

        if (item.TryGetProperty("example", out JsonElement example))
        {
            result += "\n**Example:**\n```csharp\n";
            result += example.GetString();
            result += "\n```\n";
        }

        result += "\n";
        return result;
    }
}
