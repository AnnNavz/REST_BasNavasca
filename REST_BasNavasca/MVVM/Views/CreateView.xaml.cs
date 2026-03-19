namespace REST_BasNavasca.MVVM.Views;
using REST_BasNavasca.MVVM.ViewModels;

public partial class CreateView : ContentPage
{
	public CreateView()
	{
		InitializeComponent();
        BindingContext = new MainViewModel();
    }

	private async void addProfile_Clicked(object sender, EventArgs e)
	{
		try
		{
			// Define options to specifically allow PNG images
			var options = new PickOptions
			{
				PickerTitle = "Please select a profile photo",
				FileTypes = FilePickerFileType.Png
			};

			var result = await FilePicker.Default.PickAsync(options);

			if (result != null)
			{
				var stream = await result.OpenReadAsync();
				ImageProfile.Source = ImageSource.FromStream(() => stream);

				// Optional: You can save the result.FullPath to your database/ViewModel
			}
		}
		catch (Exception ex)
		{
			// Handle picking cancelled or other errors
			await DisplayAlert("Error", "Could not load image: " + ex.Message, "OK");
		}
	}
}