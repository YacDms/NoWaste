using NoWaiste.Models;

namespace NoWaiste;

public partial class AnalysePage : ContentPage
{
    public AnalysePage(List<Ingredient> ingredients)
    {
        InitializeComponent();

        foreach (var ing in ingredients)
        {
            ingredientsList.Children.Add(new Label
            {
                Text = $"{ing.name} ({ing.amount})",
                FontSize = 18
            });
        }
    }
}
