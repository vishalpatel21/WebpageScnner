using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebPageScanner
{
    public class GetUrls
    {
        //using a regular expression, find all of the href or urls
        //in the content of the page
        public List<string> GetAllUrls(string content, string url)
        {
            List<string> lstUrls = new List<string>();
            try
            {
                Regex RegExpr = new Regex("https://([\\w+?\\.\\w+])+([a-zA-Z0-9\\~\\!\\@\\#\\$\\%\\^\\&amp;\\*\\(\\)_\\-\\=\\+\\\\\\/\\?\\.\\:\\;\\'\\,]*)?", RegexOptions.IgnoreCase);
                //Regex RegExpr = new Regex(pattern, RegexOptions.IgnoreCase);

                //get the first match
                Match match = RegExpr.Match(content);

                //loop through matches
                while (match.Success)
                {
                    //get next match
                    if (match.Groups[0].Value.Contains(url) && match.Groups[0].Value != url)
                        lstUrls.Add(match.Groups[0].Value);
                    match = match.NextMatch();
                }
            }
            catch (Exception ex)
            {
                //Exception Hadling here
            }
            return lstUrls.Distinct().ToList();
        }

        //Write to a log file for Any Exception
        private void WriteToLog(string file, string message)
        {
            using (StreamWriter w = File.AppendText(file))
            {
                w.WriteLine(DateTime.Now.ToString() + ": " + message); w.Close();
            }
        }
    }
    public class WordCnt
    {
        public string word { get; set; }
        public int count { get; set; }
    }
}