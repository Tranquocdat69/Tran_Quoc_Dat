using FilterAndSort.NETCore.Services.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections;
using Xunit;
using Moq;
using FilterAndSort.NETCore.Services;

namespace FilterAndSortUnitTest
{
    public class FilterAndSortUnitTest
    {

        private static DataTest _dataTest = new DataTest();
        private GetData _gD;
        private HandleData _hD;

        public FilterAndSortUnitTest()
        {
            _gD = new GetData(_dataTest.ConfigurationTest, _dataTest.LogExceptionMockTest.Object, _dataTest.LoggerMockTest.Object);
            _hD = new HandleData(_dataTest.ConfigurationTest, _dataTest.LogExceptionMockTest.Object);
        }

        [Fact]
        public void GetAllDirectoryNamesTest()
        {
            List<string> ListTest = _gD.getAllDirectoryNames(_dataTest.ConfigurationTest["Folder"]);
            Assert.Contains(ListTest, item => item == "D:\\WORKSPACE\\c# fpts bt\\12+13__33__20210322085224\\file1.txt");
        }

        [Fact]
        public void ReadFilesTest()
        {
            List<string> ListTest = _gD.readFiles(_dataTest.PathFileTest);
            Assert.Contains(ListTest, item => item == "8=FIX.4.49=12135=M849=VNMGW56=9999934=2801852=20190517 15:05:00.18130001=STO20004=G355=VN000000AGR520026=0.030541=0.020027=310=016");
        }

        [Theory]
        [ClassData(typeof(ClassMemberFilterTest))]
        public void FilterTest(int expected, List<string> listTest)
        {
            List<string> listFilterTest = _hD.Filter(listTest);
            Assert.Equal(expected, listFilterTest.Count);
        }
        
        
        [Theory]
        [ClassData(typeof(ClassMemberReplaceValueTagTest))]
        public void ReplaceValueTagTest(List<string> listTest)
        {
            List<string> listFilterTest = _hD.ReplaceValueTag(listTest);
            Assert.Contains(listFilterTest, item => item.Contains("34=1"));
        }
        
        [Theory]
        [InlineData(63693680400.065 ,DataTest.DateTimeString)]
        public void ConvertDateTimeToSecondTest(double expected ,string DateTimeString)
        {
            double resultTest = _hD.ConvertDateTimeToSecond(DateTimeString);
            Assert.Equal(expected, resultTest);
        }
    }
}