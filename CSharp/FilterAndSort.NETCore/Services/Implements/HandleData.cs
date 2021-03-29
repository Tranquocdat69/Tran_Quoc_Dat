using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace FilterAndSort.NETCore.Services.Implements
{
    class HandleData : IHandleData
    {
        //đọc dữ liệu từ file appsettings.json
        IConfiguration _configuration;
        public HandleData(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<string> Filter(List<string> data)
        {
            //lấy key là filter trong appsettings.json
            string keyFilter = _configuration["Filter"];
            List<string> listFilter = new List<string>();
            foreach(var d in data) 
            {
                //kiểm tra nếu chứa keyFilter thì add vào list
                if (d.Contains(keyFilter))
                {
                    listFilter.Add(d);
                }
            }
            return listFilter;
        }
        //regular expression để lấy tag = 52 và value của tag 52
        string regex = @"52=\d{8}\s+\d{1,2}:\d{1,2}:\d{1,2}[.\d]+";
        private int Comparer(string line1, string line2)
        {
            //lấy ra chuỗi khớp với regular expression
            string tag52_1 = Regex.Match(line1, regex).ToString();
            string tag52_2 = Regex.Match(line2, regex).ToString();
            //biến đổi thời gian sang giây
            double second_1 = ConvertDateTimeToSecond(tag52_1);
            double second_2 = ConvertDateTimeToSecond(tag52_2);
            //so sánh thời gian (giây) giữa các dòng để sắp xếp
            if (second_1 > second_2)
            {
                return 1;
            }
            else if (second_2 > second_1)
            {
                return -1;
            }
                return 0;
        }
        public List<string> Sort(List<string> data)
        {
            data.Sort(Comparer);
            return data;
        }

        //biến đổi value của tag 52 sang dạng giây để so sánh
        private double ConvertDateTimeToSecond(string dateTimeString)
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
                folder = Directory.CreateDirectory("Output").ToString();
            }
            string path = folder + "\\" + name;
            //ghi tất cả các dòng trong list data sang đường dẫn path
            File.WriteAllLines(path, data);
        }

        public List<string> ReplaceValueTag(List<string> data)
        {
            //lấy ra chuỗi chứa tag 34 và value của tag 34
            string regex = @"34=\d+";
            for (int i = 0; i < data.Count; i++)
            {
                //kiểm tra các dòng với regular expression
                string strRegex = Regex.Match(data[i], regex).ToString();
                if (strRegex != null)
                {
                    string replaceText = "34=" + (i + 1);
                    //thay thế giá trị của tag 34 bằng giá trị mới
                    data[i] = data[i].Replace(strRegex, replaceText);
                }
            }
            return data;
        }
    }
}
