using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGameMAUI.Models
{
    public class GameState
    {
        public Guid Id { get; set; }

        public bool WonLoss { get; set; }   

        public int Tries { get; set; }

        public string Username { get; set; }

        public int NumberToGuess { get; set; }  
    }
}
