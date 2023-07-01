
using GuessingGameMAUI.Models;
using System.Net.Http.Headers;
using System.Text.Json;


namespace GuessingGameMAUI;

public partial class GamePlay : ContentPage
{

    private HttpClient client = new();
   
    private string username;

    private Model _result;

    private void ClientSetup(HttpClient client)
    {
        client.BaseAddress = new Uri("http://localhost:5053/api/data");
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
        client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");
    }

    public GamePlay()
    {

        InitializeComponent();
        ClientSetup(client);

        // Hiding the start button
        Guess.IsVisible = false;
        Submit.IsVisible = false;
        Menu.IsVisible = false;
        StartGame.IsVisible = false;
        History.IsVisible = false;
        DeleteUser.IsVisible = false;
        DeleteUserHistory.IsVisible = false;
        GameInformation.IsVisible = false;
    }


    // Login phase of the application
    public void LoginPhase()
    {
        TextEditor.IsVisible = true;
        RegisterButton.IsVisible = true;
        LoginButton.IsVisible = true;

        // Hiding other information
        Guess.IsVisible = false;
        Submit.IsVisible = false;
        Menu.IsVisible = false;
        StartGame.IsVisible = false;
        History.IsVisible = false;
        DeleteUser.IsVisible = false;
        DeleteUserHistory.IsVisible = false;
        GameInformation.IsVisible = false;
    }

    // Menu phase of the app
    public void PhaseOneDone()
    {
        TextEditor.IsVisible = false;
        RegisterButton.IsVisible = false;
        LoginButton.IsVisible = false;

        // Turning on the start button after successful login
        Menu.IsVisible = true;
        StartGame.IsVisible = true;
        History.IsVisible = true;
        DeleteUser.IsVisible = true;
        DeleteUserHistory.IsVisible = true;

    }

    // Play game phase of the app
    public void PlayPhase()
    {

        GameInformation.IsVisible = true;
        Guess.IsVisible = true;
        Submit.IsVisible = true;

        Menu.IsVisible = false;
        StartGame.IsVisible = false;
        History.IsVisible = false;
        DeleteUser.IsVisible = false;
        DeleteUserHistory.IsVisible = false;
       
    }


    // History game phase of the app
    public void HistoryPhase()
    {
        Menu.IsVisible = false;
        GameInformation.IsVisible = false;
        StartGame.IsVisible=false;
        History.IsVisible=false;
        DeleteUser.IsVisible=false;
        DeleteUserHistory.IsVisible = false;

    }


    // Deleting history phase of the app
    public void DeleteHistoryPhase()
    {
        Menu.IsVisible = false;
        GameInformation.IsVisible = false;
        StartGame.IsVisible = false;
        History.IsVisible = false;
        DeleteUser.IsVisible = false;
        DeleteUserHistory.IsVisible = false;
    }

    // Navigates back to the menu phase of the application
    private void QuitAsync(object sender, EventArgs e)
    {
        // Menu visible
        UserHistoryTitle.IsVisible = false;
        NoHistory.IsVisible = false;
        HistoryCollection.IsVisible = false;
        DeletedMessage.IsVisible = false;
        Guess.IsVisible=false;
        Submit.IsVisible=false;
        GameInformation.IsVisible=false;
        GuessResults.IsVisible=false;
        GameStats.IsVisible=false;

        Menu.IsVisible = true;
        StartGame.IsVisible = true;
        History.IsVisible = true;
        DeleteUser.IsVisible = true;
        DeleteUserHistory.IsVisible = true;

        // Add logic to go back to login page
    }


    // Delete user phase of the app
    public void DeleteUserPhase()
    {
        GameInformation.IsVisible = false;
        StartGame.IsVisible = false;
        History.IsVisible = false;
        DeleteUser.IsVisible = false;
        DeleteUserHistory.IsVisible = false;
    }


    private string UsernameCheck(string username)
    {
        List<char> chars = new()
        {
            '{', '}', '|', '^', '~', '[', ']', '`', '/'
        };
        if (string.IsNullOrEmpty(username))
        {
            return "Username cannot be null or empty";
        }
        else
        {
            foreach (char c in chars)
            {
                if (username.Contains(c))
                {
                    return "Username cannot contain illegal characters such as '{', '}', '|', '^', '~', '[', ']', '`'.";
                }
            }
        }
        return "Checking our database...";
    }
    public async void LoginAsync(object sender, EventArgs e)
    {
        bool register = false;
        username = TextEditor.Text;
        string displayMessage = UsernameCheck(username);
        if (displayMessage == "Checking our database...")
        {
            Model result = await ValidateUserName(client, username, register);
            displayMessage = result.Message;

            if (displayMessage == $"Found your username {username}! May the forces be with you...\n")
            {
                // Login phase is complete
                PhaseOneDone();
            }

        }
        await DisplayAlert("Alert", displayMessage, "OK");

    }

   

