using GuessingGameMAUI.Models;
using GuessingGameMAUI.Services;


namespace GuessingGameMAUI;

public partial class RegistrationPage : ContentPage
{

    private string username;

    private readonly HttpClient client;

    private readonly ClientSetup clientSetup = new();


	public RegistrationPage()
	{
		InitializeComponent();
        client = clientSetup.GetClient();
	}

    private void QuitAsync(object sender, EventArgs e)
    {
        Navigation.PushAsync(new MainPage());
    }

    private async void SumbitAsync(object sender, EventArgs e)
    {
        bool register = true;
        username = TextEditor.Text;
        string displayMessage = UsernameCheck.ValidateUsername(username);

        if (displayMessage == "Checking our database...")
        {
            Model result = await ValidateUsername(client, username, register);
            displayMessage = result.Message;
            await DisplayAlert("Alert", displayMessage, "OK");

            if (displayMessage != $"Username {username} has been registered.")
            {
                TextEditor.Text = "";
            }
            else
            {
                await Navigation.PushAsync(new Menu(username));
            }
        }
    }

    private async static Task<Model> ValidateUsername(HttpClient client, string username, bool register)
    {
        HttpResponseMessage message;
        if (register)
        {
            message = await client.GetAsync($"{client.BaseAddress}/register/{username}");
        }
        else
        {
            message = await client.GetAsync($"{client.BaseAddress}/{username}");
        }
        var result = await Deserialize.GetResult<Model>(message);
        return result;

    }
}