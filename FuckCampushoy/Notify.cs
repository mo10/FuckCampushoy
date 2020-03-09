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
            notifyIcon1.Visible = false;
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
                    var response = await cli.Request("https://ustl.cpdaily.com/wec-counselor-collector-apps/stu/collector/queryCollectorProcessingList")
                        .WithTimeout(10)
                        .PostJsonAsync(new { pageSize = 6, pageNumber = 1 })
                        .ReceiveJson<CollectorList>();
                    
                    if(response.code != "0")
                    {
                        notifyIcon1.BalloonTipTitle = "更新失败";
                        notifyIcon1.BalloonTipText = $"{response.message}";
                        notifyIcon1.ShowBalloonTip(60);

                        message.Invoke(new Action(() =>
                        {
                            message.Text = $"更新失败{response.code} {response.message}";
                        }));
                    }
                    else
                    {
                        if(response.datas.totalSize > 0)
                        {
                            foreach (var f in response.datas.rows)
                            {
                                if (f.isHandled == 0)
                                {
                                    // 请求问卷内容
                                    var ret = await cli.Request("https://ustl.cpdaily.com/wec-counselor-collector-apps/stu/collector/getFormFields")
                                        .WithTimeout(10)
                                        .PostJsonAsync(new
                                        {
                                            pageSize = 10,
                                            pageNumber = 1,
                                            formWid = f.formWid,
                                            collectorWid = f.wid,
                                        })
                                        .ReceiveJson<FormMessage>();
                                    if (ret.code == "0")
                                    {
                                        foreach (var row in ret.datas.rows)
                                        {
                                            if (row.fieldType != 1)
                                            {
                                                notifyIcon1.BalloonTipTitle = "填写失败";
                                                notifyIcon1.BalloonTipText = $"存在不支持自动回答的问题，请手动提交";
                                                notifyIcon1.ShowBalloonTip(60);
                                                return;
                                            }
                                            if (row.title.IndexOf("是否") == -1)
                                            {
                                                notifyIcon1.BalloonTipTitle = "填写失败";
                                                notifyIcon1.BalloonTipText = $"模棱两可的问题";
                                                notifyIcon1.ShowBalloonTip(60);
                                                return;
                                            }
                                            row.value = "否";

                                        }
                                        // 提交问卷
                                        var ret2 = await cli.Request("https://ustl.cpdaily.com/wec-counselor-collector-apps/stu/collector/submitForm")
                                            .WithTimeout(10)
                                            .PostJsonAsync(new
                                            {
                                                schoolTaskWid = 0,
                                                formWid = f.formWid,
                                                collectWid = f.wid,
                                                form = ret.datas.rows,
                                            })
                                            .ReceiveJson<SendReq>();
                                        if (ret2.code != "0")
                                        {
                                            notifyIcon1.BalloonTipTitle = "提交失败";
                                            notifyIcon1.BalloonTipText = $"{ret2.message}";
                                            notifyIcon1.ShowBalloonTip(60);
                                        }

                                        notifyIcon1.BalloonTipTitle = "成功提交";
                                        notifyIcon1.BalloonTipText = $"该死的问卷已经自动帮你回完了";
                                        notifyIcon1.ShowBalloonTip(60);
                                    }
                                }
                            }

                        }
                        // 无新数据
                        message.Invoke(new Action(() =>
                        {
                            message.Text = $"上次更新:{DateTime.Now.ToString("HH:mm")}";
                        }));
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
                    notifyIcon1.ShowBalloonTip(60);
                }
            }
        }

        public void Notify_Login(IDictionary<string, Cookie> cookies)
        {
            this.cookies = cookies;
            this.notifyIcon1.Visible = true;
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
                Task.Run(() => { OnTimedEvent(this, null); });
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
            timer.Stop();
            parent?.Close();
            System.Environment.Exit(0);
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }
    }
}
