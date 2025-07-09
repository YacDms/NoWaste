namespace NoWaiste.Services;

public class RecipeResult
{
    public string? Title { get; set; }
    public string? ImageUrl { get; set; }
    public string? Instructions { get; set; }
}

public class RecipeSearchResult
{
    public int id { get; set; }
    public string? title { get; set; }
    public string? image { get; set; }
}

public class RecipeDetail
{
    public string? title { get; set; }
    public string? image { get; set; }
    public string? instructions { get; set; }
}
