using Captura.Video;
using Captura.Windows;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Captura.ViewCore.ViewModels
{

    public class 未来之窗_翻书效果SettingsViewModel
    {
        public 未来之窗Settings_特效_翻书 CWSettings = Settings.东方仙盟特效_翻书;
        public ICommand PickBorderColorCommand { get; }

        public 未来之窗_翻书效果SettingsViewModel()
        {

            未来之窗Settings_特效_翻书 CWSettings = Settings.东方仙盟特效_翻书;

            /*
            PickBorderColorCommand = new ReactiveCommand()
             .WithSubscribe(() =>
             {
                 var dialog = new ColorDialog
                 {
                     // 从独立配置读取当前颜色
                     Color = LensSettings.BorderColor
                 };

                 if (dialog.ShowDialog() == DialogResult.OK)
                 {
                     // 更新配置（自动同步到内存）
                     LensSettings.BorderColor = dialog.Color;
                     // 立即保存到独立文件
                     Settings.SaveFairyAllianceConfig();
                 }
             });
            */


        }
    }


}
