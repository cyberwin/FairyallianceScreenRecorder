using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Captura.Audio;
using Captura.Models;
using Captura.Windows.Gdi;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger;
using Captura.Windows.未来之窗;
using System.Runtime;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWin_Effect;
using Captura.Windows;
using SharpDX.Direct3D9;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;

// ReSharper disable MethodSupportsCancellation

namespace Captura.Video
{
    /// <summary>
    /// Default implementation of <see cref="IRecorder"/> interface.
    /// Can output to <see cref="IVideoFileWriter"/> or <see cref="IAudioFileWriter"/>.
    /// </summary>
    public class Recorder : IRecorder
    {
        #region Fields
        IAudioProvider _audioProvider;
        IVideoFileWriter _videoWriter;
        IImageProvider _imageProvider;

        readonly int _frameRate;

        readonly Stopwatch _sw;

        readonly ManualResetEvent _continueCapturing;
        readonly CancellationTokenSource _cancellationTokenSource;
        readonly CancellationToken _cancellationToken;

        readonly Task _recordTask;

        readonly object _syncLock = new object();

        Task<bool> _frameWriteTask;
        Task _audioWriteTask;
        int _frameCount;
        long _audioBytesWritten;
        readonly int _audioBytesPerFrame, _audioChunkBytes;
        const int AudioChunkLengthMs = 200;
        byte[] _audioBuffer, _silenceBuffer;

        readonly IFpsManager _fpsManager;
        #endregion

        /// <summary>
        /// Creates a new instance of <see cref="IRecorder"/> writing to <see cref="IVideoFileWriter"/>.
        /// </summary>
        /// <param name="VideoWriter">The <see cref="IVideoFileWriter"/> to write to.</param>
        /// <param name="ImageProvider">The image source.</param>
        /// <param name="FrameRate">Video Frame Rate.</param>
        /// <param name="AudioProvider">The audio source. null = no audio.</param>
        public Recorder(IVideoFileWriter VideoWriter, IImageProvider ImageProvider, int FrameRate,
            IAudioProvider AudioProvider = null,
            IFpsManager FpsManager = null)
        {
            _videoWriter = VideoWriter ?? throw new ArgumentNullException(nameof(VideoWriter));
            _imageProvider = ImageProvider ?? throw new ArgumentNullException(nameof(ImageProvider));
            _audioProvider = AudioProvider;
            _fpsManager = FpsManager;

            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            Log_Engine.write_logV2("Screna", "RecorderC","初始化");
            未来之窗_属性触发处理.On未来之窗翻页模式变更 -= GlobalSettings_On未来之窗_特效_翻页_模式变更;

            if (FrameRate <= 0)
                throw new ArgumentException("Frame Rate must be possitive", nameof(FrameRate));

            _frameRate = FrameRate;

            _continueCapturing = new ManualResetEvent(false);

            if (VideoWriter.SupportsAudio && AudioProvider != null)
            {
                var wf = AudioProvider.WaveFormat;

                _audioBytesPerFrame = (int) ((1.0 / FrameRate)
                                             * wf.SampleRate
                                             * wf.Channels
                                             * (wf.BitsPerSample / 8.0));

                _audioChunkBytes = (int) (_audioBytesPerFrame * (FrameRate * AudioChunkLengthMs / 1000.0));
            }
            else _audioProvider = null;

            _sw = new Stopwatch();

            _recordTask = Task.Factory.StartNew(async () => await DoRecord(), TaskCreationOptions.LongRunning);

         
        }

