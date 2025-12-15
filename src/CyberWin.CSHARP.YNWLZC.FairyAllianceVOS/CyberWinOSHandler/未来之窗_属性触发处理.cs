using CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberPHP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CyberWin.CSHARP.YNWLZC.FairyAllianceVOS.CyberWinOSHandler
{
    public class 未来之窗_属性触发处理
    {
        /// <summary>
        /// 未来之窗笔刷模式：Y=笔刷模式，N=鼠标模式（清空画布）
        /// </summary>
        public static string 未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式
        {
            get => Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式;
            set
            {

                Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = value;
                /*
                if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 == value) return;

                Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = value?.ToUpper() switch
                {
                    "Y" => "Y",
                    _ => "N" // 非Y均视为N
                };
                */


                // 触发事件：变量变化时通知所有订阅者（如RegionSelector）
                On未来之窗笔刷模式变更?.Invoke(null, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 笔刷模式变更事件（供UI层监听更新）
        /// </summary>
        public static event EventHandler On未来之窗笔刷模式变更;


        public static event EventHandler On未来之窗翻页模式变更;

        public static string 未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式
        {
            get => Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式;
            set
            {

                Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_特效翻页模式 = value;
                /*
                if (Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 == value) return;

                Public_Var.未来之窗_东方仙盟_仙盟创梦_录像_未来之窗笔刷模式 = value?.ToUpper() switch
                {
                    "Y" => "Y",
                    _ => "N" // 非Y均视为N
                };
                */


                // 触发事件：变量变化时通知所有订阅者（如RegionSelector）
                On未来之窗翻页模式变更?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
