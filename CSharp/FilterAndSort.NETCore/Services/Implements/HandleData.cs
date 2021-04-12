using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FilterAndSort.NETCore.Services.Implements
{
    public class HandleData : IHandleData
    {
        private readonly ILogFile _logException;


        //đọc dữ liệu từ file appsettings.json
        IConfiguration _configuration;
        public HandleData(IConfiguration configuration, ILogFile logException)
        {
            _logException = logException;
            _configuration = configuration;
        }
        public List<string> Filter(List<string> data)
        {
            //lấy key là filter trong appsettings.json
            string keyFilter = _configuration["Filter"];
            List<string> listFilter = new List<string>();
            foreach (var d in data) 
            {
                //kiểm tra nếu chứa keyFilter thì add vào list
                var check = false;
                try
                {
                    check = d.Contains(keyFilter);
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(keyFilter), keyFilter == null ? "NULL" : "");
                    break;
                }

                if (check)
                {
                    listFilter.Add(d);
                }
            }
            return listFilter;
        }

        //regular expression để lấy tag = 52 và value của tag 52
        string regex_52 = @"52=\d{8}\s+\d{1,2}:\d{1,2}:\d{1,2}[.\d]+";
        string regex_34 = @"34=\d+";
        private int Comparer(string line1, string line2)
        {
            string tag52_1 = "";
            string tag52_2 = "";

            if (line1 != null && line2 != null)
            {
                try
                {
                    tag52_1 = Regex.Match(line1, regex_52).ToString();
                    tag52_2 = Regex.Match(line2, regex_52).ToString();
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(regex_52), (regex_52 == null || regex_52 == "") ? "NULL" : "");
                    _logException.LogInformation();
                    Environment.Exit(0);
                }catch (RegexMatchTimeoutException rmte)
                {
                    _logException.LogErrorExceptionParameter(rmte, nameof(regex_52), (regex_52 == null || regex_52 == "") ? "NULL" : regex_52);
                    _logException.LogInformation();
                    Environment.Exit(0);
                }
            }
            else
            {
                return 0;
            }

            //biến đổi thời gian sang giây
            double second_1 = -1;
            double second_2 = -2;

            if (tag52_1.Length > 0)
            {
                second_1 = ConvertDateTimeToSecond(tag52_1);
            }

            if (tag52_2.Length > 0)
            {
                second_2 = ConvertDateTimeToSecond(tag52_2);
            }
            //so sánh thời gian (giây) giữa các dòng để sắp xếp
            if (second_1 < second_2)
            {
                return -1;
            }
            else if (second_1 == second_2)
            {
                string tag34_1 = "";
                string tag34_2 = "";
                try
                {
                    tag34_1 = Regex.Match(line1, regex_34).ToString();
                    tag34_2 = Regex.Match(line2, regex_34).ToString();
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(regex_34), (regex_34 == null || regex_34 == "") ? "NULL" : "");
                    _logException.LogInformation();
                    Environment.Exit(0);
                }
                catch (RegexMatchTimeoutException rmte)
                {
                    _logException.LogErrorExceptionParameter(rmte, nameof(regex_34), (regex_34 == null || regex_34 == "") ? "NULL" : regex_34);
                    _logException.LogInformation();
                    Environment.Exit(0);
                }

                int value34_1 = int.Parse(tag34_1.Split("=")[1]);
                int value34_2 = int.Parse(tag34_2.Split("=")[1]);

                if (value34_1 < value34_2)
                {
                    return -1;
                }else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        public List<string> Sort(List<string> data)
        {
            data.Sort(Comparer);
            return data;
        }

        //biến đổi value của tag 52 sang dạng giây để so sánh
        public double ConvertDateTimeToSecond(string dateTimeString)
        {
            //tách tag 52 và value thành 1 array
            string[] splitString = dateTimeString.Split("=");
            //lấy array tại vị trí 1 => value của tag
            string value = splitString[1];
            //dùng hàm substring, cắt chuỗi theo index và length để lấy ra dữ liệu tương ứng theo năm tháng ngày,....
            int year = int.Parse(value.Substring(0, 4));
            int month = int.Parse(value.Substring(4, 2));
            int day = int.Parse(value.Substring(6, 2));
            int hour = int.Parse(value.Substring(9, 2));
            int minute = int.Parse(value.Substring(12, 2));
            double second = double.Parse(value.Substring(15));
            //lấy datetime của dữ liệu trừ đi giá trị minvalue (giá trị cố định) 
            return new DateTime(year, month, day).Subtract(DateTime.MinValue).TotalSeconds + (hour * 60 * 60) + (minute * 60) + second;
        }

        public void WiteLinesToFile(List<string> data)
        {
            //lấy tên folder và tên file output trong file appsettings.json
            string folder = _configuration["FolderOutput"];
            string name = _configuration["FileOutput"];
            //kiểm tra nếu đường dẫn folder trống thì tạo mới 1 folder tên là Output
            if (folder == "")
            {
                try
                {
                    folder = Directory.CreateDirectory("D:\\temp2\\MDDS_DATA\\ALL_OUTPUT_DAT").ToString();
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(folder), (folder == null || folder == "") ? "NULL" : folder);
                }
                catch (UnauthorizedAccessException uae)
                {
                    _logException.LogErrorExceptionParameter(uae, nameof(folder), (folder == null || folder == "") ? "NULL" : folder);
                }
                catch (PathTooLongException ple)
                {
                    _logException.LogErrorExceptionParameter(ple, nameof(folder), folder);
                }
                catch (IOException ioe)
                {
                    _logException.LogErrorExceptionParameter(ioe, nameof(folder), (folder == null || folder == "") ? "NULL" : folder);
                }
            }
            else
            {
                //nếu FolderOutput khác rỗng thì kiểm tra nếu chưa tồn tại đường dẫn folder thì tạo mới 
                if (!Directory.Exists(folder))
                {
                    try
                    {
                        Directory.CreateDirectory(folder);

                    }
                    catch (UnauthorizedAccessException uae)
                    {
                        _logException.LogErrorExceptionParameter(uae, nameof(folder), folder);
                    }
                    catch (PathTooLongException ple)
                    {
                        _logException.LogErrorExceptionParameter(ple, nameof(folder), folder);
                    }
                    catch (IOException ioe)
                    {
                        _logException.LogErrorExceptionParameter(ioe, nameof(folder), folder);
                    }
                }
            }
            string path = folder + "\\" + name;
            //ghi tất cả các dòng trong list data sang đường dẫn path
            try
            {
                File.WriteAllLines(path, data);
            }
            catch (ArgumentNullException ane)
            {
                _logException.LogErrorExceptionParameter(ane, nameof(path), (path == null || path == "") ? "NULL" : "");
            }
            catch (DirectoryNotFoundException dne)
            {
                _logException.LogErrorExceptionParameter(dne, nameof(path), path);
            }
            catch (UnauthorizedAccessException uae)
            {
                _logException.LogErrorExceptionParameter(uae, nameof(path), path);
            }
        }

        public List<string> ReplaceValueTag(List<string> data)
        {
            //lấy ra chuỗi chứa tag 34 và value của tag 34
            string regex = @"34=\d+";
            for (int i = 0; i < data.Count; i++)
            {
                //kiểm tra các dòng với regular expression
                string strRegex = "";
                try
                {
                    strRegex = Regex.Match(data[i], regex).ToString();
                }
                catch (ArgumentNullException ane)
                {
                    _logException.LogErrorExceptionParameter(ane, nameof(regex), (regex == null) ? "NULL" : "");
                    break;
                }
                catch (RegexMatchTimeoutException rmte)
                {
                    _logException.LogErrorExceptionParameter(rmte, nameof(regex), regex);
                    break;
                }
                if (strRegex.Length > 0)
                {
                    string replaceText = "34=" + (i + 1); ;
                    //thay thế giá trị của tag 34 bằng giá trị mới
                    data[i] = data[i].Replace(strRegex, replaceText);
                }
            }
            return data;
        }
    }
}
