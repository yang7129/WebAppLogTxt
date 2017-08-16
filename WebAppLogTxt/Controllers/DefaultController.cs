using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAppLogTxt.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
        #region Logs
        /// <summary> 
        /// 紀錄資料純放文字檔 <para></para>  
        /// 只會刪除存放天數小於6個月的txt檔案 
        /// </summary> 
        /// <param name="Msg">訊息</param>  
        public void LogTxt(string Msg)
        {
            if (!string.IsNullOrEmpty(Msg))
                LogTxt(Msg, "log", 180);
        }
        /// <summary> 
        /// 紀錄資料純放文字檔 <para></para>  
        /// 可自訂存放天數的txt檔案 
        /// </summary> 
        /// <param name="Msg">訊息</param> 
        /// <param name="directory">自訂資料夾(選填)</param> 
        /// <param name="retainDay">存放天數(選填)</param>  
        public void LogTxt(string Msg, string directory = "log", int retainDay = 180)
        {
            DeleteFile("*.txt", retainDay, directory);

            if (!string.IsNullOrEmpty(Msg))
            {
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                try
                {
                    System.IO.File.AppendAllText(string.Format(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory) + "/Log-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")), string.Concat(new object[] { Msg, Environment.NewLine }));
                }
                catch (Exception Ex)
                {
                    System.IO.File.AppendAllText(string.Format(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory) + "/Log-{0}.txt", DateTime.Now.ToString("yyyy-MM-dd")), string.Concat(new object[] { "=============Error===============" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + Environment.NewLine + Ex.ToString() + Environment.NewLine + "=============Error===============", Environment.NewLine }));
                }
            }
        }
        private void DeleteFile(string FilePattern, int retainDay, string directory)
        {
            try
            {
                System.IO.Directory.CreateDirectory(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                String[] FileCollection;
                if (string.IsNullOrEmpty(FilePattern))
                    FileCollection = System.IO.Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory));
                else
                    FileCollection = System.IO.Directory.GetFiles(System.Web.Hosting.HostingEnvironment.MapPath("~/" + directory), FilePattern);

                for (int i = 0; i < FileCollection.Length; i++)
                {
                    System.IO.FileInfo theFileInfo = new System.IO.FileInfo(FileCollection[i]);
                    TimeSpan TIS = DateTime.Now.Subtract(theFileInfo.LastWriteTime);
                    if (TIS.TotalDays > retainDay)
                        System.IO.File.Delete(theFileInfo.FullName);
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion
    }
}