using System;
using System.Collections.Generic;
using EDDY.Nexus.Common.Utilities;
using EDDY.IS.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.DeliveryEngine.Entity.Common;
//using EDDY.Nexus.Entity.Common;

namespace EDDY.IS.DeliveryEngine.Entity
{
    public enum BlackoutType
    { 
        Onetime = 1,
        Daily = 2,
        Weekly = 3,
        Monthly = 4,
        Yearly = 5,
    }

    public class BlackoutFactory
    {
        public static DeliveryBlackoutPeriod GetDeliveryBlackoutPeriodObject(BlackoutType blackoutType)
        {
            return GetDeliveryBlackoutPeriodObject(blackoutType, null);
        }

        public static DeliveryBlackoutPeriod GetDeliveryBlackoutPeriodObject(BlackoutType blackoutType, string recurringBlackoutDataXML)
        {
            switch (blackoutType)
            {
                case BlackoutType.Onetime:
                default:
                    {
                        return new OnetimeDeliveryBlackoutPeriod();
                    }
                case BlackoutType.Daily:
                    {
                        return new DailyDeliveryBlackoutPeriod(recurringBlackoutDataXML);
                    }
                case BlackoutType.Weekly:
                    {
                        return new WeeklyDeliveryBlackoutPeriod(recurringBlackoutDataXML);
                    }
                case BlackoutType.Monthly:
                    {
                        return new MonthlyDeliveryBlackoutPeriod(recurringBlackoutDataXML);
                    }
                case BlackoutType.Yearly:
                    {
                        return new YearlyDeliveryBlackoutPeriod(recurringBlackoutDataXML);
                    }
            }
        }
    }

    public abstract class DeliveryBlackoutPeriod : CommonEntity
    {
        public BlackoutType BlackoutType { get; set; }
        public DateTime AbsloluteStartDateTime { get; set; }
        public DateTime AbsloluteEndDateTime { get; set; }
        public int UTCHourOffset { get; set; }

        public abstract BlackoutToken CheckDateBlackedOut(DateTime dateTime);

    }

    public class OnetimeDeliveryBlackoutPeriod : DeliveryBlackoutPeriod
    {
        public OnetimeDeliveryBlackoutPeriod()
        {
            this.BlackoutType = BlackoutType.Onetime;
        }

        public override BlackoutToken CheckDateBlackedOut(DateTime dateTime)
        {
            BlackoutToken returnBlackoutToken = new BlackoutToken();
            returnBlackoutToken.BlackoutCurrentlyEffective = false;
            returnBlackoutToken.BlackoutEndDateTime = DateTime.Now.ToUniversalTime();

           
                //If blackoutPeriod currently applicable
                if (this.AbsloluteStartDateTime < dateTime && dateTime < this.AbsloluteEndDateTime)
                {
                    //set return bool to true for BlackoutCurrentlyEffective
                    returnBlackoutToken.BlackoutCurrentlyEffective = true;

                    //set resume date to the end of this blackout
                    returnBlackoutToken.BlackoutEndDateTime = this.AbsloluteEndDateTime;
                }
            
            

            return returnBlackoutToken;
        }
    }

    public abstract class RecurringDeliveryBlackoutPeriod : DeliveryBlackoutPeriod
    {
        private string _recurringBlackoutDataXML;
        public string RecurringBlackoutDataXML 
        {
            get { return _recurringBlackoutDataXML; }
        }

        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public RecurringDeliveryBlackoutPeriod(string recurringBlackoutDataXML)
        {
            _recurringBlackoutDataXML = recurringBlackoutDataXML;
        }
    }

    public class DailyDeliveryBlackoutPeriod : RecurringDeliveryBlackoutPeriod
    {
        public DailyDeliveryBlackoutPeriod(string recurringBlackoutDataXML): base (recurringBlackoutDataXML)
        {
            this.BlackoutType = BlackoutType.Daily;
            SetDailyBlackoutPropertiesFromXML(recurringBlackoutDataXML);
        }

