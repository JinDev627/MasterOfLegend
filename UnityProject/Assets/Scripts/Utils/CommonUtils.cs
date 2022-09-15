using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonUtils
{
    private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }

    public static double ConvertToTimestamp(DateTime value)
    {
        TimeSpan elapsedTime = value.ToUniversalTime() - Epoch;
        return (double)elapsedTime.TotalSeconds;
    }
}
