using System.Net.Http;
using System.Text.Json;

namespace NoWaiste.Services;

public static class SpoonacularRecipeService
{
    private static readonly string apiKey = "707b99e209f742b2bc41f9cc57e43a4b"; // API KEY
    private static readonly HttpClient httpClient = new();

    public static async Task<RecipeResult?> GetRecipeFromIngredientsAsync(List<string> ingredients)
    {
        var joinedIngredients = string.Join(",", ingredients.Take(5)); // max 5 ingrédients en gratuit
        var searchUrl = $"https://api.spoonacular.com/recipes/findByIngredients?ingredients={joinedIngredients}&number=1&apiKey={apiKey}";

        var searchResponse = await httpClient.GetAsync(searchUrl);
        if (!searchResponse.IsSuccessStatusCode)
            return null;

        var searchJson = await searchResponse.Content.ReadAsStringAsync();
        var searchResults = JsonSerializer.Deserialize<List<RecipeSearchResult>>(searchJson);

        var firstRecipe = searchResults?.FirstOrDefault();
        if (firstRecipe == null)
            return null;

        // Deuxième appel pour détails
        var detailUrl = $"https://api.spoonacular.com/recipes/{firstRecipe.id}/information?includeNutrition=false&apiKey={apiKey}";
        var detailResponse = await httpClient.GetAsync(detailUrl);

        if (!detailResponse.IsSuccessStatusCode)
            return null;

        var detailJson = await detailResponse.Content.ReadAsStringAsync();
        
        var detail = JsonSerializer.Deserialize<RecipeDetail>(detailJson);

        return new RecipeResult
        {
            Title = detail?.title,
            ImageUrl = detail?.image,
            Instructions = detail?.instructions
        };
    }
}
