using GuessingGameMAUI.Models;
using GuessingGameMAUI.Services;


namespace GuessingGameMAUI;

public partial class Menu : ContentPage
{

    private readonly string username;

    private readonly HttpClient client = new();

    private readonly ClientSetup setup = new();
    
   

   
   
	public Menu(string user)
	{
		InitializeComponent();
        client = setup.GetClient();
        username = user;
        Preferences.Set("username", user);
       
	}




    private async void StartAsync(object sender, EventArgs e)
    {
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/start/{username}");
        Model model = await Deserialize.GetResult<Model>(message);
        await Navigation.PushAsync(new Start(model));
    }


 

  
    private async void SettingsAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SettingsPage());
    }
  

    private async void QuitAsync(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new MainPage());
    }

  
}