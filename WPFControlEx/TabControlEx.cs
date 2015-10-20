using System.Windows;
using System.Windows.Controls;

namespace WPFControlEx
{
    class TabControlEx:TabControl
    {
        public TabControlEx()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TabControlEx), (PropertyMetadata)new FrameworkPropertyMetadata((object)typeof(TabControlEx)));
        }
    }
}
