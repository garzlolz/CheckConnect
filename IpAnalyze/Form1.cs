using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ini;
using System.Threading;

namespace IpAnalyze
{
    public partial class Form1 : Form
    {
        private Object thisLock = new Object();
        IniFile ini = new IniFile(Application.StartupPath + "/ip.ini");
        private static List<IP_ADDRESS> All_IP = new List<IP_ADDRESS>();
        private static List<IP_ADDRESS> Reping_IP = new List<IP_ADDRESS>();
        private static string msg = "";
        int TimeOut = 5 * 1000;

        public Form1()
        {
            InitializeComponent();
            //if (inProcess == true) Time_tick(null,null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Iniform();
        }


        public class IP_ADDRESS
        {
            public string NAME { get; set; }
            public string IP { get; set; }
            public string SEQ { get; set; }
            public string RES_CODE { get; set; }
            public string RES_RESULT { get; set; }

        }

        //初始化text box
        public void Iniform()
        {
            string ip_B = InicNullToValue(ini.IniReadValue("ip_Config", "B"), "-");
            string ip_C = InicNullToValue(ini.IniReadValue("ip_Config", "C"), "-");
            string ip_D = InicNullToValue(ini.IniReadValue("ip_Config", "D"), "-");
            string ip_E = InicNullToValue(ini.IniReadValue("ip_Config", "E"), "-");
            string ip_F = InicNullToValue(ini.IniReadValue("ip_Config", "F"), "-");
            string ip_G = InicNullToValue(ini.IniReadValue("ip_Config", "G"), "-");
            string ip_H = InicNullToValue(ini.IniReadValue("ip_Config", "H"), "-");
            tb_TimeOut.Text = InicNullToValue(ini.IniReadValue("ip_Config", "TIMEOUT"), "5");
            TimeOut = Convert.ToInt16(tb_TimeOut.Text) * 1000;
            tb_nameB.Text = ip_B.Split('-')[0];
            tb_nameC.Text = ip_C.Split('-')[0];
            tb_nameD.Text = ip_D.Split('-')[0];
            tb_nameE.Text = ip_E.Split('-')[0];
            tb_nameF.Text = ip_F.Split('-')[0];
            tb_nameG.Text = ip_G.Split('-')[0];
            tb_nameH.Text = ip_H.Split('-')[0];

            tb_ipB.Text = ip_B.Split('-')[1];
            tb_ipC.Text = ip_C.Split('-')[1];
            tb_ipD.Text = ip_D.Split('-')[1];
            tb_ipE.Text = ip_E.Split('-')[1];
            tb_ipF.Text = ip_F.Split('-')[1];
            tb_ipG.Text = ip_G.Split('-')[1];
            tb_ipH.Text = ip_H.Split('-')[1];
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                ini.IniWriteValue("ip_Config", "B", $"{tb_nameB.Text}-{tb_ipB.Text}");
                ini.IniWriteValue("ip_Config", "C", $"{tb_nameC.Text}-{tb_ipC.Text}");
                ini.IniWriteValue("ip_Config", "D", $"{tb_nameD.Text}-{tb_ipD.Text}");
                ini.IniWriteValue("ip_Config", "E", $"{tb_nameE.Text}-{tb_ipE.Text}");
                ini.IniWriteValue("ip_Config", "F", $"{tb_nameF.Text}-{tb_ipF.Text}");
                ini.IniWriteValue("ip_Config", "G", $"{tb_nameG.Text}-{tb_ipG.Text}");
                ini.IniWriteValue("ip_Config", "H", $"{tb_nameH.Text}-{tb_ipH.Text}");
                ini.IniWriteValue("ip_Config", "TIMEOUT", $"{tb_TimeOut.Text}");
                MessageBox.Show("儲存成功!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void IP_initial()
        {
            All_IP = new List<IP_ADDRESS>();
            //IP_ADDRESS _ip = new IP_ADDRESS();
            foreach (Control obj in this.panel1.Controls)
            {
                if (obj is TextBox)
                {
                    IP_ADDRESS _ip = new IP_ADDRESS();
                    if ((obj as TextBox).Name.StartsWith("tb_name"))
                    {
                        _ip.NAME = obj.Text;
                        if (this.panel1.Controls[obj.Name.ToString().Replace("tb_name", "tb_ip")] != null)
                        {
                            _ip.IP = (this.panel1.Controls[obj.Name.ToString().Replace("tb_name", "tb_ip")] as TextBox).Text;
                        }
                        if (this.panel2.Controls[obj.Name.ToString().Replace("tb_name", "lb_console")] != null)
                        {
                            _ip.SEQ = (this.panel2.Controls[obj.Name.ToString().Replace("tb_name", "lb_console")] as Label).Name.Replace("lb_console", "");
                        }
                        All_IP.Add(_ip);
                    }
                }
            }
        }

        private void btn_run_Click(object sender, EventArgs e)
        {
            LogClass.EventLog.Write("----------------  START PING -----------------");
            loading();
            IP_initial();
            GoThread();
            LogClass.EventLog.Write("----------------     END     -----------------\n");
        }


        //// 執行多筆執行序
        public void GoThread()
        {
            Thread t1 = new Thread(() => GoPing(All_IP[0]));
            Thread t2 = new Thread(() => GoPing(All_IP[1]));
            Thread t3 = new Thread(() => GoPing(All_IP[2]));
            Thread t4 = new Thread(() => GoPing(All_IP[3]));
            Thread t5 = new Thread(() => GoPing(All_IP[4]));
            Thread t6 = new Thread(() => GoPing(All_IP[5]));
            Thread t7 = new Thread(() => GoPing(All_IP[6]));
            Thread t8 = new Thread(() => GoPing(All_IP[7]));

            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();

            t1.Join();
            t2.Join();
            t3.Join();
            t4.Join();
            t5.Join();
            t6.Join();
            t7.Join();
            t8.Join();
            ChangeStatus(All_IP);
        }

        //Change lb_console [status]
        void ChangeStatus(List<IP_ADDRESS> NewAddress)
        {
            foreach (IP_ADDRESS item in NewAddress)
            {
                if (this.panel2.Controls["lb_console" + item.SEQ] != null)
                {
                    if (item.RES_CODE != "00")
                    {
                        this.panel2.Controls["lb_console" + item.SEQ].BackColor = Color.Red;
                        this.panel2.Controls["lb_console" + item.SEQ].ForeColor = Color.White;

                    }
                    else
                    {
                        this.panel2.Controls["lb_console" + item.SEQ].BackColor = Color.LightGreen;
                        this.panel2.Controls["lb_console" + item.SEQ].Font = new Font("Arial", 16, FontStyle.Bold);
                        this.panel2.Controls["lb_console" + item.SEQ].ForeColor = Color.Black;
                    }
                    (this.panel2.Controls["lb_console" + item.SEQ] as Label).Text = item.RES_RESULT;

                }
            }
            //MessageBox.Show(msg);

            msg = "";
            
        }

        // Ping A,B,C,D,E,F,G,H
        public void GoPing(IP_ADDRESS ip,bool retry = false)
        {
            string res_result = "";
            string res_code = "";
            if (string.IsNullOrEmpty(ip.IP))
            {
                msg += $"{ip.NAME} IP: {ip.IP} 有誤!";
                res_code = "01";
                res_result = "IP 有誤";
                return;
            }
            else
            {
                try
                {
                    Ping pingSender = new Ping();
                    PingReply reply = pingSender.Send(ip.IP, TimeOut);
                    if (reply.Status == IPStatus.Success)
                    {
                        msg += $"{ip.NAME} IP({ip.IP}) : {reply.Status}\n";
                        res_code = "00";
                        res_result = "連線中";
                    }
                    else if (reply.Status == IPStatus.TimedOut)
                    {
                        msg += $"{ip.NAME} IP({ip.IP}) : {reply.Status}\n";
                        res_code = "03";
                        //res_result = reply.Status.ToString();
                        res_result = "要求等候逾時";
                    }
                    else if (reply.Status == IPStatus.DestinationHostUnreachable)
                    {
                        msg += $"{ip.NAME} IP({ip.IP}) : {reply.Status}\n";
                        res_code = "04";
                        //res_result = reply.Status.ToString();
                        res_result = "無法與目標主機取得連線";
                    }
                    else
                    {
                        msg += $"{ip.NAME} IP({ip.IP}) : {reply.Status}\n";
                        res_code = "02";
                        //res_result = reply.Status.ToString();
                        res_result = $"{reply.Status}";
                    }
                }
                catch (Exception ex)
                {
                    res_code = "99";
                    res_result = ex.Message;
                    msg += $"{ip.NAME} IP({ip.IP}) Error:" + ex.Message + "\n";
                }
            }
            IP_ADDRESS res = new IP_ADDRESS();
            if(retry)
                res = Reping_IP.FirstOrDefault(x => x.SEQ == ip.SEQ);
            else
                res = All_IP.FirstOrDefault(x => x.SEQ == ip.SEQ);
            if (res != null)
            {
                res.RES_CODE = res_code;
                res.RES_RESULT = res_result;
            }
            lock (thisLock)
            {
                LogClass.EventLog.Write(PadRightEx(ip.NAME, 20, ' ') + " IP(" + ip.IP + ") : " + res_result);
            }
        }


        //Change status to "Loading"
        void loading()
        {
            foreach (Control console in this.panel2.Controls)
            {
                console.Font = new Font("Arial", 16, FontStyle.Bold);
                console.BackColor = Color.Aqua;
                console.ForeColor = Color.Black;
                console.Text = "Loading...";
            }
            TimeOut = Convert.ToInt16(tb_TimeOut.Text) * 1000;
            Application.DoEvents();
        }
        public string InicNullToValue(string VALUE, string REVALUE)
        {
            if (VALUE == "")
                return REVALUE;
            else
                return VALUE;
        }
        public string PadRightEx(string str, int totalByteCount, char c)
        {
            Encoding coding = Encoding.GetEncoding("BIG5");
            int dcount = 0;
            foreach (char ch in str.ToCharArray())
            {
                if (coding.GetByteCount(ch.ToString()) == 2)
                    dcount++;
            }
            string w = str.PadRight(totalByteCount - dcount, c);
            return w;
        }

        private void cb_all_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Control control in panel_check.Controls)
            {
                if (cb_all.Checked == true)
                {
                    (control as CheckBox).Checked = true;
                }
                else if (cb_all.Checked == false)
                {
                    (control as CheckBox).Checked = false;
                }
            }
        }
        bool inProcess = false;
        private void btn_keepPing_Click(object sender, EventArgs e)
        {
            
            AddReping();
            timer_switch.Enabled = !timer_switch.Enabled;
        }





