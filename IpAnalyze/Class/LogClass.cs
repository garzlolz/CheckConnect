using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace IpAnalyze
{
    class LogClass
    {
        public static class EventLog       //Log檔寫入函式
        {
            //public static string FilePath { get; set; }
            public static string flg = null;
            //按鍵操作
            public static void Write1(string format, params object[] arg)
            {
                flg = "1";
                Write(string.Format(format, arg));
            }
            //CALL StoreProcedure 的記錄
            public static void Write2(string format, params object[] arg)
            {
                flg = "2";
                Write(string.Format(format, arg));
            }
            //
            public static void Write3(string format, params object[] arg)
            {
                flg = "3";
                Write(string.Format(format, arg));
            }
            //INI的操作動作，（亂七八糟，但是關鍵可以看）
            public static void Write4(string format, params object[] arg)
            {
                flg = "4";
                Write(string.Format(format,arg));
            }
            //列印動作，調味按鍵，清機交班，COM PORT 操作LOG
            public static void Write5(string format, params object[] arg)
            {
                flg = "5";
                Write(string.Format(format, arg));
            }
            //喫茶使用
            public static void Write6(string format, params object[] arg)
            {
                flg = "6";
                Write(string.Format(format, arg));
            }
            public static void Write9(string format, params object[] arg)
            {
                flg = "9";
                string wecanstr = format.Replace("\n", "");
                wecanstr = wecanstr.Replace("\r", "");
                wecanstr = wecanstr.Replace("{", "｛");
                wecanstr = wecanstr.Replace("}", "｝");
                Write(wecanstr);
            }


            public static void Write(string message)
            {
                //if (string.IsNullOrEmpty(FilePath))
                //{
                //    FilePath = Directory.GetCurrentDirectory();
                //}
                try
                {
                    string FilePath = Application.StartupPath + "/LOG/";
                    //string FilePath = "\\LOG\\";
                    string filename = FilePath + string.Format("{0:yyyy_MM_dd}\\Log{0:'" + flg + "'}_{0:yyyy_MM_dd}.txt", DateTime.Now);
                    FileInfo finfo = new FileInfo(filename);
                    if (finfo.Directory.Exists == false)
                    {
                        finfo.Directory.Create();
                    }
                    string writeString = string.Format("{0:HH:mm:ss.fff} {1}", DateTime.Now, message) + Environment.NewLine;
                    File.AppendAllText(filename, writeString, Encoding.Unicode);
                }
                catch{ }
            }
        }
    }
}
