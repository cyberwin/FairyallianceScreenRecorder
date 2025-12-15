using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Captura;
using Captura.Native;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWin_Effect
{
    /// <summary>
    /// 优化版翻书特效工具类（纯GDI+实现，无WPF依赖）
    /// </summary>
    public class 未来之窗_BookPageTurnEffect
    {
        // 翻页进度（0=未开始，1=完成）
        public float Progress { get; set; } = 0f;

        // 翻页方向（0=左→右，1=右→左）
        public int Direction { get; set; } = 0;

        // 褶皱半径（可动态调节：5~30，值越大弯曲越明显）
        private float _foldRadius = 15f;
        public float FoldRadius
        {
            get => _foldRadius;
            set => _foldRadius = Clamp(value, 5f, 30f); // 自定义范围限制
        }

        // 阴影透明度（可动态调节：100~255）
        private int _shadowAlpha = 180;
        public int ShadowAlpha
        {
            get => _shadowAlpha;
            set => _shadowAlpha = Clamp(value, 100, 255); // 自定义范围限制
        }

        // 缓动类型（默认：先慢后快再慢，更自然）
        public EaseType EaseType { get; set; } = EaseType.EaseInOutCubic;

        /// <summary>
        /// 自定义Clamp方法（替代Math.Clamp，适配.NET Framework 4.7.2）
        /// </summary>
        private float Clamp(float value, float min, float max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        /// <summary>
        /// 自定义Clamp方法（处理int类型）
        /// </summary>
        private int Clamp(int value, int min, int max)
        {
            return Math.Max(min, Math.Min(max, value));
        }

        /// <summary>
        /// 应用翻书特效（输入输出均为Bitmap，纯GDI+操作）
        /// </summary>
        public Bitmap ApplyEffect(Bitmap sourceImage)
        {
            // 边界处理：如果源图为空或进度为0，直接返回原图副本
            if (sourceImage == null || Progress <= 0f)
            {
                return sourceImage != null ? (Bitmap)sourceImage.Clone() : null;
            }

            // 应用缓动算法，使动画更自然
            float easedProgress = EaseCalculator.Calculate(Progress, EaseType);

            // 创建结果图像（避免修改原图）
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

            // 使用GDI+绘制
            using (Graphics g = Graphics.FromImage(resultImage))
            {
                // 高质量绘制设置
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingQuality = CompositingQuality.HighQuality;

                // 1. 先绘制原始图像作为底层
                g.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height);

                // 2. 计算翻页区域大小（基于缓动进度）
                int pageWidth = (int)(sourceImage.Width * easedProgress);
                Rectangle pageRect = Direction == 0
                    ? new Rectangle(0, 0, pageWidth, sourceImage.Height)  // 左→右翻页
                    : new Rectangle(sourceImage.Width - pageWidth, 0, pageWidth, sourceImage.Height);  // 右→左翻页

                // 3. 绘制翻起的书页形状（使用贝塞尔曲线模拟褶皱）
                using (GraphicsPath foldPath = new GraphicsPath())
                {
                    if (Direction == 0)
                    {
                        // 左→右翻页的褶皱路径
                        foldPath.AddBezier(
                            pageRect.Right, 0,  // 起点
                            pageRect.Right + FoldRadius, pageRect.Height / 4,  // 控制点1
                            pageRect.Right - FoldRadius, pageRect.Height * 3 / 4,  // 控制点2
                            pageRect.Right, pageRect.Bottom  // 终点
                        );
                        foldPath.AddLine(pageRect.Right, pageRect.Bottom, 0, pageRect.Bottom);
                        foldPath.AddLine(0, 0, pageRect.Right, 0);
                    }
                    else
                    {
                        // 右→左翻页的褶皱路径
                        foldPath.AddBezier(
                            pageRect.Left, 0,  // 起点
                            pageRect.Left - FoldRadius, pageRect.Height / 4,  // 控制点1
                            pageRect.Left + FoldRadius, pageRect.Height * 3 / 4,  // 控制点2
                            pageRect.Left, pageRect.Bottom  // 终点
                        );
                        foldPath.AddLine(pageRect.Left, pageRect.Bottom, sourceImage.Width, pageRect.Bottom);
                        foldPath.AddLine(sourceImage.Width, 0, pageRect.Left, 0);
                    }

                    // 绘制褶皱阴影（动态透明度）
                    using (LinearGradientBrush shadowBrush = new LinearGradientBrush(
                        pageRect,
                        Color.FromArgb(ShadowAlpha, Color.DarkGray),
                        Color.FromArgb(ShadowAlpha / 3, Color.Black),
                        LinearGradientMode.Vertical))
                    {
                        g.FillPath(shadowBrush, foldPath);
                    }

                    // 绘制褶皱边缘线
                    using (Pen edgePen = new Pen(Color.FromArgb(200, Color.Black), 1.5f))
                    {
                        g.DrawPath(edgePen, foldPath);
                    }
                }
            }

            return resultImage;
        }

        //2025-11-16
        // 修改后的方法：接受 IBitmapFrame，返回 IBitmapFrame
        public IBitmapFrame 未来之窗ApplyEffect(IBitmapFrame sourceFrame)
        {
            // 边界处理：源帧为空或进度为0，直接返回原帧副本
            if (sourceFrame == null || Progress <= 0f)
            {
                return sourceFrame != null ? CloneFrame(sourceFrame) : null;
            }

            // 1. 将 IBitmapFrame 转换为 Bitmap
            using (var sourceImage = sourceFrame.ToBitmap())
            {
                // 2. 应用缓动算法（复用原逻辑）
                float easedProgress = EaseCalculator.Calculate(Progress, EaseType);

                // 3. 创建结果图像（避免修改原图）
                using (var resultImage = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb))
                {
                    // 4. 使用GDI+绘制效果（复用原逻辑）
                    using (Graphics g = Graphics.FromImage(resultImage))
                    {
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingQuality = CompositingQuality.HighQuality;

                        // 绘制原始图像
                        g.DrawImage(sourceImage, 0, 0, sourceImage.Width, sourceImage.Height);

                        // 计算翻页区域
                        /*
                        int pageWidth = (int)(sourceImage.Width * easedProgress);
                        Rectangle pageRect = Direction == 0
                            ? new Rectangle(0, 0, pageWidth, sourceImage.Height)
                            : new Rectangle(sourceImage.Width - pageWidth, 0, pageWidth, sourceImage.Height);

                        */
                        int pageWidth = (int)(sourceImage.Width * easedProgress);
                        // 修复1：确保pageWidth至少为1（避免宽度为0）
                        pageWidth = Math.Max(pageWidth, 1);

                        Rectangle pageRect = Direction == 0
                            ? new Rectangle(0, 0, pageWidth, sourceImage.Height)
                            : new Rectangle(sourceImage.Width - pageWidth, 0, pageWidth, sourceImage.Height);

                        // 修复2：强制校验矩形宽高（避免极端情况）
                        if (pageRect.Width <= 0) pageRect.Width = 1;
                        if (pageRect.Height <= 0) pageRect.Height = 1;

                        // 绘制翻页褶皱和阴影
                        using (GraphicsPath foldPath = new GraphicsPath())
                        {
                            if (Direction == 0)
                            {
                                foldPath.AddBezier(
                                    pageRect.Right, 0,
                                    pageRect.Right + FoldRadius, pageRect.Height / 4,
                                    pageRect.Right - FoldRadius, pageRect.Height * 3 / 4,
                                    pageRect.Right, pageRect.Bottom
                                );
                                foldPath.AddLine(pageRect.Right, pageRect.Bottom, 0, pageRect.Bottom);
                                foldPath.AddLine(0, 0, pageRect.Right, 0);
                            }
                            else
                            {
                                foldPath.AddBezier(
                                    pageRect.Left, 0,
                                    pageRect.Left - FoldRadius, pageRect.Height / 4,
                                    pageRect.Left + FoldRadius, pageRect.Height * 3 / 4,
                                    pageRect.Left, pageRect.Bottom
                                );
                                foldPath.AddLine(pageRect.Left, pageRect.Bottom, sourceImage.Width, pageRect.Bottom);
                                foldPath.AddLine(sourceImage.Width, 0, pageRect.Left, 0);
                            }

                            // 绘制阴影
                            /*
                            using (LinearGradientBrush shadowBrush = new LinearGradientBrush(
                                pageRect,
                                Color.FromArgb(ShadowAlpha, Color.DarkGray),
                                Color.FromArgb(ShadowAlpha / 3, Color.Black),
                                LinearGradientMode.Vertical))
                            {
                                g.FillPath(shadowBrush, foldPath);
                            }

                            */
                            // 修复3：使用安全矩形（确保宽高不为0）
                            /*
                            var safeRect = pageRect;
                            if (safeRect.Width <= 0) safeRect.Width = 1;
                            if (safeRect.Height <= 0) safeRect.Height = 1;

                            using (LinearGradientBrush shadowBrush = new LinearGradientBrush(
                                safeRect,  // 用安全矩形替代
                                Color.FromArgb(ShadowAlpha, Color.DarkGray),
                                Color.FromArgb(ShadowAlpha / 3, Color.Black),
                                LinearGradientMode.Vertical))

                            // 绘制边缘线
                            using (Pen edgePen = new Pen(Color.FromArgb(200, Color.Black), 1.5f))
                            {
                                g.DrawPath(edgePen, foldPath);
                            }

                            */
                            var safeRect = pageRect;
                            if (safeRect.Width <= 0) safeRect.Width = 1;
                            if (safeRect.Height <= 0) safeRect.Height = 1;

                            // 1. 创建阴影画刷（已修复矩形问题）
                            using (LinearGradientBrush shadowBrush = new LinearGradientBrush(
                                safeRect,
                                Color.FromArgb(ShadowAlpha, Color.DarkGray),
                                Color.FromArgb(ShadowAlpha / 3, Color.Black),
                                LinearGradientMode.Vertical))
                            {
                                // 关键：用画刷填充褶皱路径，阴影才会显示
                                g.FillPath(shadowBrush, foldPath);
                            }

                            // 2. 绘制边缘线（已有代码，保留）
                            using (Pen edgePen = new Pen(Color.FromArgb(200, Color.Black), 1.5f))
                            {
                                g.DrawPath(edgePen, foldPath);
                            }
                        }
                    }

                    // 5. 将处理后的 Bitmap 转换为 IBitmapFrame 并返回
                    return resultImage.ToBitmapFrame(sourceFrame.Timestamp);
                }



            }

        }
        // 辅助方法：克隆 IBitmapFrame（避免直接引用源帧）
        private IBitmapFrame CloneFrame(IBitmapFrame frame)
        {
            int bufferSize = frame.Width * frame.Height * 4; // 假设32bpp（4字节/像素）
            byte[] buffer = new byte[bufferSize];
            frame.CopyTo(buffer);
            return new BitmapFrame(buffer, frame.Width, frame.Height, frame.Timestamp);
        }


        // 辅助方法：克隆 IBitmapFrame（避免直接引用源帧）
        private IBitmapFrame 作废CloneFrame(IBitmapFrame frame)
        {
            int bufferSize = frame.Width * frame.Height * 4; // 假设32bpp（4字节/像素）
            byte[] buffer = new byte[bufferSize];
            frame.CopyTo(buffer);
            return new BitmapFrame(buffer, frame.Width, frame.Height, frame.Timestamp);
        }
        //////////////////////
        ///2025-11-16
        ///



    }

    /// <summary>
    /// 缓动类型枚举
    /// </summary>
    public enum EaseType
    {
        Linear,          // 线性（匀速）
        EaseInCubic,     // 先慢后快
        EaseOutCubic,    // 先快后慢
        EaseInOutCubic   // 先慢→快→慢（推荐）
    }

    /// <summary>
    /// 缓动算法工具类
    /// </summary>
    public static class EaseCalculator
    {
        public static float Calculate(float progress, EaseType easeType)
        {
            switch (easeType)
            {
                case EaseType.EaseInCubic:
                    return progress * progress * progress;
                case EaseType.EaseOutCubic:
                    return 1 - (float)Math.Pow(1 - progress, 3);
                case EaseType.EaseInOutCubic:
                    return progress < 0.5f
                        ? 4f * progress * progress * progress
                        : 1f - (float)Math.Pow(-2f * progress + 2f, 3f) / 2f;
                default: // Linear
                    return progress;
            }
        }
    }
}