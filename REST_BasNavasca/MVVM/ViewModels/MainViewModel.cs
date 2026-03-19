
using REST_BasNavasca.MVVM.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace REST_BasNavasca.MVVM.ViewModels
{
    public class MainViewModel
    {
        HttpClient client;
        JsonSerializerOptions _serializerOptions;
        string baseUrl = "https://69a95a0932e2d46caf460630.mockapi.io";

        // 1. Change to public ObservableCollection
        public ObservableCollection<Renters> RentersList { get; set; } = new ObservableCollection<Renters>();

        public MainViewModel()
        {
            client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions { WriteIndented = true };
            loadUsers();
        }

        private async void loadUsers()
        {
            var url = $"{baseUrl}/api/v1/vehicles/vehiclerental";
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using (var responseStream = await response.Content.ReadAsStreamAsync())
                {
                    var data = await JsonSerializer.DeserializeAsync<ObservableCollection<Renters>>(responseStream, _serializerOptions);

                    // 2. Clear and fill the ObservableCollection
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
        VehicleModel = NewVehicleType
    };

    string json = JsonSerializer.Serialize(newRenter, _serializerOptions);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    var response = await client.PostAsync(url, content);

    if (response.IsSuccessStatusCode)
    {
        loadUsers();
    }
});
    }




}
