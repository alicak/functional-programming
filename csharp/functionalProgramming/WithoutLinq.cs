using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace functionalProgramming
{
    public static class WithoutLinq
    {
        public static void Run()
        {
            var dictionary = File.ReadAllLines(@"../../../linuxwords.txt");

            var words = new List<string>();

            foreach(var item in dictionary)
            {
                var onlyLetters = true;
                foreach (var character in item)
                {
                    if (!char.IsLetter(character))
                    {
                        onlyLetters = false;
                        break;
                    }
                }
                if (onlyLetters)
                    words.Add(item);
            }

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

            var wordsForNum = new Dictionary<string, List<string>>();

            foreach (var word in words)
            {
                var code = WordCode(word, charCode);
                if (wordsForNum.TryGetValue(code, out var value))
                    value.Add(word);
                else
                    wordsForNum.Add(code, new List<string> { word });
            }

            var result = Encode("7225247386", wordsForNum);

            Console.WriteLine(result.Count);
        }

        private static string WordCode(string word, Dictionary<char, char> charCode)
        {
            var code = new StringBuilder();
            foreach (var c in word.ToUpper())
                code.Append(charCode[c]);
            return code.ToString();
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
