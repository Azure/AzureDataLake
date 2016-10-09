using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADL_Client_Tests
{
    [TestClass]
    public class FsUnixTime_Tests : Base_Tests
    {
        [TestMethod]
        public void FSPath_Constructor_Root()
        {
            var ut0 = new AzureDataLake.Store.FsUnixTime();
            Assert.AreEqual(0, ut0.SecondsSinceEpoch);

            var dt0 = ut0.ToToDateTimeUtc();

            Assert.AreEqual(1970, dt0.Year);
            Assert.AreEqual(1, dt0.Month);
            Assert.AreEqual(1, dt0.Day);
            Assert.AreEqual(0, dt0.Hour);
            Assert.AreEqual(0, dt0.Minute);
            Assert.AreEqual(0, dt0.Second);
            Assert.AreEqual(DateTimeKind.Utc, dt0.Kind);

            var ut1 = new AzureDataLake.Store.FsUnixTime(dt0);
            Assert.AreEqual(0, ut1.SecondsSinceEpoch);
        }

        [TestMethod]
        public void FSPath_Constructor_Root2()
        {
            var d0 = new System.DateTime(2016,3,31,1,2,3,DateTimeKind.Utc);
            var ut0 = new AzureDataLake.Store.FsUnixTime(d0);
            var d1 = ut0.ToToDateTimeUtc();
            Assert.AreEqual(2016, d1.Year);
            Assert.AreEqual(3, d1.Month);
            Assert.AreEqual(31, d1.Day);
            Assert.AreEqual(1, d1.Hour);
            Assert.AreEqual(2, d1.Minute);
            Assert.AreEqual(3, d1.Second);
            Assert.AreEqual(DateTimeKind.Utc, d1.Kind);
        }
    }
}
