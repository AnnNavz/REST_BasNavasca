namespace REST_BasNavasca.MVVM.Views;
using REST_BasNavasca.MVVM.ViewModels;
using REST_BasNavasca.MVVM.Models;
public partial class MainView : ContentPage
{
	public MainView()
	{
		InitializeComponent();
        BindingContext = MainViewModel.Instance;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {

        await Navigation.PushAsync(new CreateView());
    }

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		var border = sender as Border;

		if (border?.BindingContext is Renters selectedRenter)
		{
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

    private async void trashBin_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new TrashbinView());
    }
}