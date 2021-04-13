using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FilterAndSort.NETCore.Services.Implements
{
    class Run : IRun
    {
        private readonly IGetData _getData;
        private readonly IHandleData _handleData;
        private readonly ILogger<Run> _logger;
        private readonly ILogFile _logFile;

        public Run(IGetData getData,IHandleData handleData, ILogger<Run> logger, ILogFile logFile)
        {
            _getData = getData;
            _handleData = handleData;
            _logger = logger;
            _logFile = logFile;
        }
        public void RunApp()
        {
            DateTime startTime = DateTime.Now;
            //gọi các function để lấy, lọc, sắp xếp, ghi dữ liệu
            List<string> allLines = _getData.getAllLines();
            List<string> filterdData = _handleData.Filter(allLines);
            List<string> sortedList = _handleData.Sort(filterdData);
            List<string> completedList = _handleData.ReplaceValueTag(sortedList);
            _handleData.WiteLinesToFile(completedList);
            //in ra file log.txt
            _logFile.LogInformation(startTime);
        }
    }
}
