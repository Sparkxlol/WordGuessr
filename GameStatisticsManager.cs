using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WordGuessr
{
    static internal class GameStatisticsManager
    {
        // Save:
        //  Type
        //  Word
        //  Date
        //  Rounds
        //  Victory
        //  Difficulty
        static public void SaveGame(Game game, string fileName)
        {
            GameStatistics stats = new GameStatistics(game);

            string serializedJSON = JsonSerializer.Serialize(stats);
            File.WriteAllText(fileName, serializedJSON);
        }
    }

    internal class GameStatistics
    {
        public string Type;
        public string Word;
        public string Difficulty;
        public int Rounds;
        public bool Victory;
        public DateTime CreationDate;

        public GameStatistics(Game game)
        {
            Type = game.Type;
            Word = game.ActiveWord.ChosenWord;
            Difficulty = game.Difficulty.ToString();
            Rounds = game.Round;
            Victory = game.Victory;
            CreationDate = game.CreationDate;
        }
    }
}
