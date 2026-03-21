namespace REST_BasNavasca.MVVM.Views;
using REST_BasNavasca.MVVM.ViewModels;
using REST_BasNavasca.MVVM.Models;
public partial class MainView : ContentPage
{
	public MainView()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new CreateView());
    }

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var border = sender as Border;

		// The BindingContext of the Border is the specific 'Renters' object for that row
		if (border?.BindingContext is Renters selectedRenter)
		{
			// Pass the object to the User page constructor
			await Navigation.PushAsync(new User(selectedRenter));
		}
	}

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is MainViewModel vm)
		{
			vm.loadUsers();
		}
	}
}