using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
//using System.Windows.Forms;
using  Captura.Windows;
using System.Drawing;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP
{
    public class Public_Var
    {
        public static string CyberWinPHP_Path = "";// Application.StartupPath + "/CyberWinPHP";

        public static string 打印_setting_path = CyberWinPHP_Path + "/CyberPHP_config/Printer.cyberphp";

        public static string 系统_setting_path = CyberWinPHP_Path + "/CyberPHP_config/system.cyberphp";

        public static string setting_path_系统按键 = CyberWinPHP_Path + "/CyberPHP_config/shortcut.cyberphp";

        public static string setting_path_语言_打印 = CyberWinPHP_Path + "/CyberPHP_Language/Chinese (Simplified).frl";

        public static string setting_path_报表 = CyberWinPHP_Path + "/CyberPHP_report/";

        //2018 年2月26日 
        public static bool setting_browser_ispopup = true;

        //2019-8-8-6
        public static string 系统_wlzco2o_path = CyberWinPHP_Path + "/CyberPHP_config/O2O_Settings.cyberphp";

        public static string 浏览器_setting_path = CyberWinPHP_Path + "//CyberPHP_config//CyberWin.cyber";

        public static string 系统_wlzcscriptplug_path = CyberWinPHP_Path + "/CyberPHP_config/CyberWin_Script.cyberphp";


        public static string 系统_结束时候终止全部同名进程 = "0";


        public static string 系统_显示副屏幕 = "0";

        public static string 系统_显示漂浮球 = "0";
        public static string 未来之窗系统sn = "";
        //未来之窗登录key
        public static string 未来之窗系统_start_linkley = "20220714";

        //cyberwin_crawl_open
        //2022-10-26
        //get浏览器抓包Config
        public static string 未来之窗系统_crawl_抓包 = "0";

        public static string 系统_wlzc抓包_path = CyberWinPHP_Path + "/CyberPHP_config/CyberWin_Crawl.ini";
        public static int 系统_wlzc抓包_Interval = 1000;


        public static string 系统_wlzcAIOT_OCR_Engine_path = CyberWinPHP_Path + "/CyberPHP_AIOT_OCR_Engine/";

        public static string 系统_wlzc抓包_地址匹配 = "*";
        public static string 系统_wlzc抓包_crawl_script = "";
        public static string 系统_wlzc抓包_crawl_专用script = "";

        public static string 系统_wlzc抓包_crawl_当前_Host = "";
        public static string 系统_wlzc抓包_crawl_通用_script = "";

        public static string 系统_wlzc抓包_crawl_当前_script = "";


        //2023-7-25 数据伺服
        public static string 未来之窗系统_数据伺服_服务器 = "";
        public static string 未来之窗系统_数据伺服_数据库 = "";

        //2023-9-26 本地定时器
        public static string 未来之窗系统_本地服务_类型 = "";
        public static string 未来之窗系统_本地服务_任务 = "";


        public static string 未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = "N";


        public static   未来之窗Settings_特效_翻书 东方仙盟特效_翻书;

        public static string 未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = "N";

        //豆包太傻第900版本_LensMagnifierOverlay.cs
        public static string 未来之窗_东方仙盟_仙盟创梦_录像_鼠标聚焦mouseLens = "Y";
        // System.Windows.Media.Color
        public static   Color 未来之窗_东方仙盟_仙盟创梦_粉笔画笔颜色 = Color.Black;
    }
}
