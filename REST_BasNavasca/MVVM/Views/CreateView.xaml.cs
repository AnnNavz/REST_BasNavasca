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

			}
		}
		catch (Exception ex)
		{
			await DisplayAlert("Error", "Could not load image: " + ex.Message, "OK");
		}
	}
}