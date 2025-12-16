using System;
using System.Drawing;
using System.Windows.Forms;
using Captura.Native;
using Captura.Video;
using Captura.Models;
using Captura.Windows.Gdi;
using Captura.Windows;
using System.IO;
//using Captura.Extensions; // 确保引入Even扩展方法所在的命名空间

namespace Captura.Video
{
    /// <summary>
    /// 鼠标透镜跟随器（放大镜模式）
    /// </summary>
    public class CyberWin_MouseLensProvider : IDisposable
    {
        private readonly IntPtr _hdcSrc;
        private readonly ITargetDeviceContext _dcTarget;
        public readonly Func<Point> _cursorPositionFunc;

        // 配置参数
        public int LensSize { get; set; } = 400; // 透镜尺寸
        public float ZoomFactor { get; set; } = 2.0f; // 放大倍数
        public bool DrawBorder { get; set; } = true; // 是否绘制边框
        public Color BorderColor { get; set; } = Color.Red; // 边框颜色
        public int BorderThickness { get; set; } = 2; // 边框厚度

        public CyberWin_MouseLensProvider(IPreviewWindow previewWindow, Func<Point> cursorPositionFunc = null)
        {
            write_log222("录制", "CyberWin_MouseLensProvider", "你大爷的20251207" );

            // 初始化鼠标位置获取函数（.NET 4.7.2 兼容写法）
            _cursorPositionFunc = cursorPositionFunc ?? new Func<Point>(() =>
            {
                // 使用Win32的POINT结构体（避免与System.Drawing.Point冲突）
                Point point =new Point();
                // 调用API时显式传递ref参数（.NET 4.7.2要求）
                User32.GetCursorPos(ref point);
                // 转换为System.Drawing.Point
                return new Point(point.X, point.Y);
            });

            // 初始化源DC（屏幕设备上下文）
            _hdcSrc = User32.GetDC(IntPtr.Zero);

            // 计算目标尺寸（确保宽高为偶数，兼容.NET 4.7.2的扩展方法）
            var targetRect = new Rectangle(Point.Empty, new Size(LensSize, LensSize))
                .Even(); // 调用Rectangle的Even扩展方法（需确保Extensions命名空间已引入）
            var targetSize = targetRect.Size;

            // 初始化目标DC（根据配置选择GDI或DXGI模式，显式转换接口类型）
            _dcTarget = WindowsModule.ShouldUseGdi
                ? (ITargetDeviceContext)new GdiTargetDeviceContext(_hdcSrc, targetSize.Width, targetSize.Height)
                : new DxgiTargetDeviceContext(previewWindow, targetSize.Width, targetSize.Height);
        }

        /// <summary>
        /// 获取透镜帧
        /// </summary>
        public IEditableFrame GetLensFrame()
        {
            var cursorPos = _cursorPositionFunc();
            var sourceSize = (int)(LensSize / ZoomFactor);

            write_log222("录制", "GetLensFrame",""+ ZoomFactor);

             // 计算源区域（鼠标周围区域）
             var sourceRegion = new Rectangle(
                cursorPos.X - sourceSize / 2,
                cursorPos.Y - sourceSize / 2,
                sourceSize,
                sourceSize);

            // 确保源区域在屏幕范围内（.NET 4.7.2中Screen类的用法兼容）
            var screenBounds = Screen.PrimaryScreen.Bounds;
            sourceRegion.Intersect(screenBounds);

            // 缩放绘制到目标DC（使用GDI32接口，参数类型严格匹配）
            Gdi32.StretchBlt(
                _dcTarget.GetDC(),
                0, 0, LensSize, LensSize,
                _hdcSrc,
                sourceRegion.X, sourceRegion.Y,
                sourceRegion.Width, sourceRegion.Height,
                (int)CopyPixelOperation.SourceCopy); // .NET 4.7.2中枚举需显式转换为uint

            // 绘制边框（适配现有Graphics扩展方法）
            if (DrawBorder)
            {
                //2025-12-08 确实不是这里
              //  BorderColor = Color.Black;

                // 通过目标DC获取可编辑帧进行绘制
                using (var frame = _dcTarget.GetEditableFrame() as GraphicsEditor)
                {
                    if (frame != null)
                    {
                        frame.DrawRectangle(
                            BorderColor,
                            BorderThickness,
                            new RectangleF(0, 0, LensSize - 1, LensSize - 1)
                        );
                    }
                }
            }

            return _dcTarget.GetEditableFrame();
        }

        public void Dispose()
        {
            // 释放资源（.NET 4.7.2中需显式释放非托管资源）
            _dcTarget?.Dispose();
            if (_hdcSrc != IntPtr.Zero)
            {
                User32.ReleaseDC(IntPtr.Zero, _hdcSrc);
            }
        }

        // 定义Win32的POINT结构体（避免与System.Drawing.Point冲突）
        private struct NativePoint
        {
            public int X;
            public int Y;
        }

         public static string logUUid() => Guid.NewGuid().ToString();
        public static void write_log222( string capturetype, string type, string s)
        {
            // try
            // {
            string LogFolderPath= Application.StartupPath + "/";


                string path = LogFolderPath + "/log/" + "youareold" + "/" + capturetype + "/";
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
             
                FileStream fileStream = new FileStream(path + type + "_log.log", FileMode.Append);
                StreamWriter streamWriter = new StreamWriter((Stream)fileStream);
                string str1 = logUUid();
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
           // }
           // catch (Exception ex)
         //   {
          //  }
        }

    }
}