        //set properties for daily delivery blackout
        private void SetDailyBlackoutPropertiesFromXML(string recurringBlackoutDataXML)
        {
            if (recurringBlackoutDataXML != null)
            {
                //Pull dictionary from XML
                Dictionary<string, string> dailyPropertiesDictionary = XmlUtilities.GetDictionaryFromFieldNameValueXML(recurringBlackoutDataXML, "RecurringBlackoutData");

                //Set Start Time - Default to 0
                this.StartTime = BlackoutHelper.GetDictionaryTimeSpan(dailyPropertiesDictionary, "StartTime", TimeSpan.Zero);

                //Set End Time - Default to 11:59:59pm
                this.EndTime = BlackoutHelper.GetDictionaryTimeSpan(dailyPropertiesDictionary, "EndTime", new TimeSpan(23, 59, 59));
            }
        }

        public override BlackoutToken CheckDateBlackedOut(DateTime dateTime)
        {
            BlackoutToken returnBlackoutToken = new BlackoutToken();
            returnBlackoutToken.BlackoutCurrentlyEffective = false;
            returnBlackoutToken.BlackoutEndDateTime = DateTime.Now.ToUniversalTime();

           
                //If we are in the active start and end dateTimes for this blackout
                if (this.AbsloluteStartDateTime < dateTime && dateTime < this.AbsloluteEndDateTime)
                {
                    DateTime DayZeroTime = dateTime.Subtract(dateTime.TimeOfDay); //Get beginning of day (12am) for dateTime param
                    DateTime DayStartTime = DayZeroTime.Add(this.StartTime); //calculate begin DateTime
                    DateTime DayEndTime = DayZeroTime.Add(this.EndTime); //calculate end DateTime

                    //if given dateTime time is between start and end time for this day
                    if (DayStartTime < dateTime && dateTime < DayEndTime)
                    {
                        //set return bool to true for BlackoutCurrentlyEffective
                        returnBlackoutToken.BlackoutCurrentlyEffective = true;

                        //set resume date to the end of this blackout time today
                        returnBlackoutToken.BlackoutEndDateTime = DayEndTime;
                    }
                }
            
           

            return returnBlackoutToken;
        }
    }

    public class WeeklyDeliveryBlackoutPeriod : RecurringDeliveryBlackoutPeriod
    {
        int StartDayOfWeek { get; set; }
        int EndDayOfWeek { get; set; }

        public WeeklyDeliveryBlackoutPeriod(string recurringBlackoutDataXML)
            : base(recurringBlackoutDataXML)
        {
            this.BlackoutType = BlackoutType.Daily;
            SetWeeklyBlackoutPropertiesFromXML(recurringBlackoutDataXML);
        }

        //set properties for weekly delivery blackout
        private void SetWeeklyBlackoutPropertiesFromXML(string recurringBlackoutDataXML)
        {
            if (recurringBlackoutDataXML != null)
            {
                //Pull dictionary from XML
                Dictionary<string, string> weeklyPropertiesDictionary = XmlUtilities.GetDictionaryFromFieldNameValueXML(recurringBlackoutDataXML, "RecurringBlackoutData");

                //Set Start Time - Default to 0
                this.StartTime = BlackoutHelper.GetDictionaryTimeSpan(weeklyPropertiesDictionary, "StartTime", TimeSpan.Zero);

                //Set End Time - Default to 11:59:59pm
                this.EndTime = BlackoutHelper.GetDictionaryTimeSpan(weeklyPropertiesDictionary, "EndTime", new TimeSpan(23, 59, 59));

                //Set Start Day of Week
                this.StartDayOfWeek = BlackoutHelper.GetDictionaryInt(weeklyPropertiesDictionary, "StartDayOfWeek", 0);

                //Set End Day of Week
                this.EndDayOfWeek = BlackoutHelper.GetDictionaryInt(weeklyPropertiesDictionary, "EndDayOfWeek", 6);
            }
        }

