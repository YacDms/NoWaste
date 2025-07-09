using NoWaiste.Services;
using System.Text.RegularExpressions;
using Microsoft.Maui.ApplicationModel;

namespace NoWaiste;

public partial class RecettePage : ContentPage
{
    public RecettePage(RecipeResult recette)
    {
        InitializeComponent();

        RecipeTitle.Text = recette.Title ?? "Recette";

        if (!string.IsNullOrWhiteSpace(recette.ImageUrl))
        {
            RecipeImage.Source = ImageSource.FromUri(new Uri(recette.ImageUrl));
        }

        // Liste d’étapes finale
        var steps = new List<string>();

        if (!string.IsNullOrWhiteSpace(recette.Instructions))
        {
            // Cas 1 : HTML avec <li>
            if (recette.Instructions.Contains("<li>"))
            {
                var matches = Regex.Matches(recette.Instructions, "<li>(.*?)</li>", RegexOptions.Singleline);

                foreach (Match match in matches)
                {
                    var step = Regex.Replace(match.Groups[1].Value, "<.*?>", "").Trim();
                    if (!string.IsNullOrWhiteSpace(step))
                        steps.Add(step);
                }
            }
            else
            {
                // Cas 2 : texte brut, on découpe par "."
                var rawSteps = recette.Instructions.Split(new[] { '.', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var s in rawSteps)
                {
                    var step = s.Trim();
                    if (!string.IsNullOrWhiteSpace(step))
                        steps.Add(step + ".");
                }
            }
        }

        // Affichage
        if (steps.Count > 0)
        {
            foreach (var (step, index) in steps.Select((s, i) => (s, i + 1)))
            {
                InstructionsList.Children.Add(new Label
                {
                    Text = $"{index}. {step}",
                    FontSize = 16,
                    TextColor = Colors.Black
                });
            }
        }
        else
        {
            InstructionsList.Children.Add(new Label
            {
                Text = "Aucune instruction disponible.",
                FontSize = 16,
                TextColor = Colors.Gray
            });
        }
    }
    private async void OnLireRecetteClicked(object sender, EventArgs e)
    {
        // On récupère tous les Labels ajoutés dans InstructionsList
        var steps = InstructionsList.Children
            .OfType<Label>()
            .Select(label => label.Text)
            .Where(text => !string.IsNullOrWhiteSpace(text))
            .ToList();

        if (steps.Count == 0)
        {
            await DisplayAlert("Info", "Aucune instruction à lire.", "OK");
            return;
        }

        // Récupère les voix disponibles sur l'appareil
        var locales = await TextToSpeech.GetLocalesAsync();

        // Cherche une voix en anglais (en-US ou en-GB par exemple)
        var englishLocale = locales.FirstOrDefault(l => l.Language.StartsWith("en"));

        foreach (var step in steps)
        {
            await TextToSpeech.SpeakAsync(step, new SpeechOptions
            {
                Locale = englishLocale
            });
            await Task.Delay(800); // Petite pause entre les étapes
        }
    }


}
