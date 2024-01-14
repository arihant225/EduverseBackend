using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eduverse.Backend.Entity.Functionality
{

    public class PasswordGenerator
    {
        private static Random random = new Random();

        public static string GeneratePassword(int length = 12)
        {
            string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string smallLetters = "abcdefghijklmnopqrstuvwxyz";
            string digits = "0123456789";
            string specialChars = "!@#$%^&*()_+-=[]{}|;:,.<>?";

            string allChars = capitalLetters + smallLetters + digits + specialChars;

            StringBuilder password = new StringBuilder(length);
            password.Append(capitalLetters[random.Next(capitalLetters.Length)]);
            password.Append(smallLetters[random.Next(smallLetters.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the generated password characters
            string shuffledPassword = ShufflePassword(password.ToString());

            return shuffledPassword;
        }

        private static string ShufflePassword(string password)
        {
            char[] passwordArray = password.ToCharArray();
            int n = passwordArray.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                var value = passwordArray[k];
                passwordArray[k] = passwordArray[n];
                passwordArray[n] = value;
            }
            return new string(passwordArray);
        }
    }

}
