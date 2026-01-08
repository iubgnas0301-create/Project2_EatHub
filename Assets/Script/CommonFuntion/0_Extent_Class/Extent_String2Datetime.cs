using System;
using UnityEngine;

public static class Extent_String2Datetime
{
    public static DateTime Fomat_SlashDate_2_Datetime(this string ddMMyyy) {
        string[] arrDate = ddMMyyy.Split('/');
        int day = int.Parse(arrDate[0]);
        int month = int.Parse(arrDate[1]);
        int year = int.Parse(arrDate[2]);
        return new DateTime(year, month, day);
    }
    public static DateTime Fomat_string2datetime(this string dateStr, string format = "yyyy-MM-dd HH:mm:ss") {
        return DateTime.ParseExact(dateStr, format, System.Globalization.CultureInfo.InvariantCulture); //thank you Copilot
    }
}
