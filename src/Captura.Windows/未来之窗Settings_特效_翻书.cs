using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Captura.Windows
{

    /// <summary>
    /// 豆包太傻第900版本-透镜放大设置
    /// </summary>
    public class 未来之窗Settings_特效_翻书 : PropertyStore
    {
        // 是否启用透镜效果
        private bool  _开始录制翻页 =false;
        private float _FoldSlider = 15;
        private int _ShadowSlider = 180;

        private string _EaseComboBox = "";
        public bool 开始录制翻页
        {
            get => _开始录制翻页; // 直接返回字段值
            set => _开始录制翻页 = value; // 直接将值存入字段
        }

        //  
        public int DurationSlider
        {
            get => Get(3);
            set => Set(Math.Max(5, Math.Min(1, value)));
        }

        //  
        public float FoldSlider
        {
            
            get => _FoldSlider; // 直接返回字段值
            set => _FoldSlider = value;// Math.Max(30, Math.Min(3, value);
        }


        
        public int ShadowSlider
        {
            get => _ShadowSlider;// Get(180);
            set => _ShadowSlider = value;// Set(Math.Max(255, Math.Min(100, value)));
        }

        public string EaseComboBox
        {
            get => _EaseComboBox;// Get("Linear");
            set => _EaseComboBox = value;// Set(value);
        }
      

       

        
    }
}
