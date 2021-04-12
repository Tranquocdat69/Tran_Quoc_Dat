using FilterAndSort.NETCore.Services.Implements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections;
using System.Collections.Generic;

namespace FilterAndSortUnitTest
{
    public class DataTest
    {
        public static Dictionary<string, string> DictionnaryTest = new Dictionary<string, string> {
            { "Folder", "D:\\WORKSPACE\\c# fpts bt\\12+13__33__20210322085224"},
            { "Filter", "52=20190517"},
        };

        public IConfiguration ConfigurationTest = new ConfigurationBuilder().AddInMemoryCollection(DictionnaryTest).Build();
        public Mock<ILogger<GetData>> LoggerMockTest = new Mock<ILogger<GetData>>();
        public Mock<ILogFile> LogExceptionMockTest = new Mock<ILogFile>();
        public string PathFileTest = "D:\\WORKSPACE\\c# fpts bt\\12+13__33__20210322085224\\file1.txt";
        public const string DateTimeString = "52=20190517 09:00:00.065";
    }

    public class ClassMemberFilterTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 3,  new List<string>() {
                "8=FIX.4.49=12135=M849=VNMGW56=9999934=2801852=20190517 15:05:00.18130001=STO20004=G355=VN000000AGR520026=0.030541=0.020027=310=016",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 12:00:03.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=99935=DEMO49=VNMGW56=9999934=20052=20190520 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071"
                }
            };

            yield return new object[] { 4,  new List<string>() {
                "8=FIX.4.49=12135=M849=VNMGW56=9999934=2801852=20190517 15:05:00.18130001=STO20004=G355=VN000000AGR520026=0.030541=0.020027=310=016",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 12:00:03.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=99935=DEMO49=VNMGW56=9999934=20052=20190517 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class ClassMemberReplaceValueTagTest : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new List<string>() {
                "8=FIX.4.49=12135=M849=VNMGW56=9999934=2801852=20190517 15:05:00.18130001=STO20004=G355=VN000000AGR520026=0.030541=0.020027=310=016",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 12:00:03.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=10935=f49=VNMGW56=9999934=2169452=20190517 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071",
                "8=FIX.4.49=99935=DEMO49=VNMGW56=9999934=20052=20190517 20:20:23.07130001=STO20004=T120005=AW955=VN000000AGR5336=4010=071"
                }
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
