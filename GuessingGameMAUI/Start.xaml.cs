using GuessingGameMAUI.Models;
using GuessingGameMAUI.Services;

namespace GuessingGameMAUI;

public partial class Start : ContentPage
{

	private readonly Model model;

    private readonly HttpClient client;

    private readonly ClientSetup setup = new();

    public Start(Model result)
    {
        InitializeComponent();

        model = result;
        client = setup.GetClient();
        GameInformation.Text = $"";
        DisplayInitialMessage(model);
    }


	private void DisplayInitialMessage(Model model)
	{
		string username = model.Username;
        string properCasing = char.ToUpper(username[0])+username[1..];
        GameInformation.Text = $"{properCasing}, you've been allocated a random number between the range of 1 to 20. You have 5 tries to guess the random generated.";
		
	}

    private static string ValidateGuess(string guess)
    {
        int num;
        try
        {
            num = int.Parse(guess);
            if (num > 0 & num < 21)
            {
                return guess;
            }
            else
            {
                return "Please enter a digit between the specified range of 1 to 20.";
            }

        }
        catch (Exception)
        {
            return "Please enter a digit.";
        }

    }


    private async void SubmitGuess(object sender, EventArgs e)
    {

        int wonGames = 0; int lostGames = 0;
        string guess = Guess.Text;
        string validatedGuess = ValidateGuess(guess);
        if (validatedGuess == guess)
        {

            Model result = await Guessing(client, model, validatedGuess);
            if (result.Playing == false)
            {
                if (model.Lost)
                {
                    lostGames++;
                }
                else
                {
                    wonGames++;
                }

                Guess.IsVisible = false;
                Submit.IsVisible = false;
                GuessResults.Text = result.Message;
                GameStats.Text = $"Player stats: You won {wonGames} game and lost {lostGames} games. With a total of {wonGames + lostGames} games played.";

            }
            else
            {
                GuessResults.Text = result.Message;
            }
        }
        else
        {

            await DisplayAlert("Alert", validatedGuess, "OK");
        }
        Guess.Text = "";
    }


    public static async Task<Model> Guessing(HttpClient client, Model result, string guess)
    {

        HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/{result.Username}/{result.Id}/{guess}");
        Model model = await Deserialize.GetResult<Model>(message);
        return model;
    }

    private async void InformationAsync(object sender, EventArgs e)
    {
        string rules = $"1. Enter a integer between the range of 1 and 20\n2. Submit value\n3. If correct, a message will output text confirming if you have won. Else, continue inputting your guesses until you win or your tries run out.\n\nGoodluck!";
        await DisplayAlert("The rules of the game are as follows:", rules, "OK");
    }

    private async void QuitAsync(object sender, EventArgs e)
    {
        /*HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/exitincompletegame");*/
        await Navigation.PushAsync(new Menu(model.Username));
    }
}