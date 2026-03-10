using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scratch3Sharp.Core;

// Json structure for project data

public class Scratch3ProjectInfoElement
{
    [JsonPropertyName("id")]
    public ulong ProjectId { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("author")]
    public Scratch3ProjectAuthor Author { get; set; }
    
    [JsonPropertyName("stats")]
    public Scratch3ProjectStats Stats { get; set; }
    
    [JsonPropertyName("project_token")]
    public string? ProjectToken { get; set; }
}

public class Scratch3ProjectAuthor
{
    [JsonPropertyName("id")]
    public ulong Id { get; set; }

    [JsonPropertyName("username")]
    public string? Username { get; set; } = null;
    
    [JsonPropertyName("scratchteam")]
    public bool IsScratchTeam { get; set; }
}

public class Scratch3ProjectStats
{
    [JsonPropertyName("views")]
    public uint ProjectViews { get; set; }
    
    [JsonPropertyName(("loves"))]
    public uint ProjectLoves { get; set; }
    
    [JsonPropertyName("favorites")]
    public uint ProjectFavorites { get; set; }
    
    [JsonPropertyName("remixes")]
    public uint ProjectRemixes { get; set; }
}

public static class ProjectOperations
{
    /// <summary>
    /// The difference is that GetUserProjectList does not return the username in the Scratch3ProjectAuthor.            
    /// Gets the project info given the id that was given
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <returns>Returns a Scratch3ProjectInfoElement with non-null username</returns>
    /// <exception cref="InvalidOperationException"></exception>
    
    public static async Task<Scratch3ProjectInfoElement> GetProjectInfo(ulong projectId)
    {
        string jsonProjectInfo =
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/projects/{projectId}/");
        return JsonSerializer.Deserialize<Scratch3ProjectInfoElement>(jsonProjectInfo)
            ?? throw new InvalidOperationException($"Could not deserialize {projectId}'s project information");
    }

    /// <summary>
    /// Gets all projects with x substring.
    /// </summary>
    /// <param name="substring">The substring to search for</param>
    /// <returns>Returns a List of Scratch3ProjectInfoElement with non-null Username property.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<List<Scratch3ProjectInfoElement>> GetProjectsWithSubstring(string substring)
    {
        string jsonProjectListWithSubstring =
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/search/projects?q={Uri.EscapeDataString(substring)}");
        return JsonSerializer.Deserialize<List<Scratch3ProjectInfoElement>>(jsonProjectListWithSubstring)
            ?? throw new InvalidOperationException($"Could not deserialize projects with {substring} substring");
    }

    /// <summary>
    /// Gets the raw JSON project code. Did you REALLY expect me to parse over 200KB of JSON? Nah.
    /// Warning: could be over 200KB of pure json, so good luck trying to sleep today.
    /// </summary>
    /// <param name="projectId">The project ID to search for</param>
    /// <returns>Returns a raw string of pure JSON.</returns>
    public static async Task<string> GetRawProjectCode(ulong projectId)
    {
        Scratch3ProjectInfoElement projectInfo = await GetProjectInfo(projectId);

        string jsonProjectRawCode =
            await HttpClientHelper.Client.GetStringAsync($"https://projects.scratch.mit.edu/{projectId}?token={projectInfo.ProjectToken}");
        return jsonProjectRawCode;
    }
}

/*
https://api.scratch.mit.edu/
 
/project/{proj_id}/ proj info
/search/projects?q={proj_query_substring}/ get all projects with x substring
https://projects.scratch.mit.edu/{proj_id}/ project code
*/