namespace REST_BasNavasca.MVVM.Views;
using REST_BasNavasca.MVVM.ViewModels;

public partial class TrashbinView : ContentPage
{
	public TrashbinView()
	{
		InitializeComponent();
        BindingContext = MainViewModel.Instance;
    }
}