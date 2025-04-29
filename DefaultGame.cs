using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGuessr
{
    internal class DefaultGame : Game
    {
        public DefaultGame(DifficultyType difficulty) : base()
        {
            Difficulty = difficulty;
            ActiveWord = new Word(ChooseWordFromDifficulty(difficulty));
        }
    }
}
