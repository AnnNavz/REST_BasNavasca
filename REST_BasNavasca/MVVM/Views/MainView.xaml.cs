namespace REST_BasNavasca.MVVM.Views;

public partial class MainView : ContentPage
{
	public MainView()
	{
		InitializeComponent();
	}

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new CreateView());
    }

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		await Navigation.PushAsync(new User());
	}
}