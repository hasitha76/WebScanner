WEB SCANNER
===========

- asynchronous DownloadUrlsAsync() method which is used to download webpages when provided a valid URL.
- download done by using the C# HttpClient
- recursive scan funtionality DownloadUrlsAsync(),it recursively find href urls within parent webpage,and all content 
  downloaded
- all successfull and unsuccessfull attempts are logged to downloadLog.txt 
- output directory and given website can be configured in app.config file