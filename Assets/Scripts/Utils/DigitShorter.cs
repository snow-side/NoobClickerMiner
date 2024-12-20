using System;

public static class DigitShorter
{
    public static string Short(this float num, int round = 0)
    {
        if (num >= 1e19)
            return string.Format("{0:0.##E+00}", num);
        if (num >= 1e18)
            return $"{Math.Round(num / 1e18f, round)} трил.";
        if (num >= 1e15)
            return $"{Math.Round(num / 1e15f, round)} б-ард.";
        if (num >= 1e12)
            return $"{Math.Round(num / 1e12f, round)} б-он.";
        if (num >= 1e9)
            return $"{Math.Round(num / 1e9f, round)} млр.";
        if (num >= 1e6)
            return $"{Math.Round(num / 1e6f, round)} млн.";
        if (num >= 1e3)
            return $"{Math.Round(num / 1e3f, round)} тыс.";

        return Math.Round(num, round).ToString();
    }
}