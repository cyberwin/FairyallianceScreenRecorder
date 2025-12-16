using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Captura.CyberWin_Main;
using Captura.Models;
using Captura.ViewModels;
using Captura.Windows.未来之窗;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler;
using MaterialDesignThemes.Wpf;

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
            //东方仙盟_LogHelper.WriteLog("右:", "翻书");
            // 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = "Y";
            东方仙盟_LogHelper.WriteLog("工具栏", "cyberwinhandle");
            CyberWin_FloatingToolbarWindow.ShowWindow();

            // 当窗口创建后，绑定事件
            if (CyberWin_FloatingToolbarWindow._instance != null)
            {
                var viewModel = CyberWin_FloatingToolbarWindow._instance.DataContext as CyberWin_FloatingToolbarViewModel;
                if (viewModel != null)
                {
                    BindToolbarEvents(viewModel);
                }
            }
        }

        private void BindToolbarEvents(CyberWin_FloatingToolbarViewModel viewModel)
        {
            // 绑定录制命令
          //  viewModel.OnStartRecording += () => _recorderViewModel.StartCommand.Execute(null);
          //  viewModel.OnPauseRecording += () => _recorderViewModel.PauseCommand.Execute(null);
          //  viewModel.OnStopRecording += () => _recorderViewModel.StopCommand.Execute(null);

            // 绑定放大镜命令
            viewModel.OnStartRecording += () =>
            {
                // 调用您已经做好的放大镜切换逻辑
                // 例如：_magnifierModule.Toggle();
                东方仙盟_LogHelper.WriteLog("OnStartRecording", "cyberwinhandle");
               // _recordingViewModel.RecordCommand.ExecuteIfCan();
                _helper.RecordingViewModel.RecordCommand.ExecuteIfCan();

            };
            // 绑定放大镜命令
            viewModel.OnPauseRecording += () =>
            {
                // 调用您已经做好的放大镜切换逻辑
                // 例如：_magnifierModule.Toggle();
                东方仙盟_LogHelper.WriteLog("OnPauseRecording", "cyberwinhandle");
                //  _recordingViewModel.PauseCommand.ExecuteIfCan();
                _helper.RecordingViewModel.PauseCommand.ExecuteIfCan();
            };
            // 绑定放大镜命令
            viewModel.OnStopRecording += () =>
            {
                // 调用您已经做好的放大镜切换逻辑
                // 例如：_magnifierModule.Toggle();
                东方仙盟_LogHelper.WriteLog("OnStopRecording", "cyberwinhandle");
                //_helper.RecordingViewModel.RecordCommand..ExecuteIfCan();
              //  _helper.RecordingViewModel.StopRecording();
            };

            // 绑定放大镜命令
            viewModel.OnToggleMagnifier += () =>
            {
                // 调用您已经做好的放大镜切换逻辑
                // 例如：_magnifierModule.Toggle();
             //   东方仙盟_LogHelper.WriteLog("OnToggleMagnifier放大镜", "cyberwinhandle");
                //未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens
                //  Public_Var.
               // 东方仙盟_LogHelper.WriteLog("鼠标放大原始:" + Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens, "快捷键");

                // var lensVm = ServiceProvider.Get<CyberWin_MouseLensViewModel>();
                //    public static string 未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "Y";
                if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens.Equals("Y") == true)
                {
                    Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "N";
                    viewModel.ManualIconKind = PackIconKind.MagnifyMinus;
                   // ManualIconKind
                   // viewModel .SetIconManually(PackIconKind.MagnifyMinus);
                }
                else
                {
                    Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "Y";
                   // viewModel.SetIconManually(PackIconKind.MagnifyAdd);
                    viewModel.ManualIconKind = PackIconKind.MagnifyAdd;
                }
                //东方仙盟_LogHelper.WriteLog("鼠标放大原始:" + Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens, "快捷键");


            };

            /*
             * 
             *  东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:", "快捷键");

                        if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式.Equals("Y") == true)
                        {
                            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "N";
                            东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:关闭", "快捷键");
                        }
                        else
                        {
                            // Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "Y";
                            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "Y";
                            东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:打开", "快捷键");
                        }
*/

            viewModel.OnTogglePencilTech += () =>
            {
                // 调用您已经做好的放大镜切换逻辑
                // 例如：_magnifierModule.Toggle();
             //   东方仙盟_LogHelper.WriteLog("OnTogglePencilTech-粉笔模式", "cyberwinhandle");
                //未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens
                //  Public_Var.
                未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "Y";
                // 东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:打开", "快捷键");
                //  东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:打开", "cyberwinhandle");

                //   CyberWin_FloatingToolbarWindow._instance.EnsureTopmost();
                CyberWin_FloatingToolbarWindow._instance.未来之窗启动顶部(); // 显示时启动计时器

            };

            // 绑定光标模式命令
            viewModel.OnToggleCursor += () =>
            {
                // 调用您已经做好的光标模式切换逻辑
                // 例如：GlobalSettings.未来之窗笔刷模式 = "N";
              //  东方仙盟_LogHelper.WriteLog("OnToggleCursor", "cyberwinhandle");
                未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "N";
                // 东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:打开", "快捷键");
                // 东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:关闭", "cyberwinhandle");
               // CyberWin_FloatingToolbarWindow._instance.EnsureTopmost();
                CyberWin_FloatingToolbarWindow._instance.未来之窗停止顶部(); // 显示时启动计时器

            };

            // 绑定颜色选择和画笔命令
            /*
            viewModel.OnColorSelected += (selectedColor) =>
            {
                // 将选择的颜色应用到全局设置
                // 这里假设笔刷颜色设置在 Keystrokes 中
                //  Settings.Instance.Keystrokes.Color = selectedColor;

                // 颜色设置后，激活笔刷模式
                //  GlobalSettings.未来之窗笔刷模式 = "Y";
                东方仙盟_LogHelper.WriteLog("OnToggleCursor", "cyberwinhandle");
            };
            */

            //OnTogglePencilPalette
            viewModel.OnColorSelected += (selectedColor) =>
            {
                // 将选择的颜色应用到全局设置
                // 这里假设笔刷颜色设置在 Keystrokes 中
                //  Settings.Instance.Keystrokes.Color = selectedColor;

                // 颜色设置后，激活笔刷模式
                //  GlobalSettings.未来之窗笔刷模式 = "Y";
             //   东方仙盟_LogHelper.WriteLog("OnToggleCursor", "cyberwinhandle");

                //_viewModel.BrushColor.Value = Color.FromRgb(255, 0, 0); // 红色
                // _helper.RecordingViewModel.Settings.
                Public_Var.未来之窗_东方仙盟_仙盟创梦_粉笔画笔颜色 = selectedColor;

                // .BrushColor
                //  .Subscribe(M => InkCanvas.DefaultDrawingAttributes.Color = M);
                //_helper.RecordingViewModel.br
            };

        }

        public ICommand ShowToolbarCommand { get; }

    }
}
