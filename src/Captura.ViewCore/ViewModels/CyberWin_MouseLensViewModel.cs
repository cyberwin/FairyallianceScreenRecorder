using Captura.Loc;
using Captura.ViewModels;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Captura.ViewCore.ViewModels
{
    public class CyberWin_MouseLensViewModel : ViewModelBase
    {
        public CyberWin_MouseLensViewModel(Settings Settings, ILocalizationProvider Loc) : base(Settings, Loc)
        {
        }
        // 可绑定的配置参数
        public ReactiveProperty<int> LensSize { get; } = new ReactiveProperty<int>(400);
        public ReactiveProperty<float> ZoomFactor { get; } = new ReactiveProperty<float>(2.0f);
        public ReactiveProperty<bool> DrawBorder { get; } = new ReactiveProperty<bool>(true);
        public ReactiveProperty<Color> BorderColor { get; } = new ReactiveProperty<Color>(Color.Red);
        public ReactiveProperty<int> BorderThickness { get; } = new ReactiveProperty<int>(2);
        public ReactiveProperty<bool> IsEnabled { get; } = new ReactiveProperty<bool>(false);
    }
}

