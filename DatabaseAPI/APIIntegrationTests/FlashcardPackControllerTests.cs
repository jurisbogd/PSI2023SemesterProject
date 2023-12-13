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
    public async Task GetFlashcardPackForUser_ReturnsOkResultAndFlashcardPack()
    {
        var userID = Guid.Parse("90A77343-593B-4538-B2E8-035DDB0006B5");
        var flashcardPackID = Guid.Parse("BE90F741-A06F-4D57-8E43-EF24EB29E091");

        var requestUrl = $"/api/FlashcardPack/GetFlashcardPack?packID={flashcardPackID}&userId={userID}";

        var response = await _client.GetAsync(requestUrl);

        var responseContent = await response.Content.ReadAsStringAsync();
        var flashcardPack = JsonConvert.DeserializeObject<FlashcardPack>(responseContent);

        response.EnsureSuccessStatusCode();

        Assert.Equal(200, (int)response.StatusCode);
        Assert.Equal(flashcardPackID, flashcardPack.ID);
    }

    [Fact]
    public async Task AddFlashcardPack_ReturnsOkResult()
    {
        var userID = Guid.Parse("90A77343-593B-4538-B2E8-035DDB0006B5");
        var flashcardPack = new FlashcardPack
        {
            ID = Guid.NewGuid(),
            Name = "Test Pack",
            Flashcards = new List<Flashcard>()
        };

        var requestUrl = $"/api/FlashcardPack/AddFlashcardPack?userId={userID}";
        var jsonContent = new StringContent(JsonConvert.SerializeObject(flashcardPack), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(requestUrl, jsonContent);

        var responseContent = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();

        Assert.Equal(200, (int)response.StatusCode);
        Assert.Equal("Flashcard pack added successfully", responseContent);
    }
}