using System.Net.Mail;

namespace TraineeManagement.api.Helper
{
    public static class StringValidator
    {
        public static bool IsOnlyLetters(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsLetter);
        }

        public static bool IsValidEmail(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            return MailAddress.TryCreate(input, out _);
        }
    }
}
