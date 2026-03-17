namespace REST_BasNavasca.MVVM.Views;

public partial class User : ContentPage
{
	public User()
	{
		InitializeComponent();
	}

    private void Update_Clicked(object sender, EventArgs e)
    {
		NameEntry.IsEnabled = true;
		ContactEntry.IsEnabled = true;
		DatePicker.IsEnabled = true;
		AddressEntry.IsEnabled = true;
		VehiclePicker.IsVisible = true;
		VehicleEntryData.IsVisible = false;
		SubmitUpdate.IsVisible = true;
		Update.IsVisible = false;
    }
}