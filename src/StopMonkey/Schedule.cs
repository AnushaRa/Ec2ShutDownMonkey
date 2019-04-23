using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace StopMonkey
{
    public class Schedule
    {

        private JObject getSchedule(string scheduleString)
        {
            dynamic scheduleJson;
            if (string.IsNullOrEmpty(scheduleString))
            {
                scheduleJson = JObject.Parse("{\"timezone\":\"Pacific/Auckland\",\"mon\": {\"start\": 7, \"stop\": 20},\"tue\": {\"start\": 7, \"stop\": 20},\"wed\": {\"start\": 7, \"stop\": 20},\"thu\": {\"start\": 11, \"stop\": 20},\"fri\": {\"start\": 7, \"stop\": 20}}");
                return scheduleJson;
            }

            scheduleJson = JObject.Parse(scheduleString);
            return scheduleJson;


        }

        public Action processSchedule(string schedule)
        {
            JObject scheduleJson = getSchedule(schedule);
            
            string timezone = scheduleJson.GetValue("timezone").ToString();
            DateTime currentTime = GetDateTime(timezone);
            string dayOfWeek = currentTime.DayOfWeek.ToString().ToLower().Substring(0, 3);
            
            int timeCurrentHour = currentTime.Hour;
            int startHour = Convert.ToInt32((scheduleJson.GetValue(dayOfWeek)).SelectToken("start"));
            int stopHour = Convert.ToInt32((scheduleJson.GetValue(dayOfWeek)).SelectToken("stop"));

            if (startHour == timeCurrentHour)
            {
                return Action.start;
            }
            else if (stopHour == timeCurrentHour)
            {
                return Action.stop;
            }
            return Action.none;
        }

        private DateTime GetDateTime(string timezone)
        {

            TimeZoneInfo tz = TZDatabaseToTimeZoneInfo.TZTimeZoneToTimeZoneInfo(timezone);
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.Now, tz);
            return currentTime;
        }




    }
}
