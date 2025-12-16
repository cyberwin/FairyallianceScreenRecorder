using Captura.Loc;
using Captura.Video;
using Captura.ViewModels;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Forms;
using Reactive.Bindings.Extensions;

namespace Captura.ViewCore.ViewModels
{
    public class 未来之窗_鼠标聚焦LensSettingsViewModel  
    {
        // 鼠标聚焦设置实例
      //  public 豆包太傻第900版本_LensSettings LensSettings { get; }

        // 直接绑定全局独立配置（无需新建实例）
        public 豆包太傻第900版本_LensSettings LensSettings => Settings.东方仙盟鼠标聚焦;

        //2025-12-08
        // 合法值列表（统一管理）
        //2025-12-15
        /*
         *  <ComboBoxItem Content="圆形" Tag="圆形"/>
                    <ComboBoxItem Content="圆角矩形" Tag="圆角矩形"/>
                    <ComboBoxItem Content="五角星" Tag="五角星"/>
                    <ComboBoxItem Content="心形" Tag="心形"/>
                    <ComboBoxItem Content="六边形" Tag="六边形"/>
                    <ComboBoxItem Content="钻石" Tag="钻石"/>
                    <ComboBoxItem Content="三角形" Tag="三角形"/>
                    <ComboBoxItem Content="3D圆形" Tag="3D圆形"/>
        */
        private readonly List<string> _validDrawModes = new List<string> { "向中心靠近", "原始效果" };
        private readonly List<string> _validShapes = new List<string> { "圆形", "圆角矩形", "五角星", "心形" };
        private readonly List<string> _validEffects = new List<string> { "默认", "翻书", "慢慢放大" };

        private readonly List<string> _validShapes_新版 = new List<string> { "圆形", "圆角矩形", "五角星", "心形", "六边形", "钻石", "三角形", "3D圆形" };
        // 选择边框颜色的命令
        public ICommand PickBorderColorCommand { get; }

        public 未来之窗_鼠标聚焦LensSettingsViewModel()  {
            {

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
                /*
                // ========== 核心：监听新属性变更，自动保存配置 ==========
                // 监听「超出绘制模式」变更
                LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_超出绘制模式)
                    .Subscribe(_ => Settings.SaveFairyAllianceConfig());

                // 监听「放大镜形状」变更
                LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_放大镜形状)
                    .Subscribe(_ => Settings.SaveFairyAllianceConfig());

                // 监听「出现效果」变更
                LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_出现效果)
                    .Subscribe(_ => Settings.SaveFairyAllianceConfig());
                */
        // ========== 改造后：监听+修正值+保存 ==========
        // 1. 超出绘制模式：监听变更 → 修正值 → 保存
        LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_超出绘制模式)
                    .Subscribe(newValue =>
                    {
                        var pureValue = CleanComboBoxValue(newValue, _validDrawModes, "向中心靠近");
                        if (pureValue != LensSettings.东方仙盟_鼠标聚焦放大_超出绘制模式)
                        {
                            LensSettings.东方仙盟_鼠标聚焦放大_超出绘制模式 = pureValue; // 修正值
                        }
                        Settings.SaveFairyAllianceConfig(); // 保存
                    });

                // 2. 放大镜形状：监听变更 → 修正值 → 保存
                LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_放大镜形状)
                    .Subscribe(newValue =>
                    {
                        var pureValue = CleanComboBoxValue(newValue, _validShapes_新版, "圆形");
                        if (pureValue != LensSettings.东方仙盟_鼠标聚焦放大_放大镜形状)
                        {
                            LensSettings.东方仙盟_鼠标聚焦放大_放大镜形状 = pureValue;
                        }
                        Settings.SaveFairyAllianceConfig();
                    });

                // 3. 出现效果：监听变更 → 修正值 → 保存
                LensSettings.ToReactivePropertyAsSynchronized(x => x.东方仙盟_鼠标聚焦放大_出现效果)
                    .Subscribe(newValue =>
                    {
                        var pureValue = CleanComboBoxValue(newValue, _validEffects, "默认");
                        if (pureValue != LensSettings.东方仙盟_鼠标聚焦放大_出现效果)
                        {
                            LensSettings.东方仙盟_鼠标聚焦放大_出现效果 = pureValue;
                        }
                        Settings.SaveFairyAllianceConfig();
                    });


                }


                // 核心工具方法：清理ComboBoxItem的错误值，返回纯字符串
                  string CleanComboBoxValue(string input, List<string> validValues, string defaultValue)
                {
                    if (string.IsNullOrEmpty(input)) return defaultValue;

                    // 情况1：包含"ComboBoxItem:" → 截取后面的纯文本
                    if (input.Contains("ComboBoxItem:"))
                    {
                        var parts = input.Split(new[] { "ComboBoxItem:" }, StringSplitOptions.RemoveEmptyEntries);
                        input = parts.Length > 1 ? parts[1].Trim() : defaultValue;
                    }

                    // 情况2：值不在合法列表中 → 用默认值
                    if (!validValues.Contains(input))
                    {
                        input = defaultValue;
                    }

                    return input.Trim();
                }
            }

        
    }
}

