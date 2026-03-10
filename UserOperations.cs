using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scratch3Sharp.Core;

public class Scratch3User
{
    [JsonPropertyName("id")]
    public ulong UserId { get; set; }
        
    [JsonPropertyName("username")]
    public string UserName { get; set; }
        
    [JsonPropertyName("scratchteam")]
    public bool IsScratchTeam { get; set; }
}

public static class UserOperations
{
    /// <summary>
    /// Gets the specified user's information
    /// </summary>
    /// <param name="user">The specific user to search for</param>
    /// <returns>Returns a Scratch3User</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<Scratch3User> GetUserInfo(string user)
    {
        string jsonUser = 
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/users/{user}/");
        return JsonSerializer.Deserialize<Scratch3User>(jsonUser)
               ?? throw new InvalidOperationException($"Could not deserialize user {user}");
    }

    /// <summary>
    /// Gets the specified user's projects
    /// </summary>
    /// <param name="user">The specific user that is used to search for the projects</param>
    /// <returns>Returns a List of Scratch3ProjectInfoElement</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<List<Scratch3ProjectInfoElement>> GetUserProjectList(string user)
    {
        string jsonProjectsList =
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/users/{user}/projects");
        return JsonSerializer.Deserialize<List<Scratch3ProjectInfoElement>>(jsonProjectsList)
               ?? throw new InvalidOperationException($"Could not deserialize user {user}'s projects");
    }

    /// <summary>
    /// Gets the specified user's followers list
    /// </summary>
    /// <param name="user">The specific user that is used to search for their followers</param>
    /// <returns>Returns a list of Scratch3User</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<List<Scratch3User>> GetUserFollowers(string user)
    {
        string jsonFollowersList =
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/users/{user}/followers");
        return JsonSerializer.Deserialize<List<Scratch3User>>(jsonFollowersList)
            ?? throw new InvalidOperationException($"Could not deserialize user {user}'s followers");
    }

    /// <summary>
    /// Gets the specified user's following list
    /// </summary>
    /// <param name="user">The specific user that is used to search for their following</param>
    /// <returns>Returns a list of Scratch3User</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<List<Scratch3User>> GetUserFollowing(string user)
    {
        string jsonFollowingList =
            await HttpClientHelper.Client.GetStringAsync($"https://api.scratch.mit.edu/users/{user}/following");
        return JsonSerializer.Deserialize<List<Scratch3User>>(jsonFollowingList)
            ?? throw new InvalidOperationException($"Could not deserialize user {user}'s following");
    }
}

/*

https://api.scratch.mit.edu

---------------------------
/users/{username}/ user info
/users/{username}/followers/ followers
/users/{username}/following/ following
/users/{username}/projects/ projects
https://assets.scratch.mit.edu/internalapi/asset/{asset_id}/get/ get asset id

*/