using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System;
using System.Windows.Input;
using Reactive.Bindings;
using System.Windows.Forms;
using MaterialDesignThemes;
using MaterialDesignThemes.Wpf;

using Microsoft.Xaml.Behaviors.Core;
using System.ComponentModel;

namespace Captura.CyberWin_Main
{
    public class CyberWin_FloatingToolbarViewModel : INotifyPropertyChanged //: INotifyPropertyChanged
    {


        // 命令：与View中的按钮绑定
        public ICommand StartRecordingCommand { get; }
        public ICommand PauseRecordingCommand { get; }
        public ICommand StopRecordingCommand { get; }
        public ICommand ToggleMagnifierCommand { get; }
        public ICommand ToggleBrushCommand { get; }
        public ICommand ToggleCursorCommand { get; }

        public ICommand TogglePencilPaletteCommand { get; }

        //粉笔模式
        
        public ICommand TogglePencilTechCommand { get; }

        // 事件：供主程序订阅，以执行实际功能
        public event Action OnStartRecording;
        public event Action OnPauseRecording;
        public event Action OnStopRecording;
        public event Action OnToggleMagnifier;
        public event Action OnToggleCursor;

        public event Action OnTogglePencilPalette;
        public event Action OnTogglePencilTech;
        

        // 新增：颜色选择事件，将选择的颜色传递给主程序
        public event Action<System.Drawing.Color> OnColorSelected;

        private bool _isMagnifierActive;

        private PackIconKind _manualIconKind = PackIconKind.MagnifyAdd;// MagnifyMinus;// Magnify; // 默认图标
        // 放大镜激活状态（控制自动切换）
        public bool IsMagnifierActive
        {
            get => _isMagnifierActive;
            set
            {
                _isMagnifierActive = value;
                OnPropertyChanged(nameof(IsMagnifierActive));
                // 自动切换时，重置手动图标（可选，根据需求调整）
                // ManualIconKind = value ? PackIconKind.MinusCircle : PackIconKind.Magnify;
            }
        }
        // 手动设置图标的方法（供外部调用）
        public void SetIconManually(PackIconKind kind)
        {
            ManualIconKind = kind;
        }
        // 手动设置图标的属性
        public PackIconKind ManualIconKind
        {
            get => _manualIconKind;
            set
            {
                _manualIconKind = value;
                OnPropertyChanged(nameof(ManualIconKind));
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public CyberWin_FloatingToolbarViewModel()
        {
            // 初始化所有命令
            StartRecordingCommand = new ReactiveCommand().WithSubscribe(() =>
                OnStartRecording?.Invoke());

            PauseRecordingCommand = new ReactiveCommand().WithSubscribe(() =>
                OnPauseRecording?.Invoke());

            StopRecordingCommand = new ReactiveCommand().WithSubscribe(() =>
                OnStopRecording?.Invoke());

            ToggleMagnifierCommand = new ReactiveCommand().WithSubscribe(() =>
                OnToggleMagnifier?.Invoke());

            ToggleCursorCommand = new ReactiveCommand().WithSubscribe(() =>
                OnToggleCursor?.Invoke());

            TogglePencilTechCommand = new ReactiveCommand().WithSubscribe(() =>
                OnTogglePencilTech?.Invoke());

            //粉笔颜色
            // TogglePencilPaletteCommand = new ReactiveCommand().WithSubscribe(() =>
            //    OnTogglePencilPalette?.Invoke());

            // 铅笔按钮命令：先选择颜色，再触发笔刷模式
            ToggleBrushCommand = new ReactiveCommand().WithSubscribe(() =>
            {
                using (var colorDialog = new ColorDialog())
                {
                    // 设置初始颜色为黑色，可以根据当前设置修改
                    colorDialog.Color = System.Drawing.Color.Black;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 触发颜色变更事件
                        OnColorSelected?.Invoke(colorDialog.Color);

                        // 颜色设置后，再激活笔刷模式
                        // （如果你的笔刷模式是通过全局变量控制的）
                        // GlobalSettings.未来之窗笔刷模式 = "Y";
                    }
                }
            });

            TogglePencilPaletteCommand = new ReactiveCommand().WithSubscribe(() =>
            {
                using (var colorDialog = new ColorDialog())
                {
                    // 设置初始颜色为黑色，可以根据当前设置修改
                    colorDialog.Color = System.Drawing.Color.Black;

                    if (colorDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 触发颜色变更事件
                        OnColorSelected?.Invoke(colorDialog.Color);

                        // 颜色设置后，再激活笔刷模式
                        // （如果你的笔刷模式是通过全局变量控制的）
                        // GlobalSettings.未来之窗笔刷模式 = "Y";
                    }
                }
            });
        }
    }

}