        //RePing 
        bool AddReping()
        {
            
            CB_enable(false);
            Reping_IP.Clear();  
            All_IP.Clear();
            string start = "重複連線";
            string stop = "終止連線";
            if (btn_keepPing.Text == start)
            {        
                LogClass.EventLog.Write("----------------  RESTART PING -----------------");
               
                
                btn_keepPing.Text = stop;
                btn_keepPing.BackColor = Color.Red;
                foreach (Control cb in panel_check.Controls)
                {
                    IP_ADDRESS ip = new IP_ADDRESS();
                    if (cb is CheckBox && (cb as CheckBox).Checked == true)
                    {
                        string tb_name = cb.Name.Replace("cb_", "tb_name");
                        string tb_seq = cb.Name.Replace("cb_", "");
                        string tb_ip = cb.Name.Replace("cb_", "tb_ip");
                        foreach (Control c in panel1.Controls)
                        {

                            if (c.Name == tb_name)
                            {
                                if (string.IsNullOrEmpty(cb.Text))
                                {
                                    ip.IP = panel1.Controls[tb_ip].Text;
                                    ip.NAME = panel1.Controls[tb_name].Text;
                                    ip.SEQ = tb_seq;

                                    Reping_IP.Add(ip);
                                }
                            }
                        }
                    }
                }
                iniStatus(Reping_IP);
                return true;
            }
            else
            {
               
                CB_enable(true);
                iniStatus(null);
                btn_keepPing.Text = start;
                btn_keepPing.BackColor = Color.White;
                LogClass.EventLog.Write("----------------     END     -----------------\n");
            
                return false;
            }


        }


