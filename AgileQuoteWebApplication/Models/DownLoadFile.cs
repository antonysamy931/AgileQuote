using System;
using System.Web.Mvc;


namespace AgileQuoteWebApplication
{
    public class DownloadFile : ActionResult
    {
        public DownloadFile(byte[] fileContent, string fileName)
        {
            FileContent = fileContent;
            FileName = fileName;
        }
        public DownloadFile(byte[] fileContent, string fileName,bool isInline)
        {
            FileContent = fileContent;
            FileName = fileName;
            IsInline = isInline;
        }

        private string FileName { get; set; }
        private byte[] FileContent { get; set; }
        public bool IsInline { get; set; }

        //Generate download code..
        public override void ExecuteResult(ControllerContext context)
        {
            if(context != null)
            {
                //assign download file name..
                if (IsInline)
                {
                    context.HttpContext.Response.AddHeader("content-disposition",
                                                      "inline; filename=" + FileName);
                }
                else
                {
                    context.HttpContext.Response.AddHeader("content-disposition",
                                                       "attachment; filename=" + FileName);
                }
                string extension = FileName.Substring(FileName.Length - 3, 3).ToLower();
                string returnValue = string.Empty;
                switch (extension)
                {
                    case "pdf": returnValue = "application/pdf";
                        break;
                    case "swf": returnValue = "application/x-shockwave-flash";
                        break;                    
                    default: returnValue = "application/octet-stream";
                        break; 
                }
                context.HttpContext.Response.ContentType = returnValue;                  
                //write byte array content in header to download the file.)
                if (FileContent.Length > 0)
                {
                    context.HttpContext.Response.BinaryWrite(FileContent); 
                }
                else
                {
                    context.HttpContext.Response.Write(string.Empty);
                }
            }
            
        }
    }
}