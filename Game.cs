using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGuessr
{
    public enum DifficultyType
    {
        Easy,
        Medium,
        Hard
    }

    // Plan to inherit from: TimedGame, CustomGame, etc.
    internal class Game
    {
        public Word ActiveWord { get; private set; }
        public virtual string Type { get; private set; } = "Default";
        public DifficultyType Difficulty { get; private set; }
        public int Round { get; private set; } = 0;
        public bool Complete { get; private set; } = false;

        public Game(DifficultyType difficulty)
        {
            Difficulty = difficulty;
            ActiveWord = new Word(ChooseWordFromDifficulty(Difficulty)); 
        }

        // Returns null if word not same length.
        public virtual LetterType[]? PlayRound(string word)
        {
            if (word.Length != ActiveWord.ChosenWord.Length || Complete)
                return null;

            LetterType[] comparison = ActiveWord.CompareWords(word);
            bool roundCompleted = true;

            foreach (LetterType lt in comparison)
            {
                if (lt != LetterType.Green)
                {
                    roundCompleted = false;
                }
            }

            Complete = roundCompleted || Round == 5;
            Round++;

            return comparison;
        }

        // Going to need to implement this better.
        public static string ChooseWordFromDifficulty(DifficultyType difficulty)
        {
            string[] easyWords = { "frogs", "heard", "place", "throw" };
            string[] mediumWords = { "gauge", "loops", "mauve", "blimp" };
            string[] hardWords = { "atoll", "epoxy", "zebra", "mummy", "ionic" };

            Random random = new Random();

            switch (difficulty)
            {
                case DifficultyType.Easy:
                    return easyWords[random.NextInt64() % easyWords.Length];
                case DifficultyType.Medium:
                    return mediumWords[random.NextInt64() % mediumWords.Length];
                case DifficultyType.Hard:
                    return hardWords[random.NextInt64() % hardWords.Length];
            }

            return "";
        }
    }
}
