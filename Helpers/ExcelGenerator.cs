using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Helpers
{
    public class ExcelGenerator
    {
        /// <summary>
        /// Generate a csv file on the fly
        /// </summary>
        public void GenerateCSVFromList()
        {
            var users = new List<User>();
            using (StreamWriter sw = File.CreateText("list.csv"))
            {
                foreach (var item in users)
                {
                    sw.WriteLine(item.GivenName + ";" + item.Surname + ";" + item.Mail);
                }
            }
        }

        public void ReadCSV()
        {
            var dedaList = new List<string>();
            using (StreamReader sw = File.OpenText("testutenti.csv"))
            {
                string data = sw.ReadLine();
                while ((data = sw.ReadLine()) != null)
                {
                    if (data.Contains("@"))
                    {
                        var dataArray = data.Split(';');
                        dedaList.Add(dataArray[1]);
                    }
                }
            }
        }
    }

    public class User
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
    }
}
