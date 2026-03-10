using REST_BasNavasca.MVVM.Views;

namespace REST_BasNavasca
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainView());

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            Window window = base.CreateWindow(activationState);
            window.Width = 420;
            window.Height = 800;

            return window;

        }
    }
}