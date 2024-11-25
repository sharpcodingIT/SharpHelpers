// (c) 2023 SharpCoding
// This code is licensed under MIT license (see LICENSE.txt for details)
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCoding.SharpHelpers;
using System.Linq;

namespace SharpHelpers.UnitTest.DateAndTime
{
    [TestClass]
    public class DateTimeTest
    {
        [TestMethod]
        public void TestDateTimeSmart()
        {
            var dtDictionary = new System.Collections.Generic.Dictionary<string, string>()
                    {
                        {"M/dd/yyyy hh:mm", "10/11/2023 09:00"},
                        {"MM/dd/yyyy hh:mm:ss tt zzz", "05/01/2009 01:30:42 PM -05:00"},
                        {"g", "10/11/2023 09:00"}
                    };

            DateTimeSmart.AddFormats(dtDictionary.Keys.ToArray());

            foreach (var key in dtDictionary.Keys)
            {
                DateTime dt = (DateTimeSmart)dtDictionary[key];
                Assert.IsFalse(dt == DateTime.MinValue);
            }

            DateTime dt1 = (DateTimeSmart)"01-05-2023 22:30:55";
            Assert.IsTrue(dt1 == new DateTime(2023, 5, 1, 22, 30, 55, DateTimeKind.Local));
        }

        [TestMethod]
        public void IsLeapYear_ShouldReturnTrueForLeapYear()
        {
            var leapYear = new DateTime(2024, 1, 1);
            var isLeapYear = leapYear.IsLeapYear();

            Assert.IsTrue(isLeapYear);
        }

        [TestMethod]
        public void Age_ShouldCalculateCorrectAge()
        {
            var birthDate = new DateTime(1990, 8, 18);
            var today = new DateTime(2024, 8, 18);
            var expectedAge = 34;

            var age = birthDate.Age();

            Assert.AreEqual(expectedAge, age);
        }

        [TestMethod]
        public void AddBusinessDays_ShouldSkipWeekends()
        {
            var startDate = new DateTime(2024, 8, 16); // Friday
            var expected = new DateTime(2024, 8, 20); // 2 business days later (Tuesday)

            var result = startDate.AddBusinessDays(2);

            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void IsWeekend_ShouldReturnFalseForWeekday()
        {
            var monday = new DateTime(2024, 8, 19);

            var isMondayWeekend = monday.IsWeekend();

            Assert.IsFalse(isMondayWeekend);
        }
    }
}
