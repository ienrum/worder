using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FiveLetterWordsList
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
            while (letterOverlapCount(targetList[index], prevString) <= 3 || isOverlapWordInList(result,prevString))
            {
                index = Random.Range(0, n);
                prevString = targetList[index];
            }
            result.Add(targetList[index]);
        }

        Debug.Log(string.Join(", ", result));
        return result;

    }

    static int letterOverlapCount(string a, string b)
    {
        return a.Count(c => b.Contains(c));
    }

    static bool isOverlapWordInList(List<string> list, string word)
    {
        return list.Contains(word);
    }
}
