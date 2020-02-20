using Flurl.Http;
using FuckCampushoy.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace FuckCampushoy
{
    public partial class Notify : Form
    {
        bool isStart = false;
        IDictionary<string, Cookie> cookies;
        System.Timers.Timer timer;
        Form parent;
        public Notify(Form form)
        {
            InitializeComponent();
            notifyIcon1.Icon = Resources.Icon1;
            timer = new System.Timers.Timer(1000 * 60);
            timer.AutoReset = true;
            parent = form;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        private async void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            using (var cli = new FlurlClient().EnableCookies())
            {
                try
                {
                    // 请求通知列表
                    foreach(var a in cookies)
                    {
                        cli.Cookies.Add(a.Key,a.Value);
                    }
                    var response = await cli.Request("https://ustl.cpdaily.com/wec-counselor-stu-apps/stu/notice/queryProcessingNoticeList")
                        //.WithHeaders(new { Referer = "https://ustl.cpdaily.com/wec-counselor-stu-apps/stu/mobile/index.html",
                        //    Origin = "https://ustl.cpdaily.com",
                        //    Sec_Fetch_Mode = "cors",
                        //    Sec_Fetch_Site = "same-origin",
                        //    User_Agent= "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.130 Safari/537.36",
                        //})
                        .WithTimeout(10)
                        .PostJsonAsync(new { pageSize = 6, pageNumber = 1 })
                        .ReceiveJson<CollectorList>();
                    
                    if(response.code != "0")
                    {
                        notifyIcon1.BalloonTipTitle = "更新失败";
                        notifyIcon1.BalloonTipText = $"返回:{response.code} {response.message}";
                        notifyIcon1.ShowBalloonTip(10);

                        message.Invoke(new Action(() =>
                        {
                            message.Text = $"更新失败{response.code} {response.message}";
                        }));
                    }
                    else
                    {
                        if(response.datas.totalSize == 0)
                        {
                            // 无新数据
                            message.Invoke(new Action(() =>
                            {
                                message.Text = $"上次更新:{DateTime.Now.ToString("HH:mm")}";
                            }));
                        }
                        else
                        {

                        }
                    }
                }
                catch(FlurlHttpTimeoutException)
                {
                    // 请求超时
                }
                catch(Exception ex)
                {
                    notifyIcon1.BalloonTipTitle = "程序异常";
                    notifyIcon1.BalloonTipText = $"{ex.Message}";
                    notifyIcon1.ShowBalloonTip(5);
                }
            }
        }

        public void Notify_Login(IDictionary<string, Cookie> cookies)
        {
            this.cookies = cookies;
        }

        private void Notify_Load(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                timer.Start();
                button1.Text = "停止轮询";
                isStart = true;
            }
            else
            {
                timer.Stop();
                button1.Text = "开始轮询";
                isStart = false;
            }
        }

        private void Notify_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            parent.Close();
            Application.Exit();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
