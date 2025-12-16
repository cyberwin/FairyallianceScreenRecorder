using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Linq;
using Captura.Audio;
using Captura.FFmpeg;
using Captura.Imgur;
using Captura.MouseKeyHook;
using Captura.Video;
using Captura.Windows;
using System.Windows.Forms;
using Captura.Core.Settings;
using Captura.Windows.未来之窗;
using System.Runtime;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP.CyberWinPC.Helper.Loger;

namespace Captura
{
    // 在Settings.cs中，找到原有快捷键相关配置，新增以下内容
    

    public class Settings : PropertyStore
    {
        public static Settings Instance { get; } =  new Settings();
        public wlzc_HotkeySettings 未来之窗Hotkeys { get; set; } = new wlzc_HotkeySettings(); // 原有/新增快捷键配置

        // public 豆包太傻第900版本_LensSettings 东方仙盟鼠标聚焦 { get; set; } = new 豆包太傻第900版本_LensSettings(); // 原有/新增快捷键配置
        // 修改后（确保null安全，支持反序列化）
      //  public static 豆包太傻第900版本_LensSettings 东方仙盟鼠标聚焦 { get; set; }

        // 新增：东方仙盟鼠标聚焦配置（从独立文件读取）
        public static 豆包太傻第900版本_LensSettings 东方仙盟鼠标聚焦 { get; private set; } = new 豆包太傻第900版本_LensSettings();

        // 独立配置文件路径
        private static string FairyAllianceConfigPath => Path.Combine(ServiceProvider.SettingsDir, "fairyallianceai.config");

        // 静态构造函数：程序启动时自动读取独立配置

        public static 未来之窗Settings_特效_翻书 东方仙盟特效_翻书 { get; private set; } = new 未来之窗Settings_特效_翻书();


        private static string FairyAllianceConfigPath_new => Path.Combine(ServiceProvider.SettingsDir, "fairyallianceeddect.config");




        // public 

        static Settings()
        {
           // 东方仙盟鼠标聚焦 ??= new 豆包太傻第900版本_LensSettings(); // 新增鼠标聚焦配置初始化

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new JsonConverter[]
                {
                    new StringEnumConverter
                    {
                        AllowIntegerValues = false
                    }
                }
            };

            // 读取独立配置文件
             LoadFairyAllianceConfig();

            //
            LoadFairyAllianceConfig_翻页();
        }

