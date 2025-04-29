using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGuessr
{
    internal class MultiplayerGame : Game
    {
        public MultiplayerGame(string word) : base()
        {
            Difficulty = DifficultyType.Custom;
            ActiveWord = new Word(word);
            Type = "Multiplayer";
        }
    }
}
