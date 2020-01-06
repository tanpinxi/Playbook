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

    public partial class ChildForm : Form
    {
        private int chaptNum = 0;
        private readonly int maxChapt = 3;
        private readonly int[] drawChapt = new int[] { 1 };
        private readonly string errorText = "Error reading text file";
        private string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        
        private Socket listener, gpSocket, piSocket;
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);

        public ChildForm()
        {
            InitializeComponent();

            CreateSocket();

            UpdateStoryText();

            nextVidBtn_Click(new Object(), new EventArgs());
            SendAsync(gpSocket, "Next Video");

            /**
            string vidPath = exePath + $@"\media\s{chaptNum}.mp4";
            vlcControl.SetMedia(new System.IO.FileInfo(vidPath), param);
            vlcControl.Play();
            **/
        }

        public void CreateSocket()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //IPAddress ipAddress = ipHostInfo.AddressList[0];
            //IPAddress ipAddress = IPAddress.Parse("192.168.1.109");
            IPAddress ipAddress = IPAddress.Parse("172.20.10.2");
            //IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 8080);
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 8080);

            // Create a TCP/IP socket.  
            listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(2);

                // Set the event to nonsignaled state.  
                connectDone.Reset();

                // Start an asynchronous socket to listen for connections.  
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(AcceptGPSocket), listener);

                // Wait until a connection is made before continuing.  
                connectDone.WaitOne();

                // Set the event to nonsignaled state.  
                connectDone.Reset();

                // Start an asynchronous socket to listen for connections.  
                Console.WriteLine("Waiting for a connection...");
                listener.BeginAccept(new AsyncCallback(AcceptPiSocket), listener);

                // Wait until a connection is made before continuing.  
                connectDone.WaitOne();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void AcceptGPSocket(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            connectDone.Set();

            // Get the socket that handles the client request.  
            listener = (Socket)ar.AsyncState;
            gpSocket = listener.EndAccept(ar);

            Console.WriteLine($"Socket connected to {gpSocket.RemoteEndPoint.ToString()}");

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = gpSocket;
            gpSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        public void AcceptPiSocket(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            connectDone.Set();

            // Get the socket that handles the client request.  
            listener = (Socket)ar.AsyncState;
            piSocket = listener.EndAccept(ar);

            Console.WriteLine($"Socket connected to {piSocket.RemoteEndPoint.ToString()}");

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = piSocket;
            piSocket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        public void ReceiveCallback(IAsyncResult ar)
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

                if (bytesRead > 0)
                {
                    string response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                    Console.WriteLine($"Received {response} from {incoming.RemoteEndPoint.ToString()}");

                    if (response.Contains("Next Video"))
                    {
                        if (!drawChapt.Contains(chaptNum))
                        {
                            nextVidBtn_Click(new Object(), new EventArgs());
                            SendAsync(gpSocket, "Next Video");
                        }
                    }
                    else if (response.Contains("Incoming Image"))
                    {
                        if (drawChapt.Contains(chaptNum))
                        {
                            Console.WriteLine("Incoming Image");
                            byte[] bmpbyte = ReadImage(piSocket);
                            SendAWait(gpSocket, "Incoming Image");
                            Console.WriteLine("Sending Image");
                            SendImage(gpSocket, bmpbyte);
                            nextVidBtn_Click(new Object(), new EventArgs());
                        }
                    }
                    else if (response.Contains("<EOF>"))
                    {
                        Console.WriteLine("GPForm terminated");
                        SendAsync(piSocket, "<EOF>");
                        incoming.Close();
                        CleanUp();
                    }
                }
                incoming.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private void SendAWait(Socket outgoing, String data)
        {
            Console.WriteLine($"Sending {data} normally to {outgoing.RemoteEndPoint.ToString()}");

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.
            outgoing.Send(byteData);
        }

        private void SendAsync(Socket outgoing, String data)
        {
            Console.WriteLine($"Sending {data} asyncally to {outgoing.RemoteEndPoint.ToString()}");

            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            outgoing.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), outgoing);
        }

        private void SendImage(Socket outgoing, byte[] imageData)
        {
            Console.WriteLine("Sending image");

            int total = 0;
            int size = imageData.Length;
            int dataleft = size;
            int sent;

            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = outgoing.Send(datasize);

            while (total < size)
            {
                sent = outgoing.Send(imageData, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine($"Sent {bytesSent} bytes to client.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static byte[] ReadImage(Socket incoming)
        {
            int total = 0;
            int recv;
            byte[] datasize = new byte[4];

            recv = incoming.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];


            while (total < size)
            {
                recv = incoming.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
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
                gpSocket.Close();
                piSocket.Close();
                listener.Close();
                this.Close();
            }
        }

        private void nextVidBtn_Click(object sender, EventArgs e)
        {
            if (storyLabel.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    nextVidBtn_Click(sender, e);
                }));
            }
            else
            {
                //SendAsync(gpSocket, "Next Video");

                vlcControl.Stop();

                chaptNum++;
                if (chaptNum > maxChapt)
                {
                    chaptNum = 1;
                }

                string vidPath = exePath + $@"\media\s{chaptNum}.mp4";
                vlcControl.SetMedia(new System.IO.FileInfo(vidPath), new string[] { "input-repeat=65535" });
                /**
                if (drawChapt.Contains(chaptNum))
                {
                    string imgPath = exePath + $@"\media\i{chaptNum}.png";
                    vlcControl.SetMedia(new System.IO.FileInfo(vidPath));
                    Thread t = new Thread(() => SwitchToImg(chaptNum));
                    t.Start();
                }
                else
                {
                    vlcControl.SetMedia(new System.IO.FileInfo(vidPath), new string[] { "input-repeat=65535" });
                }
                **/
                vlcControl.Play();
            }
            UpdateStoryText();
        }

        private void SwitchToImg(int i)
        {
            if (storyLabel.InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate () {
                    SwitchToImg(i);
                }));
            }
            else
            {
                Thread.Sleep(5000);
                string imgPath = exePath + $@"\media\i{i}.png";
                Console.WriteLine($"Updating to {imgPath}");
                vlcControl.SetMedia(new System.IO.FileInfo(imgPath));
            }
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            SendAsync(gpSocket, "<EOF>");
            SendAsync(piSocket, "<EOF>");
            gpSocket.Close();
            piSocket.Close();
            listener.Close();

            this.Close();
        }

        private void imageBtn_Click(object sender, EventArgs e)
        {
            string imgPath = exePath + $@"\media\i{chaptNum}.jpg";

            SendAWait(gpSocket, "Incoming Image");

            Bitmap bmp = new Bitmap(imgPath);
            MemoryStream ms = new MemoryStream();
            // Save to memory using the Jpeg format
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            // read to end
            byte[] bmpBytes = ms.ToArray();

            bmp.Dispose();
            ms.Close();

            SendImage(gpSocket, bmpBytes);
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
