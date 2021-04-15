using System;
using System.Collections.Generic;
using System.Text;

namespace FilterAndSort.NETCore.Services
{
    public interface IGetData
    {
        //lấy ra các files trong thư mục
        List<string> GetAllDirectoryNames(string pathFolder);
        //đọc từng file 1 để lấy ra các dòng trong file
        List<string> ReadFiles(string path);
        //gộp lấy tất cả các dòng trong các files
        List<string> GetAllLines();
    }
}
