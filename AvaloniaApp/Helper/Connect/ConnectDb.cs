using AvaloniaApp.Models;


namespace AvaloniaApp.Helper.Connect;

public class ConnectDb
{
    public static ProfUser2Context connect { get; set; } = new ProfUser2Context();
}