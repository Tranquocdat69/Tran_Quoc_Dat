using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FilterAndSort.NETCore.Services.Implements
{
    class GetData : IGetData
    {
        //IConfiguration dùng lấy dữ liệu trong file appsettings.json
        IConfiguration _configuration;
        public GetData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> getAllDirectoryNames()
        {
            List<string> list = new List<string>();
            //lấy trong file appsettings.json có key là folder để lấy các đường dẫn file
            list = Directory.GetFiles(_configuration["Folder"]).ToList();
            return list;
        }

        public List<string> getAllLines()
        {
            List<string> allLines = new List<string>();
            //lấy các đường dẫn
            List<string> listDirs = getAllDirectoryNames();
            foreach(var dir in listDirs)
            {
                //dùng function readFiles để đọc lần lượt dữ liệu từ các file
                List<string> listLines = readFiles(dir);
                // AddRange add các list lại với nhau thành 1 list chung 
                allLines.AddRange(listLines);
            }
            return allLines;
        }

        public List<string> readFiles(string path)
        {
            List<string> list = new List<string>();
            //đọc các dòng trong file và add vào list
            StreamReader reader = new StreamReader(path);
            string line;
            //kiểm tra nếu còn dòng thì add vào list
            while((line = reader.ReadLine()) != null)
            {
                list.Add(line);
            }
            return list;
        }
    }
}
