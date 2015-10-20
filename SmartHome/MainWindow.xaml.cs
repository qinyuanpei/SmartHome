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
using System.Windows.Interop;
using System.IO;
using WPFControlEx;
using SmartHome.Classes;
using Xml4DB;
using System.Windows.Threading;
using System.Runtime.InteropServices;


namespace SmartHome
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : WindowEx
    {
        #region 全局变量定义
        //定义应用程序词典
        private Dictionary<string, List<SmartApp>> mAppDic = new Dictionary<string, List<SmartApp>>();
        //定义应用程序列表
        private List<SmartApp> mAppList = new List<SmartApp>();
        //定义应用程序分类名称数组
        private string [] mNames=new string[]{"常用","游戏","音乐","电影","图片","工具"};
        //定义应用程序数据存储路径
        private string mFileName;
        //定义列表是否可编辑的标志变量
        private bool isEditable = false;
        //定义窗体悬停类型
        private LocationType mType = LocationType.None;
        //定义窗体状态的标志变量
        private bool isShow = true;
        //定义界面隐藏时的停靠在屏幕边缘的距离
        private const int HIDE_BORDER = 7;
        //定义应用程序配置文件路径
        private string Config = Environment.CurrentDirectory.ToString() + "\\Config";


        public MainWindow()
        {
            InitializeComponent();
        }
        #endregion

        #region 界面加载事件
        /// <summary>
        /// 界面加载
        /// </summary>
        private void WindowEx_Loaded(object sender, RoutedEventArgs e)
        {
            InitAPP();
           // InitWeather();
        }
        #endregion

        #region 初始化天气
        /// <summary>
        /// 初始化天气
        /// </summary>
        private void InitWeather()
        {
            //如果网络已连接，则获取最新的天气，否则读取本地天气
            if (WebState.isConneted())
            {
                //获取天气
                WeatherStructure mWeather = WebWeather.getWeatherByCode(WebWeather.getCodeByIP());
                SetWeather(mWeather);
                //更新本地数据
                File.Delete(Config + "\\Weather.xml");
                XmlDB mDB = XmlDBFactory.CreatXmlDB("Weather",Config + "\\Weather.xml",1);
                mDB.Insert(mWeather);
                mDB.Commit();
            }
            else
            {
                XmlDB mDB = XmlDBFactory.LoadXmlDB(Config+"\\Weather.xml");
                List<object> mObjects = mDB.ReadByObject(new WeatherStructure());
                WeatherStructure mWeather = (WeatherStructure)mObjects[0];
                SetWeather(mWeather);
            }
        }

        /// <summary>
        /// 设定天气
        /// </summary>
        /// <param name="mWeather"></param>
        private void SetWeather(WeatherStructure mWeather)
        {//天气图标
            BitmapSource mSource = null;
            //根据天气情况匹配对应的天气图标
            switch (mWeather.weather)
            {
                case "晴":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_1.png", UriKind.Relative));
                    break;
                case "晴转多云":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_2.png", UriKind.Relative));
                    break;
                case "多云转晴":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_3.png", UriKind.Relative));
                    break;
                case "雷阵雨":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_5.png", UriKind.Relative));
                    break;
                case "小到中雨":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_4.png", UriKind.Relative));
                    break;
                case "小到中雪":
                    mSource = new BitmapImage(new Uri("Resources\\Weather\\Weather_7.png", UriKind.Relative));
                    break;
            }
            //设定温度
            Weather_Tem.SetValue(Label.ContentProperty, mWeather.temp1);
            //设定城市
            Weather_City.SetValue(Label.ContentProperty, mWeather.city);
            //设定描述
            Weather_Descr.SetValue(Label.ContentProperty, mWeather.weather);
            //设定图标
            Weather_Icon.SetValue(System.Windows.Controls.Image.SourceProperty, mSource);
        }


        #endregion

        #region  初始化应用程序
        /// <summary>
        /// 初始化应用程序
        /// </summary>
        private void InitAPP()
        {
            //创建程序数据目录
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SmartHome\\Data"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SmartHome\\Data");
            }
            //创建数据文件路径
            mFileName=Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\SmartHome\\Data\\Data.Xml";
            if (!File.Exists(mFileName))
            {
                //创建XmlDB
                XmlDB mDB =XmlDBFactory.CreatXmlDB("SmartHome", mFileName, 1);
                //插入预设的应用程序
                mDB.Insert(new SmartApp("记事本","C:\\Windows\\system32\\notepad.exe","工具"));
                mDB.Insert(new SmartApp("截图工具", "C:\\Windows\\system32\\SnippingTool.exe", "工具"));
                mDB.Insert(new SmartApp("画图", "C:\\Windows\\system32\\mspaint.exe", "工具"));
                mDB.Insert(new SmartApp("IE", "C:\\Program Files\\Internet Explorer\\iexplore.exe", "工具"));
                mDB.Commit();
            }
                ReadApps();
        }

        #endregion

        #region 向指定的Grid添加应用程序
        /// <summary>
        /// 向指定的Grid添加应用程序
        /// </summary>
        private void CreateAppForGrid(Grid mGrid,List<SmartApp> mAppList)
        {
            for (int i = 0; i < mAppList.Count; i++)
            {
                SmartAppControl mControl = mAppList[i].BuilderUI(new RoutedEventHandler(App_Click),new RoutedEventHandler(App_Click));
                if (i < 4)
                {
                    mControl.SetValue(Grid.RowProperty, 0);
                    mControl.SetValue(Grid.ColumnProperty, i);
                }
                else
                {
                    mControl.SetValue(Grid.RowProperty, i / 4);
                    mControl.SetValue(Grid.ColumnProperty, i % 4);
                }
                mGrid.Children.Add(mControl);
            }
        }

        #endregion

        #region 读取App列表
        /// <summary>
        /// 读取App列表
        /// </summary>
        private void ReadApps()
        {
            XmlDB mDB = XmlDBFactory.LoadXmlDB(mFileName);
            //读取应用程序列表
            for (int i = 0; i < 6; i++)
            {
                //读取类型值为i的应用程序列表
                List<object> mObjects = mDB.ReadByObject(new SmartApp(null,null, mNames[i]));
                //将数组转化为List
                List<SmartApp> mList = new List<SmartApp>();
                for (int j = 0; j < mObjects.Count; j++)
                {
                    mList.Add((SmartApp)mObjects[j]);
                }
                mAppDic.Add(mNames[i], mList);
            }
            //将应用程序添加到对应的桌面
            foreach (KeyValuePair<string, List<SmartApp>> mValue in mAppDic)
            {
                //获取该桌面下的应用列表
                mAppList = mValue.Value;
                //获取该桌面
                Grid mGrid = getGrid(mValue.Key);
                CreateAppForGrid(mGrid, mAppList);
            }
        }

        #endregion

        #region 关闭及最小化事件
        /// <summary>
        /// 关闭应用程序事件
        /// </summary>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 最小化窗口事件
        /// </summary>
        private void BtnMin_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        #endregion

        #region 应用程序单击事件
        /// <summary>
        /// 应用程序单击事件
        /// </summary>
        private void App_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditable)
            {
                //获取当前活动Tab索引
                int mType = Tab.SelectedIndex;
                //获取Button
                Button mBtn = (Button)sender;
                //获取当前父控件
                StackPanel mPanel = (StackPanel)mBtn.Parent;
                Grid mGrid = (Grid)mPanel.Parent;
                SmartAppControl mControl = (SmartAppControl)mGrid.Parent;
                //获取父控件在Grid中的行数和列数
                int mRow = (int)mControl.GetValue(Grid.RowProperty);
                int mCol = (int)mControl.GetValue(Grid.ColumnProperty);
                //计算元素索引
                int mIndex = 4 * mRow + mCol;
                //获取指定类型的应用程序列表
                mAppList = getAppList(mType);
                //获取应用程序
                SmartApp mApp = mAppList[mIndex];
                if (!File.Exists(mApp.AppPath))
                {
                    if (MessageBox.Show("文件不存在或者应用程序无法打开！是否删除该应用？", "删除应用", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        //从列表中移除mApp
                        mAppList.RemoveAt(mIndex);
                        //修改数据文件
                        XmlDB mDB = XmlDBFactory.LoadXmlDB(mFileName);
                        mDB.Delete(new SmartApp(mApp.AppName, mApp.AppPath, mNames[mType]));
                        mDB.Commit();
                        //从桌面中移除该应用程序
                        getGrid(mNames[mType]).Children.Remove(mControl);
                        //移除全部应用程序
                        getGrid(mNames[mType]).Children.Clear();
                        //重新加载应用程序
                        CreateAppForGrid(getGrid(mNames[mType]), mAppList);
                    }
                }
                else
                {
                    //启动应用程序
                    mApp.Start();
                }
            }
            else
            {
                //获取当前活动Tab索引
                int mType = Tab.SelectedIndex;
                //获取Button
                Button mBtn = (Button)sender;
                //获取当前父控件
                Grid mGrid = (Grid)mBtn.Parent;
                SmartAppControl mControl = (SmartAppControl)mGrid.Parent;
                //获取父控件在Grid中的行数和列数
                int mRow = (int)mControl.GetValue(Grid.RowProperty);
                int mCol = (int)mControl.GetValue(Grid.ColumnProperty);
                //计算元素索引
                int mIndex = 4 * mRow + mCol;
                //获取指定类型的应用程序列表
                mAppList = getAppList(mType);
                //获取应用程序
                SmartApp mApp = mAppList[mIndex];
                //从列表中移除mApp
                mAppList.RemoveAt(mIndex);
                //修改数据文件
                XmlDB mDB = XmlDBFactory.LoadXmlDB(mFileName);
                mDB.Delete(new SmartApp(mApp.AppName, mApp.AppPath, mNames[mType]));
                mDB.Commit();
                //从桌面中移除该应用程序
                getGrid(mNames[mType]).Children.Remove(mControl);
                //移除全部应用程序
                getGrid(mNames[mType]).Children.Clear();
                //重新加载应用程序
                CreateAppForGrid(getGrid(mNames[mType]), mAppList);
                //将状态还原为可编辑
                isEditable = true;
                //将每一个App设为可编辑状态
                foreach (SmartAppControl m in getGrid(mNames[mType]).Children)
                {
                    m.SetControlEditable(true);
                }
            }
        }
        #endregion

        #region 返回指定索引的App列表
        /// <summary>
        /// 返回指定索引的App列表
        /// </summary>
        private List<SmartApp> getAppList(int mIndex)
        {
            List<SmartApp> mApps = new List<SmartApp>();
            switch (mIndex)
            {
                case 0:
                    mApps = mAppDic["常用"];
                    break;
                case 1:
                    mApps = mAppDic["游戏"];
                    break;
                case 2:
                    mApps = mAppDic["音乐"];
                    break;
                case 3:
                    mApps = mAppDic["电影"];
                    break;
                case 4:
                    mApps = mAppDic["图片"];
                    break;
                case 5:
                    mApps = mAppDic["工具"];
                    break;
            }
            return mApps;
        }

        #endregion

        #region 返回指定的Grid
        /// <summary>
        /// 返回指定的Grid
        /// </summary>
        /// <param name="mType"></param>
        /// <returns></returns>
        public Grid getGrid(string mType)
        {
            Grid mGrid = null;
            switch (mType)
            {
                case "常用":
                    mGrid = Grid0;
                    break;
                case "游戏":
                    mGrid = Grid1;
                    break;
                case "音乐":
                    mGrid = Grid2;
                    break;
                case "电影":
                    mGrid = Grid3;
                    break;
                case "图片":
                    mGrid = Grid4;
                    break;
                case "工具":
                    mGrid = Grid5;
                    break;
            }
            return mGrid;
        }

        #endregion

        #region 拖动文件或应用程序到程序界面
        /// <summary>
        /// 拖动图标到智能桌面事件
        /// </summary>
        private void WindowEx_Drop(object sender, DragEventArgs e)
        {
            //从桌面向程序界面拖拽应用程序
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                //获取文件信息
                string[] mDropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                CreatNewApp(mDropFiles[0]);
            }
        }

        #endregion

        #region 应用程序Tab菜单
        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            if (!isEditable)
            {
                isEditable = true;
                //获取当前活动Tab索引
                int mType = Tab.SelectedIndex;
                //获取Grid
                Grid mGrid = getGrid(mNames[mType]);
                //将每一个App设为可编辑状态
                foreach (SmartAppControl mControl in mGrid.Children)
                {
                    mControl.SetControlEditable(true);
                }
            }
            else
            {
                isEditable = false;
                //获取当前活动Tab索引
                int mType = Tab.SelectedIndex;
                //获取Grid
                Grid mGrid = getGrid(mNames[mType]);
                //将每一个App设为可编辑状态
                foreach (SmartAppControl mControl in mGrid.Children)
                {
                    mControl.SetControlEditable(false);
                }
            }
        }

        /// <summary>
        /// 添加新应用
        /// </summary>
        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog mDialog = new Microsoft.Win32.OpenFileDialog();
            mDialog.Multiselect = true;
            if (mDialog.ShowDialog()==true)
            {
                string[] mFiles = mDialog.FileNames;
                foreach (string mFile in mFiles)
                {
                    CreatNewApp(mFile);
                }
            }
        }

        /// <summary>
        /// 双击窗体退出当前编辑状态
        /// </summary>
        private void WindowEx_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            isEditable = false;
            //获取当前活动Tab索引
            int mType = Tab.SelectedIndex;
            //获取Grid
            Grid mGrid = getGrid(mNames[mType]);
            //将每一个App设为可编辑状态
            foreach (SmartAppControl mControl in mGrid.Children)
            {
                mControl.SetControlEditable(false);
            }
        }
        #endregion

        #region 向应用程序界面添加应用
        private void CreatNewApp(string mFile)
        {
            //获取当前Tab索引
            int mType = Tab.SelectedIndex;
            //获取当前应用程序列表
            mAppList = getAppList(mType);
            //获得XmlDB数据库
            XmlDB mDB = XmlDBFactory.LoadXmlDB(mFileName);
            SmartApp mApp = new SmartApp(mFile, mNames[mType].ToString());
            mDB.Insert(mApp);
            mDB.Commit();
            //构建界面
            SmartAppControl mControl = mApp.BuilderUI(new RoutedEventHandler(App_Click), new RoutedEventHandler(App_Click));
            if (mAppList.Count < 4)
            {
                mControl.SetValue(Grid.RowProperty, 0);
                mControl.SetValue(Grid.ColumnProperty, mAppList.Count);
            }
            else
            {
                mControl.SetValue(Grid.RowProperty, (mAppList.Count) / 4);
                mControl.SetValue(Grid.ColumnProperty, (mAppList.Count) % 4);
            }
            Grid mGrid = getGrid(mNames[mType]);
            //添加App到桌面
            mGrid.Children.Add(mControl);
            //修改词典
            mAppList = getAppList(mType);
            mAppList.Add(mApp);
        }
        #endregion

        #region 贴边自动隐藏

        private void WindowEx_MouseLeave(object sender, MouseEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                if (!this.isShow)
                {
                    this.Top = HIDE_BORDER - this.ActualHeight;
                    this.isShow = false;
                    this.Topmost = true;
                }
            }
        }

        private void WindowEx_MouseMove(object sender, MouseEventArgs e)
        {
            //定义POINT类型
            POINT mPoint;
            if (!GetCursorPos(out mPoint))
                return;
            //判断窗体状态
            if (this.WindowState == WindowState.Normal)
            {
                if (!this.isShow)
                {
                    //判断鼠标时都在窗体内
                    if (this.Left < mPoint.x && this.Left + this.ActualWidth > mPoint.x
                        && this.Top < mPoint.y && this.Top + this.ActualHeight > mPoint.y)
                    {
                        this.Top = 0;
                        this.Topmost = true;
                        this.isShow = true;
                    }
                }
                else
                {
                       //从左上角隐藏
                        if (this.Top <= 0 && this.Left <= 0)
                        {
                            this.Left = 0;
                            this.Top = HIDE_BORDER - this.ActualHeight;
                            this.mType = LocationType.TopL;
                            this.isShow = false;
                            this.Topmost = true;
                        }
                        //从右上角隐藏
                        else if (this.Top <= 0 && this.Left >= SystemParameters.VirtualScreenWidth - this.ActualWidth)
                        {
                            this.Left = SystemParameters.VirtualScreenWidth - this.ActualWidth;
                            this.Top = HIDE_BORDER - this.ActualHeight;
                            this.mType = LocationType.TopR;
                            this.isShow = false;
                            this.Topmost = true;
                        }
                        //顶部隐藏
                        else if (this.Top <= 0)
                        {
                            this.Top = HIDE_BORDER - this.ActualHeight;
                            this.mType = LocationType.Top;
                            this.isShow = false;
                            this.Topmost = true;
                        }
                        //不隐藏
                        else
                        {
                            this.mType = LocationType.None;
                        }
                 }
            }
        }
        #endregion

        #region 获取鼠标坐标的Win32API
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool GetCursorPos(out POINT pt);
        #endregion

   }
}
