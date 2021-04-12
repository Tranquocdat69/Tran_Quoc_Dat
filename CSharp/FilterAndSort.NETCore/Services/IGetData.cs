using System;
using System.Collections.Generic;
using System.Text;

namespace FilterAndSort.NETCore.Services
{
    public interface IGetData
    {
        //lấy ra các files trong thư mục
        List<string> getAllDirectoryNames(string pathFolder);
        //đọc từng file 1 để lấy ra các dòng trong file
        List<string> readFiles(string path);
        //gộp lấy tất cả các dòng trong các files
        List<string> getAllLines();
    }
}
