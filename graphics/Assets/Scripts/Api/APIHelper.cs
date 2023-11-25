using UnityEngine;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;

public static class APIHelper
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> FetchDataFromAPI(string url)
    {
        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        catch(HttpRequestException e)
        {
            Debug.Log($"Error fetching data: {e.Message}");
            Application.Quit();
            return null;
        }
    }

    public static GameState ParseJsonToGameState(string jsonString)
    {
        return JsonConvert.DeserializeObject<GameState>(jsonString);
    }
}