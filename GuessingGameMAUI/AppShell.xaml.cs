namespace GuessingGameMAUI;

public partial class AppShell : Shell
{
    
    public AppShell()
	{
		InitializeComponent();


        //Register all the routes
        Routing.RegisterRoute("login", typeof(MainPage));
        Routing.RegisterRoute("menu", typeof(Menu));    
        Routing.RegisterRoute("home", typeof(SettingsPage));
        Routing.RegisterRoute("information", typeof(InformationPage));
       

    }

    
}
