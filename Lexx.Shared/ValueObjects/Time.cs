using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Lexx.Shared.ValueObjects
{
    public struct Time : IEquatable<Time>
    {
        private static string TIME_REGEX = "^(?:(?:([-]?[0-9]*?\\d|6[0-2]):)([0-5]?\\d))?(:[0-5]?\\d)?$";

        private long _totalSeconds { get; set; }

        public Time(int hour, int minute, int second = 0)
        {
            if (minute > 59) throw new ArgumentException("minute");
            if (second > 59) throw new ArgumentException("second");
            
            _totalSeconds = second + minute * 60 + hour * 3600;
        }

        public Time(DateTime dateTime)
            : this(dateTime.Hour, dateTime.Minute, dateTime.Second)
        { }

        public Time(string time)
        {
            this = string.IsNullOrEmpty(time) ? throw new ArgumentNullException(time) : time;
        }

        public Time(long seconds)
        {
            _totalSeconds = seconds;
        }

        public static Time operator +(Time left, Time right)
            => new Time(left._totalSeconds + right._totalSeconds);

        public static Time operator -(Time left, Time right) 
            => new Time(left._totalSeconds - right._totalSeconds);

        public static implicit operator Time(string time)
        {
            var match = Regex.Match(time, TIME_REGEX);
            if (match.Success)
            {
                var negative = time.Contains("-");
                if (negative)
                    time = time.Replace("-", "");

                var timeSplit = time.Split(':').ToArray();

                var seconds = int.Parse(timeSplit.ElementAt(0)) * 3600 + int.Parse(timeSplit.ElementAt(1)) * 60;

                if (timeSplit.Length == 3)
                    seconds += int.Parse(timeSplit.ElementAt(2));

                seconds = negative ? seconds * -1 : seconds;

                return new Time(seconds);
            }
            else
                throw new InvalidCastException("Time");
        }

        public override string ToString()
        {
            var negative = _totalSeconds < 0;
            var value = negative ? _totalSeconds * -1 : _totalSeconds;

            var hh = value / 3600;
            var mm = value % 3600;

            var ss = mm % 60;
            mm = mm / 60;

            return string.Format("{0}{1:00}:{2:00}:{3:00}",negative ? "-" : "", hh, mm, ss);
        }

        public bool Equals(Time other) =>
            _totalSeconds == other._totalSeconds;
    }
}