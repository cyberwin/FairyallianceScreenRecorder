using Captura.Video;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
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
using Reactive.Bindings;
using System.Windows.Media;
using System.Windows.Forms;
using Captura.ViewCore.ViewModels; // 用于ColorDialog

namespace Captura.Pages
{
    /// <summary>
    /// CyberWin_LensSettingsPage.xaml 的交互逻辑
    /// </summary>
    public partial class CyberWin_LensSettingsPage 
    {
        // 鼠标聚焦设置实例
        

        public CyberWin_LensSettingsPage()
        {

            // 第一步：先加载保存的配置（关键！）
            Settings.LoadFairyAllianceConfig();

            // 第二步：再初始化ViewModel
         //   InitializeComponent();
           // DataContext = new 未来之窗_鼠标聚焦LensSettingsViewModel();

            InitializeComponent();
            // 初始化设置（如果需要持久化，可从配置管理器获取）
            //  DataContext = new LensSettingsViewModel(); // 绑定ViewModel
            DataContext = new 未来之窗_鼠标聚焦LensSettingsViewModel();

          //  DataContext = Settings.东方仙盟鼠标聚焦;// new 未来之窗_鼠标聚焦LensSettingsViewModel(); // 绑定ViewModel
           // Settings

            //Settings.Instance.东方仙盟鼠标聚焦
        }
        // 所有设置变更时自动保存
        private void OnSettingChanged(object sender, RoutedEventArgs e)
        {
            Settings.SaveFairyAllianceConfig();
        }

        // Slider值变更时自动保存
        private void OnSettingChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.SaveFairyAllianceConfig();
        }

    }
    
}
