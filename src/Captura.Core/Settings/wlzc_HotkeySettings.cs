using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Captura.Core.Settings
{
    public class wlzc_HotkeySettings
    {
        // 原有快捷键配置（比如开始/暂停录制、停止录制等）...

        // 新增：画笔切换快捷键（默认设为F8，可自定义）
        public Keys ToggleBrushHotkey { get; set; } = Keys.F8;
        // 标记是否已按下（避免重复触发）
        [JsonIgnore] // 忽略序列化，不保存到配置文件
        public bool IsBrushHotkeyPressed { get; set; } = false;
    }
}
