using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using OfficeCommunicator.Client.Network;
using OfficeCommunicator.Middleware.Dto;

namespace OfficeCommunicator.Client.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly string _login;
        private readonly ObservableCollection<string> _users;
        private string _selectedUser;

        private ServerListener Listener => ServerProxy.Instance.Listener;
        private ServerProxy Server => ServerProxy.Instance;

        public MainWindow(string login)
        {
            InitializeComponent();
            _login = login;
            _users = new ObservableCollection<string>();
            LbUsers.ItemsSource = _users;
            _selectedUser = "";
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            LblLogin.Content = "Login: " + _login;
            Server.GetUsers().ForEach(x => Dispatcher.Invoke(() => _users.Add(x)));
            Listener.OnUserLoggedIn += login => Dispatcher.Invoke(() =>
            {
                if (!_users.Contains(login))
                    _users.Add(login);
            });
            Listener.OnUserLoggedOut += login => Dispatcher.Invoke(() =>
            {
                if (_users.Contains(login))
                    _users.Remove(login);
            });
        }

        private async void LbUsers_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            _selectedUser = LbUsers.SelectedValue.ToString();
            TbChatLog.Text = "";
            TbMessage.Text = "";
            var messages = await Task.Run(() =>  Server.GetMessages(_selectedUser));
            messages.ForEach(x => TbChatLog.Text += $"{x.Time}|{x.Sender}: {x.Text}");
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(TbMessage.Text) || _selectedUser == null)
                return;

            var message = new ChatMessageDto
            {
                Sender = _login,
                Recipient = _selectedUser,
                Text = TbMessage.Text,
                Time = DateTime.Now
            };
            Server.SendMessage(message);
        }
    }
}
