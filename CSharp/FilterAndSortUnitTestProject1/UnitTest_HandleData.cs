using FilterAndSort.NETCore.Services.Implements;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilterAndSortUnitTest.NETCore.Tests
{
    public class UnitTest_HandleData
    {
        private static DataTest _dataTest = new DataTest();
        private HandleData _hD;

        public UnitTest_HandleData()
        {
            _hD = new HandleData(_dataTest.ConfigurationTest, _dataTest.LogExceptionMockTest.Object);
        }

        [Theory]
        [ClassData(typeof(ClassMemberFilterTest))]
        public void Test_Filter(int expected, List<string> listTest)
        {
            List<string> listFilterTest = _hD.Filter(listTest);
            Assert.Equal(expected, listFilterTest.Count);
        }


        [Theory]
        [ClassData(typeof(ClassMemberReplaceValueTagTest))]
        public void Test_ReplaceValueTag(List<string> listTest)
        {
            List<string> listFilterTest = _hD.ReplaceValueTag(listTest);
            Assert.Contains(listFilterTest, item => item.Contains("34=1"));
        }

        [Theory]
        [InlineData(63693680400.065, DataTest.DateTimeString)]
        public void Test_ConvertDateTimeToSecond(double expected, string DateTimeString)
        {
            double resultTest = _hD.ConvertDateTimeToSecond(DateTimeString);
            Assert.Equal(expected, resultTest);
        }
    }
}
