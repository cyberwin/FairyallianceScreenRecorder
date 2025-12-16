using Captura.Video;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Captura.ViewCore.ViewModels;
using Captura.Windows.Gdi;

namespace Captura.Video
{

    /// <summary>
    /// 鼠标透镜覆盖层：将透镜帧叠加到原始帧
    /// </summary>
    public class CyberWin_MouseLensOverlay : IOverlay
    {


        private readonly CyberWin_MouseLensProvider _lensProvider;
        private readonly CyberWin_MouseLensViewModel _viewModel;

        // 仅依赖已注册的核心服务，无其他额外依赖
        public CyberWin_MouseLensOverlay(CyberWin_MouseLensProvider lensProvider,
                                         CyberWin_MouseLensViewModel viewModel)
        {
            _lensProvider = lensProvider;
            _viewModel = viewModel;
        }

        public void Draw(IEditableFrame editor, Func<Point, Point> transform = null)
        {
          //  CyberWin_MouseLensProvider.write_log222("录制", "CyberWin_MouseLensOverlay", "" + "太糟糕了");

            if (!_viewModel.IsEnabled.Value)
                return;

            // 获取透镜帧（IEditableFrame 类型）
            using (var lensFrame = _lensProvider.GetLensFrame())
            {
                // 计算透镜位置（跟随鼠标，避免超出屏幕）
                var mousePos = _lensProvider._cursorPositionFunc();
                int lensSize = _lensProvider.LensSize;
                var screenBounds = Screen.PrimaryScreen.Bounds;

                // 计算目标位置（鼠标右下方 20px，防止超出屏幕）
                int destX = mousePos.X + 20;
                int destY = mousePos.Y + 20;
                destX = Math.Min(destX, screenBounds.Width - lensSize);
                destY = Math.Min(destY, screenBounds.Height - lensSize);
                destX = Math.Max(destX, 0);
                destY = Math.Max(destY, 0);
                /*

                // 核心：强制转换为 Captura 内置的 GraphicsEditor（100% 存在，官方默认实现）
                if (editor is GraphicsEditor targetEditor && lensFrame is GraphicsEditor lensEditor)
                {
                    // 使用 GDI 直接绘制，无任何接口依赖
                    using (var g = targetEditor.Graphics)
                    {
                        // 绘制透镜帧到原始帧的指定位置
                        g.DrawImage(
                            lensEditor.Bitmap,  // 透镜图像（GraphicsEditor 内置 Bitmap 属性）
                            new Rectangle(destX, destY, lensSize, lensSize) // 目标位置和尺寸
                        );
                    }
                }
                */
            }
        }

        public void Dispose() { }
    }
}
