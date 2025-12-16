using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Captura.Windows.未来之窗
{
    public class 东方仙盟_LogHelper
    {
        // 日志文件路径，可根据需要修改
        private static string _logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");

        private static bool 未来之窗debug =false;// true;
        /// <summary>
        /// 追加写入日志信息
        /// </summary>
        /// <param name="message">日志内容</param>
        /// <param name="logFileName">日志文件名(可选，默认按日期命名)</param>
        public static void WriteLog(string message, string logFileName = null)
        {
            try
            {
                if (未来之窗debug == false)
                {
                    //关闭调试
                    return ;
                }
                // 确保日志目录存在
                if (!Directory.Exists(_logFilePath))
                {
                    Directory.CreateDirectory(_logFilePath);
                }

                // 确定日志文件名，默认按日期命名
                string fileName = string.IsNullOrEmpty(logFileName)
                    ? $"{DateTime.Now:yyyyMMdd}.log"
                    : $"{logFileName}_{DateTime.Now:yyyyMMdd}.log";

                // 完整的日志文件路径
                string fullPath = Path.Combine(_logFilePath, fileName);

                // 构建日志内容，包含时间戳
                string logContent = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}{Environment.NewLine}";

                // 追加写入日志，使用UTF8编码
                File.AppendAllText(fullPath, logContent, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                // 处理日志写入过程中的异常
                Console.WriteLine($"日志写入失败: {ex.Message}");
            }
        }
    }
}
