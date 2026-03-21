
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

		public int TotalRenters => RentersList.Count;

		HttpClient client;
        JsonSerializerOptions _serializerOptions;
        string baseUrl = "https://69a95a0932e2d46caf460630.mockapi.io";

        public ObservableCollection<Renters> RentersList { get; set; } = new ObservableCollection<Renters>();

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
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Renters>>(responseStream, _serializerOptions);

                    RentersList.Clear();
                    foreach (var renter in data)
                    {
                        RentersList.Add(renter);
                    }
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
	var url = $"{baseUrl}/api/v1/vehicles/vehiclerental";

	var newRenter = new Renters
	{
		Name = NewRenterName,
		Contact = NewContactInfo,
		Date = NewDate,
		Address = NewAddress,
		VehicleModel = NewVehicleType,
        Profile = NewProfile
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
    "Delete Entry",
    $"Remove {renter.Name} from the list?",
    "Yes",
    "No");

            if (answer)
            {
                var url = $"{baseUrl}/api/v1/vehicles/vehiclerental/{renter.id}";
                var response = await client.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    RentersList.Remove(renter);
                }
            }
        });
    }

}
