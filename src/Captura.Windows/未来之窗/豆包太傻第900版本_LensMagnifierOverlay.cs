using System;
using System.Drawing;
using Captura.Video;
using Captura.Models;
using Captura;
using Captura.Windows.未来之窗;
using System.Runtime;
using System.Drawing.Drawing2D;
using System.Windows.Media.Imaging;
using System.Runtime.Remoting.Lifetime;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;

namespace Captura.Video
{
    /// <summary>
    /// 豆包太傻第900版本-鼠标透镜放大覆盖层
    /// 实现录制时跟随鼠标的圆形放大效果
    /// </summary>
    public class 豆包太傻第900版本_LensMagnifierOverlay : IOverlay
    {
        private readonly 豆包太傻第900版本_LensSettings _settings;
        private readonly IPlatformServices _platformServices;

        public 豆包太傻第900版本_LensMagnifierOverlay(豆包太傻第900版本_LensSettings settings, IPlatformServices platformServices)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _platformServices = platformServices ?? throw new ArgumentNullException(nameof(platformServices));
            东方仙盟_LogHelper.WriteLog("豆包太傻第", "录制放大");
        }
        //2025-12-11 未来之窗
        public void Draw(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            //东方仙盟_LogHelper.WriteLog("豆包太傻第:" + _settings.Enabled, "录制放大");
            if (!_settings.Enabled)
            {
               // 东方仙盟_LogHelper.WriteLog("豆包太傻第:不执行", "录制放大参数");
                return;
            }
               
            // Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens
            // D:\MyWork\vs2022\东方仙盟_屏幕录像大师\src\Captura.Core\bin\x86\Release\net472
            if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens.Equals("N") == true)
            {
              //  东方仙盟_LogHelper.WriteLog("豆包太傻第:人为停止", "录制放大参数");
                return;
            }

            var mousePos = _platformServices.CursorPosition;
            if (transform != null)
                mousePos = transform(mousePos);

            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2;
            // 原始截图区域（以鼠标为中心）
            var sourceRect = new Rectangle(
                mousePos.X - (int)(radius / _settings.ZoomFactor),
                mousePos.Y - (int)(radius / _settings.ZoomFactor),
                (int)(lensSize / _settings.ZoomFactor),
                (int)(lensSize / _settings.ZoomFactor)
            );
            var screenBounds = _platformServices.DesktopRectangle;
            sourceRect.Intersect(screenBounds);
            


