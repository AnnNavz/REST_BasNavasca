
using REST_BasNavasca.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REST_BasNavasca.MVVM.ViewModels
{
	public class MainViewModel : INotifyPropertyChanged
	{
        public static MainViewModel Instance { get; } = new MainViewModel();

        public int TotalRenters => RentersList.Count;

		HttpClient client;
		JsonSerializerOptions _serializerOptions;
		string baseUrl = "https://69a95a0932e2d46caf460630.mockapi.io";

		public ObservableCollection<Renters> RentersList { get; set; } = new ObservableCollection<Renters>();
		public ObservableCollection<Renters> TrashbinList { get; set; } = new ObservableCollection<Renters>();

        public MainViewModel()
		{
			client = new HttpClient();
			_serializerOptions = new JsonSerializerOptions { WriteIndented = true };
			loadUsers();
		}

        public async void loadUsers()
        {
            var url = $"{baseUrl}/api/v1/vehicles/vehiclerental";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<ObservableCollection<Renters>>(json, _serializerOptions);

                if (data != null)
                {
                    RentersList.Clear();
                    TrashbinList.Clear();

                    foreach (var renter in data)
                    {
                        if (renter.Status)
                            RentersList.Add(renter);
                        else
                            TrashbinList.Add(renter);
                    }
                    OnPropertyChanged(nameof(TotalRenters));
                }
            }
        }


        public string NewRenterName { get; set; }
		public string NewContactInfo { get; set; }
		public DateTime NewDate { get; set; } = DateTime.Now;
		public string NewAddress { get; set; }
		public string NewVehicleType { get; set; }

		private string _newProfile = "addprofile.png";
		public string NewProfile
		{
			get => _newProfile;
			set
			{
				_newProfile = value;
				OnPropertyChanged();
			}
		}

		public ICommand PickImageCommand => new Command(async () =>
		{
			try
			{
				var result = await FilePicker.Default.PickAsync(new PickOptions
				{
					PickerTitle = "Select a PNG Photo",
					FileTypes = FilePickerFileType.Png
				});

				if (result != null)
				{
					NewProfile = result.FullPath;

					using var stream = await result.OpenReadAsync();
					using var memoryStream = new MemoryStream();
					await stream.CopyToAsync(memoryStream);
					byte[] imageBytes = memoryStream.ToArray();

					string base64String = Convert.ToBase64String(imageBytes);

				}
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		});

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		public ICommand AddRenterCommand =>
new Command(async () =>
{
	if (string.IsNullOrWhiteSpace(NewRenterName) ||
		string.IsNullOrWhiteSpace(NewContactInfo) ||
		string.IsNullOrWhiteSpace(NewAddress) ||
		string.IsNullOrWhiteSpace(NewVehicleType))
	{
		await Application.Current.MainPage.DisplayAlert("Validation Error", "All fields are required. Please fill in all information.", "OK");
		return;
	}

	if (!NewContactInfo.All(char.IsDigit))
	{
		await Application.Current.MainPage.DisplayAlert("Invalid Input", "Please enter numbers only for the contact field.", "OK");
		return;
	}

	if (string.IsNullOrEmpty(NewProfile))
	{
		await Application.Current.MainPage.DisplayAlert("Photo Required", "Please select a profile photo for the renter.", "OK");
		return;
	}

	var url = $"{baseUrl}/api/v1/vehicles/vehiclerental";

	var newRenter = new Renters
	{
		Name = NewRenterName,
		Contact = NewContactInfo,
		Date = NewDate,
		Address = NewAddress,
		VehicleModel = NewVehicleType,
		Profile = NewProfile,Status = true
    };

	string json = JsonSerializer.Serialize(newRenter, _serializerOptions);
	var content = new StringContent(json, Encoding.UTF8, "application/json");

	var response = await client.PostAsync(url, content);

	if (response.IsSuccessStatusCode)
	{
		await Application.Current.MainPage.DisplayAlert("Success", "Renter added successfully!", "OK");

		await Application.Current.MainPage.Navigation.PopAsync();

		loadUsers();
	}
	else
	{
		await Application.Current.MainPage.DisplayAlert("Error", "Failed to add renter.", "OK");
	}
});


        public ICommand DeleteRenterCommand => new Command<Renters>(async (renter) =>
        {
            if (renter == null) return;

            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Move to Trash",
                $"Do you want to move {renter.Name} to the trash bin?",
                "Move",
                "Cancel");

            if (answer)
            {
                renter.Status = false;

                try
                {
                    var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{renter.id}";
                    string json = JsonSerializer.Serialize(renter, _serializerOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        RentersList.Remove(renter);
                        TrashbinList.Add(renter);

                        await Application.Current.MainPage.DisplayAlert("Success", "Moved to Trash Bin", "OK");
                        OnPropertyChanged(nameof(TotalRenters));
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Failed to update status on server.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        });



        public ICommand RestoreCommand => new Command<Renters>(async (renter) =>
        {
            if (renter == null) return;

            renter.Status = true;

            try
            {
                var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{renter.id}";
                string json = JsonSerializer.Serialize(renter, _serializerOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PutAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    TrashbinList.Remove(renter);
                    RentersList.Add(renter);

                    await Application.Current.MainPage.DisplayAlert("Restored", $"{renter.Name} is now active.", "OK");
                    OnPropertyChanged(nameof(TotalRenters));
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        });

        public ICommand PermanentDeleteCommand => new Command<Renters>(async (renter) =>
        {
            if (renter == null) return;

            bool answer = await Application.Current.MainPage.DisplayAlert(
                "Permanent Delete",
                $"This will permanently remove {renter.Name} from the database. This action cannot be undone.",
                "Delete Forever",
                "Cancel");

            if (answer)
            {
                try
                {
                    var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{renter.id}";
                    var response = await client.DeleteAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        TrashbinList.Remove(renter);
                        await Application.Current.MainPage.DisplayAlert("Deleted", "Record permanently removed from server.", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Server failed to delete the record.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
        });
    }
}