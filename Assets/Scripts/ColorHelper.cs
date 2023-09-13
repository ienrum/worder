using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorHelper
{
    static private List<List<string>> colorPalettes = new List<List<string>>
    {
        new List<string> { "#F2F7A1", "#35A29F", "#088395", "#071952" },
        new List<string> { "#EDB7ED", "#82A0D8", "#8DDFCB", "#ECEE81" },
        new List<string> { "#22668D", "#8ECDDD", "#FFFADD", "#FFCC70" },
        new List<string> { "#F39F5A", "#AE445A", "#662549", "#451952" },
        new List<string> { "#0F2C59", "#DAC0A3", "#EADBC8", "#F8F0E5" },
        new List<string> { "#E5D283", "#4F709C", "#213555", "#F0F0F0" },
        new List<string> { "#FFC8C8", "#FF9B82", "#FF3FA4", "#57375D" },
        new List<string> { "#E4F1FF", "#AED2FF", "#9400FF", "#27005D" },
        new List<string> { "#A6FF96", "#F8FF95", "#BC7AF9", "#FFA1F5" },
        new List<string> { "#EBEF95", "#EFD595", "#EFB495", "#EF9595" },
        new List<string> { "#EEEEEE", "#64CCC5", "#176B87", "#053B50" },
        new List<string> { "#A2C579", "#D2DE32", "#FFFFDD", "#016A70" },
        new List<string> { "#F2E8C6", "#DAD4B5", "#A73121", "#952323" },
        new List<string> { "#C63D2F", "#E25E3E", "#FF9B50", "#FFBB5C" },
        new List<string> { "#EC53B0", "#9D44C0", "#4D2DB7", "#0E21A0" },
        new List<string> { "#FFEEF4", "#E4E4D0", "#AEC3AE", "#94A684" },
        new List<string> { "#D67BFF", "#FFB6D9", "#FEFFAC", "#45FFCA" },
        new List<string> { "#F2ECBE", "#E2C799", "#C08261", "#9A3B3B" },
        new List<string> { "#CAEDFF", "#D8B4F8", "#FFC7EA", "#FBF0B2" },
        new List<string> { "#F4EEEE", "#FFB7B7", "#FFDBAA", "#96C291" },
        new List<string> { "#FFFD8C", "#97FFF4", "#7091F5", "#793FDF" },
        new List<string> { "#93B1A6", "#5C8374", "#183D3D", "#040D12" },
        new List<string> { "#435334", "#9EB384", "#CEDEBD", "#FAF1E4" },
        new List<string> { "#FFC436", "#337CCF", "#1450A3", "#191D88" },
        new List<string> { "#141E46", "#BB2525", "#FF6969", "#FFF5E0" },
        new List<string> { "#FAF0E6", "#B9B4C7", "#5C5470", "#352F44" },
        new List<string> { "#FFBFBF", "#FFE5E5", "#F3FDE8", "#A8DF8E" },
        new List<string> { "#FFBA86", "#F6635C", "#C23373", "#79155B" },
        new List<string> { "#D5FFD0", "#40F8FF", "#279EFF", "#0C356A" },
        new List<string> { "#836096", "#ED7B7B", "#F0B86E", "#EBE76C" }
    };


    public static List<string> getRandomColorList()
    {
        int n = colorPalettes.Count;
        int index = Random.Range(0, n);
        return StringHelper.StringListToLower(colorPalettes[index]);
    }
    public static Color HexToColor(string hex)
    {
        byte r = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
        byte g = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
        byte b = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
        return new Color(r / 255.0f, g / 255.0f, b / 255.0f);
    }

    public static string ColorToHex(Color color, bool includeAlpha = false)
    {
        if (includeAlpha)
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}{3:x2}",
                (int)(color.r * 255),
                (int)(color.g * 255),
                (int)(color.b * 255),
                (int)(color.a * 255));
        }
        else
        {
            return string.Format("#{0:x2}{1:x2}{2:x2}",
                (int)(color.r * 255),
                (int)(color.g * 255),
                (int)(color.b * 255));
        }
    }
    public static Color LerpHexColor(string startHex, string endHex, float t)
    {
        // HEX 문자열을 Color 객체로 변환
        Color startColor = ColorHelper.HexToColor(startHex);
        Color endColor = ColorHelper.HexToColor(endHex);

        // 색상 선형 보간
        Color lerpedColor = Color.Lerp(startColor, endColor, t);

        return lerpedColor;
    }
    public static bool isDifferentColor(Color color1, Color color2)
    {
        return color1 != color2;
    }

    public static string toColorString(string input, string colorCode)
    {
        return "<color=" + colorCode + ">" + input + "</color>";
    }


    public static bool isColorString(string input, int index, string baseHexString)
    {
        string target = baseHexString + ">";
        if (index - target.Length < 0)
        {
            return false;
        }
        if (input.Substring(index - target.Length, target.Length) == target)
        {
            return true;
        }
        return false;
    }
    public static List<Color> makeColorListByTargetColor(List<Color> colorList, Color target, int from, int to)
    {
        List<Color> tempColors = new List<Color>(colorList);
        for (int i = from; i < to; i++)
        {
            tempColors[i] = target;
        }
        return tempColors;
    }
}