        public override BlackoutToken CheckDateBlackedOut(DateTime dateTime)
        {
            BlackoutToken returnBlackoutToken = new BlackoutToken();
            returnBlackoutToken.BlackoutCurrentlyEffective = false;
            returnBlackoutToken.BlackoutEndDateTime = DateTime.Now.ToUniversalTime();

           
                //If we are in the active start and end dateTimes for this blackout
                if (this.AbsloluteStartDateTime < dateTime && dateTime < this.AbsloluteEndDateTime)
                {
                    DateTime DayZeroTime = dateTime.Subtract(dateTime.TimeOfDay); //Get beginning of day (12am) for dateTime param
                    DateTime DayStartTime = DayZeroTime.Add(this.StartTime); //calculate begin DateTime
                    DateTime DayEndTime = DayZeroTime.Add(this.EndTime); //calculate end DateTime
                    int dayOfWeekInt = (int)dateTime.DayOfWeek;

                    ////Check if the span wraps the turn of the week
                    //bool spanWraps = (this.StartDayOfWeek > this.EndDayOfWeek);

                    //if (
                    //    //blackout does NOT span end of week
                    //    (!spanWraps && this.StartDayOfWeek <= dayOfWeekInt && dayOfWeekInt <= this.EndDayOfWeek)
                    //    ||
                    //    //blackout spans end of week
                    //    (spanWraps && this.EndDayOfWeek <= dayOfWeekInt && dayOfWeekInt <= this.StartDayOfWeek)
                    //    )

                    //{
                    //    //Check Time of Day if necessary
                    //    if (1 == 2)
                    //    {
                    //        //set return bool to true for BlackoutCurrentlyEffective
                    //        returnBlackoutToken.BlackoutCurrentlyEffective = true;

                    //        //set resume date to the end of this blackout time today
                    //        returnBlackoutToken.BlackoutEndDateTime = DayEndTime;
                    //    }
                    //}
                }
           
           

            return returnBlackoutToken;
        }
    }

    public class MonthlyDeliveryBlackoutPeriod : RecurringDeliveryBlackoutPeriod
    {
        int StartCalendarDay { get; set; }
        int EndCalendarDay { get; set; }

        public MonthlyDeliveryBlackoutPeriod(string recurringBlackoutDataXML): base(recurringBlackoutDataXML)
        {
            this.BlackoutType = BlackoutType.Monthly;
            SetMonthlyBlackoutPropertiesFromXML(recurringBlackoutDataXML);
        }

        //set properties for daily delivery blackout
        private void SetMonthlyBlackoutPropertiesFromXML(string recurringBlackoutDataXML)
        {
            if (recurringBlackoutDataXML != null)
            {
                //Pull dictionary from XML
                Dictionary<string, string> monthlyPropertiesDictionary = XmlUtilities.GetDictionaryFromFieldNameValueXML(recurringBlackoutDataXML, "RecurringBlackoutData");

                //Set Start Time - Default to 0
                this.StartTime = BlackoutHelper.GetDictionaryTimeSpan(monthlyPropertiesDictionary, "StartTime", TimeSpan.Zero);

                //Set End Time - Default to 11:59:59pm
                this.EndTime = BlackoutHelper.GetDictionaryTimeSpan(monthlyPropertiesDictionary, "EndTime", new TimeSpan(23, 59, 59));

                //set Start Calendar Day - default to 1
                string startCalendarDayString = BlackoutHelper.GetDictionaryString(monthlyPropertiesDictionary, "StartCalendarDay", "1");
                this.StartCalendarDay = BlackoutHelper.GetCalendarDay(startCalendarDayString);

                //set End Calendar Day - default to L
                string endCalendarDayString = BlackoutHelper.GetDictionaryString(monthlyPropertiesDictionary, "EndCalendarDay", "L");
                this.EndCalendarDay = BlackoutHelper.GetCalendarDay(endCalendarDayString);
            }
        }

