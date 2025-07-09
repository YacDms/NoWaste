using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SkiaSharp;

namespace NoWaiste.Services
{
    public static class ImageAnalyzer
    {
        private static readonly string apiKey = "c0cb83ab0b27ed83988f641fd502f435"; // Clé API CalorieMama

        public static async Task<GroupedCalorieMamaResponse?> AnalyserImageAvecCalorieMama(Stream imageStream)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var imageBytes = ReadFully(imageStream);
            var resizedBytes = ResizeImageTo544x544(imageBytes);
            var base64Image = Convert.ToBase64String(resizedBytes);

            var jsonBody = JsonSerializer.Serialize(new
            {
                image = base64Image,
                image_type = "base64"
            });

            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("https://api-2445582032290.production.gw.apicast.io/v1/foodrecognition?user_key=c0cb83ab0b27ed83988f641fd502f435", content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("Erreur CalorieMama : " + error);
                await Application.Current.MainPage.DisplayAlert("Error", error, "OK");
                return null;
            }

            var json = await response.Content.ReadAsStringAsync();
            var rawResponse = JsonSerializer.Deserialize<ApiResponse>(json);

            if (rawResponse?.Results == null)
                return null;

            // Étape 1 : on prend tous les items, on trie par score décroissant
            var sortedItems = rawResponse.Results
                .SelectMany(r => r.Items ?? new List<RawItem>())
                .OrderByDescending(i => i.Score)
                .ToList();

            // Étape 2 : on ne garde qu’un seul item (le meilleur) par group
            var bestItemsByGroup = sortedItems
                .GroupBy(i => i.Group)
                .Select(g => g.First())
                .ToList();

            // Étape 3 : on mappe vers CalorieMamaPrediction
            var groupedResults = bestItemsByGroup
                .Select(item => new GroupedCalorieMamaResponse.GroupedResult
                {
                    Group = item.Group,
                    Items = new List<CalorieMamaPrediction>
                    {
                        new CalorieMamaPrediction
                        {
                            name = item.Name,
                            foodId = item.FoodId,
                            group = item.Group,
                            score = item.Score
                        }
                    }
                }).ToList();

            return new GroupedCalorieMamaResponse
            {
                Results = groupedResults
            };
        }

        private static byte[] ReadFully(Stream input)
        {
            using MemoryStream ms = new();
            input.CopyTo(ms);
            return ms.ToArray();
        }

        private static byte[] ResizeImageTo544x544(byte[] original)
        {
            using var input = new MemoryStream(original);
            using var originalBitmap = SKBitmap.Decode(input);
            using var resizedBitmap = originalBitmap.Resize(new SKImageInfo(544, 544), SKFilterQuality.Medium);
            using var image = SKImage.FromBitmap(resizedBitmap);
            using var data = image.Encode(SKEncodedImageFormat.Jpeg, 90);
            return data.ToArray();
        }
    }

    // Réponses de l’API CalorieMama

    public class ApiResponse
    {
        [JsonPropertyName("results")]
        public List<ApiResult>? Results { get; set; }
    }

    public class ApiResult
    {
        [JsonPropertyName("items")]
        public List<RawItem>? Items { get; set; }
    }

    public class RawItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("food_id")]
        public string? FoodId { get; set; }

        [JsonPropertyName("group")]
        public string? Group { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }

    public class CalorieMamaPrediction
    {
        public string? name { get; set; }
        public string? foodId { get; set; }
        public string? group { get; set; }
        public int score { get; set; }
    }

    public class GroupedCalorieMamaResponse
    {
        public List<GroupedResult>? Results { get; set; }

        public class GroupedResult
        {
            public string? Group { get; set; }
            public List<CalorieMamaPrediction>? Items { get; set; }
        }
    }
}