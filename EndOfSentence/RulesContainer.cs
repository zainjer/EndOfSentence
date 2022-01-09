using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace EndOfSentence
{
    public static class RulesContainer
    {
        #region Properties

        public static List<Func<string, List<string>>> ExistingRules => new()
        {
            RuleQuestionMark,
            RuleCapitalAfterPeriod,
        };

        #endregion

        #region Rules Methods

        private static List<string> RuleQuestionMark(string text)
        {
            var result = text.Split('?', StringSplitOptions.RemoveEmptyEntries);
            for (var index = 0; index < result.Length; index++)
            {
                result[index] += '?';
            }

            return result.ToList();
        }

        private static List<string> RuleCapitalAfterPeriod(string text)
        {
            var periodCount = text.GetPeriodCount();
            if (periodCount == 0) return new List<string>() {text};

            var sentences = new List<string>();
            var tempStr = new string(text);
            var currentPeriodIndex = 0;
            for (int i = 0; i < periodCount; i++)
            {
                var periodIndex = GetPeriodIndexByCount(tempStr, currentPeriodIndex);
                var nextLetter = tempStr.Substring(periodIndex+1).GetNextLetter(out int charIndex);
                if (nextLetter.IsUpper() && charIndex>0)
                {
                    var sentence = tempStr.Slice(0, periodIndex+1);
                    sentences.Add(sentence);
                    tempStr = tempStr.Substring(periodIndex + 2);
                    currentPeriodIndex = 0;
                }
                else
                {
                    currentPeriodIndex++;
                }
            }

            return sentences;
        }

        #endregion

        #region Helpers

        private static int GetPeriodIndexByCount(string text, int i)
        {
            var periodIndex = 0;
            var str = text;
            var skippedIndexLen = 0;
            for (int j = 0; j < i; j++)
            {
                var pIndex = str.IndexOf('.');
                skippedIndexLen += pIndex;
                str = str.Substring(pIndex + 1);
            }

            periodIndex = str.IndexOf('.');
            return periodIndex+skippedIndexLen;
        }

        private static char GetNextLetter(this string s,out int index)
        {
             var c = s.FirstOrDefault(char.IsLetter);
             index = s.IndexOf(c);
             return c;
        }

        private static int GetPeriodCount(this string s) => s.Count(x => x == '.');
        private static bool IsUpper(this char c) => char.IsUpper(c);

        private static string Slice(this string s, int startIndex, int endIndex)
        {
            var charArr = new char[endIndex - startIndex + 1];
            for (int i = startIndex; i <= endIndex; i++)
            {
                charArr[i] = s[i];
            }

            return new string(charArr);
        }
        #endregion
    }
}
