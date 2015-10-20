using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Input;

namespace SmartHome.Classes
{
    /// <summary>
    /// 智能桌面应用类
    /// </summary>
    public class SmartApp
    {
        /// <summary>
        /// 应用程序名称
        /// </summary>
        private string mAppName;
        public string AppName
        {
            get { return mAppName; }
            set { mAppName = value; }
        }

        /// <summary>
        /// 应用程序路径
        /// </summary>
        private string mAppPath;
        public string AppPath
        {
            get { return mAppPath; }
            set { mAppPath = value; }
        }

        /// <summary>
        /// 应用程序类型
        /// </summary>
        private string mAppType;
        public string AppType
        {
            get { return mAppType; }
            set { mAppType = value; }
        }
       


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mAppPath"></param>
        public SmartApp()
        {
            
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartApp(string mAppPath)
        {
            this.mAppPath = mAppPath;
            if (mAppPath != "")
            {
                this.mAppName = mAppPath.Substring(mAppPath.LastIndexOf("\\") + 1, mAppPath.LastIndexOf(".") - mAppPath.LastIndexOf("\\") - 1);
            }
        }


        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartApp(string mAppName, string mAppPath,string mAppType)
        {
            this.mAppName = mAppName;
            this.mAppPath = mAppPath;
            this.mAppType = mAppType;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mAppPath"></param>
        public SmartApp(string mAppPath,string mAppType)
        {
            this.mAppPath = mAppPath;
            if (mAppPath != "")
            {
                this.mAppName = mAppPath.Substring(mAppPath.LastIndexOf("\\") + 1, mAppPath.LastIndexOf(".") - mAppPath.LastIndexOf("\\") - 1);
            }
            this.mAppType = mAppType;
        }

        /// <summary>
        /// 启动App的方法
        /// </summary>
        public void Start()
        {
            Process mProcess = new Process();
            mProcess.StartInfo.FileName = this.mAppPath;
            mProcess.Start();
        }

        /// <summary>
        /// 将App实体类封装为UI元素
        /// </summary>
        /// <param name="mApp"></param>
        /// <returns></returns>
        public SmartAppControl BuilderUI(RoutedEventHandler mClickHandler1,RoutedEventHandler mClickHandler2)
        {
            SmartAppControl mControl = new SmartAppControl(this, mClickHandler1, mClickHandler2);
            return mControl;
        }
    }
}
