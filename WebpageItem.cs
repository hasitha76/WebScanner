using System;
using System.Collections.Generic;
using System.Text;

namespace WebpageScanner
{
    public class WebpageItem
    {
        private string _Url;
        private string _Html;
        private string _ResponseUrl;
        private bool _Error;
        private string _ErrorMessage;
        private bool _ServerSuccessReponse;


        public WebpageItem(string Url, string Html, string ResponseUrl)
        {
            _Url = Url;
            _Html = Html;
            _ResponseUrl = ResponseUrl;
            _Error = false;
            _ErrorMessage = "";
            _ServerSuccessReponse = true;

        }

        public WebpageItem(string Url, bool Error, string ErrorMessage, bool ServerSuccessReponse)
        {
            _Url = Url;
            _Html = "";
            _ResponseUrl = "";
            _Error = Error;
            _ErrorMessage = ErrorMessage;
            _ServerSuccessReponse = ServerSuccessReponse;
        }





        public string Url
        {
            get { return _Url; }
        }

        public string Html
        {
            get { return _Html; }
        }

        public string ResponseUrl
        {
            get { return _ResponseUrl; }
        }

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
        }

        public bool Error
        {
            get { return _Error; }
        }

        public bool ServerSuccessReponse
        {
            get { return _ServerSuccessReponse; }
        }


    }
}
