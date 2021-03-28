using System;
using System.Collections.Generic;
using System.Text;

namespace FilterAndSort.NETCore.Services
{
    public interface IHandleData
    {
        //lọc dữ liệu các dòng sau khi lấy được dữ liệu trong các files
        List<string> Filter(List<string> data);
        //sắp xếp dữ liệu các dòng sau khi lấy được dữ liệu trong các files
        List<string> Sort(List<string> data);
        //gán lại giá trị của thẻ 34 sau khi sắp xếp các dòng trong file
        List<string> ReplaceValueTag(List<string> data);

        //ghi dữ liệu ra 1 file mới 
        void WiteLinesToFile(List<string> data);

    }
}
