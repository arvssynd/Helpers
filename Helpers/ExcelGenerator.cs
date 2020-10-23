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
    }

    public class User
    {
        public string GivenName { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
    }
}
