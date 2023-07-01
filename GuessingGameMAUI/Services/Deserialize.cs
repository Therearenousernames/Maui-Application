using GuessingGameMAUI.Models;
using System.Text.Json;

namespace GuessingGameMAUI.Services
{
    public class Deserialize
    {

        // Deserializes 
        public static async Task<T> GetResult<T>(HttpResponseMessage message)
        {
            var content = await message.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var result = JsonSerializer.Deserialize<T>(content, options);
            return result;
        }
    }
}
