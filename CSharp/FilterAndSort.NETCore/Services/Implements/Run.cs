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
            //in ra file log.txt
            DateTime startTime = DateTime.Now;
            //gọi các function để lấy, lọc, sắp xếp, ghi dữ liệu
            List<string> allLines = _getData.getAllLines();
            List<string> filterdData = _handleData.Filter(allLines);
            List<string> sortedList = _handleData.Sort(filterdData);
            List<string> completedList = _handleData.ReplaceValueTag(sortedList);
            _handleData.WiteLinesToFile(completedList);
            _logFile.LogInformation(startTime);
            //DateTime endTime = DateTime.Now;
            ////tổng thời gian (ms) chạy chương trình = endTime - startEnd
            //double totalMilliseconds = endTime.Subtract(startTime).TotalMilliseconds;
            //_logger.LogInformation("begin=" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss.fff"));
            //_logger.LogInformation("end=" + DateTime.Now.ToString("yyyy-mm-dd HH:mm:ss.fff"));
            //_logger.LogInformation("ElapsedMilliseconds = {totalMilliseconds}", totalMilliseconds);
        }
    }
}
