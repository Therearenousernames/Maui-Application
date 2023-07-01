using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuessingGameMAUI.Models
{
    public class Model
    {
        public string Message { get; set; }

        public int Id { get; set; }

        public int Tries { get; set; }

        public bool Playing { get; set; }

        public bool Lost { get; set; }

        public string Username { get; set; }

        public List<GameState> GameStates { get; set; }
    }
}
