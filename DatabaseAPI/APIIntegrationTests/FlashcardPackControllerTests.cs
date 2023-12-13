using DatabaseAPI;
using DatabaseAPI.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;

namespace APIIntegrationTests;


public class FlashcardPackControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FlashcardPackControllerTests(WebApplicationFactory<Program> applicationFactory)
    {
        _client = applicationFactory.CreateClient();
    }


    [Fact]
    public async Task GetFlashcardPackForUser_ReturnsOkResult()
    {
        var userID = Guid.NewGuid();

        var flashcardPack = new FlashcardPack
        {
            ID = Guid.NewGuid(),
            Name = "Test pack",
            Flashcards = new List<Flashcard>()
        };

        var jsonContent = new StringContent(JsonConvert.SerializeObject(flashcardPack), Encoding.UTF8, "application/json");
        var requestUri = $"/api/FlashcardPack/GetFlashcardPack?packID={flashcardPack.ID}&userId={userID}";

        var response = await _client.PostAsync(requestUri, jsonContent);

        response.EnsureSuccessStatusCode();

        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();

        Assert.Equal(flashcardPack.ID.ToString(), responseContent);
    }

}