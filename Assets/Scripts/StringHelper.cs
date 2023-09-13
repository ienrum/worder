using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    static List<string> fiveLetterWords = new List<string>
    {
        "apple",
        "beach",
        "crane",
        "drama",
        "eagle",
        "flame",
        "grape",
        "haste",
        "input",
        "jolly",
        "knack",
        "latch",
        "mango",
        "nudge",
        "ocean",
        "pride",
        "quilt",
        "raven",
        "sloth",
        "train",
        "urban",
        "vault",
        "waste",
        "xerox",
        "yarns",
        "zebra",
        "brick",
        "clamp",
        "drill",
        "flock",
        "grind",
        "hover",
        "inkle",
        "joust",
        "knoll",
        "lemur",
        "mirth",
        "nymph",
        "ouija",
        "pluck",
        "quire",
        "roast",
        "slush",
        "twine",
        "upend",
        "vexed",
        "whack",
        "yodel",
        "zincs",
        "bluff",
        "creep",
        "dwarf",
        "elope",
        "frost",
        "gleam",
        "hutch",
        "ivory",
        "junks",
        "krill",
        "lunar",
        "mucus",
        "noble",
        "otter",
        "prune",
        "quark",
        "rifle",
        "skunk",
        "tulip",
        "usher",
        "vivid",
        "whale",
        "yacht",
        "zesty",
        "bleed",
        "crisp",
        "douse",
        "fjord",
        "gloom",
        "humid",
        "inert",
        "jiffy",
        "koala",
        "liver",
        "motel",
        "nifty",
        "orbit",
        "pinky",
        "quash",
        "ruler",
        "swoop",
        "twigy",
        "unzip",
        "vowel",
        "wrack",
        "yikes",
        "zippy"
    };

    static List<string> easeFiveLetterWords = new List<string>
    {
        "table",
        "chair",
        "shirt",
        "apple",
        "water",
        "bread",
        "brush",
        "green",
        "stone",
        "sugar",
        "pound",
        "floor",
        "speak",
        "train",
        "fruit",
        "write",
        "glass",
        "plant",
        "watch",
        "lunch",
        "paper",
        "music",
        "money",
        "happy",
        "drink",
        "taste",
        "clean",
        "sweet",
        "shirt",
        "climb",
        "color",
        "sharp",
        "round",
        "place",
        "paint",
        "quiet",
        "quick",
        "radio",
        "young",
        "print",
        "dream",
        "sunny",
        "sound",
        "clock",
        "brown",
        "voice",
        "break",
        "crash",
        "heavy",
        "dance",
        "smile",
        "phone",
        "white",
        "hurry",
        "photo",
        "light",
        "fresh",
        "clear",
        "piano",
        "carry",
        "sleep",
        "sight",
        "bread",
        "point",
        "laugh",
        "north",
        "black",
        "movie",
        "salty",
        "blank",
        "wrong",
        "wheel",
        "total",
        "screw",
        "chase",
        "shiny",
        "brick",
        "march",
        "cross",
        "piece",
        "store",
        "south",
        "scare",
        "front",
        "fight",
        "stand",
        "still",
        "story",
        "grade",
        "chill",
        "cloud",
        "crazy"
    };
    public static string GetShuffledLetters(List<string> wordDictionary)
    {
        string result = "";
        result = MakeStringListShuffledString(wordDictionary);

        return result;
    }
    public static List<string> getRandomFiveWordsList(bool isHard = false)
    {
        List<string> targetList;
        string prevString = "";
        if (isHard)
        {
            targetList = fiveLetterWords;
        }
        else
        {
            targetList = easeFiveLetterWords;
        }
        int n = targetList.Count;
        List<string> result = new List<string>();
        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, n);
            while (letterOverlapCount(targetList[index], prevString) <= 3 || isOverlapWordInList(result, prevString))
            {
                index = Random.Range(0, n);
                prevString = targetList[index];
            }
            result.Add(targetList[index]);
        }
        return StringListToUpper(result);
    }

    static int letterOverlapCount(string a, string b)
    {
        return a.Count(c => b.Contains(c));
    }

    static bool isOverlapWordInList(List<string> list, string word)
    {
        return list.Contains(word);
    }
    public static string charListToString(List<char> chars)
    {
        return new string(chars.ToArray());
    }

    public static List<char> stringToCharList(string word)
    {
        List<char> chars = new List<char>();
        chars.AddRange(word);
        return chars;
    }
    public static string FormatTime(float seconds)
    {
        int intSeconds = (int)seconds;
        int hours = intSeconds / 3600;
        int minutes = (intSeconds % 3600) / 60;
        int remainingSeconds = intSeconds % 60;

        return string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, remainingSeconds);
    }
    public static List<int> GetAllIndicesOf(string source, string target)
    {
        List<int> result = new List<int>();
        int index = 0;

        while ((index = source.IndexOf(target, index)) != -1)
        {
            result.Add(index);
            index += target.Length;
        }

        return result;
    }
    public static string MakeStringListShuffledString(List<string> wordDictionary)
    {

        List<char> chars;
        chars = StringHelper.stringToCharList(string.Join("", wordDictionary));

        List<char> shuffledCharList = ShuffleList(chars, 100);
        return StringHelper.charListToString(shuffledCharList).ToUpper();
    }
    public static List<string> StringListToUpper(List<string> list)
    {
        List<string> result = new List<string>(list);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] = result[i].ToUpper();
        }
        return result;
    }
    public static List<string> StringListToLower(List<string> list)
    {
        List<string> result = new List<string>(list);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] = result[i].ToLower();
        }
        return result;
    }
    private static List<T> ShuffleList<T>(List<T> list, int shuffleCount)
    {
        int n = list.Count;
        List<T> result = list.Select(t => t).ToList();
        while (shuffleCount > 1)
        {
            shuffleCount--;

            int idx = shuffleCount % n;
            int k = UnityEngine.Random.Range(0, shuffleCount % n);

            T temp = result[idx];
            result[idx] = result[k];
            result[k] = temp;
        }
        return result;
    }
}