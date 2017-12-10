using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using OnlineConverter.Models;
using System.IO;

namespace OnlineConverter.Controllers
{
    public class ConvertController : Controller
    {

       
        // GET: fileConvert
       public ActionResult Converter()
        {
          //  ViewBag.Con = "test";
            return View("Converter");
        }
      
        [HttpPost]
       public ActionResult Converter(Aorp file)
        {
            if (file.pathImage != null)
            {
                var fileName = Path.GetFileName(file.pathImage.FileName);
                var path = Path.Combine(Server.MapPath("~/Content"), fileName);
                file.pathImage.SaveAs(path);

                if (fileName != "")
                {
                    convertImage(fileName, file.ratio);
                    fileName = "";
                }

            }

            if (file.path != null)
            {
                var fileName = Path.GetFileName(file.path.FileName);
                var path = Path.Combine(Server.MapPath("~/Content"), fileName);
                file.path.SaveAs(path);
          
                if (fileName != "")
                {
                    convertAudio(fileName, file.codec, file.bitrate);
                    fileName = "";
                }
                
            }
           
         
           // return RedirectToAction("Converter");
            return View("Converter");
         
        }

  

        public void convertImage(string fileName, int ratio)
        {
            string outFile = Path.Combine(Server.MapPath("~/Content/OutImage"), fileName.Substring(0, fileName.LastIndexOf('.') + 1)) + "jpg";
            string command = "-i \"" + Path.Combine(Server.MapPath("~/Content"), fileName) +
                            "\" -y -qscale " + ratio + " \"" + outFile + "\"";
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.FileName = @"D:\home\site\repository\ffmpeg.exe";
           // ffmpeg.StartInfo.FileName = @"ffmpeg.exe";
            ffmpeg.StartInfo.Arguments = command;
            ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ffmpeg.Start();
            ffmpeg.WaitForExit();

            FileInfo fileInfo = new FileInfo(outFile);
            try
            {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileInfo.Name + "\"");

                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "text/plain";
                Response.TransmitFile(fileInfo.FullName);
                Response.Flush();

            }
            finally
            {
                System.IO.File.Delete(fileInfo.FullName);
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content"), fileName));

            }

        }

        public string convertAudio(string fileName, int codec, int bitrate)
        {
            --codec; --bitrate;
            List<string> codecs = new List<string>() { "mp3", "flac", "libopus", "aac" };
            List<string> bitrates = new List<string>() { "96k", "192k", "256k", "320k", "1140k"};
            string outEx = codecs[codec];
            string outBitrate = bitrates[bitrate];
            if (codec == 2)
                outEx = "opus";

            string outFile = Path.Combine(Server.MapPath("~/Content/OutAudio"), fileName.Substring(0, fileName.LastIndexOf('.') + 1)) + outEx;
            string command = "-i \"" + Path.Combine(Server.MapPath("~/Content"), fileName) +
                            "\" -y -map 0:a:0 -b:a " + outBitrate +" -c:a " + codecs[codec] + " \"" + outFile + "\"";
            Process ffmpeg = new Process();
            ffmpeg.StartInfo.CreateNoWindow = true;
            ffmpeg.StartInfo.FileName = @"D:\home\site\repository\ffmpeg.exe";
           // ffmpeg.StartInfo.FileName = @"ffmpeg.exe";
            ffmpeg.StartInfo.Arguments = command;
            ffmpeg.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ffmpeg.Start();
            ffmpeg.WaitForExit();

            FileInfo fileInfo = new FileInfo(outFile);
            try {
                Response.Clear();
                Response.ClearHeaders();
                Response.ClearContent();
                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileInfo.Name + "\"");

                Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                Response.ContentType = "text/plain";
                Response.TransmitFile(fileInfo.FullName);
                Response.Flush();
             
            }
            finally {
                System.IO.File.Delete(fileInfo.FullName);
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Content"), fileName));
               
            }


            return "test";
        }
    }
}