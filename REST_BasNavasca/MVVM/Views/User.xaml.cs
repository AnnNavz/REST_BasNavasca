using REST_BasNavasca.MVVM.Models;

namespace REST_BasNavasca.MVVM.Views;

public partial class User : ContentPage
{
	public User(Renters renter)
	{
		InitializeComponent();

		if (renter != null)
		{
			NameEntry.Text = renter.Name;
			ContactEntry.Text = renter.Contact;
			DatePicker.Date = renter.Date;
			AddressEntry.Text = renter.Address;
			VehicleEntryData.Text = renter.VehicleModel;
			UserProfileImage.Source = renter.Profile;

		}
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
		CancelUpdate.IsVisible = true;
    }

	private void CancelUpdate_Clicked(object sender, EventArgs e)
	{
		NameEntry.IsEnabled = false;
		ContactEntry.IsEnabled = false;
		DatePicker.IsEnabled = false;
		AddressEntry.IsEnabled = false;
		VehiclePicker.IsVisible = false;
		VehicleEntryData.IsVisible = true;
		SubmitUpdate.IsVisible = false;
		Update.IsVisible = true;
		CancelUpdate.IsVisible = false;
	}
}