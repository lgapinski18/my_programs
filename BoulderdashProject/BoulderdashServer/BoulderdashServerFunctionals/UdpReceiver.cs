using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BoulderdashServerFunctionals
{
    //public delegate void IncomingUdpPackageHandler(UdpReceiveResult result);
    public delegate void IncomingUdpPackageHandler(byte[] result);

    public class UdpReceiver
    {
        //Pole przechowujące lokalny numer portu
        private int port = 0;
        public int Port { get { return port; } set { port = value; } }

        //Pole przechowujące informację logiczną o tym czy jest realizowane obecnie nasłuchiwanie
        private bool isListening = false;

        //Pole przechowujące informację logiczną o tym czy ma być realizowane nasłuchiwanie
        private bool toBeListening = false;

        //Pole przechowujące instancję wątku realizującego nasłuchiwanie
        private Thread listeningThread = null;

        //Pole przechowujące hadnler obsługujący odebrane połączenie
        private IncomingUdpPackageHandler incomingUdpPackageHandler;


        public UdpReceiver(IncomingUdpPackageHandler incomingUdpPackageHandler)
        {
            this.incomingUdpPackageHandler = incomingUdpPackageHandler;
        }


        /// <summary>
        /// Metoda rozpoczynająca nasłuchiwanie przychodzących do gniazda poleceń.
        /// Powołuje w tym celu osobny wątek.
        /// </summary>
        public void BeginListening()
        {
            toBeListening = true;
            if (!isListening)
            {
                listeningThread = new Thread(new ThreadStart(Listen));
                listeningThread.Start();
            }
        }

        /// <summary>
        /// Metoda zwalniająca zasoby zajmowane przez gniazdo.
        /// </summary>
        public void Dispose()
        {
            //Jeżeli powołany jest wątek nasluchujący zostaje on najpierw zamknięty.
            if (listeningThread != null)
            {
                toBeListening = false;
                listeningThread.Join();
            }
        }

        /// <summary>
        /// Metoda zamykająca aktywne połączenia i zwalniająca zajmowane przez gniazdo zasoby.
        /// </summary>
        public void Close()
        {
            //Jeżeli powołany jest wątek nasluchujący zostaje on najpierw zamknięty.
            if (listeningThread != null)
            {
                toBeListening = false;
                listeningThread.Join();
            }
        }

        private void Listen()
        {
            isListening = true;

            Trace.WriteLine("Rozpoczęto nasłuchiwanie!");

            while (toBeListening)
            {
                UdpClient udpClient = new UdpClient(port);


                Task<UdpReceiveResult> resultTask = udpClient.ReceiveAsync();

                while (!resultTask.IsCompleted && toBeListening);

                if (!toBeListening)
                {
                    break;
                }

                //Powołanie wątku obsługi przyjętego połączenia
                Thread handleIncomingThread = new Thread(new ThreadStart(() => {
                    UdpReceiveResult result = resultTask.Result;
                    byte[] data = result.Buffer;
                    incomingUdpPackageHandler.Invoke(data); 
                }));
                handleIncomingThread.Start();

                udpClient.Dispose();
                udpClient.Close();
                Thread.Sleep(100);
            }

            Trace.WriteLine("Zakończono nasłuchiwanie!");
            isListening = false;
        }
    }
}
