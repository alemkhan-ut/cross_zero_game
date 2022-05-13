using System;
using UnityEngine;

public class DateFormater : MonoBehaviour
{
    public static string GetFullShortDate(DateTime dateTime)
    {
        string fullShortDateTime = $"{dateTime.ToShortDateString()} {dateTime.ToShortTimeString()}";
        return fullShortDateTime;
    }

    public static string GetFormatingDurationTime(DateTime startDateTime, DateTime endDateTime)
    {
        float durationTime = (float)(endDateTime.TimeOfDay.TotalSeconds - startDateTime.TimeOfDay.TotalSeconds);

        int hoursInDurationTime = Mathf.FloorToInt(durationTime / 3600F);
        int minutesInDurationTime = Mathf.FloorToInt(durationTime % 3600 / 60);
        int secondsInDurationTime = Mathf.FloorToInt(durationTime % 60);

        string durationFormatingTime;

        if (secondsInDurationTime <= 0)
        {
            durationFormatingTime = string.Format("0 сек.");
        }
        else
        if (minutesInDurationTime <= 0)
        {
            durationFormatingTime = string.Format($"{secondsInDurationTime} сек.");
        }
        else
        if (hoursInDurationTime <= 0)
        {
            durationFormatingTime = string.Format($"{minutesInDurationTime} мин. : {secondsInDurationTime} сек.");
        }
        else
        {
            durationFormatingTime = string.Format($"{hoursInDurationTime} час. : {minutesInDurationTime} мин. : {secondsInDurationTime} сек.");
        }

        return durationFormatingTime;
    }
}
