using Captura.Hotkeys;
using Captura.Models;
using Captura.Video;
using Captura.ViewCore.ViewModels;
using Captura.Windows.未来之窗;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler;
using SharpDX.Direct3D9;
using System.Windows.Controls;
using System.Windows.Input;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class HotkeyActor : IHotkeyActor
    {
        readonly ScreenShotViewModel _screenShotViewModel;
        readonly RecordingViewModel _recordingViewModel;
        readonly Settings _settings;
        readonly VideoSourcesViewModel _videoSourcesViewModel;
        readonly RegionSourceProvider _regionSourceProvider;

        // 新增RegionSelectorViewModel字段
        // readonly RegionSelectorViewModel _regionSelectorViewModel;

        public HotkeyActor(ScreenShotViewModel ScreenShotViewModel,
            RecordingViewModel RecordingViewModel,
            Settings Settings,
            VideoSourcesViewModel VideoSourcesViewModel,
            RegionSourceProvider RegionSourceProvider)//,
                                                      // RegionSelectorViewModel regionSelectorViewModel)
        {
            _screenShotViewModel = ScreenShotViewModel;
            _recordingViewModel = RecordingViewModel;
            _settings = Settings;
            _videoSourcesViewModel = VideoSourcesViewModel;
            _regionSourceProvider = RegionSourceProvider;
            //  _regionSelectorViewModel = regionSelectorViewModel;
            // 初始化字段
            //  _regionSelectorViewModel = RegionSelectorViewModel;
        }

        public void Act(ServiceName Service)
        {
            switch (Service)
            {
                case ServiceName.Recording:
                    _recordingViewModel.RecordCommand.ExecuteIfCan();
                    break;

                case ServiceName.Pause:
                    _recordingViewModel.PauseCommand.ExecuteIfCan();
                    break;

                case ServiceName.ScreenShot:
                    _screenShotViewModel.ScreenShotCommand.ExecuteIfCan();
                    break;

                case ServiceName.ActiveScreenShot:
                    _screenShotViewModel.ScreenShotActiveCommand.ExecuteIfCan();
                    break;

                case ServiceName.DesktopScreenShot:
                    _screenShotViewModel.ScreenShotDesktopCommand.ExecuteIfCan();
                    break;

                case ServiceName.ToggleMouseClicks:
                    _settings.Clicks.Display = !_settings.Clicks.Display;
                    break;

                case ServiceName.ToggleKeystrokes:
                    _settings.Keystrokes.Display = !_settings.Keystrokes.Display;
                    break;

                case ServiceName.ScreenShotRegion:
                    _screenShotViewModel.ScreenshotRegionCommand.ExecuteIfCan();
                    break;

                case ServiceName.ScreenShotScreen:
                    _screenShotViewModel.ScreenshotScreenCommand.ExecuteIfCan();
                    break;

                case ServiceName.ScreenShotWindow:
                    _screenShotViewModel.ScreenshotWindowCommand.ExecuteIfCan();
                    break;

                case ServiceName.ToggleRegionPicker:
                    // Stop any recording in progress
                    if (_recordingViewModel.RecorderState.Value != RecorderState.NotRecording)
                    {
                        _recordingViewModel.RecordCommand.Execute(null);
                    }

                    if (_videoSourcesViewModel.SelectedVideoSourceKind != _regionSourceProvider)
                    {
                        _videoSourcesViewModel.SelectedVideoSourceKind = _regionSourceProvider;

                        if (_settings.RegionPickerHotkeyAutoStartRecording)
                        {
                            _recordingViewModel.RecordCommand.Execute(null);
                        }
                    }
                    else _videoSourcesViewModel.SetDefaultSource();
                    break;


                case ServiceName.wlzcToggleBrushHot://未来之窗切换画笔
                    {
                        东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:", "快捷键");

                        if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式.Equals("Y") == true)
                        {
                            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "N";
                           // 东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:关闭", "快捷键");
                        }
                        else
                        {
                            // Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "Y";
                            未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "Y";
                            东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换画笔:打开", "快捷键");
                        }


                    }

                    break;
                // 在Act方法中添加
                case ServiceName.wlzcToggleMouseLens:
                    {
                        //CyberWin_MouseLensViewModel
                        东方仙盟_LogHelper.WriteLog("鼠标放大原始:"+ Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens, "快捷键");

                        // var lensVm = ServiceProvider.Get<CyberWin_MouseLensViewModel>();
                        //    public static string 未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "Y";
                       if( Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens.Equals("Y") == true){
                            Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "N";
                        }
                        else
                        {
                            Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "Y";
                        }
                        东方仙盟_LogHelper.WriteLog("鼠标放大原始:" + Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens, "快捷键");


                        //  lensVm.IsEnabled.Value = !lensVm.IsEnabled.Value;
                    }
                    break;
                case ServiceName.未来之窗_交互_翻书://未来之窗切换画笔
                    {
                        //  _recordingViewModel.未来之窗清空画笔换鼠标命令
                        未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = "Y";
                        //FoldSlider
                    }
                    break;
            }
        }

    }
}