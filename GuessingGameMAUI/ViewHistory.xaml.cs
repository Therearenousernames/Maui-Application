
using GuessingGameMAUI.Models;
using GuessingGameMAUI.Services;

namespace GuessingGameMAUI;

public partial class ViewHistory : ContentPage
{

	private readonly Model model;

	private readonly ClientSetup clientSetup = new();

	private readonly HttpClient client;

	public ViewHistory(Model result)
	{
		InitializeComponent();
		client = clientSetup.GetClient();
		model = result;
		GetUserHistory(client);
	}

    private async void QuitAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Menu(model.Username));
    }

	private async void GetUserHistory(HttpClient client)
	{
		string username = model.Username; ;
		HttpResponseMessage message = await client.GetAsync($"{client.BaseAddress}/history/{username}");
		Model result = await Deserialize.GetResult<Model>(message);

		UserHistoryTitle.Text = $"Game history for {username}:";
		List<GameState> gameStates = result.GameStates;
		List<History> histories = new();

		if (gameStates == null || gameStates.Count == 0)
		{
			NoHistory.Text = $"{username} has no game history.";
		} else
		{
			foreach(GameState gameState in gameStates)
			{
				histories.Add(new History { Message = $"Game id: {gameState.Id}  Lost: {gameState.WonLoss}  Number to guess: {gameState.NumberToGuess}  Remaining tries: {gameState.Tries}" });
			}
			BindingContext = new ConvertHistoryToXAML { Message = result.Message, Data = histories };
		}
	}

    private async void DeletUserHistoryAsync(object sender, EventArgs e)
    {
		bool answer = await DisplayAlert("Question?", "Are you sure?", "Yes", "No");
		if (answer == true)
		{
            string username = model.Username;
            HttpResponseMessage httpResponse = await client.GetAsync($"{client.BaseAddress}/deletehistory/{username}");
            Model result = await Deserialize.GetResult<Model>(httpResponse);
            await DisplayAlert("Alert", result.Message, "OK");
            DeleteUserHistory.IsVisible = false;
            HistoryCollection.IsVisible = false;
            NoHistory.Text = $"{username} has no game history.";

        } else
		{
			HistoryCollection.IsVisible = true;

		}

        


    }
}