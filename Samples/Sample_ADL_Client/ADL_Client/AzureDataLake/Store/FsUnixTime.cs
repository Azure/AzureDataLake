using System;

namespace AzureDataLake.Store
{
    public struct FsUnixTime
    {
        public readonly long SecondsSinceEpoch;

        public FsUnixTime(long value)
        {
            if (value < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(value));
            }
            this.SecondsSinceEpoch = value;
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

            int v = (int) time.Subtract(FsUnixTime.EpochDateTime).TotalSeconds;

            if (v < 0)
            {
                throw new System.ArgumentOutOfRangeException(nameof(time));
            }

            this.SecondsSinceEpoch = v;
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
            var dt = FsUnixTime.EpochDateTime.AddSeconds(this.SecondsSinceEpoch);
            return dt;
        }
    }
}