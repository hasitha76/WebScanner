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

            //Create a webpage downloader 
            var scanner = new WebsiteScanner(websiteUrl,outputDirectory);

            //scan and download the content of all links
            scanner.ScanAndDownLoadAsync();

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
