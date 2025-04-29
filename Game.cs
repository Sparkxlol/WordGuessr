using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WordGuessr
{
    public enum DifficultyType
    {
        Easy,
        Medium,
        Hard,
        Custom
    }

    // Plan to inherit from: TimedGame, CustomGame, etc.
    abstract internal class Game
    {
        public Word ActiveWord { get; protected set; }
        public virtual string Type { get; protected set; } = "Default";
        public DifficultyType Difficulty { get; protected set; }
        public int Round { get; protected set; } = 0;
        public bool Complete { get; protected set; } = false;
        public bool Victory { get; protected set; } = false;
        public DateTime CreationDate { get; private set; }

        public const int TotalRounds = 6;

        // ActiveWord should always be assigned in subclasses.
        protected Game()
        {
            ActiveWord = new Word("placeholder");
            CreationDate = DateTime.Now;
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

            Complete = roundCompleted || Round == TotalRounds - 1;
            Victory = roundCompleted;
            Round++;

            return comparison;
        }

        // Going to need to implement this better.
        public static string ChooseWordFromDifficulty(DifficultyType difficulty)
        {
            string[] easyWords = { "frogs", "heard", "place", "blimp" };
            string[] mediumWords = { "gauge", "loops", "mauve", "throw" };
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
                default:
                    return "";
            }
        }
    }
}
