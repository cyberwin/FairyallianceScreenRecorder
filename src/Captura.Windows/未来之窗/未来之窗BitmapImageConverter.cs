using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Drawing;
using Captura; // IBitmapImage 所在命名空间
using Captura.Windows.Gdi; // DrawingImage 所在命名空间


namespace Captura.Windows.未来之窗
{
    public class 未来之窗BitmapImageConverter
    {

        /// <summary>
        /// 将 System.Drawing.Image 转换为 Captura.IBitmapImage（DrawingImage 实现）
        /// </summary>
        /// <param name="image">原始的 System.Drawing.Image 对象</param>
        /// <returns>Captura.IBitmapImage 实例</returns>
        /// <exception cref="ArgumentNullException">当 image 为 null 时抛出</exception>
        public static IBitmapImage ToIBitmapImage(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException(nameof(image), "传入的 Image 不能为 null");
            }

            // 核心：用 DrawingImage 包装 Image，得到 IBitmapImage
            return new DrawingImage(image);
        }

        /// <summary>
        /// 将 Captura.IBitmapImage 转换回 System.Drawing.Image（仅支持 DrawingImage 实现）
        /// </summary>
        /// <param name="bitmapImage">Captura.IBitmapImage 实例</param>
        /// <param name="createCopy">是否创建 Image 副本（避免原对象被释放后影响）</param>
        /// <returns>System.Drawing.Image 对象</returns>
        /// <exception cref="ArgumentException">当 bitmapImage 不是 DrawingImage 类型时抛出</exception>
        /// <exception cref="ArgumentNullException">当 bitmapImage 为 null 时抛出</exception>
        public static Image ToImage(IBitmapImage bitmapImage, bool createCopy = false)
        {
            if (bitmapImage == null)
            {
                throw new ArgumentNullException(nameof(bitmapImage), "传入的 IBitmapImage 不能为 null");
            }

            // 关键修改：将 C# 9.0 的 is not 替换为 C# 8.0 支持的 !is 语法
            if (!(bitmapImage is DrawingImage drawingImage))
            {
                throw new ArgumentException(
                    "仅支持 Captura.Windows.Gdi.DrawingImage 类型的 IBitmapImage 转换",
                    nameof(bitmapImage));
            }

            // 第二步：获取内部的 Image 对象
            Image originalImage = drawingImage.Image;

            if (originalImage == null)
            {
                throw new InvalidOperationException("DrawingImage 内部的 Image 对象为 null");
            }

            // 可选：创建副本，避免原对象被释放后影响转换后的实例
            if (createCopy)
            {
                return new Bitmap(originalImage);
            }

            return originalImage;
        }

    }
 
}