        #region 独立配置文件读写方法
        /// <summary>
        /// 读取 fairyallianceai.config 配置
        /// </summary>
        public static void LoadFairyAllianceConfig()
        {
            try
            {
                // 确保配置目录存在
                if (!Directory.Exists(ServiceProvider.SettingsDir))
                {
                    Directory.CreateDirectory(ServiceProvider.SettingsDir);
                }

                // 如果文件不存在，使用默认配置
                if (!File.Exists(FairyAllianceConfigPath))
                {
                    东方仙盟鼠标聚焦 = new 豆包太傻第900版本_LensSettings();
                    SaveFairyAllianceConfig(); // 自动生成默认配置文件
                    return;
                }

                // 读取并反序列化配置
                var json = File.ReadAllText(FairyAllianceConfigPath);
                东方仙盟鼠标聚焦 = JsonConvert.DeserializeObject<豆包太傻第900版本_LensSettings>(json)
                                   ?? new 豆包太傻第900版本_LensSettings();
            }
            catch (Exception ex)
            {
                // 异常时使用默认配置
                东方仙盟鼠标聚焦 = new 豆包太傻第900版本_LensSettings();
                东方仙盟_LogHelper.WriteLog($"读取东方仙盟配置失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 保存配置到 fairyallianceai.config
        /// </summary>
        public static  bool SaveFairyAllianceConfig()
        {
            try
            {
                var json = JsonConvert.SerializeObject(东方仙盟鼠标聚焦, Formatting.Indented);
                File.WriteAllText(FairyAllianceConfigPath, json);
                return true;
            }
            catch (Exception ex)
            {
                东方仙盟_LogHelper.WriteLog($"保存东方仙盟配置失败：{ex.Message}", "错误");
                return false;
            }
        }
        #endregion

        //FairyAllianceConfigPath_new
        #region 独立配置文件读写方法-动画效果
        /// <summary>
        /// 读取 fairyallianceai.config 配置
        /// </summary>
        public static void LoadFairyAllianceConfig_翻页()
        {
            try
            {
                // 确保配置目录存在
                if (!Directory.Exists(ServiceProvider.SettingsDir))
                {
                    Directory.CreateDirectory(ServiceProvider.SettingsDir);
                }

                Log_Engine.write_logV2("配置", "路径", "翻页FoldSlider：" + FairyAllianceConfigPath_new);

                // 如果文件不存在，使用默认配置
                if (!File.Exists(FairyAllianceConfigPath_new))
                {
                    东方仙盟特效_翻书 = new 未来之窗Settings_特效_翻书();
                    Public_Var.东方仙盟特效_翻书 = 东方仙盟特效_翻书;
                    SaveFairyAllianceConfig_翻页(); //  
                    return;
                }

                // 读取并反序列化配置
                var json = File.ReadAllText(FairyAllianceConfigPath_new);
                未来之窗Settings_特效_翻书  东方仙盟特效_翻书2 = JsonConvert.DeserializeObject<未来之窗Settings_特效_翻书>(json)
                                 ;//  ?? new 未来之窗Settings_特效_翻书();


                Log_Engine.write_logV2("配置", "路径", "翻页FoldSlider："+json);


                Public_Var.东方仙盟特效_翻书 = 东方仙盟特效_翻书2;

                Log_Engine.write_logV2("配置", "路径", "FoldSlider你妈的：" + 东方仙盟特效_翻书2.FoldSlider);

                Log_Engine.write_logV2("配置", "路径", "FoldSlider东方仙盟特效_翻书呢喃：" + Public_Var.东方仙盟特效_翻书.FoldSlider);
            }
            catch (Exception ex)
            {
                // 异常时使用默认配置
                东方仙盟特效_翻书 = new 未来之窗Settings_特效_翻书();
                东方仙盟_LogHelper.WriteLog($"读取东方仙盟配置失败：{ex.Message}", "错误");
            }
        }

        /// <summary>
        /// 保存配置到 fairyallianceai.config
        /// </summary>
        public static bool SaveFairyAllianceConfig_翻页()
        {
            try
            {
                //Settings
                //var json = JsonConvert.SerializeObject(东方仙盟特效_翻书, Formatting.Indented);
                var json = JsonConvert.SerializeObject(东方仙盟特效_翻书, Formatting.Indented);
                File.WriteAllText(FairyAllianceConfigPath_new, json);
                return true;
            }
            catch (Exception ex)
            {
                东方仙盟_LogHelper.WriteLog($"保存东方仙盟配置失败：{ex.Message}", "错误");
                return false;
            }
        }
        #endregion

        public Settings()
        {
             
        }

        public Settings(FFmpegSettings FFmpeg, WindowsSettings WindowsSettings)
        {
            this.FFmpeg = FFmpeg;
            this.WindowsSettings = WindowsSettings;
        }

        static string GetPath() => Path.Combine(ServiceProvider.SettingsDir, "Captura.json");

        public bool Load()
        {
            try
            {
                var json = File.ReadAllText(GetPath());

                JsonConvert.PopulateObject(json, this);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                var sortedProperties = JObject.FromObject(this).Properties().OrderBy(J => J.Name);

                var jobj = new JObject(sortedProperties.Cast<object>().ToArray());

                File.WriteAllText(GetPath(), jobj.ToString());

                return true;
            }
            catch
            {
                return false;
            }
        }

        public ProxySettings Proxy { get; } = new ProxySettings();

        public ImgurSettings Imgur { get; } = new ImgurSettings();

        public WebcamOverlaySettings WebcamOverlay { get; set; } = new WebcamOverlaySettings();

        public MouseOverlaySettings MousePointerOverlay { get; set; } = new MouseOverlaySettings
        {
            Color = Color.FromArgb(200, 239, 108, 0)
        };

        public MouseClickSettings Clicks { get; set; } = new MouseClickSettings();
        
        public KeystrokesSettings Keystrokes { get; set; } = new KeystrokesSettings();

        public TextOverlaySettings Elapsed { get; set; } = new TextOverlaySettings();

        public ObservableCollection<CensorOverlaySettings> Censored { get; } = new ObservableCollection<CensorOverlaySettings>();
        
        public VisualSettings UI { get; } = new VisualSettings();

        public ScreenShotSettings ScreenShots { get; } = new ScreenShotSettings();

        public VideoSettings Video { get; } = new VideoSettings();

        public AudioSettings Audio { get; } = new AudioSettings();

        public FFmpegSettings FFmpeg { get; }

        public ObservableCollection<CustomOverlaySettings> TextOverlays { get; } = new ObservableCollection<CustomOverlaySettings>();

        public ObservableCollection<CustomImageOverlaySettings> ImageOverlays { get; } = new ObservableCollection<CustomImageOverlaySettings>();

        public SoundSettings Sounds { get; } = new SoundSettings();

        public TraySettings Tray { get; } = new TraySettings();

        public StepsSettings Steps { get; } = new StepsSettings();

        public AroundMouseSettings AroundMouse { get; } = new AroundMouseSettings();

        public WindowsSettings WindowsSettings { get; }

        public int PreStartCountdown
        {
            get => Get(0);
            set => Set(value);
        }

        public int Duration
        {
            get => Get(0);
            set => Set(value);
        }

        public bool CopyOutPathToClipboard
        {
            get => Get<bool>();
            set => Set(value);
        }

        public string OutPath
        {
            get => Get<string>();
            set => Set(value);
        }

        public string GetOutputPath()
        {
            var path = OutPath;

            string DefaultOutDir() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), nameof(Captura));

            // If Output Dircetory is not set, fallback to default
            path = string.IsNullOrWhiteSpace(path)
                ? DefaultOutDir()
                : path.Replace(ServiceProvider.CapturaPathConstant, ServiceProvider.AppDir);

            // If drive is not present, fallback to default
            if (!Directory.Exists(Path.GetPathRoot(path)))
            {
                path = DefaultOutDir();
            }

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        public string FilenameFormat
        {
            get => Get("%yyyy%-%MM%-%dd%/%HH%-%mm%-%ss%");
            set => Set(value);
        }

        public string GetFileName(string Extension, string FileName = null)
        {
            if (FileName != null)
                return FileName;

            if (!Extension.StartsWith("."))
                Extension = $".{Extension}";

            var outPath = GetOutputPath();

            if (string.IsNullOrWhiteSpace(FilenameFormat))
                return Path.Combine(outPath, $"{DateTime.Now:yyyy-MM-dd-HH-mm-ss}{Extension}");

            var now = DateTime.Now;

            var filename = FilenameFormat
                .Replace("%computer%", Environment.MachineName)
                .Replace("%user%", Environment.UserName)

                .Replace("%yyyy%", now.ToString("yyyy"))
                .Replace("%yy%", now.ToString("yy"))
                
                .Replace("%MMMM%", now.ToString("MMMM"))
                .Replace("%MMM%", now.ToString("MMM"))
                .Replace("%MM%", now.ToString("MM"))
                
                .Replace("%dd%", now.ToString("dd"))
                .Replace("%ddd%", now.ToString("ddd"))
                .Replace("%dddd%", now.ToString("dddd"))
                
                .Replace("%HH%", now.ToString("HH"))
                .Replace("%hh%", now.ToString("hh"))

                .Replace("%mm%", now.ToString("mm"))
                .Replace("%ss%", now.ToString("ss"))
                .Replace("%tt%", now.ToString("tt"))
                .Replace("%zzz%", now.ToString("zzz"));
            
            var path = Path.Combine(outPath, $"{filename}{Extension}");

            东方仙盟_LogHelper.WriteLog("获取文件名1:" + path,"保存");

            path = path.Trim('.');
            东方仙盟_LogHelper.WriteLog("获取文件名1-1:" + path, "保存");

            var baseDir = Path.GetDirectoryName(path);
            if (baseDir != null) 
                Directory.CreateDirectory(baseDir);

            if (!File.Exists(path))
                return path;

            var i = 1;

            do
            {
                path = Path.Combine(outPath, $"{filename} ({i++}){Extension}");
                东方仙盟_LogHelper.WriteLog("获取文件名2:" + path, "保存");
            }
            while (File.Exists(path));

            return path;
        }

        public bool IncludeCursor
        {
            get => Get(true);
            set => Set(value);
        }

        public bool RegionPickerHotkeyAutoStartRecording
        {
            get => Get(true);
            set => Set(value);
        }
    }
}
