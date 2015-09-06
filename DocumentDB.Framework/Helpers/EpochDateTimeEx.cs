using System;

namespace DocumentDB.Framework.Helpers
{
    public static class EpochDateTimeEx
    {
        /// <summary>
        ///     Convert a DateTime to the number of seconds that have elapsed since January 1, 1970 (midnight UTC/GMT)
        /// </summary>
        public static int ToEpochDateTime(this DateTime date)
        {
            if (date == null)
            {
                throw new ArgumentNullException(nameof(date));
            }
            return (int)(date - new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Convert an amount of seconds elapsed since January 1, 1970 (midnight UTC/GMT) to DateTime
        /// </summary>
        public static DateTime ToDateTime(this int epochDateTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(epochDateTime);
        }
    }
}