using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace WebpageScanner
{
    public class WebsiteScanner
    {
        public static CancellationToken CancelToken;

        private string _logPath;
        private string _baseUrl; 


        private List<LogMessage> logMessages = new List<LogMessage>();

        private List<WebpageItem> webpageList = new List<WebpageItem>();

        public WebsiteScanner( string outputDirectory)
        {

            CancelToken = new CancellationToken();
            _logPath = outputDirectory;
            
        }

        private string GetBasePath(string baseUrl)
        {
            Uri webUri = new Uri(baseUrl);
            string host = webUri.Host.Replace("www.", "");
            int index = host.LastIndexOf(".");
            host = host.Substring(0, index);

            return host;
        }

        private async Task DownloadUrlsAsync(string url)
        {
            HttpClient client;
            bool successResponse = false;

            try
            {
                
                using (client = new HttpClient() { Timeout = TimeSpan.FromMinutes(1) })
                {
   
                    HttpResponseMessage response = await client.GetAsync(url, CancelToken).ConfigureAwait(false);
                    successResponse = response.IsSuccessStatusCode;

                    //Throw exception for bad reponse code.
                    response.EnsureSuccessStatusCode();

                    //Download content for good response code.
                    using (HttpContent content = response.Content)
                    {
                        //Get the WebpageItem data .
                        string responseUri = response.RequestMessage.RequestUri.ToString();
                        WebpageItem webpageItem = null;


                        string html = await content.ReadAsStringAsync().ConfigureAwait(false);

                        webpageItem = new WebpageItem(url, html, responseUri);
                        WriteToLog(url + "|Success" + "\r\n");
                        webpageList.Add(webpageItem);


                        Console.WriteLine(url + "|Success");

                        var allOtherUrls = GetAllUrls(html);

                        // recursively scan other web pages
                        // prevent getting same url
                        // take only pages which contain base url
                        foreach (string uriString in allOtherUrls)
                        {

                            if (!webpageList.Any(x=>x.Url==uriString) && uriString.Contains(_baseUrl))
                            {
                                await DownloadUrlsAsync(uriString);
                            }
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                
                WriteToLog(url + "|FAILED|" + e.Message + "\r\n");
                
            }
        }

        /// <summary>
        /// Returns all urls in string content
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private string[] GetAllUrls(string str)
        {
            string pattern = @"<a.*?href=[""'](?<url>.*?)[""'].*?>(?<name>.*?)</a>";

            MatchCollection matches = Regex.Matches(str, pattern, RegexOptions.IgnoreCase);

            string[] matchList = new string[matches.Count];

            int c = 0;

            foreach (Match match in matches)
                matchList[c++] = match.Groups["url"].Value;

            return matchList;
        }


        /// <summary>
        ///  write to the log file
        /// </summary>
        /// <param name="Message"></param>
        private void WriteToLog(string message)
        {
            logMessages.Add(new LogMessage(_logPath + "downloadLog.txt", message));
        }

        /// <summary>
        /// Dedicates a single background thread to writing log messages so there is no file contention.
        /// </summary>
        private void StartLogger()
        {
            var downloadPages = Task.Factory.StartNew(() =>
            {
                foreach (var logMessage in logMessages)
                {
                    File.AppendAllText(logMessage.Filepath, logMessage.Text);
                }
            });
        }

        /// <summary>
        /// Download content and save it in a Disk
        /// </summary>
        private void StartDownload()
        {
            foreach (var webpageItem in webpageList)
            {
                if (webpageItem.Error == false)
                {
                    //Save the downloaded html
                    string fileName = webpageItem.Url.GetHashCode().ToString();
                    string filePath = _logPath + fileName;
                    if (!File.Exists(filePath))
                    {
                        File.WriteAllText(filePath, webpageItem.Html);

                    }

                }
            }
        }

        public async void ScanAndDownLoadAsync(string url)
        {
            // set baseurl
            _baseUrl = GetBasePath(url);

            // Async call to download the URL provided
            await DownloadUrlsAsync(url);

            // start logger
            StartLogger();

            // start download
            StartDownload();

            Console.WriteLine("Done.");
        }





    }
}
