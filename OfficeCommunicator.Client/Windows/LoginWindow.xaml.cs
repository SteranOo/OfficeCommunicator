using System.Configuration;
using System.Threading.Tasks;
using System.Windows;
using OfficeCommunicator.Client.Network;
using OfficeCommunicator.Middleware.Utils;

namespace OfficeCommunicator.Client.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private IStringEncoder Encoder
        {
            get
            {
                var setting = ConfigurationManager.AppSettings["PasswordEncoding"];
                IStringEncoder encoder = null; 
                switch (setting)
                {
                    case "Md5":
                        encoder = new Md5StringEncoder();
                        break;
                    case "Sha256":
                        encoder = new Sha256StringEncoder();
                        break;
                }
                return encoder;
            }
        }

        private async void BtnSignIn_Click(object sender, RoutedEventArgs e)
        {
            BtnSignIn.IsEnabled = false;
            var login = TbLogin.Text;
            var password = PbPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return;

            var encodedPassword = Encoder.EncodeString(password);
            var result = await Task.Run(() => ServerProxy.Instance.Login(login, encodedPassword));

            if (result)
            {
                new MainWindow(login).Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid login or password.", "Sign In", MessageBoxButton.OK, MessageBoxImage.Warning);
                BtnSignIn.IsEnabled = true;
            }
        }

        private async void BtnSignUp_Click(object sender, RoutedEventArgs e)
        {
            BtnSignUp.IsEnabled = false;
            var login = TbLogin.Text;
            var password = PbPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                return;

            var result = await Task.Run(() => ServerProxy.Instance.Register(login, password));

            if (result)
                MessageBox.Show("You have successfully registered", "Sign Up", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("User with this login already exists", "Sign Up", MessageBoxButton.OK, MessageBoxImage.Warning);

            BtnSignUp.IsEnabled = true;
        }
    }
}
