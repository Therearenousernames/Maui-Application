using GuessingGameMAUI.Services;
using GuessingGameMAUI.Models;

namespace GuessingGameMAUI;

public partial class SettingsPage : ContentPage
{
    private readonly string username;

    private readonly HttpClient client = new();

    private readonly ClientSetup setup = new();
	public SettingsPage()
	{
		InitializeComponent();
        client = setup.GetClient();
        var user = Preferences.Get("username", "default_value");
        username = user;
        Allowances.Text = "These are the options available to you: ";

	}

    private async void QuitAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Menu(username));
    }

    private async void HistoryAsync(object sender, EventArgs e)
    {
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/history/{username}");
        Model result = await Deserialize.GetResult<Model>(message);
        await Navigation.PushAsync(new ViewHistory(result));
    }

    private async void DeletUserAsync(object sender, EventArgs e)
    {
        bool answer = await DisplayAlert("Question?", "Are you sure?", "Yes", "No");
        if (answer == true)
        {
            HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/deleteusernandhistory/{username}");
            Model model = await Deserialize.GetResult<Model>(message);
            string displayMessage = model.Message;
            await DisplayAlert("Alert", displayMessage, "OK");
            await Navigation.PushAsync(new RegistrationPage());
        }

    }
}