            if (sourceRect.Width > 0 && sourceRect.Height > 0)
            {
                using (var sourceImage = ScreenShot.Capture(sourceRect, false))
                {
                    // ========== 计算绘制位置（处理边界） ==========
                    int drawX = mousePos.X - radius - 5; // 预留阴影偏移空间
                    int drawY = mousePos.Y - radius - 5;
                    if (_settings.东方仙盟_鼠标聚焦放大_超出绘制模式 == "向中心靠近")
                    {
                        if (drawX < 0) drawX = 0;
                        if (drawY < 0) drawY = 0;
                        if (drawX + lensSize + 10 > screenBounds.Width)
                            drawX = screenBounds.Width - lensSize - 10;
                        if (drawY + lensSize + 10 > screenBounds.Height)
                            drawY = screenBounds.Height - lensSize - 10;
                    }

                    // ========== 调用工具函数生成带形状+阴影的放大镜图像 ==========
                    // 将其转换为 IBitmapImage 实现类 DrawingImage
                    // IBitmapImage bitmapImage = new Captura.Windows.Gdi.DrawingImage(sourceImage);
                    // 3. IBitmapImage → Image（两种方式）
                    // 方式1：直接获取原引用（节省内存，需注意原对象的释放）
                    Image sourceImage转换 = 未来之窗BitmapImageConverter.ToImage(sourceImage);

                    /*
                    using (var magnifierImage = 放大镜MagnifierImageHelper.CreateMagnifierImage(
                        sourceImage转换,
                        lensSize,
                        _settings.东方仙盟_鼠标聚焦放大_放大镜形状,
                        _settings.BorderColor,
                        _settings.BorderThickness,
                        shadowOffset: 5, // 可改为配置项
                        cornerRadius: 15 // 可改为配置项
                    ))
                    */
                    //2025-12-15
                    using (var magnifierImage = 放大镜MagnifierImageHelper.CreateMagnifierImage2D(
                        sourceImage转换,
                        lensSize,
                        _settings.东方仙盟_鼠标聚焦放大_放大镜形状,
                        _settings.BorderColor,
                        _settings.BorderThickness,
                        shadowOffset: 5, // 可改为配置项
                        cornerRadius: 15 // 可改为配置项
                    ))
                    {
                        // 绘制最终图像到屏幕
                        IBitmapImage magnifierImage转换 = 未来之窗BitmapImageConverter.ToIBitmapImage(magnifierImage);

                        editor.DrawImage(magnifierImage转换, new Rectangle(drawX, drawY, magnifierImage.Width, magnifierImage.Height));
                    }
                }
            }
        }
        //2025-12-08 02
        public void Draw_备份二20251211(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            东方仙盟_LogHelper.WriteLog("豆包太傻第:" + _settings.Enabled, "录制放大");

            // 如果未启用则不绘制
            if (!_settings.Enabled)
            {
                //东方仙盟_LogHelper.WriteLog("豆包太傻第:不执行" , "录制放大参数");
                return;
            }

            // 超出绘制模式配置（默认：向中心靠近）
            string 超出绘制模式 = _settings.东方仙盟_鼠标聚焦放大_超出绘制模式;//?? "向中心靠近";

            // 获取当前鼠标位置
            var mousePos = _platformServices.CursorPosition;

            // 应用坐标转换
            if (transform != null)
                mousePos = transform(mousePos);

            // 计算透镜基础参数
            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2;
            var zoomFactor = _settings.ZoomFactor;

            东方仙盟_LogHelper.WriteLog("lensSize:" + lensSize, "录制放大参数");
            东方仙盟_LogHelper.WriteLog("zoomFactor:" + zoomFactor, "录制放大参数");


            // ========== 核心：源区域始终以鼠标为中心（不调整），保证捕捉到鼠标聚焦位置 ==========
            var sourceRect = new Rectangle(
                mousePos.X - (int)(radius / zoomFactor),
                mousePos.Y - (int)(radius / zoomFactor),
                (int)(lensSize / zoomFactor),
                (int)(lensSize / zoomFactor)
            );

            // 确保源区域在屏幕范围内（仅裁剪，不移动中心）
            var screenBounds = _platformServices.DesktopRectangle;
            sourceRect.Intersect(screenBounds);

            // ========== 仅调整放大镜的绘制位置（保证完整显示） ==========
            // 初始绘制位置（以鼠标为中心）
            int drawX = mousePos.X - radius;
            int drawY = mousePos.Y - radius;

            // 当超出绘制模式为"向中心靠近"时，调整绘制位置（仅移动放大镜，不改变源区域）
            if (超出绘制模式 == "向中心靠近")
            {
                // 左边界：不能小于0
                if (drawX < 0)
                {
                    drawX = 0;
                }
                // 上边界：不能小于0
                if (drawY < 0)
                {
                    drawY = 0;
                }
                // 可选：右边界（不超出屏幕宽度）
                if (drawX + lensSize > screenBounds.Width)
                {
                    drawX = screenBounds.Width - lensSize;
                }
                // 可选：下边界（不超出屏幕高度）
                if (drawY + lensSize > screenBounds.Height)
                {
                    drawY = screenBounds.Height - lensSize;
                }
            }

            // 如果源区域有效则绘制放大效果
            if (sourceRect.Width > 0 && sourceRect.Height > 0)
            {
                // 捕获源区域图像（始终以鼠标为中心，保证聚焦位置正确）
                using (var sourceImage = ScreenShot.Capture(sourceRect, false))
                {
                    // 绘制放大后的内容（使用调整后的绘制位置，保证完整显示）
                    editor.DrawImage(
                        sourceImage,
                        new Rectangle(
                            drawX,          // 调整后的X（仅移动放大镜位置）
                            drawY,          // 调整后的Y（仅移动放大镜位置）
                            lensSize,
                            lensSize
                        )
                    );
                }

                东方仙盟_LogHelper.WriteLog("绘制透镜边框:", "录制放大");

                // 绘制透镜边框（使用调整后的位置）
                editor.DrawEllipse(
                    _settings.BorderColor,
                    _settings.BorderThickness,
                    new RectangleF(
                        drawX,          // 调整后的X
                        drawY,          // 调整后的Y
                        lensSize,
                        lensSize
                    )
                );
            }
        }


        //2025-12-08
        public void Draw_这样导致顶部鼠标聚焦的位置没有捕捉到(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            东方仙盟_LogHelper.WriteLog("豆包太傻第:" + _settings.Enabled, "录制放大");

            // 如果未启用则不绘制
            if (!_settings.Enabled)
            {
                return;
            }

            // 超出绘制模式配置（默认：向中心靠近）
            string 超出绘制模式 = _settings.东方仙盟_鼠标聚焦放大_超出绘制模式;//?? "向中心靠近";

            东方仙盟_LogHelper.WriteLog("超出绘制模式:"+ 超出绘制模式, "录制放大");

            // 获取当前鼠标位置
            var mousePos = _platformServices.CursorPosition;

            // 应用坐标转换
            if (transform != null)
                mousePos = transform(mousePos);

            // 计算透镜区域
            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2;
            var zoomFactor = _settings.ZoomFactor;

            // 计算原始源区域位置
            int sourceX = mousePos.X - (int)(radius / zoomFactor);
            int sourceY = mousePos.Y - (int)(radius / zoomFactor);
            int sourceWidth = (int)(lensSize / zoomFactor);
            int sourceHeight = (int)(lensSize / zoomFactor);

            // 确保源区域在屏幕范围内
            var screenBounds = _platformServices.DesktopRectangle;

            // 计算最终绘制位置（处理边界超出）
            int drawX = mousePos.X - radius;
            int drawY = mousePos.Y - radius;

            // 当超出绘制模式为"向中心靠近"时，调整位置确保不超出屏幕左/上边界
            if (超出绘制模式 == "向中心靠近")
            {
                // 左边界检查：如果绘制位置小于0，向右调整
                if (drawX < 0)
                {
                    drawX = 0;
                    // 同时调整源区域位置以保持对应关系
                    sourceX = (int)(drawX + radius - (radius / zoomFactor));
                }

                // 上边界检查：如果绘制位置小于0，向下调整
                if (drawY < 0)
                {
                    drawY = 0;
                    // 同时调整源区域位置以保持对应关系
                    sourceY = (int)(drawY + radius - (radius / zoomFactor));
                }

                // 可选：右边界和下边界也可以做同样处理（根据需求）
                // 右边界
                if (drawX + lensSize > screenBounds.Width)
                {
                    drawX = screenBounds.Width - lensSize;
                    sourceX = (int)(drawX + radius - (radius / zoomFactor));
                }
                // 下边界
                if (drawY + lensSize > screenBounds.Height)
                {
                    drawY = screenBounds.Height - lensSize;
                    sourceY = (int)(drawY + radius - (radius / zoomFactor));
                }
            }

            // 创建源区域矩形
            var sourceRect = new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight);

            // 确保源区域在屏幕范围内
            sourceRect.Intersect(screenBounds);

            // 如果源区域有效则绘制放大效果
            if (sourceRect.Width > 0 && sourceRect.Height > 0)
            {
                // 捕获源区域图像
                using (var sourceImage = ScreenShot.Capture(sourceRect, false))
                {
                    // 绘制放大后的内容（使用调整后的drawX/drawY）
                    editor.DrawImage(
                        sourceImage,
                        new Rectangle(
                            drawX,          // 调整后的X坐标
                            drawY,          // 调整后的Y坐标
                            lensSize,
                            lensSize
                        )
                    );
                }

                东方仙盟_LogHelper.WriteLog("绘制透镜边框:", "录制放大");

                // 绘制透镜边框（使用调整后的位置）
                editor.DrawEllipse(
                    _settings.BorderColor,
                    _settings.BorderThickness,
                    new RectangleF(
                        drawX,          // 调整后的X坐标
                        drawY,          // 调整后的Y坐标
                        lensSize,
                        lensSize
                    )
                );
            }
        }


        public void Draw_由于绘制鼠标在顶部和左侧会被切割(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            东方仙盟_LogHelper.WriteLog("豆包太傻第:"+ _settings.Enabled,"录制放大");
          //  _settings.
            // 如果未启用则不绘制
           
            if (!_settings.Enabled)
                return;
           
            // 获取当前鼠标位置
            var mousePos = _platformServices.CursorPosition;

            // 应用坐标转换
            if (transform != null)
                mousePos = transform(mousePos);

            // 计算透镜区域
            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2;
            var sourceRect = new Rectangle(
                mousePos.X - (int)(radius / _settings.ZoomFactor),
                mousePos.Y - (int)(radius / _settings.ZoomFactor),
                (int)(lensSize / _settings.ZoomFactor),
                (int)(lensSize / _settings.ZoomFactor)
            );

            // 确保源区域在屏幕范围内
            var screenBounds = _platformServices.DesktopRectangle;
            sourceRect.Intersect(screenBounds);

            // 如果源区域有效则绘制放大效果
            if (sourceRect.Width > 0 && sourceRect.Height > 0)
            {
                // 捕获源区域图像
                using (var sourceImage = ScreenShot.Capture(sourceRect, false))
                {
                    // 绘制放大后的内容
                    editor.DrawImage(
                        sourceImage,
                        new Rectangle(
                            mousePos.X - radius,
                            mousePos.Y - radius,
                            lensSize,
                            lensSize
                        )
                    );
                }

                东方仙盟_LogHelper.WriteLog("绘制透镜边框:" , "录制放大");

                // 绘制透镜边框
                editor.DrawEllipse(
                    _settings.BorderColor,
                    _settings.BorderThickness,
                    new RectangleF(
                        mousePos.X - radius,
                        mousePos.Y - radius,
                        lensSize,
                        lensSize
                    )
                );
            }
        }
        public void Draw未来之窗(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            if (!_settings.Enabled)
                return;

            // 获取鼠标位置并转换坐标
            var mousePos = _platformServices.CursorPosition;
            if (transform != null)
                mousePos = transform(mousePos);

            // 透镜参数
            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2f;
            var zoomFactor = _settings.ZoomFactor;

            // 计算原始截图区域
            var sourceRect = new Rectangle(
                x: mousePos.X - (int)(radius / zoomFactor),
                y: mousePos.Y - (int)(radius / zoomFactor),
                width: (int)(lensSize / zoomFactor),
                height: (int)(lensSize / zoomFactor)
            );

            // 限制在屏幕范围内
            var screenBounds = _platformServices.DesktopRectangle;
            sourceRect.Intersect(screenBounds);
            if (sourceRect.Width <= 0 || sourceRect.Height <= 0)
                return;

            // 捕获原始图像
            using (var sourceImage = ScreenShot.Capture(sourceRect, false))
            {
                // 目标绘制区域（矩形）
                var targetRect = new RectangleF(
                    x: mousePos.X - radius,
                    y: mousePos.Y - radius,
                    width: lensSize,
                    height: lensSize
                );

                // 关键：绘制圆形放大区域（通过遮罩实现）
                DrawCircularMagnifiedArea(editor, sourceImage, targetRect, radius, lensSize);

                // 绘制透镜边框
                editor.DrawEllipse(
                   _settings.BorderColor,
                     _settings.BorderThickness,
                    targetRect
                );
            }
        }
        /// <summary>
        /// 绘制圆形放大区域（适配IEditableFrame接口）
        /// </summary>
        private void DrawCircularMagnifiedArea(IEditableFrame editor, IBitmapImage sourceImage,
                                              RectangleF targetRect, float radius,float lensSize)
        {
            // 1. 先绘制放大的矩形图像（临时绘制到编辑器）
            var tempRect = new RectangleF(
                x: targetRect.X - 1,  // 稍微扩大避免边缘锯齿
                y: targetRect.Y - 1,
                width: targetRect.Width + 2,
                height: targetRect.Height + 2
            );
            editor.DrawImage(sourceImage, tempRect);

            // 2. 绘制圆形遮罩（外部区域用背景色覆盖，只保留圆形区域）
            // 2.1 绘制外圆遮罩（覆盖矩形图像的四个角）
            var maskColor = Color.Black;  // 遮罩颜色（建议用背景色，这里暂时用黑色）
            var maskRadius = radius + _settings.BorderThickness;

            // 上半圆遮罩（覆盖矩形上半部分）
            editor.FillEllipse(
               maskColor,
                new RectangleF(
                    x: targetRect.X,
                    y: targetRect.Y - maskRadius,
                    width: lensSize,
                    height: maskRadius * 2
                )
            );

            // 下半圆遮罩（覆盖矩形下半部分）
            editor.FillEllipse(
                maskColor,
                  new RectangleF(
                    x: targetRect.X,
                    y: targetRect.Y + radius,
                    width: lensSize,
                    height: maskRadius * 2
                )
            );

            // 左半圆遮罩（覆盖矩形左半部分）
            editor.FillEllipse(
                 maskColor,
                 new RectangleF(
                    x: targetRect.X - maskRadius,
                    y: targetRect.Y,
                    width: maskRadius * 2,
                    height: lensSize
                )
            );

            // 右半圆遮罩（覆盖矩形右半部分）
            editor.FillEllipse(
                maskColor,
                new RectangleF(
                    x: targetRect.X + radius,
                    y: targetRect.Y,
                    width: maskRadius * 2,
                    height: lensSize
                )
            );
        }

        public void Draw作废3(IEditableFrame editor, Func<Point, Point> transform = null)
        {
            // 如果未启用则不绘制
            if (!_settings.Enabled)
                return;

            // 获取当前鼠标位置并转换坐标
            var mousePos = _platformServices.CursorPosition;
            if (transform != null)
                mousePos = transform(mousePos);

            // 透镜核心参数
            var lensSize = _settings.LensDiameter;
            var radius = lensSize / 2f; // 圆形半径（用float保证精度）
            var zoomFactor = _settings.ZoomFactor;

            // 1. 计算原始截图区域（鼠标周围的小矩形，后续放大为圆形）
            var sourceRect = new Rectangle(
                x: mousePos.X - (int)(radius / zoomFactor),
                y: mousePos.Y - (int)(radius / zoomFactor),
                width: (int)(lensSize / zoomFactor),
                height: (int)(lensSize / zoomFactor)
            );

            // 确保截图区域在屏幕范围内
            var screenBounds = _platformServices.DesktopRectangle;
            sourceRect.Intersect(screenBounds);
            if (sourceRect.Width <= 0 || sourceRect.Height <= 0)
                return;

            // 2. 捕获原始区域图像（矩形）
            using (var sourceImage = ScreenShot.Capture(sourceRect, false))
            {
                // 3. 绘制放大后的矩形图像（临时绘制，后续会被遮罩裁剪）
                var targetRect = new Rectangle(
                    x: mousePos.X - (int)radius,
                    y: mousePos.Y - (int)radius,
                    width: lensSize,
                    height: lensSize
                );
                editor.DrawImage(sourceImage, targetRect);

                // 4. 绘制圆形遮罩：中间透明（显示放大图像），外部黑色（遮挡矩形边缘）
                // 遮罩区域 = 透镜矩形（和放大图像一样大）
                var maskRect = targetRect;
                using (var maskBrush = new SolidBrush(Color.Black)) // 遮罩颜色（外部遮挡色）
                {
                    // 先填充整个遮罩矩形（黑色）
                 //   editor.FillRectangle(maskBrush, maskRect);
                 //2025-11-13
                    editor.FillRectangle(Color.Black, maskRect);

                    // 再在中间绘制透明圆形（挖空，露出下方的放大图像）
                    // 利用 IEditableFrame 的混合模式：Clear 模式会清除该区域的颜色（变为透明）
                    using (var circlePen = new Pen(Color.Transparent, 0))
                    using (var circleBrush = new SolidBrush(Color.Transparent))
                    {
                        // 设置混合模式为 Clear（清除像素）
                     //   editor.SetCompositingMode(CompositingMode.SourceCopy);

                        //2025-11-13
                        

                     //   editor.FillEllipse(circleBrush, new RectangleF(
                      //      maskRect.X, maskRect.Y, maskRect.Width, maskRect.Height));

                        editor.FillEllipse(Color.Transparent, new RectangleF(
                            maskRect.X, maskRect.Y, maskRect.Width, maskRect.Height));
                        // 恢复默认混合模式
                      //  editor.SetCompositingMode(CompositingMode.SourceOver);
                    }
                }

                /*

                // 5. 绘制圆形边框（最终美化）
                editor.DrawEllipse(
                    color: _settings.BorderColor,
                    thickness: _settings.BorderThickness,
                    rect: new RectangleF(
                        mousePos.X - radius,
                        mousePos.Y - radius,
                        lensSize,
                        lensSize
                    )
                );
                */

                // 绘制透镜边框
                editor.DrawEllipse(
                    _settings.BorderColor,
                    _settings.BorderThickness,
                    new RectangleF(
                        mousePos.X - radius,
                        mousePos.Y - radius,
                        lensSize,
                        lensSize
                    )
                );

            }
        }

        public void Dispose()
        {
            // 释放资源
        }
    }

    /// <summary>
    /// 豆包太傻第900版本-透镜放大设置
    /// </summary>
    public class 豆包太傻第900版本_LensSettings : PropertyStore
    {
        // 是否启用透镜效果
        public bool Enabled
        {
            get => Get(false);
            set => Set(value);
        }

        // 透镜直径(px)
        public int LensDiameter
        {
            get => Get(400);
            set => Set(Math.Max(100, Math.Min(1000, value)));
        }

        // 缩放倍数
        public float ZoomFactor
        {
            get => Get(2.0f);
            set => Set(Math.Max(1.1f, Math.Min(5.0f, value)));
        }

        // 边框颜色
        public Color BorderColor
        {
            get => Get(Color.Red);
            set => Set(value);
        }

        // 边框厚度
        public int BorderThickness
        {
            get => Get(2);
            set => Set(Math.Max(1, Math.Min(10, value)));
        }

        //2025-12-08
        public string 东方仙盟_鼠标聚焦放大_超出绘制模式
        {
            get => Get("向中心靠近");
            set => Set(value);
        }

        public string 东方仙盟_鼠标聚焦放大_放大镜形状
        {
            get => Get("圆形");
            set => Set(value);
        }

        public string 东方仙盟_鼠标聚焦放大_出现效果
        {
            get => Get("默认");
            set => Set(value);
        }
    }

    /// <summary>
    /// 豆包太傻第900版本-透镜效果模块
    /// </summary>
    public class 豆包太傻第900版本_LensModule
    {
        private readonly 豆包太傻第900版本_LensSettings _settings;
        private readonly IPlatformServices _platformServices;

        public 豆包太傻第900版本_LensModule(豆包太傻第900版本_LensSettings settings, IPlatformServices platformServices)
        {
            _settings = settings;
            _platformServices = platformServices;
        }

        /// <summary>
        /// 创建透镜覆盖层
        /// </summary>
        public IOverlay CreateOverlay()
        {
            return new 豆包太傻第900版本_LensMagnifierOverlay(_settings, _platformServices);
        }
    }
}
