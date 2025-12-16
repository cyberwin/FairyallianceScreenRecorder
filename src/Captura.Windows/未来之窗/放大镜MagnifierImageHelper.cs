using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Captura.Windows.未来之窗
{
    //2025-12-11
    public class 放大镜MagnifierImageHelper
    {



        /// <summary>
        /// 创建带形状+阴影的放大镜图像
        /// </summary>
        /// <param name="sourceImage">原始截图</param>
        /// <param name="size">放大镜尺寸</param>
        /// <param name="shape">形状：圆形/圆角矩形/矩形</param>
        /// <param name="borderColor">边框颜色</param>
        /// <param name="borderThickness">边框厚度</param>
        /// <param name="shadowOffset">阴影偏移量</param>
        /// <param name="shadowColor">阴影颜色</param>
        /// <param name="cornerRadius">圆角矩形半径（仅圆角矩形生效）</param>
        /// <returns>带形状+阴影的最终图像</returns>
        /// 
        //IBitmapImage
        //  public static Image CreateMagnifierImage(Image sourceImage, int size, string shape,
        //         Color borderColor, int borderThickness, int shadowOffset = 5,
        //         Color shadowColor = default, int cornerRadius = 15)
        public static Image CreateMagnifierImage(Image sourceImage, int size, string shape,
               Color borderColor, int borderThickness, int shadowOffset = 5,
               Color shadowColor = default, int cornerRadius = 15)
        
            {
            东方仙盟_LogHelper.WriteLog("shape"+ shape, "特效251211");

            // 默认阴影颜色：半透明黑色
            if (shadowColor == default)
                    shadowColor = Color.FromArgb(100, 0, 0, 0);

                // 创建最终图像（包含阴影区域，尺寸需扩大）
                var finalSize = size + shadowOffset * 2;
                var finalImage = new Bitmap(finalSize, finalSize);
                var g = Graphics.FromImage(finalImage);
                g.SmoothingMode = SmoothingMode.AntiAlias; // 抗锯齿，边缘更平滑

                // ========== 步骤1：创建形状路径（核心：决定图像裁剪形状） ==========
                var path = new GraphicsPath();
                var mainRect = new Rectangle(shadowOffset, shadowOffset, size, size); // 主体位置（预留阴影空间）

                switch (shape.ToLower())
                {
                    case "圆形":
                        path.AddEllipse(mainRect);
                        break;
                    case "圆角矩形":
                        path.AddArc(mainRect.X, mainRect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                        path.AddArc(mainRect.Right - cornerRadius * 2, mainRect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                        path.AddArc(mainRect.Right - cornerRadius * 2, mainRect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                        path.AddArc(mainRect.X, mainRect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                        path.CloseAllFigures();
                        break;
                //2025-12-15
                case "五角星":
                    path = CreateStarPath(mainRect, 5);
                    break;
                case "心形":
                    path = CreateHeartPath(mainRect);
                    break;
                case "六边形":
                case "钻石":
                    path = CreateHexagonPath(mainRect);
                    break;
                case "三角形":
                    path = CreateTrianglePath(mainRect);
                    break;
                default: // 矩形/其他形状默认矩形
                        path.AddRectangle(mainRect);
                        break;
                }

                // ========== 步骤2：绘制阴影 ==========
                var shadowRect = new Rectangle(mainRect.X + shadowOffset, mainRect.Y + shadowOffset, size, size);
                var shadowPath = new GraphicsPath();
                switch (shape.ToLower())
                {
                    case "圆形":
                        shadowPath.AddEllipse(shadowRect);
                        break;
                    case "圆角矩形":
                        shadowPath.AddArc(shadowRect.X, shadowRect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                        shadowPath.AddArc(shadowRect.Right - cornerRadius * 2, shadowRect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                        shadowPath.AddArc(shadowRect.Right - cornerRadius * 2, shadowRect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                        shadowPath.AddArc(shadowRect.X, shadowRect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                        shadowPath.CloseAllFigures();
                        break;
                    default:
                        shadowPath.AddRectangle(shadowRect);
                        break;
                }
                g.FillPath(new SolidBrush(shadowColor), shadowPath);
                shadowPath.Dispose();

                // ========== 步骤3：裁剪并绘制主体图像（核心：实现PNG透明效果） ==========
                // 1. 设置裁剪区域为形状路径
                g.SetClip(path);
                // 2. 绘制放大后的原始图像（填充裁剪区域）
                g.DrawImage(sourceImage, mainRect, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
                // 3. 取消裁剪
                g.ResetClip();

                // ========== 步骤4：绘制边框 ==========
                var borderPen = new Pen(borderColor, borderThickness)
                {
                    Alignment = PenAlignment.Center // 边框居中，避免超出形状
                };
                g.DrawPath(borderPen, path);

                // ========== 资源释放 ==========
                path.Dispose();
                borderPen.Dispose();
                g.Dispose();

                return finalImage;
            }

        /// <summary>
        /// 创建带形状的放大镜图像（支持圆形、圆角矩形、矩形、五角星、心形、六边形、三角形）
        /// </summary>
        /// <param name="sourceImage">原始图像</param>
        /// <param name="size">放大镜主体尺寸（边长/直径）</param>
        /// <param name="shape">形状名称：圆形、圆角矩形、五角星、心形、六边形、三角形</param>
        /// <param name="borderColor">边框颜色</param>
        /// <param name="borderThickness">边框厚度</param>
        /// <param name="shadowOffset">阴影偏移量</param>
        /// <param name="shadowColor">阴影颜色（默认半透明黑色）</param>
        /// <param name="cornerRadius">圆角矩形的圆角半径（默认15）</param>
        /// <returns>生成的放大镜图像</returns>
        public static Image CreateMagnifierImage2D(Image sourceImage, int size, string shape,
            Color borderColor, int borderThickness, int shadowOffset = 5,
            Color shadowColor = default, int cornerRadius = 15)
        {
            // 日志输出
            // 东方仙盟_LogHelper.WriteLog("shape" + shape, "特效251211");

            if (shape == "3D圆形")
            {
                /*
                 *         /// <param name="borderColor">边框颜色</param>
        /// <param name="borderThickness">边框厚度</param>
        /// <param name="groundColor">地面颜色（半透明）</param>
        /// <param name="shadowColor">悬浮阴影颜色</param>
        /// <param name="highlightColor">镜面高光颜色</param>
        /// <param name="cornerRadius">圆角半径</param>
        */
                // 地面颜色（半透明）：浅蓝，透明度0.2（基底呼应科技主色）
                /*
                Color 科技groundColor =  Color.FromArgb(0.18f, 0.68f, 0.95f, 0.2f); // 浅天蓝，Alpha=0.2（#2EAEF233）
                                                                          // 悬浮阴影颜色：深蓝，透明度0.3（阴影与主色同系，更协调）
                Color 科技shadowColor = new Color(0.05f, 0.3f, 0.6f, 0.3f); // 深海蓝，Alpha=0.3（#0D4D994D）
                                                                        // 镜面高光颜色：淡青色/浅蓝白，透明度1（高光提亮，增强科技质感）
                Color 科技highlightColor = new Color(0.8f, 0.95f, 1f, 1f); // 淡青蓝，Alpha=1（#CCF2FF）
                */
                Color techGroundColor = Color.FromArgb(
                        (int)(0.2f * 255),    // Alpha：0.2 → 51（0.2×255=51）
                        (int)(0.18f * 255),   // R：0.18 → 46（0.18×255≈46）
                        (int)(0.68f * 255),   // G：0.68 → 173（0.68×255≈173）
                        (int)(0.95f * 255)    // B：0.95 → 242（0.95×255≈242）
                    );

                // 悬浮阴影颜色：深海蓝，Alpha=0.3（#0D4D994D）
                Color techShadowColor = Color.FromArgb(
                    (int)(0.3f * 255),    // Alpha：0.3 → 77（0.3×255=76.5→77）
                    (int)(0.05f * 255),   // R：0.05 → 13（0.05×255=12.75→13）
                    (int)(0.3f * 255),    // G：0.3 → 77（0.3×255=76.5→77）
                    (int)(0.6f * 255)     // B：0.6 → 153（0.6×255=153）
                );

                // 镜面高光颜色：淡青蓝，Alpha=1（#CCF2FF）
                Color techHighlightColor = Color.FromArgb(
                    (int)(1f * 255),      // Alpha：1 → 255（完全不透明）
                    (int)(0.8f * 255),    // R：0.8 → 204（0.8×255=204）
                    (int)(0.95f * 255),   // G：0.95 → 242（0.95×255≈242）
                    (int)(1f * 255)       // B：1 → 255（完全蓝色）
                );
                return Create3DMagnifierImage(sourceImage, size, shape, borderColor, borderThickness,
                    techGroundColor, techShadowColor, techHighlightColor
                    , cornerRadius);
            }


            // 默认阴影颜色：半透明黑色
            if (shadowColor == default)
                shadowColor = Color.FromArgb(100, 0, 0, 0);

            // 创建最终图像（包含阴影区域，尺寸需扩大）
            var finalSize = size + shadowOffset * 2;
            var finalImage = new Bitmap(finalSize, finalSize);
            var g = Graphics.FromImage(finalImage);
            g.SmoothingMode = SmoothingMode.AntiAlias; // 抗锯齿，边缘更平滑

         
            // ========== 步骤1：创建形状路径（核心：决定图像裁剪形状） ==========
            var mainRect = new Rectangle(shadowOffset, shadowOffset, size, size); // 主体位置（预留阴影空间）
            var path = CreateShapePath(shape, mainRect, cornerRadius);

            // ========== 步骤2：绘制阴影 ==========
            var shadowRect = new Rectangle(mainRect.X + shadowOffset, mainRect.Y + shadowOffset, size, size);
            var shadowPath = CreateShapePath(shape, shadowRect, cornerRadius);
            g.FillPath(new SolidBrush(shadowColor), shadowPath);
            shadowPath.Dispose();

            // ========== 步骤3：裁剪并绘制主体图像（核心：实现PNG透明效果） ==========
            // 1. 设置裁剪区域为形状路径
            g.SetClip(path);
            // 2. 绘制放大后的原始图像（填充裁剪区域）
            g.DrawImage(sourceImage, mainRect, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
            // 3. 取消裁剪
            g.ResetClip();

            // ========== 步骤4：绘制边框 ==========
            var borderPen = new Pen(borderColor, borderThickness)
            {
                Alignment = PenAlignment.Center // 边框居中，避免超出形状
            };
            g.DrawPath(borderPen, path);

            // ========== 资源释放 ==========
            path.Dispose();
            borderPen.Dispose();
            g.Dispose();

            return finalImage;
        }

        /// <summary>
        /// 创建带高级立体效果的放大镜图像
        /// </summary>
        /// <param name="sourceImage">原始图像（通常是屏幕截图的一部分）</param>
        /// <param name="size">放大镜主体尺寸</param>
        /// <param name="shape">形状名称</param>
        /// <param name="borderColor">边框颜色</param>
        /// <param name="borderThickness">边框厚度</param>
        /// <param name="groundColor">地面颜色（半透明）</param>
        /// <param name="shadowColor">悬浮阴影颜色</param>
        /// <param name="highlightColor">镜面高光颜色</param>
        /// <param name="cornerRadius">圆角半径</param>
        /// <returns>生成的立体放大镜图像</returns>
        public static Image Create3DMagnifierImage(Image sourceImage, int size, string shape,
            Color borderColor, int borderThickness,
            Color groundColor = default,
            Color shadowColor = default,
            Color highlightColor = default,
            int cornerRadius = 15)
        {
            // 默认颜色设置
            if (groundColor == default)
                groundColor = Color.FromArgb(40, Color.White); // 半透明白色地面
            if (shadowColor == default)
                shadowColor = Color.FromArgb(80, Color.Black); // 半透明黑色阴影
            if (highlightColor == default)
                highlightColor = Color.FromArgb(220, Color.White); // 明亮的白色高光

            // 为所有效果预留足够的空间
            int padding = size / 4; // 留出较大的边距，用于显示阴影和地面
            int finalSize = size + padding * 2;
            var finalImage = new Bitmap(finalSize, finalSize);
            using (var g = Graphics.FromImage(finalImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.Clear(Color.Transparent); // 关键：画布必须是透明的

                // 主体位置和路径
                var mainRect = new Rectangle(padding, padding, size, size);
                var path = CreateShapePath(shape, mainRect, cornerRadius);

                // ========== 步骤1：绘制【悬浮阴影】（在地面之下，主体之下） ==========
                if (shape.ToLower() == "3D圆形")
                {
                    // 阴影比主体大，更模糊，位于正下方
                    int shadowSize = size + size / 4;
                    int shadowOffsetY = padding / 2; // 只向下有轻微偏移
                    using (var shadowPath = new GraphicsPath())
                    {
                        shadowPath.AddEllipse(new Rectangle(
                            (finalSize - shadowSize) / 2,
                            padding + shadowOffsetY,
                            shadowSize,
                            shadowSize / 2 // 压扁的椭圆，更像地面接触阴影
                        ));

                        // 使用模糊效果绘制阴影
                        DrawSoftShadow(g, shadowPath, 0, shadowSize / 8, shadowColor);
                    }
                }

                // ========== 步骤2：绘制【半透明地面】（在阴影之上，主体之下） ==========
                using (var groundBrush = new SolidBrush(groundColor))
                {
                    g.FillPath(groundBrush, path);
                }

                // ========== 步骤3：裁剪并绘制【主体图像】 ==========
                g.SetClip(path);
                g.DrawImage(sourceImage, mainRect, new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), GraphicsUnit.Pixel);
                g.ResetClip();

                // ========== 步骤4：绘制【边框】 ==========
                using (var borderPen = new Pen(borderColor, borderThickness) { Alignment = PenAlignment.Center })
                {
                    g.DrawPath(borderPen, path);
                }

                // ========== 步骤5：绘制【镜面高光】（在主体之上，最顶层） ==========
                if (shape.ToLower() == "3D圆形")
                {
                    // 高光位置在放大镜上半部分，是一个细长的椭圆
                    int highlightHeight = size / 8;
                    using (var highlightPath = new GraphicsPath())
                    {
                        highlightPath.AddEllipse(new Rectangle(
                            mainRect.X,
                            mainRect.Y - highlightHeight / 2,
                            mainRect.Width,
                            highlightHeight
                        ));

                        // 使用半透明画笔绘制高光
                        using (var highlightBrush = new SolidBrush(highlightColor))
                        {
                            g.FillPath(highlightBrush, highlightPath);
                        }
                    }
                }
            }
            return finalImage;
        }
        /// <summary>
        /// 绘制柔和阴影的核心方法
        /// </summary>
        private static void DrawSoftShadow(Graphics g, GraphicsPath shapePath, int offset, int blurRadius, Color color)
        {
            using (var shadowBmp = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height, PixelFormat.Format32bppArgb))
            using (var shadowG = Graphics.FromImage(shadowBmp))
            {
                shadowG.Clear(Color.Transparent);
                shadowG.FillPath(Brushes.Black, shapePath);

                var blurEffect = new 未来之窗_特效_高斯模糊Blur { Radius = blurRadius };
                var imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(blurEffect.CreateColorMatrix());
                shadowG.DrawImage(shadowBmp, new Rectangle(0, 0, shadowBmp.Width, shadowBmp.Height), 0, 0, shadowBmp.Width, shadowBmp.Height, GraphicsUnit.Pixel, imageAttributes);

                var state = g.Save();
                g.TranslateTransform(offset, offset);
                g.DrawImage(shadowBmp, new Rectangle(0, 0, shadowBmp.Width, shadowBmp.Height), 0, 0, shadowBmp.Width, shadowBmp.Height, GraphicsUnit.Pixel, CreateColorTintImageAttributes(color));
                g.Restore(state);
            }
        }
        /// <summary>
        /// 创建一个用于给图像染色的ImageAttributes
        /// </summary>
        public static ImageAttributes CreateColorTintImageAttributes(Color tintColor)
        {
            float[][] colorMatrixElements = {
            new float[] {0, 0, 0, 0, 0},
            new float[] {0, 0, 0, 0, 0},
            new float[] {0, 0, 0, 0, 0},
            new float[] {0, 0, 0, tintColor.A / 255f, 0},
            new float[] {tintColor.R / 255f, tintColor.G / 255f, tintColor.B / 255f, 0, 1}
        };
            ColorMatrix colorMatrix = new ColorMatrix(colorMatrixElements);
            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            return imageAttr;
        }

        /// <summary>
        /// 根据形状名称创建对应的GraphicsPath
        /// </summary>
        /// <param name="shape">形状名称</param>
        /// <param name="rect">形状所在的矩形区域</param>
        /// <param name="cornerRadius">圆角矩形的圆角半径</param>
        /// <returns>对应形状的GraphicsPath</returns>
        private static GraphicsPath CreateShapePath(string shape, Rectangle rect, int cornerRadius)
        {
            var path = new GraphicsPath();
            string shapeLower = shape.ToLower();

            switch (shapeLower)
            {
                case "圆形":
                    path.AddEllipse(rect);
                    break;
                case "圆角矩形":
                    // 绘制圆角矩形
                    path.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                    path.AddArc(rect.Right - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                    path.AddArc(rect.Right - cornerRadius * 2, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                    path.CloseAllFigures();
                    break;
                case "五角星":
                    path = CreateStarPath(rect, 5); // 5角星
                    break;
                case "心形":
                    path = CreateHeartPath(rect);
                    break;
                case "六边形":
                case "钻石": // 六边形和钻石统一用正六边形
                    path = CreateHexagonPath(rect);
                    break;
                case "三角形":
                    path = CreateTrianglePath(rect);
                    break;
                default: // 矩形/其他形状默认矩形
                    path.AddRectangle(rect);
                    break;
            }

            return path;
        }
        #region 形状路径创建方法 (与之前版本相同)

        private static GraphicsPath CreateStarPath(Rectangle rect, int pointsCount = 5)
        {
            var path = new GraphicsPath();
            float centerX = rect.X + rect.Width / 2f;
            float centerY = rect.Y + rect.Height / 2f;
            float outerRadius = Math.Min(rect.Width, rect.Height) / 2f;
            float innerRadius = outerRadius * 0.382f;

            PointF[] points = new PointF[pointsCount * 2];
            for (int i = 0; i < pointsCount * 2; i++)
            {
                float angle = i * (float)(Math.PI / pointsCount) - (float)(Math.PI / 2);
                float radius = i % 2 == 0 ? outerRadius : innerRadius;
                points[i] = new PointF(
                    centerX + (float)Math.Cos(angle) * radius,
                    centerY + (float)Math.Sin(angle) * radius
                );
            }
            path.AddPolygon(points);
            path.CloseAllFigures();
            return path;
        }
        /// <summary>
        /// 创建心形路径
        /// </summary>
        /// <param name="rect">所在矩形</param>
        /// <returns>心形路径</returns>
        private static GraphicsPath CreateHeartPath(Rectangle rect)
        {
            var path = new GraphicsPath();
            float centerX = rect.X + rect.Width / 2f;
            float centerY = rect.Y + rect.Height / 2f;
            float width = rect.Width / 2f;
            float height = rect.Height / 2f;

            // 心形由两个贝塞尔曲线和一条直线组成（也可以用数学公式绘制）
            // 左半部分贝塞尔曲线
            path.AddBezier(
                centerX, centerY - height / 2,
                centerX - width, centerY + height / 4,
                centerX, centerY + height / 1.2f,
                centerX, centerY + height / 1.2f
            );
            // 右半部分贝塞尔曲线
            path.AddBezier(
                centerX, centerY - height / 2,
                centerX + width, centerY + height / 4,
                centerX, centerY + height / 1.2f,
                centerX, centerY + height / 1.2f
            );
            path.CloseAllFigures();
            return path;
        }

        /// <summary>
        /// 创建正六边形路径（钻石形状）
        /// </summary>
        /// <param name="rect">所在矩形</param>
        /// <returns>正六边形路径</returns>
        private static GraphicsPath CreateHexagonPath(Rectangle rect)
        {
            var path = new GraphicsPath();
            float centerX = rect.X + rect.Width / 2f;
            float centerY = rect.Y + rect.Height / 2f;
            float radius = Math.Min(rect.Width, rect.Height) / 2f;

            PointF[] points = new PointF[6];
            for (int i = 0; i < 6; i++)
            {
                float angle = i * (float)(Math.PI / 3) - (float)(Math.PI / 6); // 从顶部偏右开始，正六边形角度间隔60度
                points[i] = new PointF(
                    centerX + (float)Math.Cos(angle) * radius,
                    centerY + (float)Math.Sin(angle) * radius
                );
            }

            path.AddPolygon(points);
            path.CloseAllFigures();
            return path;
        }

        /// <summary>
        /// 创建等边三角形路径
        /// </summary>
        /// <param name="rect">所在矩形</param>
        /// <returns>三角形路径</returns>
        private static GraphicsPath CreateTrianglePath(Rectangle rect)
        {
            var path = new GraphicsPath();
            float centerX = rect.X + rect.Width / 2f;
            float topY = rect.Y;
            float bottomLeftX = rect.X;
            float bottomRightX = rect.Right;
            float bottomY = rect.Bottom;

            // 等边三角形的三个顶点（正三角）
            PointF[] points = new PointF[]
            {
            new PointF(centerX, topY), // 顶部顶点
            new PointF(bottomLeftX, bottomY), // 左下角顶点
            new PointF(bottomRightX, bottomY) // 右下角顶点
            };

            path.AddPolygon(points);
            path.CloseAllFigures();
            return path;
        }

        #endregion
    }
}
 
