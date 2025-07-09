using Microsoft.Maui.Media;
using NoWaiste.Services;

namespace NoWaiste;

public partial class CameraPage : ContentPage
{
    public CameraPage()
    {
        InitializeComponent();
    }

    private async void OnTakePhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null)
            {
                await DisplayAlert("Annulé", "Aucune photo prise.", "OK");
                return;
            }

            using var stream = await photo.OpenReadAsync();

            var resultat = await ImageAnalyzer.AnalyserImageAvecCalorieMama(stream);

            if (resultat?.Results == null || resultat.Results.Count == 0)
            {
                await DisplayAlert("Analyse", "Aucun aliment détecté.", "OK");
                return;
            }

            // On récupère tous les items (1 seul par group)
            var items = resultat.Results
                .SelectMany(r => r.Items)
                .ToList();

            // Navigation vers la page des résultats
            await Navigation.PushAsync(new ResultatsPage(items));

            /*string message = "Aliments détectés :\n\n";

            foreach (var result in resultat.Results)
            {
                foreach(var item in result.Items)
                {
                    message += $"Name: {item.name} Group: {item.group}, Score: {item.score})\n";
                }
            }

            await DisplayAlert("Résultat", message, "OK");*/
        }
        catch (FeatureNotSupportedException)
        {
            await DisplayAlert("Erreur", "Caméra non supportée sur cet appareil.", "OK");
        }
        catch (PermissionException)
        {
            await DisplayAlert("Erreur", "Permission caméra refusée.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Erreur", $"Une erreur est survenue : {ex.Message}", "OK");
        }
    }
}
