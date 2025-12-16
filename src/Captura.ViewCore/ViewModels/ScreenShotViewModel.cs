using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Captura.Loc;
using Captura.Models;
using Captura.Video;
using Captura.Webcam;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

using System.Windows.Forms;

namespace Captura.ViewModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ScreenShotViewModel : ViewModelBase
    {
        public DiskWriter DiskWriter { get; }
        public ClipboardWriter ClipboardWriter { get; }
        public ImageUploadWriter ImgurWriter { get; }

        public ScreenShotViewModel(ILocalizationProvider Loc,
            Settings Settings,
            DiskWriter DiskWriter,
            ClipboardWriter ClipboardWriter,
            ImageUploadWriter ImgurWriter,
            ScreenShotModel ScreenShotModel,
            VideoSourcesViewModel VideoSourcesViewModel,
            WebcamModel WebcamModel,
            IPlatformServices PlatformServices) : base(Settings, Loc)
        {
            this.DiskWriter = DiskWriter;
            this.ClipboardWriter = ClipboardWriter;
            this.ImgurWriter = ImgurWriter;

            ScreenShotCommand = new[]
                {
                    VideoSourcesViewModel
                        .ObserveProperty(M => M.SelectedVideoSourceKind)
                        .Select(M => M is NoVideoSourceProvider),
                    VideoSourcesViewModel
                        .ObserveProperty(M => M.SelectedVideoSourceKind)
                        .Select(M => M is WebcamSourceProvider),
                    WebcamModel
                        .ObserveProperty(M => M.SelectedCam)
                        .Select(M => M is NoWebcamItem)
                }
                .CombineLatest(M =>
                {
                    var noVideo = M[0];
                    var webcamMode = M[1];
                    var noWebcam = M[2];

                    if (webcamMode)
                        return !noWebcam;

                    return !noVideo;
                })
                .ToReactiveCommand()
                .WithSubscribe(async M =>
                {
                    var bmp = await ScreenShotModel.GetScreenShot(VideoSourcesViewModel.SelectedVideoSourceKind);

                    await ScreenShotModel.SaveScreenShot(bmp);
                });

            async Task ScreenShotWindow(IWindow Window)
            {
                var img = ScreenShotModel.ScreenShotWindow(Window);

                await ScreenShotModel.SaveScreenShot(img);
            }

            ScreenShotActiveCommand = new ReactiveCommand()
                .WithSubscribe(async () => await ScreenShotWindow(PlatformServices.ForegroundWindow));

            ScreenShotDesktopCommand = new ReactiveCommand()
                .WithSubscribe(async () =>
                {
                    await Task.Delay(300);

                    await ScreenShotWindow(PlatformServices.DesktopWindow);
                });

            ScreenshotRegionCommand = new ReactiveCommand()
                .WithSubscribe(async () =>
                {
                    await Task.Delay(300);

                    await ScreenShotModel.ScreenshotRegion();
                });

            ScreenshotWindowCommand = new ReactiveCommand()
                .WithSubscribe(async () =>
                {
                    await Task.Delay(300);

                    await ScreenShotModel.ScreenshotWindow();
                });

            ScreenshotScreenCommand = new ReactiveCommand()
                .WithSubscribe(async () =>
                {
                    await Task.Delay(300);

                    await ScreenShotModel.ScreenshotScreen();
                });



            // 新增：切换画笔工具命令
            ToggleBrushCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    
                    // 查找RegionSelector窗口（画笔功能所在窗口）
              //      var regionSelector = Application.Current.Windows
                //        .OfType<RegionSelector>()
                  //      .FirstOrDefault();

                  //  if (regionSelector != null)
                 //   {
                        // 切换画笔状态（假设RegionSelector有ToggleBrush方法）
                     //   regionSelector.ToggleBrush();
                  //  }
                });

            // 新增：清空画笔笔迹命令
            ClearBrushCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    //var regionSelector = Application.Current.Windows
                   //     .OfType<RegionSelector>()
                     //   .FirstOrDefault();

                //    if (regionSelector != null)
                //    {
                        // 清空画笔（假设RegionSelector有ClearBrush方法）
                   //     regionSelector.ClearBrushStrokes();
                   // }
                });

            // 新增：切换光标显示状态命令
            ToggleCursorCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    // 直接操作设置中的光标显示状态（项目中已有IncludeCursor设置）
                    Settings.IncludeCursor = !Settings.IncludeCursor;
                });

        }

        public ICommand ScreenShotCommand { get; }
        public ICommand ScreenShotActiveCommand { get; }
        public ICommand ScreenShotDesktopCommand { get; }
        public ICommand ScreenshotRegionCommand { get; }
        public ICommand ScreenshotWindowCommand { get; }
        public ICommand ScreenshotScreenCommand { get; }

        public IEnumerable<ImageFormats> ScreenShotImageFormats { get; } = Enum
            .GetValues(typeof(ImageFormats))
            .Cast<ImageFormats>();

        // 新增：定义切换画笔和清空画笔的命令
        public ICommand ToggleBrushCommand { get; }
        public ICommand ClearBrushCommand { get; }
        public ICommand ToggleCursorCommand { get; }
    }
}