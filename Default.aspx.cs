using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebPageScanner
{
    public partial class _Default : Page
    {
        List<WordCnt> WCntSingle = new List<WordCnt>();
        List<WordCnt> WCntPair = new List<WordCnt>();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                lblMsg.Text = "";
        }
        public static List<WordCnt> WordCount(string content, int numWords = int.MaxValue)
        {
            var delimiterChars = new char[] { ' ' };
            List<WordCnt> WCnt = new List<WordCnt>();
            try
            {
                //Get the Count of Top 10 Words With maximum count
                WCnt = content
                    .Split(delimiterChars)
                    .Where(x => x.Length > 2)
                    .Select(x => x.ToLower())
                    .GroupBy(x => x)
                    .Select(x => new { Word = x.Key, Count = x.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(numWords)
                    .ToDictionary(x => x.Word, x => x.Count).Select(y => new WordCnt() { word = y.Key, count = y.Value }).ToList();
            }
            catch (Exception ex)
            {
                //Exception handling Here
            }
            return WCnt;
        }
        public static List<WordCnt> WordCountPair(List<string> content, int numWords = int.MaxValue)
        {
            List<WordCnt> WCnt = new List<WordCnt>();
            try
            {
                //Get the Count of Top 10 Pair Words With maximum count
                WCnt = content
                    .Select(x => x.ToString().ToLower())
                    .GroupBy(x => x)
                    .Select(x => new { Word = x.Key, Count = x.Count() })
                    .OrderByDescending(x => x.Count)
                    .Take(numWords)
                    .ToDictionary(x => x.Word, x => x.Count).Select(y => new WordCnt() { word = y.Key, count = y.Value }).ToList();
            }
            catch (Exception ex)
            {
                //Exception handling Here
            }
            return WCnt;
        }
        protected void BtnGetWordCount_Click(object sender, EventArgs e)
        {
            //string Url = "https://www.314e.com/";
            if (txtUrl.Text.Trim() != "")
            {
                string Url = txtUrl.Text;
                GetWebContent(Url, 1, new List<string>());

                if (WCntSingle.Count > 0)
                {
                    //Get Top 10 Count For Single Word
                    WCntSingle = WCntSingle.GroupBy(x => x.word).Select(x => new { Word = x.Key, Count = x.Sum(z => z.count) }).OrderByDescending(x => x.Count)
                        .Take(10)
                        .ToDictionary(x => x.Word, x => x.Count).Select(y => new WordCnt() { word = y.Key, count = y.Value }).ToList();
                    grdSingleword.DataSource = WCntSingle;
                    grdSingleword.DataBind();
                }

                GetWebContentPair(Url, 1, new List<string>());

                if (WCntPair.Count > 0)
                {
                    //Get Top 10 Count For Pair Word
                    WCntPair = WCntPair.GroupBy(x => x.word).Select(x => new { Word = x.Key, Count = x.Sum(z => z.count) }).OrderByDescending(x => x.Count)
                    .Take(10)
                    .ToDictionary(x => x.Word, x => x.Count).Select(y => new WordCnt() { word = y.Key, count = y.Value }).ToList();

                    grdPairword.DataSource = WCntPair;
                    grdPairword.DataBind();
                }
            }
            else
            {
                lblMsg.Text = "Please Enter URL.";
            }
        }
        public void GetWebContent(string url, int urlcnt, List<string> lstUrls)
        {
            string StringFromTheInput = url;
            string content = String.Empty;

            using (var client = new WebClient())
            {
                //Get the content from Web URL
                content = client.DownloadString(StringFromTheInput);
            }

            //Get the List of URLS from Web Page
            lstUrls = new GetUrls().GetAllUrls(content,txtUrl.Text);
            foreach (var item in lstUrls)
            {
                using (var client = new WebClient())
                {
                    //Get the content from Web URL
                    content = client.DownloadString(StringFromTheInput);
                }
                //Add Top 10 Words from Each URL
                WCntSingle.AddRange(WordCount(content, 10));
                if (urlcnt >= 4)
                    break;
                urlcnt++;
                //Call Recursive Function For Going upto 4 level
                GetWebContent(item, urlcnt, lstUrls);
            }
        }
        public void GetWebContentPair(string url, int urlcnt, List<string> lstUrls)
        {
            string StringFromTheInput = url;
            string content = String.Empty;
            using (var client = new WebClient())
            {
                //Get the content from Web URL
                content = client.DownloadString(StringFromTheInput);
            }

            //Get the List of URLS from Web Page
            lstUrls = new GetUrls().GetAllUrls(content,txtUrl.Text);
            var delimiterChars = new char[] { ' ' };
            foreach (var item in lstUrls)
            {
                try
                {
                    using (var client = new WebClient())
                    {
                        //Get the content from Web URL
                        content = client.DownloadString(StringFromTheInput);
                    }

                    //Logic To Find Out Pairs of Words
                    var contentpair = content.Split(delimiterChars);
                    var pairs = new List<string>();
                    for (int i = 0; i < contentpair.Length - 1; i++)
                    {
                        if (contentpair[i].Trim() != "" && contentpair[i + 1].Trim() != "")
                            pairs.Add(contentpair[i] + " " + contentpair[i + 1]);
                    }
                    //Add Top 10 Pair Words from Each URL
                    WCntPair.AddRange(WordCountPair(pairs, 10));
                    if (urlcnt >= 4)
                        break;
                    urlcnt++;
                    //Call Recursive Function For Going upto 4 level
                    GetWebContentPair(item, urlcnt, lstUrls);
                }
                catch (Exception ex)
                {
                    //Exception Handling Here
                }
            }
        }
    }
    
}