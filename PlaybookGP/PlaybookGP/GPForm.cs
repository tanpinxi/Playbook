using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Playbook
{

    public partial class GPForm : Form
    {
        private int chaptNum = 0, imgNum = 1;
        private readonly int maxChapt = 3;
        private readonly string errorText = "Error reading text file";
        private string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private Socket childSocket;
        private const int port = 8080;

        // ManualResetEvent instances signal completion.  
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        // The response from the remote device.  
        private string response = String.Empty;

        private PicForm picform;

        public GPForm()
        {
            InitializeComponent();

            CreateSocket();

            picform = new PicForm();
            picform.Hide();
            
            //UpdateStoryText();

            /**
            string vidPath = exePath + $@"\media\s{chaptNum}.mp4";
            vlcControl.SetMedia(new System.IO.FileInfo(vidPath), param);
            vlcControl.Play();
            **/
        }

        public ManualResetEvent allDone = new ManualResetEvent(false);

        private void CreateSocket()
        {
            // Connect to a remote device.  
            try
            {
                //IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                //IPAddress ipAddress = ipHostInfo.AddressList[0];
                //IPAddress ipAddress = IPAddress.Parse("192.168.1.112");
                IPAddress ipAddress = IPAddress.Parse("172.20.10.2");
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                // Create a TCP/IP socket.
                childSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                childSocket.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), childSocket);
                connectDone.WaitOne();

                Receive(childSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket serverRequest = (Socket)ar.AsyncState;
                serverRequest.EndConnect(ar);

                Console.WriteLine($"Socket connected to {serverRequest.RemoteEndPoint.ToString()}");

                // Signal that the connection has been made.  
                connectDone.Set();
            }
            catch (Exception e)
            {
                string message = "ChildForm is inactive!";
                string title = "Error";
                MessageBoxButtons buttons = MessageBoxButtons.OK;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.OK)
                {
                    CleanUp();
                }
            }
        }

        private void Receive(Socket incoming)
        {
            try
            {
                Console.WriteLine($"Listening to {incoming.RemoteEndPoint.ToString()}");
                // Create the state object.  
                StateObject state = new StateObject();
                state.workSocket = incoming;

                // Begin receiving the data from the remote device.  
                incoming.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the state object and the client socket   
                // from the asynchronous state object.  
                StateObject state = (StateObject)ar.AsyncState;
                Socket incoming = state.workSocket;

                // Read data from the remote device.  
                int bytesRead = incoming.EndReceive(ar);

                Console.WriteLine($"Received {bytesRead} from {incoming.RemoteEndPoint.ToString()}");

                response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                Console.WriteLine($"Received {response} from {incoming.RemoteEndPoint.ToString()}");

                if (response.Contains("Next Video"))
                {
                    NextChapter();
                }
                else if (response.Contains("Incoming Image"))
                {
                    ReceiveImage();
                }
                else if (response.Contains("<EOF>"))
                {
                    Console.WriteLine("ChildForm terminated");
                    incoming.Close();
                    CleanUp();
                }
                incoming.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket outgoing, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            outgoing.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), outgoing);
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveImage()
        {
            byte[] data = ReadImage(childSocket);
            MemoryStream ms = new MemoryStream(data);
            try
            {
                Image bmp = Image.FromStream(ms);
                UpdateImage(bmp);
            }
            catch { }
        }

        private static byte[] ReadImage(Socket s)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];

            recv = s.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];

            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        private void AcceptImage(int i)
        {
            Thread.Sleep(5000);

            string imgPath = $@"C:\Users\Pinxi\Dropbox\Makerthon\i{imgNum}.jpg";
            if (File.Exists(imgPath))
            {
                UpdateImage(imgPath);
                imgNum++;
            }
            else
            {
                if (i <= 10)
                {
                    AcceptImage(++i);
                }
            }
        }

        private void UpdateImage(string imgPath)
        {
            UpdateImage(Image.FromFile(imgPath));
        }

        private void UpdateImage(Image bmp)
        {
            if (storyLabel.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    UpdateImage(bmp);
                }));
            }
            else
            {
                picform.SetImage(bmp);
                picform.ShowDialog();
                picform.Hide();
                NextChapter();
            }
        }

        private void CleanUp()
        {
            if (storyLabel.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate ()
                {
                    CleanUp();
                }));
            }
            else
            {
                childSocket.Close();
                this.Close();
            }
        }

        private void NextChapter()
        {
            if (storyLabel.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    NextChapter();
                }));
            }
            else
            {
                vlcControl.Stop();

                chaptNum++;
                if (chaptNum > maxChapt)
                {
                    chaptNum = 1;
                }

                string vidPath = exePath + $@"\media\s{chaptNum}.mp4";
                vlcControl.SetMedia(new System.IO.FileInfo(vidPath), new string[] { "input-repeat=65535" });
                vlcControl.Play();
            }
            UpdateStoryText();
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Send(childSocket, "<EOF>");
            childSocket.Close();

            this.Close();
        }

        private void UpdateStoryText()
        {
            try
            {
                if (storyLabel.InvokeRequired)
                {
                    Invoke(new MethodInvoker(delegate () {
                        UpdateStoryText();
                    }));
                }
                else
                {
                    string filePath = exePath + $@"\media\t{chaptNum}.txt";
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        // Read the stream to a string, and write the string to the console.
                        string text = sr.ReadToEnd();
                        storyLabel.Text = text;
                    }
                }
            }
            catch (IOException e)
            {
                storyLabel.Text = errorText;
            }
        }
    }
}
