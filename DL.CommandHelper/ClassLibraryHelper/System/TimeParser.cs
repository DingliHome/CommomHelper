using System;
using System.Globalization;

namespace ClassLibraryHelper.System
{
    /// <summary>
    /// 时间类
    /// 1、SecondToMinute(int Second) 把秒转换成分钟
    /// </summary>
    public class TimeParser
    {

        #region 返回某年某月最后一天

        /// <summary>
        /// 返回某年某月最后一天
        /// </summary>
        /// <param name="year">年份</param>
        /// <param name="month">月份</param>
        /// <returns>日</returns>
        public static int GetMonthLastDate(int year, int month)
        {
            var lastDay = new DateTime(year, month,
                                       new GregorianCalendar().GetDaysInMonth(year, month));
            return lastDay.Day;
        }

        #endregion

        #region 返回时间差

        public static string DateDiff(DateTime dateTime1, DateTime dateTime2)
        {
            string dateDiff = null;
            TimeSpan ts = dateTime2 - dateTime1;
            if (ts.Days >= 1)
            {
                dateDiff = dateTime1.Month + "月" + dateTime1.Day + "日";
            }
            else
            {
                if (ts.Hours > 1)
                {
                    dateDiff = ts.Hours + "小时前";
                }
                else
                {
                    dateDiff = ts.Minutes + "分钟前";
                }
            }
            return dateDiff;
        }

        #endregion

        #region 格式化日期时间
        /// <summary>
        /// 格式化日期时间
        /// </summary>
        /// <param name="dateTime1">日期时间</param>
        /// <param name="dateMode">显示模式</param>
        /// <returns>0-9种模式的日期</returns>
        public static string FormatDate(DateTime dateTime1, string dateMode)
        {
            switch (dateMode)
            {
                case "0":
                    return dateTime1.ToString("yyyy-MM-dd");
                case "1":
                    return dateTime1.ToString("yyyy-MM-dd HH:mm:ss");
                case "2":
                    return dateTime1.ToString("yyyy/MM/dd");
                case "3":
                    return dateTime1.ToString("yyyy年MM月dd日");
                case "4":
                    return dateTime1.ToString("MM-dd");
                case "5":
                    return dateTime1.ToString("MM/dd");
                case "6":
                    return dateTime1.ToString("MM月dd日");
                case "7":
                    return dateTime1.ToString("yyyy-MM");
                case "8":
                    return dateTime1.ToString("yyyy/MM");
                case "9":
                    return dateTime1.ToString("yyyy年MM月");
                default:
                    return dateTime1.ToString();
            }
        }
        #endregion
    }
}