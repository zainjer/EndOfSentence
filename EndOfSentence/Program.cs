using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EndOfSentence
{
    class Program
    {
        static void Main(string[] args)
        {
            var corpus = GetCorpus();
            var rules = GetRules();
            var result = Tokenize(corpus, rules);
            PrintSentences(result);
        }

        static List<string> Tokenize(string corpus, List<Func<string,List<string>>> rules)
        {
            var rulesLength = rules.Count;

            var sentences = new List<string>() {corpus};
            for (int i = 0; i < rulesLength; i++)
            {
                var tempList = new List<string>();
                foreach (var text in sentences)
                {
                    var splitSentences = rules[i].Invoke(text);
                    tempList.AddRange(splitSentences);
                }
                sentences = tempList;
            }

            return sentences;
        }

        
        private static List<Func<string, List<string>>> GetRules()
        {
            var rules = RulesContainer.ExistingRules;
            return rules;
        }
        
        private static void PrintSentences(List<string> result)
        {
            Console.WriteLine("---------Split Sentences [Without Sequencing]--------\n");
            result.ForEach(Console.WriteLine);
            Console.Read();
        }
        
        static string GetCorpus()
        {
           var text =  File.ReadAllText("corpus.txt");
           text = text.Replace(Environment.NewLine, " ");
           return text;
        }
    }
}