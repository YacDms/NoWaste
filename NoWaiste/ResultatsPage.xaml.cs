using NoWaiste.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace NoWaiste;

public partial class ResultatsPage : ContentPage
{
    public ObservableCollection<CalorieMamaPrediction> Items { get; set; }

    public ICommand RemoveCommand { get; }

    public ResultatsPage(List<CalorieMamaPrediction> predictions)
    {
        InitializeComponent();

        Items = new ObservableCollection<CalorieMamaPrediction>(predictions);
        RemoveCommand = new Command<CalorieMamaPrediction>(OnRemoveItem);

        BindingContext = this;
    }

    private void OnRemoveItem(CalorieMamaPrediction item)
    {
        if (Items.Contains(item))
            Items.Remove(item);
    }

    private async void OnAddItemClicked(object sender, EventArgs e)
    {
        string name = await DisplayPromptAsync("Ajouter", "Nom de l'aliment :");
        string group = await DisplayPromptAsync("Ajouter", "Groupe :");
        string scoreStr = await DisplayPromptAsync("Ajouter", "Score (entier) :");

        if (int.TryParse(scoreStr, out int score))
        {
            Items.Add(new CalorieMamaPrediction
            {
                name = name,
                group = group,
                foodId = Guid.NewGuid().ToString(), // temporaire
                score = score
            });
        }
        else
        {
            await DisplayAlert("Erreur", "Score invalide", "OK");
        }
    }

    private async void OnRecetteClicked(object sender, EventArgs e)
    {
        if (Items == null || Items.Count == 0)
        {
            await DisplayAlert("Erreur", "Aucun aliment détecté pour générer une recette.", "OK");
            return;
        }

        var ingredients = Items.Select(i => i.name).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();

        var recette = await SpoonacularRecipeService.GetRecipeFromIngredientsAsync(ingredients);

        if (recette != null)
        {
            //string msg = $"Nom : {recette.Title}\n\nInstructions :\n{recette.Instructions ?? "Non disponibles."}";

            await Navigation.PushAsync(new RecettePage(recette));
        }
        else
        {
            await DisplayAlert("Erreur", "Aucune recette trouvée.", "OK");
        }
    }
}
