using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AndroidForeverHang
{
    public partial class MainPage : ContentPage
    {
        private enum Status
        {
            ConnectFailed,
            Disconnected,
            Disconnecting,
            Cancelling,
            Connecting, 
            Connected,
            Canceled,
        }

        private CancellationTokenSource _tokenSource;
        private Socket _socket = null;
        private Status _status = Status.Disconnected; 

        public MainPage()
        {
            InitializeComponent();

        }

        private void OnCancel()
        {
            _socket?.Close();
            _socket?.Dispose();
            _socket = null; 
        }

        private void UpdateStatus(Status status)
        {
            _status = status;

            switch(_status)
            {
                case Status.Connected:
                    StatusLabel.Text = "Connected!";

                    ConnectButton.IsEnabled = false;
                    DisconnectButton.IsEnabled = true;
                    DisconnectButton.Text = "Disconnect";
                    break;
                case Status.Connecting:
                    StatusLabel.Text = "Connecting...";

                    ConnectButton.IsEnabled = false;
                    DisconnectButton.IsEnabled = true;
                    DisconnectButton.Text = "Stop Connecting";
                    break;
                case Status.Disconnected:
                    StatusLabel.Text = "Disconnected!";

                    ConnectButton.IsEnabled = true;
                    DisconnectButton.IsEnabled = false;
                    DisconnectButton.Text = "Disconnect";
                    break;

                case Status.Canceled:
                    StatusLabel.Text = "Connect Cancelled!";

                    ConnectButton.IsEnabled = true;
                    DisconnectButton.IsEnabled = false;
                    DisconnectButton.Text = "Disconnect";
                    break;
                case Status.ConnectFailed:
                    StatusLabel.Text = "Connect Failed!";

                    ConnectButton.IsEnabled = true;
                    DisconnectButton.IsEnabled = false;
                    DisconnectButton.Text = "Disconnect";
                    break;
                case Status.Disconnecting:

                    break; 
            }

        }

        private async void ConnectButton_Clicked(object sender, EventArgs e)
        {
            if(_status != Status.Connected && _status != Status.Connecting)
            {
                UpdateStatus(Status.Connecting);

                _tokenSource = new CancellationTokenSource();

                CancellationToken token = _tokenSource.Token;
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                try
                {
                    using (token.Register(OnCancel))
                    {
                        await _socket.ConnectAsync("176.222.222.222", 1234);

                        UpdateStatus(Status.Connected);
                        DisconnectButton.Text = "Disconnect";
                    }
                }
                catch(ObjectDisposedException)
                {
                    UpdateStatus(Status.Canceled); 
                }
                catch (Exception)
                {
                    UpdateStatus(Status.ConnectFailed);
                }
            }
        }

        private void DisconnectButton_Clicked(object sender, EventArgs e)
        {
            if(_status == Status.Connecting || _status == Status.Connected)
            {
                switch (_status)
                {
                    case Status.Connected:
                        StatusLabel.Text = "Disconnecting...";

                        _socket.Shutdown(SocketShutdown.Both);
                        _socket.Close();
                        UpdateStatus(Status.Disconnected);
                        break;
                    case Status.Connecting:
                        StatusLabel.Text = "Cancelling...";

                        _tokenSource.Cancel();
                        break;
                }
            }
        }
    }
}
