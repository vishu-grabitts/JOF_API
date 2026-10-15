using System;
using System.IO;
using System.Web;

namespace POD.DAL
{
    public static class Helper
    {
        // Log an Exception
        public static void LogException(Exception exc, string source = null)
        {
            try
            {
                // Include logic for logging exceptions
                // Get the absolute path to the log file
                string logFile = "~/Content/ErrorLog_API.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);

                // Open the log file for append and write the log
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine("********** {0} **********", Helper.GetCurrentDateTime());
                if (exc.InnerException != null)
                {
                    sw.Write("Inner Exception Type: ");
                    sw.WriteLine(exc.InnerException.GetType().ToString());
                    sw.Write("Inner Exception: ");
                    sw.WriteLine(exc.InnerException.Message);
                    sw.Write("Inner Source: ");
                    sw.WriteLine(exc.InnerException.Source);
                    if (exc.InnerException.StackTrace != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.StackTrace);
                    }

                    if (exc.InnerException != null && exc.InnerException.InnerException != null)
                    {
                        sw.WriteLine("Inner Stack Trace: ");
                        sw.WriteLine(exc.InnerException.InnerException.Message);
                        sw.WriteLine(exc.InnerException.InnerException.Source);
                        sw.WriteLine(exc.InnerException.InnerException.InnerException + "");
                    }

                }
                sw.Write("Exception Type: ");
                sw.WriteLine(exc.GetType().ToString());
                sw.WriteLine("Exception: " + exc.Message);
                sw.WriteLine("Source: " + source);
                sw.WriteLine("Stack Trace: ");
                if (exc.StackTrace != null)
                {
                    sw.WriteLine(exc.StackTrace);
                    sw.WriteLine();
                }
                sw.Close();
            }catch(Exception ex) { }
        }

        public static void LogMessage(string  message)
        {
            try
            {
                // Include logic for logging exceptions
                // Get the absolute path to the log file
                string logFile = "~/Content/ErrorLog_API.txt";
                logFile = HttpContext.Current.Server.MapPath(logFile);

                // Open the log file for append and write the log
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine("********** {0} **********", Helper.GetCurrentDateTime());
                sw.Write(message);
                sw.Close();
            }
            catch (Exception ex) { }
        }

        public static void LogException(string fileName, string error)
        {
            try
            {
                // Include logic for logging exceptions
                // Get the absolute path to the log file
                string logFile = "~/Content/Log/" + fileName;
                logFile = HttpContext.Current.Server.MapPath(logFile);

                // Open the log file for append and write the log
                StreamWriter sw = new StreamWriter(logFile, true);
                sw.WriteLine("********** {0} **********", Helper.GetCurrentDateTime());
                sw.WriteLine(error);

                sw.Close();
            }
            catch (Exception ex) { }
        }

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }

}
