using Captura;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWin_Effect
{
    // 扩展方法类：提供 IBitmapFrame 与 Bitmap 的互转
    public static class BitmapFrameExtensions
    {
        // IBitmapFrame 转 Bitmap
        public static Bitmap ToBitmap(this IBitmapFrame frame)
        {
            if (frame == null)
                throw new ArgumentNullException(nameof(frame));

            // 假设像素格式为 32bppArgb（4字节/像素）
            PixelFormat pixelFormat = PixelFormat.Format32bppArgb;
            int bytesPerPixel = Image.GetPixelFormatSize(pixelFormat) / 8;
            int bufferSize = frame.Width * frame.Height * bytesPerPixel;

            // 从 frame 复制像素数据
            byte[] pixelBuffer = new byte[bufferSize];
            frame.CopyTo(pixelBuffer);

            // 创建 Bitmap 并写入数据
            using (var bitmap = new Bitmap(frame.Width, frame.Height, pixelFormat))
            {
                Rectangle rect = new Rectangle(0, 0, frame.Width, frame.Height);
                BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.WriteOnly, pixelFormat);
                try
                {
                    Marshal.Copy(pixelBuffer, 0, bitmapData.Scan0, bufferSize);
                }
                finally
                {
                    bitmap.UnlockBits(bitmapData);
                }
                return new Bitmap(bitmap); // 返回副本避免资源释放问题
            }
        }

        // Bitmap 转 IBitmapFrame
        public static IBitmapFrame ToBitmapFrame(this Bitmap bitmap, TimeSpan timestamp)
        {
            if (bitmap == null)
                throw new ArgumentNullException(nameof(bitmap));

            // 锁定 Bitmap 像素数据
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, bitmap.PixelFormat);
            try
            {
                // 计算缓冲区大小并复制像素数据
                int bytesPerPixel = Image.GetPixelFormatSize(bitmap.PixelFormat) / 8;
                int bufferSize = bitmap.Width * bitmap.Height * bytesPerPixel;
                byte[] pixelBuffer = new byte[bufferSize];
                Marshal.Copy(bitmapData.Scan0, pixelBuffer, 0, bufferSize);

                // 创建 IBitmapFrame 实例（使用你的实现类）
                return new BitmapFrame(pixelBuffer, bitmap.Width, bitmap.Height, timestamp);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
        }
    }
}
