using System;
using System.Configuration;
using System.Threading.Tasks;

namespace WebpageScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            // get the website
            var websiteUrl = ConfigurationManager.AppSettings.Get("website");

            //output location
            var outputDirectory = ConfigurationManager.AppSettings.Get("outputdirectory");



            //var maxConcurrent = StrToIntDef(maxConcurrentStr, 10);

            //Create a webpage downloader with 100 download threads
            var scanner = new WebsiteScanner(outputDirectory);

            //scan and download the content of all links
            scanner.ScanAndDownLoadAsync(websiteUrl);

            Console.ReadKey();

        }

        public static int StrToIntDef(string s, int @default)
        {
            int number;
            if (int.TryParse(s, out number))
                return number;
            return @default;
        }

    }
}
