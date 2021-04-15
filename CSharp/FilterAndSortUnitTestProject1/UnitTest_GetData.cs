using FilterAndSort.NETCore.Services.Implements;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilterAndSortUnitTest.NETCore.Tests
{
    public class UnitTest_GetData
    {
        private static DataTest _dataTest = new DataTest();
        private GetData _gD;

        public UnitTest_GetData()
        {
            _gD = new GetData(_dataTest.ConfigurationTest, _dataTest.LogExceptionMockTest.Object, _dataTest.LoggerMockTest.Object);
        }

        [Fact]
        public void Test_GetAllDirectoryNames()
        {
            List<string> ListTest = _gD.GetAllDirectoryNames(_dataTest.ConfigurationTest["Folder"]);
            Assert.Contains(ListTest, item => item == "D:\\WORKSPACE\\DataTest_C#\\file1.txt");
        }

        [Fact]
        public void Test_ReadFiles()
        {
            List<string> ListTest = _gD.ReadFiles(_dataTest.PathFileTest);
            Assert.Contains(ListTest, item => item == "8=FIX.4.49=12135=M849=VNMGW56=9999934=2801852=20190517 15:05:00.18130001=STO20004=G355=VN000000AGR520026=0.030541=0.020027=310=016");
        }

    }
}
