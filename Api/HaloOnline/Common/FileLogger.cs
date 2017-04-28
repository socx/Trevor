using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace HaloOnline.Common
{
    public class FileLogger : ILogger
    {
        static ReaderWriterLock locker = new ReaderWriterLock();

        /// <summary>
        /// File Path fo error log file.
        /// </summary>
        public String ErrorLogFilePath { get; set; }

        /// <summary>
        /// Log error message and stack trace in error log file
        /// </summary>
        /// <param name="exception"></param>
        public void LogError(Exception exception)
        {
            var message = new StringBuilder(exception.Message);
            System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace(exception, true);
            message.AppendLine("Stack Trace" + exception.StackTrace);
            message.AppendLine("FileName : " + stackTrace.GetFrame(stackTrace.FrameCount - 1).GetFileName());
            message.AppendLine("Method : " + stackTrace.GetFrame(stackTrace.FrameCount - 1).GetMethod());
            message.AppendLine("Line Number : " + stackTrace.GetFrame(stackTrace.FrameCount - 1).GetFileLineNumber());
            message.AppendLine("Column Number : " + stackTrace.GetFrame(stackTrace.FrameCount - 1).GetFileColumnNumber());

            Log(DateTime.Now.ToString() + "\t(ERROR) \t===>" + message);
        }

        public void LogInfo(string message)
        {
            Log(DateTime.Now.ToString() + "\t(INFO) \t===>" + message);
        }

        private void Log(string message)
        {
            try
            {
                locker.AcquireWriterLock(int.MaxValue);
                string filename = $"{DateTime.Today.ToString("yyyy_MM_dd")}.txt";
                string fullPath = Path.Combine(ErrorLogFilePath, filename);
                if (!File.Exists(fullPath))
                {
                    File.Create(fullPath).Close();
                }
                //File.AppendAllText(fullPath, message + Environment.NewLine);

                string currentContent = String.Empty;
                currentContent = File.ReadAllText(fullPath);
                File.WriteAllText(fullPath, message + Environment.NewLine + currentContent);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                locker.ReleaseWriterLock();
            }
        }
    }
}