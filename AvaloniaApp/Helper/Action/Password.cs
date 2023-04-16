using System.Linq;

namespace AvaloniaApp.Helper.Action;

public class Password
{
    internal static bool Check(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsNumber) &&
               password.Any(char.IsSymbol);
    }
}