        async Task DoRecord()
        {
            try
            {
                var frameInterval = TimeSpan.FromSeconds(1.0 / _frameRate);
                _frameCount = 0;

                Log_Engine.write_logV2("Screna", "Recorder", "DoRecord" );

              

                // Returns false when stopped

                while (_continueCapturing.WaitOne() && !_cancellationToken.IsCancellationRequested)
                {
                    var timestamp = _sw.Elapsed;

                    if (_frameWriteTask != null)
                    {
                        // If false, stop recording
                        if (!await _frameWriteTask)
                            return;

                        if (!WriteDuplicateFrame())
                            return;
                    }

                    if (_audioWriteTask != null)
                    {
                        await _audioWriteTask;
                    }

                    _frameWriteTask = Task.Run(() => FrameWriter(timestamp));

                    var timeTillNextFrame = timestamp + frameInterval - _sw.Elapsed;

                    if (timeTillNextFrame > TimeSpan.Zero)
                        Thread.Sleep(timeTillNextFrame);
                }
            }
            catch (Exception e)
            {
                lock (_syncLock)
                {
                    if (!_disposed)
                    {
                        ErrorOccurred?.Invoke(e);

                        Dispose(false);
                    }
                }
            }
        }


       

        bool FrameWriter(TimeSpan Timestamp)
        {
            var editableFrame = _imageProvider.Capture();

            var frame = editableFrame.GenerateFrame(Timestamp);
            Log_Engine.write_logV2("Screna", "Recorder", "FrameWriter"+ frame.ToString());

            //   Log_Engine.write_logFrame("Screna", "Recorder", frame);
            //  Log_Engine.write_logImg("Screna", "Recorder", frameBitmap);

            // 假设已有一个 DrawingFrame 实例
            // DrawingFrame drawingFrame = frame; // 替换为实际获取帧的逻辑
            //  frame.

            // 从 DrawingFrame 中获取 Bitmap
            //  Bitmap frameBitmap = frame.Image as Bitmap;

            if (Public_Var.东方仙盟特效_翻书.开始录制翻页 == true)
            {
                Public_Var.东方仙盟特效_翻书.开始录制翻页 = false;
                未来之窗StartPageTurn(1);

                _currentProgress = 0.01f;

            }
            

            if (_currentProgress > 0f && _currentProgress < 1f)
            {
                frame = _pageTurnEffect.未来之窗ApplyEffect(frame);
                Log_Engine.write_logV2("动画", "测试成功", "进度：" + _currentProgress.ToString());
            }
            else
            {
                Log_Engine.write_logV2("动画","测试失败","进度："+ _currentProgress.ToString());
            }

            var success = AddFrame(frame);

            if (!success)
            {
                return false;
            }

            _fpsManager?.OnFrame();

            try
            {
                _audioWriteTask = Task.Run(WriteAudio);

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        bool WriteDuplicateFrame()
        {
            var requiredFrames = _sw.Elapsed.TotalSeconds * _frameRate;
            var diff = requiredFrames - _frameCount;

            // Write atmost 1 duplicate frame
            if (diff >= 1)
            {
                if (!AddFrame(RepeatFrame.Instance))
                    return false;
            }

            return true;
        }

        bool AddFrame(IBitmapFrame Frame)
        {
            try
            {
                //
                //Log_Engine.write_logV2("Screna", "WriteDuplicateFrame", "FrameWriter" + _frameCount.ToString());

                //  _videoWriter.WriteFrame(Frame);
                // 如果正在翻页，应用特效
              
                _videoWriter.WriteFrame(Frame);

                ++_frameCount;

                return true;
            }
            catch (InvalidOperationException)
            {
                return false;
            }
        }

        void WriteAudio()
        {
            if (_audioProvider == null)
            {
                return;
            }

            // These values need to be long otherwise can get out of range in a few hours
            var shouldHaveWritten = (_frameCount - 1L) * _audioBytesPerFrame;

            // Already written more than enough, skip for now
            if (_audioBytesWritten >= shouldHaveWritten)
            {
                return;
            }

            var toWrite = (int)(shouldHaveWritten - _audioBytesWritten);

            // Only write if data to write is more than chunk size.
            // This gives enough time for the audio provider to buffer data from the source.
            if (toWrite < _audioChunkBytes)
            {
                return;
            }

            // Reallocate buffer as needed
            if (_audioBuffer == null || _audioBuffer.Length < toWrite)
            {
                _audioBuffer = new byte[toWrite];
            }

            var read = _audioProvider.Read(_audioBuffer, 0, toWrite);

            // Nothing read
            if (read == 0)
            {
                return;
            }

            _videoWriter.WriteAudio(_audioBuffer, 0, read);
            _audioBytesWritten += read;

            // Fill with silence to maintain synchronization
            var silenceToWrite = toWrite - read;

            // Write silence only when more than a threshold
            // Threshold should ideally be a bit greater than chunk size
            if (silenceToWrite <= _audioChunkBytes * 1.5)
            {
                return;
            }
            
            // Reallocate silence buffer: An array of zeros.
            if (_silenceBuffer == null || _silenceBuffer.Length < silenceToWrite)
            {
                _silenceBuffer = new byte[silenceToWrite];
            }

            _videoWriter.WriteAudio(_silenceBuffer, 0, silenceToWrite);
            _audioBytesWritten += silenceToWrite;
        }

        #region Dispose
        async void Dispose(bool TerminateRecord)
        {
            if (_disposed)
                return;

            _disposed = true;

            _cancellationTokenSource.Cancel();

            // Resume record loop if paused so it can exit
            _continueCapturing.Set();

            // Ensure all threads exit before disposing resources.
            if (TerminateRecord)
                _recordTask.Wait();

            try
            {
                if (_frameWriteTask != null)
                    await _frameWriteTask;
            }
            catch { }

            try
            {
                if (_audioWriteTask != null)
                    await _audioWriteTask;
            }
            catch { }

            if (_audioProvider != null)
            {
                _audioProvider.Stop();
                _audioProvider.Dispose();
                _audioProvider = null;
            }

            _imageProvider?.Dispose();
            _imageProvider = null;

            _videoWriter.Dispose();
            _videoWriter = null;

            _audioBuffer = _silenceBuffer = null;

            _continueCapturing.Dispose();
        }

        /// <summary>
        /// Frees all resources used by this instance.
        /// </summary>
        public void Dispose()
        {
            lock (_syncLock)
            {
                Dispose(true);
            }
        }

        bool _disposed;

        /// <summary>
        /// Fired when an error occurs
        /// </summary>
        public event Action<Exception> ErrorOccurred;

        void ThrowIfDisposed()
        {
            lock (_syncLock)
            {
                if (_disposed)
                    throw new ObjectDisposedException("this");
            }
        }
        #endregion

        /// <summary>
        /// Start Recording.
        /// </summary>
        public void Start()
        {
            ThrowIfDisposed();

            _sw?.Start();

            _audioProvider?.Start();
            
            _continueCapturing?.Set();
        }

        /// <summary>
        /// Stop Recording.
        /// </summary>
        public void Stop()
        {
            ThrowIfDisposed();

            _continueCapturing?.Reset();
            _audioProvider?.Stop();

            _sw?.Stop();
        }

        private bool _isPageTurning = false; // 标记是否正在翻页

        private System.Timers.Timer _pageTurnTimer; // 控制翻页进度
        private static 未来之窗_BookPageTurnEffect _pageTurnEffect = new 未来之窗_BookPageTurnEffect();
        private static 未来之窗Settings_特效_翻书 _cwSettings;

        private float _currentProgress; // 当前翻页进度（0~1）
        /// <summary>
        /// 开始翻书特效（供UI按钮调用）
        /// </summary>
        /// <param name="direction">翻页方向：0=左→右，1=右→左</param>
        public void 未来之窗StartPageTurn(int direction)
        {
            _cwSettings = Public_Var.东方仙盟特效_翻书;

            // 检查配置：如果未启用翻书，直接返回
            /*
            if (!_cwSettings.开始录制翻页)
            {
                东方仙盟_LogHelper.WriteLog("翻书失败", "未启用翻书特效");
                return;
            }
            */

            // 初始化翻书参数（从配置读取，就是你UI设置的数值）
            _pageTurnEffect.Direction = direction;
            _pageTurnEffect.FoldRadius = _cwSettings.FoldSlider; // 褶皱程度
            _pageTurnEffect.ShadowAlpha = _cwSettings.ShadowSlider; // 阴影深度
            //重新
            _pageTurnEffect.EaseType = EaseType.EaseInOutCubic;// _cwSettings.EaseComboBox; // 缓动类型
            _pageTurnEffect.Progress = 0f; // 重置进度

            _isPageTurning = true;

            // 停止之前的定时器（防止重复翻页）
            _pageTurnTimer?.Stop();

            // 计算进度步长（按配置的时长匀速推进）
            var durationMs = _cwSettings.DurationSlider * 1000; // 翻书时长（秒→毫秒）
            var intervalMs = 30; // 每30ms更新一帧（约33fps，流畅不卡顿）
            var totalFrames = durationMs / intervalMs;
            var progressStep = 1f / totalFrames; // 每帧进度增量

           _currentProgress = 0.01f;


            // 启动定时器，控制翻页进度
            _pageTurnTimer = new System.Timers.Timer(intervalMs);
            _pageTurnTimer.Elapsed += (s, e) =>
            {
                // 线程安全：锁定变量，避免并发问题
                lock (_syncLock)
                {
                    _pageTurnEffect.Progress += progressStep;

                    // 翻页完成：停止定时器，标记结束
                    if (_pageTurnEffect.Progress >= 1f)
                    {
                        _pageTurnEffect.Progress = 1f;
                        _isPageTurning = false;
                        _pageTurnTimer.Stop();
                        东方仙盟_LogHelper.WriteLog(direction == 0 ? "左翻页" : "右翻页", "翻书完成");
                    }
                }
            };
            _pageTurnTimer.Start();

            东方仙盟_LogHelper.WriteLog(direction == 0 ? "左翻页" : "右翻页", "翻书开始");
        }

        

        /// <summary>
        /// 全局笔刷模式变更时触发
        /// </summary>
        private void GlobalSettings_On未来之窗_特效_翻页_模式变更(object sender, EventArgs e)
        {
            UpdateBrushModeByGlobalSetting_特效_翻页_模式变更();
        }

        /// <summary>
        /// 根据全局变量更新笔刷模式
        /// </summary>
        private void UpdateBrushModeByGlobalSetting_特效_翻页_模式变更()
        {
            switch (未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式)
            {
                case "Y":
                    // 笔刷模式：启用绘制
                    // DurationSlider
                    Log_Engine.write_logV2("动画", "改变1", "进度：" + 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式);
                    Log_Engine.write_logV2("动画", "参数", "DurationSlider：" + Public_Var.东方仙盟特效_翻书.DurationSlider);


                    Public_Var.东方仙盟特效_翻书.DurationSlider = 5;

                    Log_Engine.write_logV2("动画", "参数", "DurationSlider：" + Public_Var.东方仙盟特效_翻书.DurationSlider);
                    Log_Engine.write_logV2("动画", "参数", "FoldSlider：" + Public_Var.东方仙盟特效_翻书.FoldSlider);
                    Log_Engine.write_logV2("动画", "参数", "ShadowSlider：" + Public_Var.东方仙盟特效_翻书.ShadowSlider);



                    /*
                     *      _pageTurnEffect.FoldRadius = _cwSettings.FoldSlider; // 褶皱程度
            _pageTurnEffect.ShadowAlpha = _cwSettings.ShadowSlider; // 阴影深度
            //重新
                    */


                    Log_Engine.write_logV2("动画", "改变1", "进度：" + 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式);


                    //  东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换翻页:启用", "翻页");



                    未来之窗StartPageTurn(1);

                   // _pageTurnEffect.Progress = 0.1f;

                //    Log_Engine.write_logV2("动画", "改变1", "进度：" + 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式);

                //    东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换翻页:启用", "翻页");

                    //  _viewModel.SelectedTool = Tools;// InkCanvas.e;
                    // _viewModel.Tools = Tools;// InkCanvas.e;
                    // _viewModel.SelectedTool=
                    未来之窗StartPageTurn(1);
                    break;

                case "N":
                    // 鼠标模式：清空画布 + 禁用绘制

                  //  东方仙盟_LogHelper.WriteLog("快捷键未来之窗切换翻页:启用", "翻页");
                    Log_Engine.write_logV2("动画", "改变1", "进度：" + 未来之窗_属性触发处理.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式);


                    break;
            }
        }
    }
}
