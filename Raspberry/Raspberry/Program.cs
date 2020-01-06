using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Device.Gpio;
using System.Collections.Generic;

namespace Raspberry
{
    class Program
    {
        static void Main(string[] args) 
        {
            new Raspberry().Run();
        }
    }

    class Raspberry
    {
        private bool onEvenPage = false, running = true;
        private List<String> sentImgs = new List<String>();
        private GpioController controller;

        private Socket childSocket;
        private const int port = 8080;
        private ManualResetEvent connectDone = new ManualResetEvent(false);

        private Thread t;

        public void Run()
        {
            CreateSocket();
            AssignPins();

            /**
            while (true)
            {
                Console.ReadLine();
                GetImage(new Object(), null);
            }
            **/

            t = new Thread(runForever);
            t.Start();
        }

        private void runForever()
        {
            while (running)
            {

            }
        }

        private void AssignPins()
        {
            //26 - button power out
            //19 - button power in
            //16 - page power out
            //20 - even pages circuit - blue
            //21 - odd pages circuit - yellow

            controller = new GpioController();
            controller.OpenPin(26, PinMode.Output);
            controller.OpenPin(19, PinMode.Input);
            controller.OpenPin(16, PinMode.Output);
            controller.OpenPin(20, PinMode.Input);
            controller.OpenPin(21, PinMode.Input);

            controller.RegisterCallbackForPinValueChangedEvent(19, PinEventTypes.Rising, GetImage);
            controller.RegisterCallbackForPinValueChangedEvent(20, PinEventTypes.Rising, FlipToOdd);
            controller.RegisterCallbackForPinValueChangedEvent(21, PinEventTypes.Rising, FlipToEven);

            controller.Write(26, PinValue.High);
            controller.Write(16, PinValue.High);

            Console.WriteLine("Pins Assigned");

            Thread reader = new Thread(ReadPins);
            reader.Start();
        }

        private void ReadPins()
        {
            while (true)
            {
                Console.WriteLine($"Blue : {controller.Read(20)}, Yellow: {controller.Read(21)}");
                Thread.Sleep(1000);
            }
        }

        private void FlipPage()
        {
            SendAsync(childSocket, "Next Video");
        }

        private void PassDrawing(string imgPath)
        {
            SendAWait(childSocket, "Incoming Image");

            Bitmap bmp = new Bitmap(imgPath);
            MemoryStream ms = new MemoryStream();
            // Save to memory using the Jpeg format
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            // read to end
            byte[] bmpBytes = ms.ToArray();

            bmp.Dispose();
            ms.Close();

            SendImage(childSocket, bmpBytes);
        }
        private void GetImage(object sender, PinValueChangedEventArgs args)
        {
            Console.WriteLine("Getting Image");

            string rootPath = @"/home/pi/.tuxpaint/saved";
            string[] allFiles = Directory.GetFiles(rootPath);

            foreach (string fileName in allFiles)
            {
                if (fileName.Contains(".png") && !sentImgs.Contains(fileName))
                {
                    PassDrawing(fileName);
                    sentImgs.Add(fileName);
                    break;
                }
            }
        }

        private void FlipToEven(object sender, PinValueChangedEventArgs args)
        {
            //Console.WriteLine($"Blue : {controller.Read(20)}, Yellow: {controller.Read(21)}");
            if (!onEvenPage && controller.Read(20).Equals(PinValue.Low))
            {
                Console.WriteLine("Switching to even page");
                FlipPage();
                onEvenPage = true;
            }
        }

        private void FlipToOdd(object sender, PinValueChangedEventArgs args)
        {
            //Console.WriteLine($"Blue : {controller.Read(20)}, Yellow: {controller.Read(21)}");
            if (onEvenPage && controller.Read(21).Equals(PinValue.Low))
            {
                Console.WriteLine("Switching to odd page");
                FlipPage();
                onEvenPage = false;
            }
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

        private void CreateSocket()
        {
            // Connect to a remote device.  
            try
            {
                Console.WriteLine("Creating Sockets");
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
                Console.WriteLine(e.ToString());
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

                string response = Encoding.ASCII.GetString(state.buffer, 0, bytesRead);
                Console.WriteLine($"Received {response} from {incoming.RemoteEndPoint.ToString()}");

                if (response.Contains("<EOF>"))
                {
                    Console.WriteLine("ChildForm terminated");
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

        public void CleanUp()
        {
            childSocket.Close();
            controller.ClosePin(26);
            controller.ClosePin(19);
            controller.ClosePin(16);
            controller.ClosePin(20);
            controller.ClosePin(21);
            controller.Dispose();
            running = false;
        }
    }
}