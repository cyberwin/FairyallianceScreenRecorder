using Captura.Windows.未来之窗;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Timers;
using System.Windows.Controls;

using System.Windows.Interop;
using System.Runtime.InteropServices;

namespace Captura.CyberWin_Main
{
    /// <summary>
    /// CyberWin_FloatingToolbarWindow.xaml 的交互逻辑
    /// </summary>
    public partial class CyberWin_FloatingToolbarWindow : Window
    {
        // 单例实例
        public static CyberWin_FloatingToolbarWindow _instance;

        //2025-12-15
        // Windows API 常量（用于设置顶层窗口）
        private const int HWND_TOPMOST = -1;
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOACTIVATE = 0x0010; // 不激活窗口，关键：避免抢占焦点

        // Windows API 声明（设置窗口位置和层级）
        [DllImport("user32.dll")]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        // Windows API 声明（设置窗口扩展样式，禁用焦点）
        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_NOACTIVATE = 0x08000000; // 窗口不接收激活焦点
        private const int WS_EX_TOOLWINDOW = 0x00000080; // 工具窗口，不在任务栏显示（可选，增强体验）


        // 定时检查的计时器（轻量，避免频繁触发）
        private readonly Timer _topmostCheckTimer;
      //  public   Timer _topmostCheckTimer;

        // 通过静态方法访问
        public static void ShowWindow()
        {
            if (_instance == null)
            {
                _instance = new CyberWin_FloatingToolbarWindow();
                _instance.Show();
            }
            else
            {
                if (_instance.IsVisible)
                {
                    _instance.Hide();
                    _instance._topmostCheckTimer.Stop(); // 隐藏时停止计时器
                }
                else
                {
                    _instance.Show();
                    _instance.EnsureTopmost(); // 显示时确保置顶
                    _instance._topmostCheckTimer.Start(); // 显示时启动计时器
                }
            }
        }

        // 窗口初始化时设置样式（禁用焦点+工具窗口）
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hWnd = new WindowInteropHelper(this).Handle;

            // 获取当前窗口扩展样式
            int exStyle = GetWindowLong(hWnd, GWL_EXSTYLE);
            // 添加：WS_EX_NOACTIVATE（不激活） + WS_EX_TOOLWINDOW（工具窗口，可选）
            exStyle |= WS_EX_NOACTIVATE | WS_EX_TOOLWINDOW;
            // 应用新样式
            SetWindowLong(hWnd, GWL_EXSTYLE, exStyle);

            // 初始化时直接设置系统级顶层
            EnsureTopmost();
        }

        public void 未来之窗启动顶部()
        {
            _topmostCheckTimer.Start();
        }
        public void 未来之窗停止顶部()
        {
            _topmostCheckTimer.Stop();
        }
        // 封装「安全置顶」方法，外部也可调用
        public void EnsureTopmost()
        {
            // 方案1：WPF原生方式（推荐，无依赖）
            Topmost = false; // 先重置，触发系统更新层级
            Topmost = true;  // 再设为置顶，优先级更高
                             // 不调用Activate()，避免抢占焦点！

            // 方案2：若原生方式失效，用Windows API（更强制，仍不抢占焦点）
            // var hWnd = new WindowInteropHelper(this).Handle;
            // // HWND_TOPMOST = (IntPtr)(-1)，SWP_NOSIZE|SWP_NOMOVE 不改变窗口大小和位置
            // User32.SetWindowPos(hWnd, (IntPtr)(-1), 0, 0, 0, 0, 0x0001 | 0x0002);
            var hWnd = new WindowInteropHelper(this).Handle;
            // 系统级置顶：HWND_TOPMOST（-1）表示永远在最顶层，SWP_NOACTIVATE表示不激活窗口（关键）
            SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE | SWP_NOACTIVATE);
        }

        // 在 CyberWin_FloatingToolbarWindow 类中添加
        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // 实现窗口拖拽功能（如果需要）
           // DragMove();
           // e.Handled = true;
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                this.DragMove(); // 关键方法：启动窗口拖拽
            }
        }

        // 实现拖拽功能
        public void Grid_MouseLeftButtonDown作废(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
                e.Handled = true;
                this.DragMove();
            }
        }

        private CyberWin_FloatingToolbarWindow()
        {
            InitializeComponent();
            DataContext = new CyberWin_FloatingToolbarViewModel();

            this.Loaded += Window_Loaded;

            // 初始化定时检查计时器（每隔500ms检查一次，轻量不占用资源）
            _topmostCheckTimer = new Timer(500); // 500ms一次，可根据需要调整
          //  _topmostCheckTimer = new Timer(500); // 500ms一次，可根据需要调整
            _topmostCheckTimer.Elapsed += (s, e) =>
            {
                // 跨线程调用WPF控件，需要用Dispatcher
                this.Dispatcher.Invoke(() =>
                {
                    if (this.IsVisible)
                    {
                     //   EnsureTopmost(); // 定时轻量置顶
                    }
                    EnsureTopmost(); // 定时轻量置顶
                });
            };
            _topmostCheckTimer.AutoReset = true; // 循环触发
            _topmostCheckTimer.Enabled = false; // 初始不启动，显示时再启动

            // 策略1：鼠标悬停到工具栏时，立即置顶（用户要操作时触发）
            this.MouseEnter += (s, e) =>
            {
                EnsureTopmost();
            };

            // 策略2：工具栏的任意按钮被鼠标移到时，立即置顶（兜底）
            foreach (var child in ((System.Windows.Controls.Panel)this.Content).Children)
            {
                if (child is System.Windows.Controls.UIElementCollection uiElement)
                {
                    /*
                    uiElement.MouseEnter += (s, e) =>
                    {
                        EnsureTopmost();
                    };
                    */
                   // uiElement.mo
                }
            }
        }
        // 对外暴露的方法：画笔模式切换时调用（策略3：关键操作时触发）
        public static void OnPencilModeChanged()
        {
            if (_instance != null && _instance.IsVisible)
            {
                _instance.Dispatcher.Invoke(() =>
                {
                    _instance.EnsureTopmost(); // 画笔模式切换时强制置顶
                });
            }
        }

        // 在窗口加载时设置其初始位置
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 确保窗口已渲染，以便获取 ActualHeight
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                // 屏幕工作区的宽度和高度
               // double screenWidth = SystemParameters.WorkAreaWidth;
              //  double screenHeight = SystemParameters.WorkAreaHeight;

                double screenWidth = SystemParameters.WorkArea.Width;
                double screenHeight = SystemParameters.WorkArea.Height;

                // 窗口的宽度和高度
                double windowWidth = this.ActualWidth;
                double windowHeight = this.ActualHeight;

                // 设置位置在右侧垂直居中
              //  this.Left = screenWidth - windowWidth - 20; // 右边界留出20像素边距
                this.Top = (screenHeight - windowHeight) / 2;
                this.Left = screenWidth - windowWidth - 20; // 右边界留出20像素边距
                东方仙盟_LogHelper.WriteLog("windowWidth"+ windowWidth, "window");
                东方仙盟_LogHelper.WriteLog("windowHeight+"+ windowHeight, "window");
            }), System.Windows.Threading.DispatcherPriority.Loaded);
        }

     

        // 窗口关闭时清理单例
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _instance = null;
        }
    }
}
