using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApplicationProject.Utilities;

namespace WpfApplicationProject.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private const int MAX_LOGIN_NUMBER = 3;

        public bool IsLogin { get; set; }

        public TextBox TxtUsername { get; set; }

        public PasswordBox TxtPassword { get; set; }

        public ICommand LoadedWindowCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand CloseWindowCommand { get; set; }

        private int _counter;

        public LoginViewModel()
        {

            LoadedWindowCommand = new RelayCommand<Window>(x => x == null ? false : true, x => LoadedWindow(x));

            LoginCommand = new RelayCommand<Window>(x => x == null ? false : true, x => Login(x));

            CloseWindowCommand = new RelayCommand<Window>(x => x == null ? false : true, x => CloseWindow(x));

        }

        private void LoadedWindow(Window window)
        {
            _counter = 0;

            TxtUsername = SystemMethods.FindChild<TextBox>(window, "txtUserName");

            TxtPassword = SystemMethods.FindChild<PasswordBox>(window, "txtPassword");
        }

        private void CloseWindow(Window window)
        {
            window.Close();
        }

        private void Login(Window window)
        {
            if (window == null) return;

            if (!CheckLogin()) return;

            string password = SystemMethods.GetHash(TxtPassword.Password);

            var user = DataProvider.Ins.DB.Users.Where(x => x.Username == TxtUsername.Text && x.Password == password && x.Active == true).FirstOrDefault();

            if(user != null)
            {
                IsLogin = true;

                window.Close();

                return;
            }

            MessageBox.Show("Tài khoản đăng nhập không chính xác", "Thông tin đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);

            _counter++;

            IsLogin = false;

            if (_counter > MAX_LOGIN_NUMBER)
            {
                MessageBox.Show($"Bạn đã đăng nhập sai quá { MAX_LOGIN_NUMBER } lần\r\n\r\nChương trình sẽ kết thúc xin vui lòng thử lại sau", "Thông tin đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);

                window.Close();
            }
        }

        private bool CheckLogin()
        {
            if (string.IsNullOrEmpty(TxtUsername.Text))
            {
                MessageBox.Show("Tài khoản đăng nhập không được để trống", "Thông tin đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);

                TxtUsername.Focus();

                return false;
            }

            if (string.IsNullOrEmpty(TxtPassword.Password))
            {
                MessageBox.Show("Mật khẩu không được để trống", "Thông tin đăng nhập", MessageBoxButton.OK, MessageBoxImage.Warning);

                TxtPassword.Focus();

                return false;
            }

            return true;
        }
    }
}
