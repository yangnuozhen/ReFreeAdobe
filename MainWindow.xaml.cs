using ReFreeAdobe.src;
using ReFreeAdobe.src.model;
using ReFreeAdobe.src.view;
using ReFreeAdobe.Properties;
using ReFreeAdobe.src;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FreeAdobe
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        
        private static List<PatchInfo> patchInfos;
        private List<AdobeProductBean> adobeProductBeans = new List<AdobeProductBean>();
        public MainWindow()
        {
            
            InitializeComponent();
            initView();
            startCheck();
        }

        private void initView() {
            
            patchInfos = AdobePatchUtil.loadAllProduct();
            HashSet<string> productSet = new HashSet<string>();
            foreach (PatchInfo info in patchInfos)
            {
                if (!productSet.Contains(info.Product + info.Version)) {
                    productSet.Add(info.Product+info.Version);
                    adobeProductBeans.Add(new AdobeProductBean(info.Product, info.Version, "resources/" + info.ProductName + ".png", info.ProductName + " " + info.Version, "", "https://www.baidu.com",info.RelativeLaunchPath));
                }
                
            }
            lbProduct.ItemsSource = adobeProductBeans;
            
        }

       

        private void btnPatch_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            AdobeProductBean bean = btn.DataContext as AdobeProductBean;
            if (bean != null) {
                if (!File.Exists(bean.LaunchPath))
                {
                    HelpWindow helpWindow = new HelpWindow("提示", "软件未安装，或者未安装在默认目录，请按照以下提示使用\n\n" +
                        "1、前往Adobe官网下载Creative Cloud并安装\n" +
                "2、通过Creative Cloud下载你想要的应用例如Photoshop（需要登录Adobe ID）\n" +
                "3、在本软件内找到你下载的版本，点击优化即可使用\n" +
                "4、如果更改过安装目录，请在左下角设置内修改安装目录\n\n" +
                "点击确定按钮去官网下载Creative Cloud", new HelperHandler("help"));
                    helpWindow.Show();
                }
                else
                {
                    try
                    {
                        bool succ = AdobePatchUtil.patchProduct(bean.Product, bean.Version);
                        if (succ)
                        {
                            //alert(bean.Name + "优化成功，已经可以正常使用");
                            PatchResult patchResult = new PatchResult("优化成功", bean.Name + "优化成功，已经可以正常使用", true);
                            patchResult.ShowDialog();
                        }
                        else
                        {
                            //alert(bean.Name + "优化失败,可能已经优化过，请尝试启动。如果未优化成功请联系开发者反馈");
                            PatchResult patchResult = new PatchResult("优化失败", bean.Name + "优化失败,可能已经优化过，请尝试启动。如果未优化成功请联系开发者反馈", false);
                            patchResult.ShowDialog();
                        }
                        //https://github.com/yangnuozhen/ReFreeAdobe/issues/1
                        if (bean.Name == "Photoshop 2023" || bean.Name == "Photoshop 2024")
                        {
                            
                            HelpWindow helpWindow = new HelpWindow("Photoshop提示", "Photoshop 2023以上的版本加入了内置的授权验证系统，会联网干预非正版软件的运作(症状请见https://github.com/yangnuozhen/ReFreeAdobe/issues/1)。\n" +
                                "我们将会尝试禁止Photoshop本体联网，并通过修改hosts文件的方式来绕过Adobe的联网正版检测。\n\n是否继续?", new HelperHandler("PSBN"));
                            helpWindow.Show();
                        }
                    }
                    catch(Exception e1) {
                        alert(bean.Name + "发生未知错误，请联系开发者反馈:"+e1.ToString(),true);
                    }
                    
                    
                }
            }
            
        }

        public void btnLaunch_Click(object sender, RoutedEventArgs e) {
            Button btn = sender as Button;
            AdobeProductBean bean = btn.DataContext as AdobeProductBean;
            
            if (bean != null) {
                if (!File.Exists(bean.LaunchPath))
                {
                    HelpWindow helpWindow = new HelpWindow("提示", "软件未安装，或者未安装在默认目录，请按照以下提示使用\n\n1、前往Adobe官网下载Creative Cloud并安装\n" +
                "2、通过Creative Cloud下载你想要的应用例如Photoshop（需要登录Adobe ID）\n" +
                "3、在本软件内找到你下载的版本，点击优化即可使用\n" +
                "4、注意不要更改Adobe系列软件的安装目录\n\n" +
                "点击确定按钮去官网下载Creative Cloud", new HelperHandler("help"));
                    helpWindow.Show();
                }
                else {
                    Process.Start(bean.LaunchPath);
                }
                
            }

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnMinus_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MoveMainWindow(object sender, MouseButtonEventArgs e)
        {
            base.DragMove();
        }

        private static void alert(string notice,bool isFatal) {
            if(isFatal)
            {
                MessageBox.Show(notice, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBox.Show(notice, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            //MessageBox.Show(notice,"提醒");
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            
            HelpWindow helpWindow = new HelpWindow("帮助", "使用方式如下:\n1、前往Adobe官网下载Creative Cloud并安装\n" +
                "2、通过Creative Cloud下载你想要的应用例如Photoshop（需要登录Adobe ID）\n" +
                "3、在本软件内找到你下载的版本，点击优化即可使用\n" +
                "4、注意不要更改Adobe系列软件的安装目录\n\n" +
                "点击确定按钮去官网下载Creative Cloud", new HelperHandler("help"));
            helpWindow.Show();
        }

        private void btnSetting_Click(object sender, RoutedEventArgs e)
        {
            SettingWindow settingWindow = new SettingWindow();
            settingWindow.Show();
        }

        class HelperHandler : NotifyEventListener
        {
            string eventType;

            public HelperHandler(string eventType)
            {
                this.eventType = eventType;
            }

            public void onCancelClicked(object msg)
            {
                if (eventType.Equals("verify")||eventType.Equals("enable")||eventType.Equals("update")) {
                    Environment.Exit(0);
                }
            }

            public void onOkClicked(object msg)
            {
                if (eventType.Equals("help"))
                {
                    Process.Start("https://creativecloud.adobe.com/apps/download/creative-cloud?locale=zh-cn");
                }
                else if (eventType.Equals("update")) {
                    Process.Start(startConfigBean.Download);
                }
                else if (eventType.Equals("enable"))
                {
                    Environment.Exit(0);
                }
                else if (eventType.Equals("verify"))
                {
                    string code=(string)msg;
                    if (!code.Equals("从来不想")) {
                        Environment.Exit(0);
                    }

                }
                else if(eventType.Equals("PSBN"))
                {
                    string Result;
                    Result = BlockPSNetwork.BlockPsNetwork();
                    HelpWindow helpWindow = new HelpWindow("Photoshop提示", "已尝试设置。\n\n" + Result , new HelperHandler("PSBNResult"));
                    helpWindow.Show();
                }

            }
        }

        private void btnLogo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://nuozhen.top/ReFreeAdobe/");
        }
        private static StartConfigBean startConfigBean;
        private void startCheck() {
            Task updateTask = new Task(() =>
            {
                startConfigBean = StartCheck.doCheck();
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (startConfigBean != null)
                    {
                        HelperHandler handler = new HelperHandler("null");
                        bool show = false;
                        if (startConfigBean.Enable.Equals("0"))
                        {
                            //弹窗暂停使用
                            handler = new HelperHandler("enable");
                            show = true;
                        }
                        else if (startConfigBean.Verify.Equals("1"))
                        {
                            //弹窗提示用户输入密码
                            handler = new HelperHandler("verify");
                            show = true;
                        }
                        else if (startConfigBean.Version.CompareTo("2.0") > 0)
                        {
                            //弹窗提示更新
                            handler = new HelperHandler("update");
                            show = true;

                        }
                        if (show) {
                            HelpWindow helpWindow = new HelpWindow(startConfigBean.Title, startConfigBean.Notice, handler);
                            helpWindow.Show();
                        }

                    }
                    else
                    {

                    }
                }));

            });
            updateTask.Start();
        }


    }

    
}
