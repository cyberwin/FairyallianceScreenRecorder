using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using Captura.ViewModels;
using Captura.Windows.未来之窗;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler;
using SharpDX;
using SharpDX.Direct3D9;
using Color = System.Windows.Media.Color;
using Cursors = System.Windows.Input.Cursors;

namespace Captura
{
    public partial class RegionSelector
    {
        readonly RegionSelectorViewModel _viewModel;

        public RegionSelector(RegionSelectorViewModel ViewModel)
        {
            _viewModel = ViewModel;

            InitializeComponent();

            // Prevent Closing by User
            Closing += (S, E) => E.Cancel = true;

            ViewModel
                .BrushColor
                .Subscribe(M => InkCanvas.DefaultDrawingAttributes.Color = M);

            ViewModel
                .BrushSize
                .Subscribe(M => InkCanvas.DefaultDrawingAttributes.Height = InkCanvas.DefaultDrawingAttributes.Width = M);

            ViewModel
                .SelectedTool
                .Subscribe(OnToolChange);

            ViewModel
                .ClearAllDrawingsCommand
                .Subscribe(() => InkCanvas.Strokes.Clear());

            InkCanvas.DefaultDrawingAttributes.FitToCurve = true;

            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "N";


            未来之窗_属性触发处理.On未来之窗笔刷模式变更 += GlobalSettings_On未来之窗笔刷模式变更;
        }

        void OnToolChange(InkCanvasEditingMode Tool)
        {
            InkCanvas.EditingMode = Tool;

            if (Tool == InkCanvasEditingMode.Ink)
            {
                InkCanvas.UseCustomCursor = true;
                InkCanvas.Cursor = Cursors.Pen;
            }
            else InkCanvas.UseCustomCursor = false;

            InkCanvas.Background = new SolidColorBrush(Tool == InkCanvasEditingMode.None
                ? Colors.Transparent
                : Color.FromArgb(1, 0, 0, 0));
        }

        // Prevent Maximizing
        protected override void OnStateChanged(EventArgs E)
        {
            if (WindowState != WindowState.Normal)
                WindowState = WindowState.Normal;

            base.OnStateChanged(E);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo SizeInfo)
        {
            InkCanvas.Strokes.Clear();

            base.OnRenderSizeChanged(SizeInfo);
        }

        public IntPtr Handle => new WindowInteropHelper(this).Handle;

        void UIElement_OnPreviewMouseLeftButtonDown(object Sender, MouseButtonEventArgs E)
        {
            DragMove();
        }

        void Thumb_OnDragDelta(object Sender, DragDeltaEventArgs E)
        {
            void DoTop() => _viewModel.ResizeFromTop(E.VerticalChange);

            void DoLeft() => _viewModel.ResizeFromLeft(E.HorizontalChange);

            void DoBottom()
            {
                var height = Region.Height + E.VerticalChange;

                if (height > 0)
                    Region.Height = height;
            }

            void DoRight()
            {
                var width = Region.Width + E.HorizontalChange;

                if (width > 0)
                    Region.Width = width;
            }

            if (Sender is FrameworkElement element)
            {
                switch (element.Tag)
                {
                    case "Bottom":
                        DoBottom();
                        break;

                    case "Left":
                        DoLeft();
                        break;

                    case "Right":
                        DoRight();
                        break;

                    case "TopLeft":
                        DoTop();
                        DoLeft();
                        break;

                    case "TopRight":
                        DoTop();
                        DoRight();
                        break;

                    case "BottomLeft":
                        DoBottom();
                        DoLeft();
                        break;

                    case "BottomRight":
                        DoBottom();
                        DoRight();
                        break;
                }
            }
        }
        /// <summary>
        /// 全局笔刷模式变更时触发
        /// </summary>
        private void GlobalSettings_On未来之窗笔刷模式变更(object sender, EventArgs e)
        {
            UpdateBrushModeByGlobalSetting();
        }

        /// <summary>
        /// 根据全局变量更新笔刷模式
        /// </summary>
        private void UpdateBrushModeByGlobalSetting()
        {
            switch (未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式)
            {
                case "Y":
                    // 笔刷模式：启用绘制
                    InkCanvas.EditingMode = InkCanvasEditingMode.Ink;
                    
                    InkCanvas.Cursor = Cursors.Pen;
                    InkCanvas.UseCustomCursor = true;
                    InkCanvas.Cursor = Cursors.Pen;
                    InkCanvas.Background = new SolidColorBrush(Color.FromArgb(1, 0, 0, 0));
                    //base.OnRenderSizeChanged(SizeInfo);


                    东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:启用", "笔刷");

                    //  _viewModel.SelectedTool = Tools;// InkCanvas.e;
                    // _viewModel.Tools = Tools;// InkCanvas.e;
                    // _viewModel.SelectedTool=
                    break;

                case "N":
                    // 鼠标模式：清空画布 + 禁用绘制
                    InkCanvas.EditingMode = InkCanvasEditingMode.None;
                    InkCanvas.Strokes.Clear();
                    InkCanvas.UseCustomCursor = false;
                    InkCanvas.Background = new SolidColorBrush(Colors.Transparent);
               

                    break;
            }
        }

        // 窗口关闭时取消订阅（避免内存泄漏）
        private void Window_Closed(object sender, EventArgs e)
        {
            未来之窗_属性触发处理.On未来之窗笔刷模式变更 -= GlobalSettings_On未来之窗笔刷模式变更;
        }
    }
}
