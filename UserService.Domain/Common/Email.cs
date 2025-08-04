using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UserService.Application.Common.Models;

namespace UserService.Domain.Common
{
    public class Email
    {


        public string Value { get; set; }


        private Email(string value)
        {
            Value = value;
        }

        public static Result<Email> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Result<Email>.Failure(new List<string>()
                {
                    "Email can't be empty!"
                });

            if (!EsEmailValido(email))
                return Result<Email>.Failure(new List<string>()
                {
                    "Invalid email format. Must be example@domain.com"
                });

            return Result<Email>.Success(new Email(email.Trim().ToLowerInvariant()));

        }

        public static bool EsEmailValido(string email)
        {
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }


        public override string ToString()
        {
            return Value;
        }


        public static implicit operator string(Email email) => email.Value;
    }
}
