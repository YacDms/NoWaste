namespace NoWaiste
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CameraPage), typeof(CameraPage));
            Routing.RegisterRoute(nameof(AnalysePage), typeof(AnalysePage));
            Routing.RegisterRoute(nameof(RecettePage), typeof(RecettePage));
            Routing.RegisterRoute(nameof(ResultatsPage), typeof(ResultatsPage));
        }
    }
}
