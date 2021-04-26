using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FilterAndSort.NETCore.Services.Implements
{
    public class GetData : IGetData
    {
        private readonly ILogFile _logException;
        private readonly ILogger<GetData> _logger;
        //IConfiguration dùng lấy dữ liệu trong file appsettings.json
        private readonly IConfiguration _configuration;
        private string _pathFolder = "";

        public GetData(IConfiguration configuration, ILogFile logException, ILogger<GetData> logger)
        {
            _configuration = configuration;
            _logException = logException;
            _logger = logger;
            _pathFolder = _configuration["Folder"];
        }
        public List<string> GetAllDirectoryNames(string pathFolder)
        {
            List<string> list = new List<string>();
            //lấy trong file appsettings.json có key là folder để lấy các đường dẫn file
            try
            {
                list = Directory.GetFiles(pathFolder).ToList();
            }
            catch (Exception ex)
            {
                _logException.LogErrorExceptionParameter(ex, nameof(pathFolder), pathFolder == null ? "NULL" : pathFolder);
            }
            return list;
        }

        public List<string> GetAllLines()
        {
            List<string> allLines = new List<string>();
            //lấy các đường dẫn
            List<string> listDirs = GetAllDirectoryNames(_pathFolder);
            foreach(var dir in listDirs)
            {
                //dùng function readFiles để đọc lần lượt dữ liệu từ các file
                List<string> listLines = ReadFiles(dir);
                if (listLines.Count == 0)
                {
                    break;
                }
                // AddRange add các list lại với nhau thành 1 list chung 
                try
                {
                    allLines.AddRange(listLines);
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(listLines), (listLines == null) ? "NULL" : "");
                    break;
                }
            }
            return allLines;
        }

        public List<string> ReadFiles(string path)
        {
            List<string> list = new List<string>();
            //đọc các dòng trong file và add vào list
            StreamReader reader;
            string line;
            try
            {
                reader = new StreamReader(path);
                //kiểm tra nếu còn dòng thì add vào list
                try
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        list.Add(line);
                    }
                }
                catch (OutOfMemoryException ex)
                {
                    _logException.LogErrorExceptionParameter(ex,"","");
                }

            }
            catch (ArgumentNullException ex)
            {
                _logException.LogErrorExceptionParameter(ex, nameof(path), path == null ? "NULL" : path);
            }
            catch (FileNotFoundException ex)
            {
                _logException.LogErrorExceptionParameter(ex, nameof(path), path == null ? "NULL" : path);
            }

            return list;
        }
    }
}
