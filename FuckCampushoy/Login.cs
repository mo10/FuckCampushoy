using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using Flurl;
using Flurl.Http;

namespace FuckCampushoy
{

    public partial class Login : Form
    {
        static readonly HttpClient client = new HttpClient();
        Notify notifyForm;
        IDictionary<string, Cookie> cookies;

        public Login()
        {
            InitializeComponent();
            cookies = null;
            notifyForm = new Notify(this);
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Task.Run(() => get()).ContinueWith((t) =>
            {
                if (this.cookies != null)
                {
                    this.Invoke(new Action(() =>
                    {
                        notifyForm.Notify_Login(this.cookies);
                        notifyForm.Show();
                        this.Hide();
                    }));
                }

            });
        }

        private async Task get()
        {
            try
            {
                button1.Invoke(new Action(() =>
                {
                    button1.Visible = false;
                    button1.Enabled = false;
                }));
                notice.Invoke(new Action(() =>
                {
                    notice.Text = "二维码加载中";
                }));
                qrcode.Invoke(new Action(() =>
                {
                    qrcode.Image = null;
                }));
                using (var cli = new FlurlClient().EnableCookies())
                {
                    // 请求登陆页面，获取Cookie与ClientId
                    var response = await cli.Request("https://ustl.cpdaily.com/wec-counselor-stu-apps/stu/mobile/index.html#/forAppNotice").GetAsync();
                    var _cookie = cli.Cookies;
                    var query = HttpUtility.ParseQueryString(response.RequestMessage.RequestUri.Query);
                    // 二维码id请求参数
                    var jsLogin = new
                    {
                        clientId = query.GetValues("client_id")[0],
                        redirectUri = query.GetValues("redirect_uri")[0],
                        scope = query.GetValues("scope")[0],
                        responseType = query.GetValues("response_type")[0],
                        state = query.GetValues("state")[0],
                        supportFreeLogin = ""
                    };
                    // 请求二维码id
                    QRLogin ret = await cli.Request("https://www.cpdaily.com/connect/qrcode/jsLogin").PostJsonAsync(jsLogin).ReceiveJson<QRLogin>();
                    // 出错
                    if (ret.errCode != 0)
                    {
                        MessageBox.Show(ret.errMsg);
                        return;
                    }
                    // 加载二维码
                    Stream stream = await cli.Request($"https://www.cpdaily.com/connect/qrcode/image/{ret.data.qrId}").GetStreamAsync();
                    qrcode.Invoke(new Action(() =>
                    {
                        qrcode.Image = Bitmap.FromStream(stream);
                    }));
                    notice.Invoke(new Action(() =>
                    {
                        notice.Text = "请使用今日校园APP扫码登陆";
                    }));

                    while (true)
                    {
                        QRLoginValidation retv = await cli.Request($"https://www.cpdaily.com/connect/qrcode/validation/{ret.data.qrId}").PostJsonAsync(jsLogin).ReceiveJson<QRLoginValidation>();

                        switch (retv.data.status)
                        {
                            case 1:
                                continue;
                            case 2:
                            case 3:
                                notice.Invoke(new Action(() =>
                                {
                                    notice.Text = "扫码成功，请确认登陆";
                                }));
                                break;
                            case 4:
                                notice.Invoke(new Action(() =>
                                {
                                    notice.Text = "登陆成功";
                                }));
                                // 请求转跳链接 获得"MOD_AUTH_CAS"
                                await cli.Request(retv.data.redirectUrl).GetAsync();

                                this.cookies = cli.Cookies;
                                saveCookie(this.cookies);
                                return;
                            case 5:
                                qrcode.Invoke(new Action(() =>
                                {
                                    qrcode.Image = null;
                                }));
                                notice.Invoke(new Action(() =>
                                {
                                    notice.Text = "二维码失效 请点击刷新";
                                }));
                                button1.Invoke(new Action(() =>
                                {
                                    button1.Visible = true;
                                    button1.Enabled = true;
                                }));
                                return;
                        }
                    }
                }
            }catch(Exception)
            {
                notice.Invoke(new Action(() =>
                {
                    notice.Text = "二维码加载失败请 检查网络连接";
                }));
                button1.Invoke(new Action(() =>
                {
                    button1.Visible = true;
                    button1.Enabled = true;
                }));
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Task.Run(() => get());
        }

        private void saveCookie(IDictionary<string,Cookie> cookies)
        {
            var savefile = AppDomain.CurrentDomain.BaseDirectory + "cookie.bin";
            var fd = File.OpenWrite(savefile);
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(fd, cookies);
            fd.Close();

        }
    }
}
