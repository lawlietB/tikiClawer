using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using TinTrinhLibrary;

namespace tikiCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start...");
            
            //get name to log
            string str = DateTime.Now.ToString().Trim();
            string name_log = "";
            if (DateTime.Now.Hour < 10 && DateTime.Now.Hour > 12)
            {
                name_log += "tikidata" + str.Substring(0, 2) + str.Substring(3, 2) + str.Substring(6, 2) + "_" + str.Substring(9, 1) + str.Substring(11, 2) + str.Substring(14, 2) + str.Substring(17, 2) + ".txt";
            }
            else
            {
                name_log += "tikidata" + str.Substring(0, 2) + str.Substring(3, 2) + str.Substring(6, 2) + "_" + str.Substring(9, 2) + str.Substring(12, 2) + str.Substring(15, 2) + str.Substring(18, 2) + ".txt";
            }
           
            String filepath = "..\\..\\..\\data_tiki\\" + name_log;
            FileStream fs = new FileStream(@filepath, FileMode.Create);            
            StreamWriter sWriter = new StreamWriter(fs, Encoding.UTF8);     
             
            string html="";

            WebClient client = new WebClient();

            //duyet cac trang: co 4 trang, moi trang 25 san pham nhung khong the load trang thu 4............
            for (int i = 1; i <= 3; i++)
            {
                    html += client.Get("https://tiki.vn/bestsellers/sach-truyen-tieng-viet/c316?p=" + i, "https://tiki.vn/bestsellers/sach-truyen-tieng-viet/c316", "");
            }

            string _book = "data-price=\"(.*?)\" data-title=\"(.*?)\" data-brand=\"(.*?)\" data-category=\"(.*?)\">";
            string _review = "<p class=\"review\">((.*?))</p>";
            string _description = "<div class=\"description\">(.*?)<a href=";
            
            MatchCollection booksList = Regex.Matches(html, _book, RegexOptions.Singleline);
            MatchCollection reviewList = Regex.Matches(html, _review, RegexOptions.Singleline);
            MatchCollection descriptionList = Regex.Matches(html, _description, RegexOptions.Singleline);

            string _data = DateTime.Now.ToString().Trim() + "/r/n";

            for (int i = 0; i < booksList.Count; i++)
            {
                _data += booksList[i].Groups[2].Value.Trim() + "\r\n";
                _data += booksList[i].Groups[3].Value.Trim() + "\r\n";
                _data += booksList[i].Groups[1].Value.Trim() + "đ\r\n";
                _data += booksList[i].Groups[4].Value.Trim() + "\r\n";
                _data += reviewList[i].Groups[1].Value.Trim()+ "\r\n";
                _data += descriptionList[i].Groups[1].Value.Trim() + "\r\n\r\n";
            }
          
            //ghi ra file
            sWriter.WriteLine(_data);
            sWriter.Flush();
            fs.Close();

         
            Console.WriteLine("OK, Saved to: tikiCrawler\\data_tiki\\" + name_log);
            Console.ReadLine();
        }
    }
}




