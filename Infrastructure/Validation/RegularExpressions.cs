using System.Text.RegularExpressions;

namespace BibliotecaApp.Infrastructure.Validation;

internal static class RegularExpressions
{
    private static readonly Regex Email = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled);

    private static readonly Regex Cep = new(
        @"^\d{5}-\d{3}$",
        RegexOptions.Compiled);

    private static readonly Regex Isbn = new(
        @"^(97(8|9))?\d{9}(\d|X)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public static bool IsValidEmail(string? email)
    {
        return !string.IsNullOrWhiteSpace(email) && Email.IsMatch(email);
    }

    public static bool IsValidCep(string? cep)
    {
        return !string.IsNullOrWhiteSpace(cep) && Cep.IsMatch(cep);
    }

    public static bool IsValidIsbn(string? isbn)
    {
        return !string.IsNullOrWhiteSpace(isbn) && Isbn.IsMatch(isbn);
    }
}
