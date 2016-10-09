using System;

namespace AzureDataLake.Store
{
    public struct FsUnixTime
    {
        public readonly long MillisecondsSinceEpoch;

        public FsUnixTime(long value)
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }
            this.MillisecondsSinceEpoch = value;
        }

        public FsUnixTime(System.DateTime time)
        {
            if (time.Kind != DateTimeKind.Utc)
            {
                throw new System.ArgumentOutOfRangeException("time Kind must be utc");

            }

            if (time<FsUnixTime.EpochDateTime)
            {
                throw new System.ArgumentOutOfRangeException(nameof(time));
            }

            long v = (long) time.Subtract(FsUnixTime.EpochDateTime).TotalMilliseconds;

            if (v < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(time));
            }

            this.MillisecondsSinceEpoch = v;
        }

        public static FsUnixTime UtcNow()
        {
            var ut = new FsUnixTime(DateTime.UtcNow);
            return ut;
        }

        public static FsUnixTime Epoch
        {
            get
            {
                var ut = new FsUnixTime(0);
                return ut;
            }
        }

        public static System.DateTime EpochDateTime
        {
            get
            {
                return new System.DateTime(1970, 1, 1,0,0,0,0, System.DateTimeKind.Utc);
            }
        }

        public DateTime ToToDateTimeUtc()
        {
            var dt = FsUnixTime.EpochDateTime.AddMilliseconds(this.MillisecondsSinceEpoch);
            return dt;
        }

        public static DateTime ToToDateTimeUtc(long seconds)
        {
            var ut = new FsUnixTime(seconds);
            var dt = ut.ToToDateTimeUtc();
            return dt;
        }

        public static FsUnixTime? TryParseDouble(long? d)
        {
            if (d.HasValue)
            {
                return  new FsUnixTime(d.Value);
            }
            else
            {
                return null;
            }
        }
    }
}