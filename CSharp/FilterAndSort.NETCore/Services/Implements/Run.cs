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
        private readonly ILogFile _logger;

        public Run(IGetData getData,IHandleData handleData, ILogFile logger)
        {
            _getData = getData;
            _handleData = handleData;
            _logger = logger;
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
            _logger.LogInformation();
        }
    }
}
