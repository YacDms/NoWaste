namespace NoWaiste;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
    }

    private async void OnCameraClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(CameraPage));
    }
}