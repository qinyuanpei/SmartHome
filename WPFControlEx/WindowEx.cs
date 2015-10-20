using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WPFControlEx
{

        /// <summary>
        /// 带阴影效果的的Window扩展类
        /// </summary>
        public class WindowEx : Window
        {

            public WindowEx()
            {
                this.DefaultStyleKey = typeof(WindowEx);
                this.MouseLeftButtonDown+=WindowEx_MouseLeftButtonDown;
            }

            private void WindowEx_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
            {
                this.DragMove();
            }


        }
}
