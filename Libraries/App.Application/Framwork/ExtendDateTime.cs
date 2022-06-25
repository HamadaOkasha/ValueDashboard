using App.Application.Services;
using App.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.Application.Framwork
{
    public static class ExtendDateTime
    {
        public static DateTime ReternLocalDate(this DateTime date)
        {
            string easternZoneId = "Arab Standard Time";
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(easternZoneId);
            var currentDate = TimeZoneInfo.ConvertTime(date, TimeZoneInfo.Local, easternZone);
            return currentDate;
        }
    }
}
