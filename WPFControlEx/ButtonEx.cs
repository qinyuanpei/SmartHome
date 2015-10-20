using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPFControlEx
{
    /// <summary>
    /// 这是一个Button的扩展类，用于构建一些图形化的状态按钮
    /// </summary>
    public class ButtonEx : Button
    {
        /// <summary>
        /// 设置当鼠标移动时的图像笔刷
        /// </summary>
        public static readonly DependencyProperty Property_HoverBrush;

        /// <summary>
        /// 鼠标经过时的图像笔刷
        /// </summary>
        public ImageBrush HoverBrush
        {
            get
            {
                return base.GetValue(ButtonEx.Property_HoverBrush) as ImageBrush;
            }
            set
            {
                base.SetValue(ButtonEx.Property_HoverBrush, value);
            }
        }


       
        static ButtonEx()
        {
            ButtonEx.Property_HoverBrush = DependencyProperty.Register("EnterBrush", typeof(ImageBrush), typeof(ButtonEx), new PropertyMetadata(null));
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ButtonEx), new FrameworkPropertyMetadata(typeof(ButtonEx)));
        }

        public ButtonEx()
        {
            base.Content = "";
            base.Background = Brushes.Orange;
        }
    }
}
