using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoulderdashServerFunctionals;

namespace BoulderdashServerView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        //Objekt służący do realizacji odbierania datagramów UDP
        UdpReceiver udpReceiver;

        //Objekt realizujący zapis wyniku gry do pliku
        ScoreRecorder scoreRecorder;

        //Pole przechowujące ścieżkę do pliku, który przechowuje rekordy wyników gier.
        private string recordFilePath = "resultsRecords.txt";

        //lista dostępnych adresów IPv4 dla urzązenia zbindowana z kontrolką ComboBox
        private ObservableCollection<string> resultsRecords = new ObservableCollection<string>();
        public ObservableCollection<string> ResultsRecords
        {
            get { return resultsRecords; }
            set { resultsRecords = value; }
        }

        //Pole przechowujące lokalny numer wykorzystywanego portu.
        //Jest zbindowane z polem typu TexBox w interfejsi.
        //W czasie przypisywania wartości następuje walidacja, aby sprawdzić
        //czy użycie wskazanego portu jest możliwe
        private int port = 10000;
        public int Port
        {
            get { return port; }
            set
            {
                port = value;
                udpReceiver.Port = port;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Port"));
            }
        }


        public MainWindow()
        {
            udpReceiver = new UdpReceiver(handleIncommingUdpPackage);
            udpReceiver.Port = port;

            udpReceiver.BeginListening();

            scoreRecorder = new ScoreRecorder(recordFilePath);

            InitializeComponent();
        }


        /// <summary>
        /// Metoda obsługi przychodzącego pakietu udp.
        /// </summary>
        /// <param name="result"></param>
        //private void handleIncommingUdpPackage(UdpReceiveResult result)
        private void handleIncommingUdpPackage(byte[] result)
        {
            string udpMsg = Encoding.UTF8.GetString(result);

            string finalMsg = scoreRecorder.writeScore("Wynik ostaniej rozgrywki to: " + udpMsg);

            //resultsRecords.Add(finalMsg);
            this.Dispatcher.Invoke(() => { resultsRecords.Add(finalMsg); });

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ResultsRecords"));
        }

        /// <summary>
        /// Metoda służąca jako uproszenie tworzenia message box-ów
        /// </summary>
        /// <param name="caption">Zawartość tytułu okienka</param>
        /// <param name="messageBoxText">Treść okienka</param>
        /// <param name="icon">Typ ikonki w okienku</param>
        private void signalMessage(string caption, string messageBoxText, MessageBoxImage icon)
        {
            MessageBoxButton button = MessageBoxButton.OK;
            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.OK);
        }

        /// <summary>
        /// Metoda wywoływana podczas inicjalizacji ComboBox'a IPv4AddressesCB
        /// WYkonuje wypełnienie list dostępnych adresó Ipv4 dla urządzenia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResultsRecords_Initialized(object sender, EventArgs e)
        {
            ResultsRecords.Clear();

            foreach (string record in scoreRecorder.readScores()) //przeszukanie listy nadanych adresów w celu odnalezienia adresów IPv4
            {
                if (record.Equals(""))
                {
                    continue;
                }
                resultsRecords.Add(record);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            udpReceiver.Close();
        }
    }
}
