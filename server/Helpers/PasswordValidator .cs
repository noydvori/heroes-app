using System.Text.RegularExpressions;

public static class PasswordValidator
{
    public static bool IsValid(string password)
    {
        if (password.Length < 8)
            return false;

        bool hasUpper = Regex.IsMatch(password, @"[A-Z]");
        bool hasDigit = Regex.IsMatch(password, @"\d");
        bool hasSpecial = Regex.IsMatch(password, @"[^a-zA-Z0-9]");

        return hasUpper && hasDigit && hasSpecial;
    }
}