        public override BlackoutToken CheckDateBlackedOut(DateTime dateTime)
        {
            BlackoutToken returnBlackoutToken = new BlackoutToken();
            returnBlackoutToken.BlackoutCurrentlyEffective = false;
            returnBlackoutToken.BlackoutEndDateTime = DateTime.Now.ToUniversalTime();

           
                //If we are in the active start and end dateTimes for this blackout
                if (this.AbsloluteStartDateTime < dateTime && dateTime < this.AbsloluteEndDateTime)
                {
                    DateTime DayZeroTime = dateTime.Subtract(dateTime.TimeOfDay); //Get beginning of day (12am) for dateTime param
                    DateTime DayStartTime = DayZeroTime.Add(this.StartTime); //calculate begin DateTime
                    DateTime DayEndTime = DayZeroTime.Add(this.EndTime); //calculate end DateTime

                    //if given dateTime is within the blackout calendar Days
                    if ((this.StartCalendarDay < dateTime.Day && dateTime.Day < this.EndCalendarDay) ||
                        (dateTime.Day == this.StartCalendarDay && DayStartTime < dateTime) ||
                        (dateTime.Day == this.EndCalendarDay && dateTime < DayEndTime))
                    {
                        //set return bool to true for BlackoutCurrentlyEffective
                        returnBlackoutToken.BlackoutCurrentlyEffective = true;

                        //set resume date to the end of this blackout time
                        //CurrentYear/CurrentMonth/EndCalendarDay/EndTime
                        returnBlackoutToken.BlackoutEndDateTime = new DateTime(dateTime.Year, dateTime.Month, this.EndCalendarDay).Add(this.EndTime);
                    }
                }
           
           

            return returnBlackoutToken;
        }

    }

    public class YearlyDeliveryBlackoutPeriod : RecurringDeliveryBlackoutPeriod
    {
        int StartMonth { get; set; }
        int StartCalendarDay { get; set; }

        int EndMonth { get; set; }
        int EndCalendarDay { get; set; }

        public YearlyDeliveryBlackoutPeriod(string recurringBlackoutDataXML): base(recurringBlackoutDataXML)
        {
            this.BlackoutType = BlackoutType.Yearly;
            SetYearlyBlackoutPropertiesFromXML(recurringBlackoutDataXML);
        }

        //set properties for daily delivery blackout
        private void SetYearlyBlackoutPropertiesFromXML(string recurringBlackoutDataXML)
        {
            if (recurringBlackoutDataXML != null)
            {
                //Pull dictionary from XML
                Dictionary<string, string> monthlyPropertiesDictionary = XmlUtilities.GetDictionaryFromFieldNameValueXML(recurringBlackoutDataXML, "RecurringBlackoutData");

                //Set Start Time - Default to 0
                this.StartTime = BlackoutHelper.GetDictionaryTimeSpan(monthlyPropertiesDictionary, "StartTime", TimeSpan.Zero);

                //Set End Time - Default to 11:59:59pm
                this.EndTime = BlackoutHelper.GetDictionaryTimeSpan(monthlyPropertiesDictionary, "EndTime", new TimeSpan(23,59,59));

                //set start month - default to JAN
                this.StartMonth = BlackoutHelper.GetDictionaryInt(monthlyPropertiesDictionary, "StartMonth", 1);

                //set end month - default to DEC
                this.EndMonth = BlackoutHelper.GetDictionaryInt(monthlyPropertiesDictionary, "EndMonth", 12);

                //set Start Calendar Day - default to 1
                string startCalendarDayString = BlackoutHelper.GetDictionaryString(monthlyPropertiesDictionary, "StartCalendarDay", "1");
                this.StartCalendarDay = BlackoutHelper.GetCalendarDay(startCalendarDayString, this.EndMonth);

                //set End Calendar Day - default to L
                string endCalendarDayString = BlackoutHelper.GetDictionaryString(monthlyPropertiesDictionary, "EndCalendarDay", "L");
                this.EndCalendarDay = BlackoutHelper.GetCalendarDay(endCalendarDayString, this.EndMonth);

            }
        }

