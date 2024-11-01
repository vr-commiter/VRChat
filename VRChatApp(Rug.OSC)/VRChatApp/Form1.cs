using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using System.Windows.Forms;
using Rug.Osc;
using TrueGearSDK;
using System.Net;
using System.Net.NetworkInformation;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace VRChatApp
{
    public partial class Form1 : Form
    {
        private static TrueGearPlayer _player = null;

        private static string[] eletrics = { "TrueGearA1", "TrueGearA2", "TrueGearA3", "TrueGearA4", "TrueGearA5", "TrueGearB1", "TrueGearB2", "TrueGearB3", "TrueGearB4", "TrueGearB5", "TrueGearC1", "TrueGearC2", "TrueGearC3", "TrueGearC4", "TrueGearC5", "TrueGearD1", "TrueGearD2", "TrueGearD3", "TrueGearD4", "TrueGearD5", "TrueGearE1", "TrueGearE2", "TrueGearE3", "TrueGearE4", "TrueGearE5", "TrueGearF1", "TrueGearF2", "TrueGearF3", "TrueGearF4", "TrueGearF5", "TrueGearG1", "TrueGearG2", "TrueGearG3", "TrueGearG4", "TrueGearG5", "TrueGearH1", "TrueGearH2", "TrueGearH3", "TrueGearH4", "TrueGearH5", "TrueGearArmL", "TrueGearArmR" };
        //private static string[] eletrics = { "A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "B4", "B5", "C1", "C2", "C3", "C4", "C5", "D1", "D2", "D3", "D4", "D5", "E1", "E2", "E3", "E4", "E5", "F1", "F2", "F3", "F4", "F5", "G1", "G2", "G3", "G4", "G5", "H1", "H2", "H3", "H4", "H5", "ArmL", "ArmR" };
        private static int[] numbers = { 1, 5, 9, 13, 17, 0, 4, 8, 12, 16, 100, 104, 108, 112, 116, 101, 105, 109, 113, 117, 102, 106, 110, 114, 118, 103, 107, 111, 115, 119, 3, 7, 11, 15, 19, 2, 6, 10, 14, 18, 0, 100 };
        private static float[] percentage = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static List<int> shockNumbers = null;
        private static List<int> eletricsNumbers = null;

        private static EffectObject effectObject = null;
        private static TrackObject shockObject = null;
        private static TrackObject eletricObject = null;

        private static string _SteamExe;
        private const string STEAM_OPENURL = "steam://rungameid/438100";

        private int receiveSocket = Properties.Settings.Default.ReceiverPort;
        private int sendSocket = Properties.Settings.Default.SenderPort;

        private int shakePower = Properties.Settings.Default.ShakeIntensity;
        private int electricalPower = Properties.Settings.Default.ElectricalIntensity;
        private int electricalCount = Properties.Settings.Default.ElectricalCount;
        private bool feedbackOnce = Properties.Settings.Default.FeedbackOnce;

        private OscSender sender = null;
        private OscReceiver receiver = null;

        private Thread sendMsg = null;
        private Thread listenMsg = null;

        private bool canListen = false;

        private static string logFilePath;
        private static StreamWriter logStreamWriter;
        private static string relogFilePath;
        private static string tglogFilePath;
        //private static StreamWriter relogStreamWriter;
        //private static StreamWriter tglogStreamWriter;

        public Form1()
        {
            //当有两个程序运行的时候，关闭前一个程序，保留当前程序
            string currentProcessName = Process.GetCurrentProcess().ProcessName;
            Process[] processes = Process.GetProcessesByName(currentProcessName);
            if (processes.Length > 1)
            {
                if (processes[0].UserProcessorTime.TotalMilliseconds > processes[1].UserProcessorTime.TotalMilliseconds)
                {
                    processes[0].Kill();
                }
                else
                {
                    processes[1].Kill();
                }
            }

            InitializeComponent();
            checkBox1.Checked = Properties.Settings.Default.CheckBoxSelect;
            checkBox2.Checked = Properties.Settings.Default.FeedbackOnce;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            receive.Text = receiveSocket.ToString();
            send.Text = sendSocket.ToString();
            shakePowerText.Text = shakePower.ToString();
            electricalPowerText.Text = electricalPower.ToString();
            electricalCountText.Text = electricalCount.ToString();
            close.Enabled = false;

            Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void start_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Console.WriteLine(checkBox1.Checked);
        }

        private void receive_TextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void receive_TextChanged(object sender, EventArgs e)
        {
            if (receive.Text.Length > 0)
            {
                if (int.TryParse(receive.Text, out int value))
                {
                    if (value < 0 || value > 65535)
                    {
                        receive.Text = "9001";
                        MessageBox.Show("请输入0-65535之间的整数");
                    }
                }
            }
            if (receive.Text != "")
            {
                receiveSocket = int.Parse(receive.Text);
            }
            else
            {
                receiveSocket = 9001;
            }
            Console.WriteLine($"receiveSocket ：{receiveSocket}");
        }

        private void send_TextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void send_TextChanged(object sender, EventArgs e)
        {
            if (send.Text.Length > 0)
            {
                if (int.TryParse(send.Text, out int value))
                {
                    if (value < 0 || value > 65535)
                    {
                        send.Text = "9002";
                        MessageBox.Show("请输入0-65535之间的整数");
                    }
                }
            }
            if (send.Text != "")
            {
                sendSocket = int.Parse(send.Text);
            }
            else
            {
                sendSocket = 9002;
            }
            Console.WriteLine($"sendSocket ：{sendSocket}");
        }

        private void shakePowerText_TextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void shakePowerText_TextChanged(object sender, EventArgs e)
        {
            if (shakePowerText.Text.Length > 0)
            {
                if (int.TryParse(shakePowerText.Text, out int value))
                {
                    if (value < 1 || value > 100)
                    {
                        shakePowerText.Text = "50";
                        MessageBox.Show("请输入1-100之间的整数");
                    }
                }
            }
            if (shakePowerText.Text != "")
            {
                shakePower = int.Parse(shakePowerText.Text);
            }
            else
            {
                shakePower = 50;
            }
            Console.WriteLine($"shakePower ：{shakePower}");
        }

        private void electricalPowerText_TextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void electricalPowerText_TextChanged(object sender, EventArgs e)
        {
            if (electricalPowerText.Text != "")
            {
                electricalPower = int.Parse(electricalPowerText.Text);
            }
            else
            {
                electricalPower = 30;
            }
            Console.WriteLine($"electricalPower ：{electricalPower}");
        }

        private void electricalCountText_TextKeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void electricalCountText_TextChanged(object sender, EventArgs e)
        {
            if (electricalCountText.Text.Length > 0)
            {
                if (int.TryParse(electricalCountText.Text, out int value))
                {
                    if (value < 0 || value > 15)
                    {
                        electricalCountText.Text = "10";
                        MessageBox.Show("请输入0-15的整数");
                    }
                }
            }
            if (electricalCountText.Text != "")
            {
                electricalCount = int.Parse(electricalCountText.Text);
            }
            else
            {
                electricalCount = 10;
            }
            Console.WriteLine($"electricalCount ：{electricalCount}");
        }

        public static string SteamExePath()
        {
            return (string)Registry.GetValue(@"HKEY_CURRENT_USER\SOFTWARE\Valve\Steam", "SteamExe", null);
        }

        static bool IsPortInUse(int port)
        {
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = properties.GetActiveTcpListeners();
            IPEndPoint[] udpEndPoints = properties.GetActiveUdpListeners();

            return tcpEndPoints.Any(endpoint => endpoint.Port == port) ||
                   udpEndPoints.Any(endpoint => endpoint.Port == port);
        }

        private void Start()
        {
            if (IsPortInUse(receiveSocket))
            {
                MessageBox.Show($"端口 {receiveSocket} 已被占用,请切换端口。");
                return;
            }
            if (receiveSocket == sendSocket && checkBox1.Checked)
            {
                Close();
                receiveSocket = 9001;
                sendSocket = 9002;
                receive.Text = receiveSocket.ToString();
                send.Text = sendSocket.ToString();
                MessageBox.Show("接收端口和转发端口不可一致");
                return;
            }
            //InitLog(Path.Combine(Application.StartupPath, "log"));
            receiver = new OscReceiver(receiveSocket);
            try
            {
                // 尝试连接
                receiver.Connect();
                Console.WriteLine("Connection successful.");
                start.Enabled = false;
                checkBox1.Enabled = false;
                receive.Enabled = false;
                send.Enabled = false;
                close.Enabled = true;

                receive.Text = receiveSocket.ToString();
                send.Text = sendSocket.ToString();
                shakePowerText.Text = shakePower.ToString();
                electricalPowerText.Text = electricalPower.ToString();
                electricalCountText.Text = electricalCount.ToString();

                shockNumbers = new List<int>();
                eletricsNumbers = new List<int>();

                effectObject = new EffectObject();
                shockObject = new TrackObject();
                eletricObject = new TrackObject();

                _player = new TrueGearPlayer("438100", "VRChat");

                effectObject.uuid = "VRChatMsg";
                effectObject.name = "VRChatMsg";
                effectObject.keep = false;

                shockObject.start_time = 0;
                shockObject.end_time = 150;
                shockObject.stopName = "";
                shockObject.intensity_mode = IntensityMode.Const;
                shockObject.action_type = ActionType.Shake;
                shockObject.once = false;
                shockObject.interval = 0;

                eletricObject.start_time = 0;
                eletricObject.end_time = 150;
                eletricObject.stopName = "";
                eletricObject.intensity_mode = IntensityMode.Const;
                eletricObject.action_type = ActionType.Electrical;
                eletricObject.once = false;



                if (checkBox1.Checked)
                {
                    sender = new OscSender(IPAddress.Parse("127.0.0.1"), sendSocket);
                    sendTip.Text = $"转发OSC端口 ：{sendSocket} ...";
                }
            }
            catch (Exception ex)
            {
                Close();
                receiveTip.Text = "链接端口失败: " + ex.Message;
                Console.WriteLine("Failed to connect: " + ex.Message);
            }

            // 如果连接成功，开始接收消息
            if (receiver.State == OscSocketState.Connected)
            {
                // 这里可以添加接收和处理 OSC 数据的代码
                Thread.Sleep(500);
                _SteamExe = SteamExePath();

                //当有两个程序运行的时候，关闭前一个程序，保留当前程序
                string VRCProcessName = "VRChat";
                Process[] processes = Process.GetProcessesByName(VRCProcessName);
                if (processes.Length <= 0)
                {
                    if (_SteamExe != null) Process.Start(_SteamExe, STEAM_OPENURL);
                }


                receiveTip.Text = $"监听OSC端口 ：{receiveSocket} ...";

                _player.Start();

                canListen = true;

                sendMsg = new Thread(SendMsg);
                listenMsg = new Thread(new ThreadStart(ListenMsg));
                sendMsg.Start();
                listenMsg.Start();
            }
            Console.WriteLine(receiver.State);


        }

        private static void InitLog(string logFolderPath)
        {
            if (!Directory.Exists(logFolderPath))
            {
                Directory.CreateDirectory(logFolderPath);
            }
            DateTime currentTime = DateTime.Now;
            string disTime = currentTime.Year.ToString() + currentTime.Month.ToString() + currentTime.Day.ToString() + currentTime.Hour.ToString() + currentTime.Minute.ToString() + currentTime.Second.ToString();
            Console.WriteLine(disTime);
            relogFilePath = Path.Combine(logFolderPath, disTime + "OscMsg.txt");
            tglogFilePath = Path.Combine(logFolderPath, disTime + "TrueGear.txt");
            //relogStreamWriter = new StreamWriter(relogFilePath, true);
            //tglogStreamWriter = new StreamWriter(tglogFilePath, true);
        }

        private void ListenMsg()
        {
            // 持续监听OSC消息
            while (receiver.State == OscSocketState.Connected)
            {
                try
                {
                    OscPacket packet = receiver.Receive();

                    //DateTime currentTime = DateTime.Now;
                    //string formattedTime = currentTime.ToString("yyyy-MM-dd HH:mm:ss.fff");

                    //relogStreamWriter.WriteLine(formattedTime + "  " + packet.ToString());

                    MessageDispose(packet);

                    if (sender != null)
                    {
                        sender.Connect();
                        sender.Send(packet);
                        sender.Close();
                    }
                }
                catch (Exception ex)
                {

                }

            }
        }

        private void MessageDispose(OscPacket packet)
        {
            if (packet is OscMessage message)
            {
                //Console.WriteLine("------------------------------------");
                //Console.WriteLine(message.ToString());
                //Console.WriteLine(message);
                int index = Array.FindIndex(eletrics, element => message.ToString().Contains(element));
                //Console.WriteLine(message.Address);
                //Console.WriteLine(index);

                if (index != -1)
                {
                    // 匹配逗号后面的数字部分
                    string pattern = @"(?<=,\s*)\d+(\.\d+)?";

                    Match match = Regex.Match(message.ToString(), pattern);

                    //Console.WriteLine("---------------------------------------");

                    if (match.Success)
                    {
                        if (float.TryParse(match.Value, out float result))
                        {
                            percentage[index] = result;
                            if (index == 40 || index == 41)
                            {
                                if (!eletricsNumbers.Contains(numbers[index]))
                                {
                                    eletricsNumbers.Add(numbers[index]);
                                }                                
                            }
                            else
                            {
                                if (!shockNumbers.Contains(numbers[index]))
                                {
                                    shockNumbers.Add(numbers[index]);
                                }
                            }
                        }
                    }
                }
            }
        }


        private bool canMasPower = false;
        private int[] lastSN = new int[0];
        private int[] lastEN = new int[0];
        private int sameCount = 0;


        private bool CheckEffect(EffectObject effectObject)
        {
            Console.WriteLine("----------------------------");
            if (effectObject.trackList.Length < 2)
            {
                if (shockNumbers.Count != 0)
                {
                    return lastSN.SequenceEqual(effectObject.trackList[0].index);
                }
                return lastEN.SequenceEqual(effectObject.trackList[0].index);
            }
            else
            {
                return lastSN.SequenceEqual(effectObject.trackList[0].index) && lastEN.SequenceEqual(effectObject.trackList[1].index);
            }
        }

        private void SendMsg()
        {
            while (true)
            {
                try
                {
                    float maxShakePower = GetMaxShakePower();
                    float maxElePower = GetMaxElePower();

                    shockObject.start_intensity = (int)((int)shakePower * maxShakePower);
                    shockObject.end_intensity = (int)((int)shakePower * maxShakePower);

                    eletricObject.start_intensity = (int)((int)electricalPower * maxElePower);
                    eletricObject.end_intensity = (int)((int)electricalPower * maxElePower);

                    eletricObject.interval = electricalCount;
                    int count = 0;
                    if (shockNumbers.Count != 0)
                    {
                        int index = 0;
                        shockObject.index = new int[shockNumbers.Count];
                        foreach (int shockNumber in shockNumbers)
                        {
                            shockObject.index[index] = shockNumber;
                            index++;
                        }
                        count++;
                    }
                    if (eletricsNumbers.Count != 0)
                    {
                        int index = 0;
                        eletricObject.index = new int[eletricsNumbers.Count];
                        foreach (int eletricsNumber in eletricsNumbers)
                        {
                            eletricObject.index[index] = eletricsNumber;
                            index++;
                        }
                        count++;
                    }
                    if (count != 0)
                    {
                        effectObject.trackList = new TrackObject[count];
                        if (shockNumbers.Count != 0)
                        {
                            effectObject.trackList[0] = shockObject;
                            if (eletricsNumbers.Count != 0)
                            {
                                effectObject.trackList[1] = eletricObject;
                            }
                        }
                        else
                        {
                            effectObject.trackList[0] = eletricObject;
                        }
                        //Console.WriteLine("---------------------------------------");
                        //Console.WriteLine($"shockObject :{shockObject.start_intensity}");
                        //Console.WriteLine($"eletricObject :{eletricObject.start_intensity}");
                        if (shockObject.start_intensity == 0 && eletricObject.start_intensity == 0)
                        {
                            if (canMasPower)
                            {
                                shockObject.start_intensity = shakePower;
                                eletricObject.start_intensity = electricalPower;
                            }
                            canMasPower = true;
                        }
                        else
                        {
                            canMasPower = false;
                        }
                        if (CheckEffect(effectObject))
                        {
                            sameCount++;
                        }
                        else
                        {
                            sameCount = 0;
                        }
                        if (effectObject.trackList.Length < 2)
                        {
                            if (shockNumbers.Count != 0)
                            {
                                lastSN = new int[effectObject.trackList[0].index.Length];
                                Array.Copy(effectObject.trackList[0].index, lastSN, effectObject.trackList[0].index.Length);
                            }
                            else
                            {
                                lastEN = new int[effectObject.trackList[0].index.Length];
                                Array.Copy(effectObject.trackList[0].index, lastEN, effectObject.trackList[0].index.Length);                                
                            }
                        }
                        else
                        {
                            lastSN = new int[effectObject.trackList[0].index.Length];
                            lastEN = new int[effectObject.trackList[1].index.Length];
                            Array.Copy(effectObject.trackList[0].index, lastSN, effectObject.trackList[0].index.Length);
                            Array.Copy(effectObject.trackList[1].index, lastEN, effectObject.trackList[1].index.Length);
                        }
                        if (!feedbackOnce || sameCount < 1)
                        {
                            _player.SendPlayEffectByContent(effectObject);
                        }
                        shockNumbers = new List<int>();
                        eletricsNumbers = new List<int>();
                        shockObject.index = new int[0];
                        eletricObject.index = new int[0];
                        percentage = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }
                    else
                    {
                        shockNumbers = new List<int>();
                        eletricsNumbers = new List<int>();
                        shockObject.index = new int[0];
                        eletricObject.index = new int[0];
                        percentage = new float[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                    }
                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception :{ex}");
                }
                
            }
        }


        private float GetMaxShakePower()
        { 
            float maxShakePower = 0f;
            for (int i = 0; i <= 39; i++)
            {
                if (maxShakePower < percentage[i])
                {
                    maxShakePower = percentage[i];
                }
            }
            return maxShakePower;
        }

        private float GetMaxElePower()
        {
            float maxElePower = 0f;
            for (int i = 40; i <= 41; i++)
            {
                if (maxElePower < percentage[i])
                {
                    maxElePower = percentage[i];
                }
            }
            return maxElePower;
        }

        private static string lastEffectObject = null;
        private static int sameEffectObjectCount = 0;

        internal void LOG(string v)
        {
            DateTime currentDateTime = DateTime.Now;
            using (StreamWriter writer = new StreamWriter("log.txt", true))
            {
                writer.WriteLine("{0} - {1}", currentDateTime, v);
            }
        }

        private void Close()
        {
            //relogStreamWriter.Close();
            //tglogStreamWriter.Close();

            canListen = false;

            if (listenMsg != null)
            {
                listenMsg.Abort();
                listenMsg = null;
            }

            Thread.Sleep(100);
            if (sendMsg != null)
            {
                sendMsg.Abort();
                sendMsg = null;
            }
            Thread.Sleep(100);

            receiveTip.Text = "已关闭OSC监听...";
            if (receiver != null)
            {
                receiver.Close();
                receiver = null;
            }


            Thread.Sleep(100);


            sender = null;
            sendTip.Text = null;



            start.Enabled = true;
            checkBox1.Enabled = true;
            receive.Enabled = true;
            send.Enabled = true;
            close.Enabled = false;

            _player = null;

            Properties.Settings.Default.ReceiverPort = receiveSocket;
            Properties.Settings.Default.SenderPort = sendSocket;
            Properties.Settings.Default.CheckBoxSelect = checkBox1.Checked;
            Properties.Settings.Default.FeedbackOnce = checkBox2.Checked;

            Properties.Settings.Default.ShakeIntensity = shakePower;
            Properties.Settings.Default.ElectricalIntensity = electricalPower;
            Properties.Settings.Default.ElectricalCount = electricalCount;
            Properties.Settings.Default.Save();


        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            feedbackOnce = checkBox2.Checked;
        }
    }
}
