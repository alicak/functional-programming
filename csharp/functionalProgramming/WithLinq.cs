using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace functionalProgramming
{
    public static class WithLinq
    {
        public static void Run()
        {
            var dictionary = File.ReadAllLines(@"../../../linuxwords.txt");
            var words = dictionary.Where(w => w.ToCharArray().All(c => char.IsLetter(c)));

            var mnem = new Dictionary<char, string> {  {'2', "ABC" }, { '3', "DEF"}, { '4', "GHI"}, { '5', "JKL"},
                { '6', "MNO"}, { '7', "PQRS"}, { '8', "TUV"}, { '9', "WXYZ"} };

            var charCode = new Dictionary<char, char>();
            foreach (var (digit, chars) in mnem)
            {
                foreach (var character in chars)
                {
                    charCode.Add(character, digit);
                }
            }

            var wordsForNum = words.GroupBy(w => WordCode(w, charCode)).ToDictionary(g => g.Key, g => g.ToList());

            var result = Encode("7225247386", wordsForNum);

            Console.WriteLine(result.Count);
        }

        private static string WordCode(string word, Dictionary<char, char> charCode)
        {
            return new string(word.ToUpper().ToCharArray().Select(c => charCode[c]).ToArray());
        }

        private static HashSet<List<string>> Encode(string number, Dictionary<string, List<string>> wordsForNum)
        {
            if (string.IsNullOrEmpty(number))
                return new HashSet<List<string>> { new List<string>() };

            else
            {
                var result = new HashSet<List<string>>();
                for (int split = 1; split <= number.Length; split++)
                {
                    if (!wordsForNum.TryGetValue(number.Substring(0, split), out var words))
                        continue;

                    foreach (var word in words)
                    {
                        foreach (var rest in Encode(number.Substring(split), wordsForNum))
                        {
                            var list = new List<string>(rest);
                            list.Insert(0, word);
                            result.Add(list);
                        }
                    }
                }
                return result;
            }
        }
    }
}
