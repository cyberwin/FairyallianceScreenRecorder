using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Captura.Models;
using Captura.ViewModels;
using Captura.Windows.未来之窗;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler;

namespace Captura
{
    public partial class MainWindow
    {
        public static MainWindow Instance { get; private set; }

        readonly MainWindowHelper _helper;

        //未来之窗
        private bool _isBrushEnabled = false; // 画笔当前状态（是否启用）
                                              // 假设原有画笔工具实例（项目中实际的画笔对象，比如 _brushTool）
       // private IBrushTool _brushTool;

        public MainWindow()
        {
            Instance = this;
            
            InitializeComponent();

            _helper = ServiceProvider.Get<MainWindowHelper>();

            _helper.MainViewModel.Init(!App.CmdOptions.NoPersist, !App.CmdOptions.Reset);

            _helper.HotkeySetup.Setup();

            _helper.TimerModel.Init();

            Loaded += (Sender, Args) =>
            {
                RepositionWindowIfOutside();

                ServiceProvider.Get<WebcamPage>().SetupPreview();

                _helper.HotkeySetup.ShowUnregistered();
            };

            if (App.CmdOptions.Tray || _helper.Settings.Tray.MinToTrayOnStartup)
                Hide();

            Closing += (Sender, Args) =>
            {
                if (!TryExit())
                    Args.Cancel = true;
            };

            // Register to bring this instance to foreground when other instances are launched.
            SingleInstanceManager.StartListening(WakeApp);
        }

        void WakeApp()
        {
            Dispatcher.Invoke(() =>
            {
                if (WindowState == WindowState.Minimized)
                {
                    WindowState = WindowState.Normal;
                }

                Activate();
            });
        }

        void RepositionWindowIfOutside()
        {
            // Window dimensions taking care of DPI
            var rect = new RectangleF((float) Left,
                (float) Top,
                (float) ActualWidth,
                (float) ActualHeight).ApplyDpi();
            
            if (!Screen.AllScreens.Any(M => M.Bounds.Contains(rect)))
            {
                Left = 50;
                Top = 50;
            }
        }

        void Grid_PreviewMouseLeftButtonDown(object Sender, MouseButtonEventArgs Args)
        {
            DragMove();

            Args.Handled = true;
        }

        void MinButton_Click(object Sender, RoutedEventArgs Args) => SystemCommands.MinimizeWindow(this);

        void CloseButton_Click(object Sender, RoutedEventArgs Args)
        {
            if (_helper.Settings.Tray.MinToTrayOnClose)
            {
                Hide();
            }
            else Close();
        }

        void SystemTray_TrayMouseDoubleClick(object Sender, RoutedEventArgs Args)
        {
            if (Visibility == Visibility.Visible)
            {
                Hide();
            }
            else this.ShowAndFocus();
        }

        bool TryExit()
        {
            if (!_helper.RecordingViewModel.CanExit())
                return false;

            ServiceProvider.Dispose();

            return true;
        }

        void MenuExit_Click(object Sender, RoutedEventArgs Args) => Close();

        void HideButton_Click(object Sender, RoutedEventArgs Args) => Hide();

        void ShowMainWindow(object Sender, RoutedEventArgs E) => this.ShowAndFocus();


        //2025-11-13
        /*
        // 打开翻书设置窗口
        private void PageTurnSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new PageTurnSettingsWindow();
            settingsWindow.Owner = this;

            // 保存设置的回调（传递给 ScreenRecorder）
            settingsWindow.OnSave = (duration, foldRadius, shadowAlpha, easeType) =>
            {
                if (RecorderManager.Current is ScreenRecorder screenRecorder)
                {
                    screenRecorder.SetPageTurnParams(duration, foldRadius, shadowAlpha);
                    screenRecorder.SetPageTurnEase(easeType);
                }
            };

            settingsWindow.Show();
        }
        */

        
          private void PageTurnNowButton_Click(object sender, RoutedEventArgs e)
        {
        }
        // 左→右翻书
        private void PageTurnLeftButton_Click(object sender, RoutedEventArgs e)
        {
            /*
             * < ComboBoxItem Tag = "Linear" > 线性（匀速）</ ComboBoxItem >
                < ComboBoxItem Tag = "EaseInCubic" IsSelected = "True" > 先慢后快 </ ComboBoxItem >
                < ComboBoxItem Tag = "EaseOutCubic" > 先快后慢 </ ComboBoxItem >
                < ComboBoxItem Tag = "EaseInOutCubic" > 先慢→快→慢（推荐）</ ComboBoxItem >
                */
            //   if (RecorderManager.Current is ScreenRecorder screenRecorder)
            //   {
            //       screenRecorder.StartPageTurn(direction: 0);
            //   }
            东方仙盟_LogHelper.WriteLog("左:", "翻书");
            //  RecordingViewModel.
            //  _helper.HotkeySetup.Setup
          //  Public_Var.东方仙盟特效_翻书.EaseComboBox = "";
                未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = "Y";

            Public_Var.东方仙盟特效_翻书.开始录制翻页 = true;

            Log_Engine.write_logV2("动画", "M-改变1", "进度：" + 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式);
            Log_Engine.write_logV2("动画", "M-参数", "DurationSlider：" + Public_Var.东方仙盟特效_翻书.DurationSlider);

            Log_Engine.write_logV2("动画", "M-参数", "FoldSlider：" + Public_Var.东方仙盟特效_翻书.FoldSlider);


            Public_Var.东方仙盟特效_翻书.DurationSlider = 5;
            Public_Var.东方仙盟特效_翻书.FoldSlider = 35;
            Public_Var.东方仙盟特效_翻书.ShadowSlider = 300;

            Log_Engine.write_logV2("动画", "M2-参数", "DurationSlider：" + Public_Var.东方仙盟特效_翻书.DurationSlider);
            Log_Engine.write_logV2("动画", "M2-参数", "FoldSlider：" + Public_Var.东方仙盟特效_翻书.FoldSlider);
            Log_Engine.write_logV2("动画", "M2-参数", "ShadowSlider：" + Public_Var.东方仙盟特效_翻书.ShadowSlider);




        }

        // 右→左翻书
        private void PageTurnRightButton_Click(object sender, RoutedEventArgs e)
        {
            // if (RecorderManager.Current is ScreenRecorder screenRecorder)
            //  {
            //     screenRecorder.StartPageTurn(direction: 1);
            // }
            东方仙盟_LogHelper.WriteLog("右:", "翻书");
            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = "Y";
        }

    }
}
