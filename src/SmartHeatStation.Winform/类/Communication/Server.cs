using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

using SmartHeatStationParaInfo;

using 流量计检定上位机;

namespace SocketCom
{
    public class Server
    {

        const int DefaultPortId = 8181;
        int clientCount = 0;
        /// <summary>
        /// 监听对象
        /// </summary>
        public TcpListener listener = new TcpListener(IPAddress.Parse("192.168.223.195"), DefaultPortId);
        /// <summary>
        /// 客户端1
        /// </summary>
        public TcpClient client1;
        /// <summary>
        /// 客户端2
        /// </summary>
        public TcpClient[] Clients;
        public TcpClient client2;
        /// <summary>
        /// 客户端1的名字
        /// </summary>
        public string client1Name;
        /// <summary>
        /// 客户端2的名字
        /// </summary>
        public string client2Name;
        /// <summary>
        /// 所有用户是否已准备
        /// </summary>
        public bool everyOneIsOk = false;
        public bool client1IsOk = false;
        public bool client2IsOk = false;

        public int sleep = 100;

        /// <summary>
        /// 寻远接收客户端的连接请求，当连接2个客户端后关闭端口监听。
        /// </summary>
        public void Connection() //循环检测是否有连接请求,接受最先请求的两个连接,得到两个TcpClient,然后拒绝其他的所有连接
        {
            while (true)
            {
                if (client1 == null)
                {
                    client1 = listener.AcceptTcpClient();   //得到一个客户端
                }
                if (client1 != null)   //如果已有两个客户端,关闭监听,终端循环.
                {
                    //listener.Stop();
                    //break;
                }
            }
        }

        /// <summary>
        /// 循环接收客户端1的请求数据
        /// </summary>
        public void AccpetClient1Data()
        {

            while (true)
            {
                try
                {
                    NetworkStream Ns1 = client1.GetStream();
                    string str1 = "";
                    byte[] bytes1 = new byte[108];
                    Ns1.Read(bytes1, 0, 108);
                    str1 = Encoding.Default.GetString(bytes1);
                    if (str1.StartsWith("OK", StringComparison.Ordinal))
                    {
                        throw (null);
                        continue;
                    }
                    if (str1.StartsWith("SetPara", StringComparison.Ordinal))
                    {
                        str1 = str1.Replace("SetPara", "");
                        var strResult = str1.Split('|');
                        var index = Convert.ToInt32(strResult[0]);
                        var strParaKind = strResult[1];
                        var strOperation = strResult[2];

                        if (strOperation == "Open")
                        {
                            Form_MainShow.Stations[index].水箱进水阀.阀开信号 = "1";
                            Form_MainShow.Stations[index].水箱进水阀.阀关信号 = "0";
                            //Form_MainShow.formMain.formMainShow.formMyFindData?.InitData();
                            //Form_MainShow.formMain.formMainShow.formMyFindData?.formStationInfo?.InitData();
                            System.Windows.Forms.MessageBox.Show($"{Form_MainShow.Stations[index].Name}的水箱补水阀被打开了");
                        }
                        else if (strOperation == "Close")
                        {

                            Form_MainShow.Stations[index].水箱进水阀.阀开信号 = "0";
                            Form_MainShow.Stations[index].水箱进水阀.阀关信号 = "1";
                            //Form_MainShow.formMain.formMainShow.formMyFindData?.InitData();
                            //Form_MainShow.formMain.formMainShow.formMyFindData?.formStationInfo?.InitData();
                            System.Windows.Forms.MessageBox.Show($"{Form_MainShow.Stations[index].Name}的水箱补水阀被关闭了");
                        }

                        //System.Windows.Forms.MessageBox.Show($"DFSDFSFSFSF");
                        continue;
                    }
                }
                catch
                {
                    break;
                }

            }
        }

 
        /// <summary>
        /// 发送字符串给客户端
        /// </summary>
        /// <param name="str">要发送的字符串</param>
        /// <param name="client">客户端编号,只能是1,2</param>
        /// <returns>是否发送成功</returns>
        public bool SendDataForClient(string str, int client)
        {
            try
            {

                NetworkStream Ns = null;
                if (client == 1)
                {
                    Ns = this.client1.GetStream();
                }
                if (client == 2)
                {
                    Ns = this.client2.GetStream();//得到客户端的网络流对象
                }
                if (client != 1 && client != 2)
                {
                    return false;
                }
                byte[] byteStr = Encoding.Default.GetBytes(str); 
                Ns.Write(byteStr, 0, byteStr.Length);  //把2个比特流对象写入server与client的连接管道中
                Thread.Sleep(sleep);
            }
            catch
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 通知客户端选出的地主
        /// </summary>
     
    }
}
