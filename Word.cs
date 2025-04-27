using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordGuessr
{
    internal enum LetterType
    {
        Green,
        Yellow,
        Gray
    }

    internal class Word
    {
        public string ChosenWord { get; set; }
        private Dictionary<char, int> letterCount = new Dictionary<char, int>();

        public Word(string word)
        {
            ChosenWord = word;

            foreach (char c in ChosenWord)
            {
                letterCount[c] = letterCount.GetValueOrDefault(c) + 1;
            }
        }

        public LetterType[] CompareWords(string word)
        {
            LetterType[] letterComparison = new LetterType[word.Length];
            Dictionary<char, int> remainingLetters = new Dictionary<char, int>();

            // Check for greens before yellows.
            for (int i = 0; i < word.Length; i++)
            {
                if (ChosenWord[i] == word[i])
                {
                    letterComparison[i] = LetterType.Green;
                }
                // Count non-green letters in givenWord.
                else
                {
                    remainingLetters[ChosenWord[i]] = remainingLetters.GetValueOrDefault(ChosenWord[i]) + 1;
                }
            }

            // Check for yellows.
            for (int i = 0; i < word.Length; i++)
            {
                if (ChosenWord[i] != word[i])
                {
                    // If there are remaining letters in givenWord equal to ChosenWord[i]
                    if (remainingLetters.GetValueOrDefault(word[i]) > 0)
                    {
                        letterComparison[i] = LetterType.Yellow;
                        remainingLetters[word[i]]--;
                    }
                    else
                    {
                        letterComparison[i] = LetterType.Gray;
                    }
                }
            }

            return letterComparison;
        }

        public string ComparisonToString(string word)
        {
            LetterType[] comparison = CompareWords(word);
            StringBuilder currentString = new StringBuilder("");

            for (int i = 0; i < comparison.Length; i++)
            {
                currentString.Append($"{word[i]}: {Enum.GetName(typeof(LetterType), comparison[i])} ");
            }

            return currentString.ToString();
        }
    }
}
