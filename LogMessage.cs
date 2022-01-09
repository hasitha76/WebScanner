using System;
using System.Collections.Generic;
using System.Text;

namespace WebpageScanner
{
    public class LogMessage
    {
        public string Filepath { get; set; }
        public string Text { get; set; }

        public LogMessage(string FilePath, string MessageText)
        {
            Filepath = FilePath;
            Text = MessageText;
        }
    }
}
