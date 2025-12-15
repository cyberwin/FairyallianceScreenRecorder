using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Captura;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger
{
    public static class Log_Engine
    {
        private static string logOutConfigFilePath = Application.StartupPath + "/CyberWinPHP/CyberWinPHP_config/LogConfig.cyberphp";

        private static bool isNotOutputLogMain(string type) => false;

        private static bool isNotOutputLogChild(string type) => false;

        public static string logUUid() => Guid.NewGuid().ToString();

        public static void write_log(string capturetype, string type, string s) => Log_Engine.write_log(Application.StartupPath + "/", capturetype, type, s);

        public static void write_log(string LogFolderPath, string capturetype, string type, string s)
        {
            try
            {
                if (Log_Engine.isNotOutputLogMain(capturetype))
                    return;
                string path = LogFolderPath + "/log/" + DateTime.Now.ToLongDateString() + "/" + capturetype + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (Log_Engine.isNotOutputLogChild(type))
                    return;
                FileStream fileStream = new FileStream(path + type + "_log.log", FileMode.Append);
                StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
                string str1 = Log_Engine.logUUid();
                DateTime now = DateTime.Now;
                string longDateString = now.ToLongDateString();
                string str2 = "日志ID：" + str1 + "\r\n====================================================================================\r\n" + longDateString;
                now = DateTime.Now;
                string longTimeString = now.ToLongTimeString();
                streamWriter.WriteLine(str2 + longTimeString + "<<<<<<<<<<<<<<<<<<<<<<<<<<");
                streamWriter.WriteLine(s);
                streamWriter.WriteLine("===============================================================================>>>>>>>end<<<<<<<<===============================================================================");
                streamWriter.WriteLine("");
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        public static void write_logV2(string capturetype, string type, string s)
        {
            try
            {
                string path = Application.StartupPath + "/log/" + DateTime.Now.ToLongDateString() + "/" + capturetype + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                FileStream fileStream = new FileStream(path + type + "_log.log", FileMode.Append);
                StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
                streamWriter.WriteLine("日志ID：" + Log_Engine.logUUid() + "==============================\r\n" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + "<<<<<<<<<<<<<<<<<<<<<<<<<<");
                streamWriter.WriteLine(s);
                streamWriter.WriteLine("");
                streamWriter.Flush();
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 将 IBitmapFrame 转换为 Bitmap（假设像素格式为 32位ARGB，即 4字节/像素）
        /// </summary>
        /// <param name="frame">IBitmapFrame 实例</param>
        /// <returns>转换后的 Bitmap</returns>
        public static Bitmap 未来之窗ToBitmap(this IBitmapFrame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            // 1. 确认像素格式（这里假设为 32bppArgb，即每个像素 4 字节，需根据实际情况调整）
            const PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            int bytesPerPixel = Image.GetPixelFormatSize(pixelFormat) / 8; // 4 字节/像素

            // 2. 计算缓冲区大小（宽度 × 高度 × 每个像素字节数）
            int bufferSize = frame.Width * frame.Height * bytesPerPixel;
            byte[] pixelBuffer = new byte[bufferSize];

            // 3. 从 IBitmapFrame 复制像素数据到缓冲区
            frame.CopyTo(pixelBuffer);

            // 4. 创建 Bitmap 并写入像素数据
            using (var bitmap = new Bitmap(frame.Width, frame.Height, pixelFormat))
            {
                // 锁定 Bitmap 的像素区域，获取内存地址
                Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
                BitmapData bitmapData = bitmap.LockBits(
                    rect,
                    ImageLockMode.WriteOnly,
                    pixelFormat
                );

                try
                {
                    // 将字节数组中的像素数据复制到 Bitmap 的内存区域
                    Marshal.Copy(pixelBuffer, 0, bitmapData.Scan0, bufferSize);
                }
                finally
                {
                    // 解锁像素区域（必须执行，否则 Bitmap 会损坏）
                    bitmap.UnlockBits(bitmapData);
                }

                // 返回一个新的 Bitmap 副本（避免原 bitmap 被 Dispose 影响）
                return new Bitmap(bitmap);
            }
        }

       
        public static void write_logFrame(string capturetype, string type, IBitmapFrame frame)
        {
            Bitmap frameBitmap = 未来之窗ToBitmap(frame);
            try
            {
               

                string path = Application.StartupPath + "/log/" + DateTime.Now.ToLongDateString() + "/" + capturetype + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // 定义保存路径（例如保存为 PNG 格式）
                string savePath = path + type + DateTime.Now.Ticks + ".png";

                // 保存图片（支持格式：.bmp, .jpg, .png, .gif 等）
                frameBitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 释放 Bitmap 资源（如果不再使用）
                frameBitmap.Dispose();
            }
        }
    

    public static void write_logImg(string capturetype, string type, Bitmap frameBitmap )
        {
            try
            {
                string path = Application.StartupPath + "/log/" + DateTime.Now.ToLongDateString() + "/" + capturetype + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                // 定义保存路径（例如保存为 PNG 格式）
                string savePath = path + type + DateTime.Now.Ticks + ".png";

                // 保存图片（支持格式：.bmp, .jpg, .png, .gif 等）
                frameBitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Png);

            }
            catch (Exception ex)
            {
            }
        }
    }
}