        void GoRePing()
        {
            
            List<Thread> threads = new List<Thread>();
            foreach (IP_ADDRESS item in Reping_IP)
            {
                Thread t = new Thread(() => GoPing(item,true));
                threads.Add(t);
            }
            foreach (var thread in threads)
            {
                thread.Start();//start thread and pass it the port
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
           
        }

        private void Time_tick(object sender, EventArgs e)
        {      
            
            if(inProcess == false)
            {
                
                inProcess = true;
                iniStatus(Reping_IP);
                GoRePing();               
                Application.DoEvents();          
                ChangeStatus(Reping_IP);
                Application.DoEvents();
                Thread.Sleep(10000);
                inProcess = false;
               
            }
                      
        }

        void iniStatus(List<IP_ADDRESS> ips)
        {
            foreach(Control lb in panel2.Controls)
            {
                lb.Text = "尚未測試連線";
                lb.Font = new Font("Arial", 16);
                lb.BackColor = Color.PowderBlue;
                lb.ForeColor = Color.Black;
            }

            try
            {
                if (ips!=null)
                {
                    foreach (IP_ADDRESS ip in ips)
                    {
                        panel2.Controls["lb_console" + ip.SEQ].Text = "Loading";
                        panel2.Controls["lb_console" + ip.SEQ].Font = new Font("Arial", 16, FontStyle.Bold);
                        panel2.Controls["lb_console" + ip.SEQ].BackColor = Color.Aqua;
                        panel2.Controls["lb_console" + ip.SEQ].ForeColor = Color.Black;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return;
            }
  
        }


        void CB_enable(bool status)
        {
            foreach(Control cb in panel_check.Controls)
            {
                cb.Enabled = status;
            }
        }
    }
}

