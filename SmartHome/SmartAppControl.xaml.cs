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
using System.Drawing;
using SmartHome.Classes;
using System.Windows.Interop;
using System.IO;

namespace SmartHome
{
    /// <summary>
    /// SmartAppControl.xaml 的交互逻辑
    /// </summary>
    public partial class SmartAppControl : UserControl
    {
        private BitmapSource mSource;
        public SmartAppControl(SmartApp mApp,RoutedEventHandler mClick1,RoutedEventHandler mClick2)
        {
            //默认的控件初始化函数
            InitializeComponent();
            //通过SmartApp实现数据绑定
            mAppButton.Click += mClick1;
            mAppDelete.Click += mClick2;
            //获取应用程序图标
            if (!File.Exists(mApp.AppPath))
            {
                 mSource = new BitmapImage(new Uri("Resources\\Icon\\Icon_Error.png", UriKind.Relative));
            }
            else
            {
                Icon mIcon = System.Drawing.Icon.ExtractAssociatedIcon(mApp.AppPath);
                mSource = Imaging.CreateBitmapSourceFromHBitmap(
                    mIcon.ToBitmap().GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            //绑定应用程序图标
            mAppImage.Source = mSource;
            //绑定应用程序名称
            mAppLabel.Content = mApp.AppName;
            //绑定ToolTip
            mToolTip1.Content = mApp.AppName;
            mToolTip2.Content = mApp.AppName;
        }

        public void SetControlEditable(bool isEditable)
        {
            if (isEditable)
            {
                mAppDelete.Visibility = Visibility.Visible;
                mAppButton.IsEnabled = false;
            }
            else 
            {
                mAppDelete.Visibility = Visibility.Hidden;
                mAppButton.IsEnabled = true;
            }
        }


    }
}
