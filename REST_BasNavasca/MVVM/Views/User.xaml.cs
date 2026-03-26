using REST_BasNavasca.MVVM.Models;

namespace REST_BasNavasca.MVVM.Views;

public partial class User : ContentPage
{
	private string _temporaryPhotoPath;
	private Renters _currentRenter;
	private HttpClient client = new HttpClient();
	private string baseUrl = "https://69a95a0932e2d46caf460630.mockapi.io";
	public User(Renters renter)
	{
		InitializeComponent();

		_currentRenter = renter;


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

		UserProfileImage.Source = _currentRenter.Profile;
		_temporaryPhotoPath = null;
	}

	private async void SubmitUpdate_Clicked(object sender, EventArgs e)
	{
		var updatedRenter = new Renters
		{
			id = _currentRenter.id, 
			Name = NameEntry.Text,
			Contact = ContactEntry.Text,
			Date = DatePicker.Date,
			Address = AddressEntry.Text,
			VehicleModel = VehiclePicker.IsVisible ? VehiclePicker.SelectedItem?.ToString() : VehicleEntryData.Text,
			Profile = !string.IsNullOrEmpty(_temporaryPhotoPath) ? _temporaryPhotoPath : _currentRenter.Profile
		};

		string json = System.Text.Json.JsonSerializer.Serialize(updatedRenter);
		var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

		var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{_currentRenter.id}";
		var response = await client.PutAsync(url, content);

		if (response.IsSuccessStatusCode)
		{
			await DisplayAlert("Success", "Information updated!", "OK");

			await Navigation.PopAsync();
		}
		else
		{
			await DisplayAlert("Error", "Failed to update record.", "OK");
		}
	}

	private async void ChangeProfilePic_Tapped(object sender, EventArgs e)
	{
		
	}

	private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
	{
		if (!SubmitUpdate.IsVisible) return;

		try
		{
			var result = await FilePicker.Default.PickAsync(new PickOptions
			{
				PickerTitle = "Select a Profile Photo",
				FileTypes = FilePickerFileType.Images
			});

			if (result != null)
			{
				_temporaryPhotoPath = result.FullPath;
				UserProfileImage.Source = _temporaryPhotoPath;
			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", $"Unable to pick image: {ex.Message}", "OK");
		}
	}

    private async void DeleteButton_Clicked(object sender, EventArgs e)
    {
        if (_currentRenter == null) return;

        // Standard confirmation alert as seen in your ViewModel
        bool answer = await DisplayAlert(
            "Delete Entry",
            $"Are you sure you want to permanently remove {_currentRenter.Name}?",
            "Yes",
            "No");

        if (answer)
        {
            try
            {
                // Use the same endpoint logic as your MainViewModel
                var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{_currentRenter.id}";
                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Deleted", "The renter has been removed.", "OK");

                    // Navigate back to the list view
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error", "Failed to delete the record from the server.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}