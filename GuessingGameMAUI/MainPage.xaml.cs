using GuessingGameMAUI.Services;
using GuessingGameMAUI.Models;


namespace GuessingGameMAUI;

public partial class MainPage : ContentPage
{

	private string username;

	private readonly HttpClient client;

	private readonly ClientSetup setup = new();

	public MainPage()
	{
		InitializeComponent();
		client = setup.GetClient();
        
	}



    private async void LoginAsync(object sender, EventArgs e)
    {
        bool register = false;
        username = TextEditor.Text;
        string displayMessage = UsernameCheck.ValidateUsername(username);
       
        if (displayMessage == "Checking our database...")
        {
            Model result = await ValidateUsername(client, username, register);
            displayMessage = result.Message;
            await DisplayAlert("Alert", displayMessage, "OK");
            if (displayMessage == $"Welcome back {username}! May the forces be with you...\n")
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

            // add condition for when there is no connection for both endpoints
        }
        var result = await Deserialize.GetResult<Model>(message);
        return result;

    }

    private void QuitAsync(object sender, EventArgs e)
    {
        Application.Current?.CloseWindow(Application.Current.MainPage.Window);
    }

    private async void HereTapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new RegistrationPage());
    }
}