    public async void RegisterAsync(object sender, EventArgs e)
    {
        bool register = true;
        string username = TextEditor.Text;
        string displayMessage = UsernameCheck(username);
        if (displayMessage == "Checking our database...")
        {
            Model result = await ValidateUserName(client, username, register);
            displayMessage = result.Message;
            if (displayMessage == $"Username {username} has been registered.")
            {
                // Login phase is complete
                PhaseOneDone();
            }
            else if (displayMessage == $"{username} already exists. Please try again.")
            {
                // clears user input
                TextEditor.Text = "";
            }
        }
        await DisplayAlert("Alert", displayMessage, "OK");

    }

    private async void HistoryAsync(object sender, EventArgs e)
    {
        HistoryPhase();
        Model result = await GetUserHistory(client);
        UserHistoryTitle.Text = $"Game history for {username}:";
        List<GameState> gameStates = result.GameStates;
        List<History> userHistoryMessages = new();
           
        if (gameStates == null || gameStates.Count == 0)
        {
            NoHistory.Text = $"{username} has no game history.";
        } else
        {
            foreach(GameState gameState in gameStates)
            {
                userHistoryMessages.Add(new History { Message = $"Game Id: {gameState.Id}  Number to guess: {gameState.NumberToGuess}  Won/Lost: {gameState.WonLoss}  Remaining Tries: {gameState.Tries}" }); 
            }
            BindingContext = new ConvertHistoryToXAML { Message = result.Message, Data = userHistoryMessages };
        }
    }

 

    private async Task<Model> DeleteHistory(HttpClient client)
    {
        DeleteHistoryPhase();
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/deletehistory/{username}");
        Model result = await GetResult<Model>(message);
        return result;
        
    }

   

    private async void DeletUserHistoryAsync(object sender, EventArgs e)
    {
        Model result = await DeleteHistory(client);

        // DeletedMessage switches on for all values related to deleting
        DeletedMessage.Text = result.Message;

    }

    private async Task<Model> DeleteUserAndHistory(HttpClient client)
    {
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/deleteusernandhistory/{username}");
        return await GetResult<Model>(message);
    }

    private async void DeletUserAsync(object sender, EventArgs e)
    {
        Model result = await DeleteUserAndHistory(client);

        // DeletedMessage switches on for all values related to deleting
        DeletedMessage.Text= result.Message;
        LoginPhase();
        
    }

    private async Task<Model> GetUserHistory(HttpClient client)
    {
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/history/{username}");
        Model result = await GetResult<Model>(message);
        return result;

    }

    public void WinOrLosePhase()
    {
        Guess.IsVisible = false;
        Submit.IsVisible = false;
    }



    public async void StartAsync(object sender, EventArgs e)
    {
        _result = await Start(client);
        string displayMessage = _result.Message;
        GameInformation.Text = displayMessage;
        PlayPhase();
    }

    private async void SubmitGuess(object sender, EventArgs e)
    {
        int wonGames = 0; int lostGames = 0;
        string guess = Guess.Text;
        string validatedGuess = ValidateGuess(guess);
        if (validatedGuess == guess)
        {

            Model model= await Guessing(client, _result, validatedGuess);
            if (model.Playing == false)
            {
                if (model.Lost)
                {
                    lostGames++;
                }
                else
                {
                    wonGames++;
                }

                WinOrLosePhase();
                GuessResults.Text = model.Message;
                GameStats.Text = $"Player stats: You won {wonGames} game and lost {lostGames} games. With a total of {wonGames + lostGames} games played.";


            } else
            {
                GuessResults.Text = model.Message;
            }
            
           
        } else
        {
           
            await DisplayAlert("Alert", validatedGuess, "OK");
        }
        Guess.Text = "";


    }

    private string ValidateGuess(string guess) 
    {
        int num; 
        try
        {
            num = int.Parse(guess);
            if (num > 0 & num < 21)
            {
                return guess;
            } else
            {
                return "Please enter a digit between the specified range of 1 to 20.";
            }
           
        } catch (Exception)
        {
            return "Please enter a digit.";
        }
     
    }
    
    
    public async Task<Model> Guessing (HttpClient client, Model result, string guess)
    {
       
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/{result.Username}/{result.Id}/{guess}");
        Model model = await GetResult<Model> (message);
        return model;
    }
    

    private async Task<Model> Start(HttpClient client)
    {
        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/start/{username}");
        Model result = await GetResult<Model>(message);
        return result;
       
    }



    private async Task<Model> ValidateUserName(HttpClient client, string username, bool register)
    {
        HttpResponseMessage message;
        if  (register)
        {
            message = await client.GetAsync($"{client.BaseAddress}/register/{username}");
        } else
        {
            message = await client.GetAsync($"{client.BaseAddress}/{username}");
        }
        var result = await GetResult<Model>(message);
        return result;
       
    }


  
    

    // Deserializes 
    static async Task<T> GetResult<T>(HttpResponseMessage message)
    {
        var content = await message.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var result = JsonSerializer.Deserialize<T>(content, options);
        return result;
    }


}
