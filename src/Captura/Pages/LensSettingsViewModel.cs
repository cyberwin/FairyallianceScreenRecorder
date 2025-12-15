using Captura.Video;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Reactive.Bindings;
using System.Windows.Media;
using System.Windows.Forms; // 用于ColorDialog

namespace Captura.Pages
{
    public class LensSettingsViewModel
    {
        // 鼠标聚焦设置实例
        public 豆包太傻第900版本_LensSettings LensSettings { get; }

        // 选择边框颜色的命令
        public ICommand PickBorderColorCommand { get; }

        public LensSettingsViewModel()
        {
            // 初始化设置（如果需要持久化，可从配置管理器获取）
            LensSettings = new 豆包太傻第900版本_LensSettings();

            // 颜色选择命令
            PickBorderColorCommand = new ReactiveCommand()
                .WithSubscribe(() =>
                {
                    var dialog = new ColorDialog
                    {
                        Color = System.Drawing.Color.FromArgb(
                            LensSettings.BorderColor.A,
                            LensSettings.BorderColor.R,
                            LensSettings.BorderColor.G,
                            LensSettings.BorderColor.B)
                    };

                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        var color = dialog.Color;
                        LensSettings.BorderColor = color;// Color.FromArgb( color.A, color.R, color.G, color.B);
                    }
                });
        }
    }
}