        public override BlackoutToken CheckDateBlackedOut(DateTime dateTime)
        {
            BlackoutToken returnBlackoutToken = new BlackoutToken();
            returnBlackoutToken.BlackoutCurrentlyEffective = false;
            returnBlackoutToken.BlackoutEndDateTime = DateTime.Now.ToUniversalTime();

           //If we are in the active start and end dateTimes for this blackout
                if (this.AbsloluteStartDateTime < dateTime && dateTime < this.AbsloluteEndDateTime)
                {
                    DateTime DayZeroTime = dateTime.Subtract(dateTime.TimeOfDay); //Get beginning of day (12am) for dateTime param
                    DateTime DayStartTime = DayZeroTime.Add(this.StartTime); //calculate begin DateTime
                    DateTime DayEndTime = DayZeroTime.Add(this.EndTime); //calculate end DateTime

                    //if given dateTime month is within the blackout months
                    if (this.StartMonth < dateTime.Month && dateTime.Month < this.EndMonth)
                    {

                    }
                    else
                    { 
                    
                    }
                }
           
           

            return returnBlackoutToken;
        }

    }


    public class BlackoutHelper
    {
        public static string GetDictionaryString(Dictionary<string, string> dictionary, string key, string defaultValue)
        {
            if (dictionary.ContainsKey(key))
            {
                return (string)dictionary[key];
            }
            else
            {
                return defaultValue;
            }
        }

        public static int GetDictionaryInt(Dictionary<string, string> dictionary, string key, int defaultValue)
        {
           
                if (dictionary.ContainsKey(key))
                {
                    return Convert.ToInt32(dictionary[key]);
                }
                else
                {
                    return defaultValue;
                }
           
        }

        public static DateTime GetDictionaryDateTime(Dictionary<string, string> dictionary, string key, DateTime defaultValue)
        {
            string stringVal = GetDictionaryString(dictionary, key, "");

           
                if (stringVal == "")
                {
                    return defaultValue;
                }
                else
                {
                    return Convert.ToDateTime(stringVal);
                }

        }

        public static TimeSpan GetDictionaryTimeSpan(Dictionary<string, string> dictionary, string key, TimeSpan defaultValue)
        {
            string stringVal = GetDictionaryString(dictionary, key, "");

            if (stringVal == "")
            {
                return defaultValue;
            }
            else
            {
                return GetTimeSpanFromString(stringVal);
            }
        }

        public static TimeSpan GetTimeSpanFromString(string timeSpanString)
        {
            int h = Convert.ToInt16(timeSpanString.Substring(0, 2));
            int m = Convert.ToInt16(timeSpanString.Substring(3, 2));
            int s = Convert.ToInt16(timeSpanString.Substring(6, 2));
            return new TimeSpan(h, m, s);
        }

        public static int GetCalendarDay(string calendarDayString)
        {
            //Overloaded, Month Optional
            //default, use current month
            return GetCalendarDay(calendarDayString, DateTime.Now.ToUniversalTime().Month);
        }

        public static int GetCalendarDay(string calendarDayString, int month)
        {
            int daysInCurrentMonth = DateTime.DaysInMonth(DateTime.Now.ToUniversalTime().Year, month);
            int calendarDay;

            //L is last day in this month - else its an Int
            if (calendarDayString.ToUpper() == "L")
            {
                return DateTime.DaysInMonth(DateTime.Now.ToUniversalTime().Year, month);
            }
            else
            {
                calendarDay = Convert.ToInt16(calendarDayString);
            }

            //Cap return int
            if (calendarDay > daysInCurrentMonth)
            {
                return daysInCurrentMonth;
            }
            else
            {
                return calendarDay;
            }
            
        }
    }

    public class BlackoutToken
    {
        public bool BlackoutCurrentlyEffective { get; set; }
        public DateTime BlackoutEndDateTime { get; set; }
    }

}
