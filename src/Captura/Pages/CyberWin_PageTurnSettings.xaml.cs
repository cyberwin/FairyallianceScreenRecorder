using Captura.ViewCore.ViewModels;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWin_Effect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Captura.Pages
{
    /// <summary>
    /// CyberWin_PageTurnSettings.xaml 的交互逻辑
    /// </summary>
    public partial class CyberWin_PageTurnSettings : Page
    {
        // 回调：将设置传递给主窗口
       // public Action<int, float, int, EaseType> OnSave;
        public CyberWin_PageTurnSettings()
        {
            InitializeComponent();
            // 滑块联动文本显示
            Log_Engine.write_logV2("配置", "翻页", "FoldSlider：" + Public_Var.东方仙盟特效_翻书.FoldSlider);
            Settings.LoadFairyAllianceConfig_翻页();
            DataContext = new 未来之窗_翻书效果SettingsViewModel();

            /*

            DurationSlider.ValueChanged += (s, e) =>
                DurationText.Text = $"当前：{DurationSlider.Value}秒";

            FoldSlider.ValueChanged += (s, e) =>
                FoldText.Text = $"当前：{FoldSlider.Value:F0}";

            ShadowSlider.ValueChanged += (s, e) =>
                ShadowText.Text = $"当前：{ShadowSlider.Value:F0}";
            */
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // 转换参数
            var duration = (int)(DurationSlider.Value * 1000); // 秒→毫秒
            var foldRadius = (float)FoldSlider.Value;
            var shadowAlpha = (int)ShadowSlider.Value;
            var easeType = (EaseType)Enum.Parse(typeof(EaseType),
                (EaseComboBox.SelectedItem as ComboBoxItem).Tag.ToString());


            //     Settings.东方仙盟特效_翻书.开始录制翻页
            Settings.东方仙盟特效_翻书.DurationSlider = duration;
            Settings.东方仙盟特效_翻书.FoldSlider = foldRadius;


            Settings.东方仙盟特效_翻书.ShadowSlider = shadowAlpha;


            Settings.东方仙盟特效_翻书.EaseComboBox = (EaseComboBox.SelectedItem as ComboBoxItem).Tag.ToString();

            // 回调传递参数
            //  OnSave?.Invoke(duration, foldRadius, shadowAlpha, easeType);
            //  Close();
            Settings.SaveFairyAllianceConfig_翻页();
        }

        private void OnSettingChanged(object sender, RoutedEventArgs e)
        {
            Settings.SaveFairyAllianceConfig_翻页();
        }

        
    }
}
