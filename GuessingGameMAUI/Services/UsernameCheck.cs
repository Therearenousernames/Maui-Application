using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGameMAUI.Services
{
    public class UsernameCheck
    {

       public static string ValidateUsername(string username)
        {
            List<char> chars = new()
        {
            '{', '}', '|', '^', '~', '[', ']', '`', '/'
        };
            if (string.IsNullOrEmpty(username))
            {
                return "Username cannot be null or empty";
            }
            else
            {
                foreach (char c in chars)
                {
                    if (username.Contains(c))
                    {
                        return "Username cannot contain illegal characters such as '{', '}', '|', '^', '~', '[', ']', '`'.";
                    }
                }
            }
            return "Checking our database...";
        }
    }
}
