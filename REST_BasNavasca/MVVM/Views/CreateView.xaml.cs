namespace REST_BasNavasca.MVVM.Views;
using REST_BasNavasca.MVVM.ViewModels;

public partial class CreateView : ContentPage
{
	public CreateView()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }
}