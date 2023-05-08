using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApplicationProject.DataAccesses.Entities;
using WpfApplicationProject.Utilities;

namespace WpfApplicationProject.ViewModel
{
    public class UserViewModel: BaseViewModel
    {

        private TextBox _txtFirstname, _txtLastname, _txtUsername, _txtDescription;

        private PasswordBox _pwbPassword;

        private CheckBox _chkActive;

        public ICommand LoadedWindowCommand { get; set; }

        public ICommand AddNewUserCommand { get; set; }

        public ICommand UpdateUserCommand { get; set; }

        public ICommand ExitWindowCommand { get; set; }

        public MainViewModel MainVM { get; set; }

        public User CurrentUser { get; set; }

        private bool _isAddNewUser;

        public bool IsAddNewUser
        {
            get { return _isAddNewUser; }

            set { _isAddNewUser = value; OnPropertyChanged(); }
        }

        private bool _isUpdateUser;

        public bool IsUpdateUser
        {
            get { return _isUpdateUser; }

            set { _isUpdateUser = value; OnPropertyChanged(); }
        }

        public UserViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>(x => { return x == null ? false : true; }, x => { LoadedWindow(x); });

            AddNewUserCommand = new RelayCommand<Window>(x => { return x == null ? false : true; }, x => { AddNewUser(x); });

            UpdateUserCommand = new RelayCommand<Window>(x => { return x == null ? false : true; }, x => { UpdateUser(x); });

            ExitWindowCommand = new RelayCommand<Window>(x => { return x == null ? false : true; }, x => { x.Close(); });
        }

        private void LoadedWindow(Window window)
        {
            _txtFirstname = SystemMethods.FindChild<TextBox>(window, "txtFirstname");

            _txtLastname = SystemMethods.FindChild<TextBox>(window, "txtLastname");

            _txtUsername = SystemMethods.FindChild<TextBox>(window, "txtUsername");

            _pwbPassword = SystemMethods.FindChild<PasswordBox>(window, "pwbPassword");

            _chkActive = SystemMethods.FindChild<CheckBox>(window, "chkActive");

            _txtDescription = SystemMethods.FindChild<TextBox>(window, "txtDescription");

            LoadUser();
        }

        private void LoadUser()
        {
            if (CurrentUser == null || IsUpdateUser == false) return;

            _txtFirstname.Text = CurrentUser.Firstname;

            _txtLastname.Text = CurrentUser.Lastname;

            _txtUsername.Text = CurrentUser.Username;

            _pwbPassword.Password = CurrentUser.Password;

            _chkActive.IsChecked = CurrentUser.Active;

            _txtDescription.Text = CurrentUser.Description;
        }

        private void AddNewUser(Window window)
        {
            if (window == null) return;

            if (!CheckInput("12345")) return;

            var query = DataProvider.Ins.DB.Users.Add(new User()
            {
                Id = SystemMethods.GetNewID(),
                Firstname = _txtFirstname.Text,
                Lastname = _txtLastname.Text,
                Username = _txtUsername.Text,
                Password = SystemMethods.GetHash(_pwbPassword.Password),
                Active = _chkActive.IsChecked,
                Description =_txtDescription.Text
            });

            var result = DataProvider.Ins.DB.SaveChanges();

            if(result == 1)
            {
                MainVM.GetAllUsers();

                MessageBox.Show("Thêm thông tin thành công", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);

                ClearAll("123456");

            }
            else
            {
                 MessageBox.Show("Thêm thông tin thất bại, xin vui lòng kiểm tra lại thông tin", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
            _txtFirstname.Focus();
        }

        private void UpdateUser(Window window )
        {
            if (window == null || CurrentUser == null) return;

            if (!CheckInput("12346", CurrentUser)) return;

            var user = DataProvider.Ins.DB.Users.Where(x => x.Id == CurrentUser.Id).FirstOrDefault();

            if(user == null)
            {
                MessageBox.Show($"Không tồn tại user với Id là { CurrentUser.Id }", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            user = GetUserUpdate(user);

            var result = DataProvider.Ins.DB.SaveChanges();

            if(result == 1)
            {
                MainVM.GetAllUsers();

                MessageBox.Show("Cập nhật dữ liệu thành công!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);

                CurrentUser = null;

                window.Close();

                return;
            } 
            else
            {
                MessageBox.Show("Cập nhật dữ liệu thất bại!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private User GetUserUpdate(User user)
        {
            string hashPassword = SystemMethods.GetHash(_pwbPassword.Password);

            if (user.Password == _pwbPassword.Password) hashPassword = user.Password;

            user.Firstname = _txtFirstname.Text;

            user.Lastname = _txtLastname.Text;

            user.Username = _txtUsername.Text;

            user.Password = hashPassword;

            user.Active = _chkActive.IsChecked;

            user.Description = _txtDescription.Text;

            return user;
        }

        private bool CheckInput(string key, object obj = null)
        {
            if (key.Contains("1") && string.IsNullOrEmpty(_txtFirstname.Text))
            {
                MessageBox.Show("Firstname không được để trống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                _txtFirstname.Focus();

                return false;
            }

            if (key.Contains("2") && string.IsNullOrEmpty(_txtLastname.Text))
            {
                MessageBox.Show("Lastname không được để trống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                _txtLastname.Focus();

                return false;
            }


            if(key.Contains("3") && string.IsNullOrEmpty(_txtUsername.Text))
            {
                MessageBox.Show("Username không được để trống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                _txtUsername.Focus();

                return false;
            }

            if (key.Contains("4") && string.IsNullOrEmpty(_pwbPassword.Password))
            {
                MessageBox.Show("Password không được để trống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                _pwbPassword.Focus();

                return false;
            }

            if(key.Contains("5"))
            {
                var query = DataProvider.Ins.DB.Users.Where(x => x.Username == _txtUsername.Text).FirstOrDefault();

                if(query != null)
                {
                    MessageBox.Show($"Username \"{ _txtUsername.Text }\" đã tồn tại trong hệ thống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                    _txtUsername.Focus();

                    _txtUsername.SelectAll();

                    return false;
                }

            }

            if (key.Contains("6"))
            {
                if (obj == null || obj.GetType() != typeof(User)) return false;

                User user = obj as User;

                var query = DataProvider.Ins.DB.Users.Where(x => x.Id != user.Id && x.Username == _txtUsername.Text).FirstOrDefault();

                if(query != null)
                {
                    MessageBox.Show($"Username \"{ _txtUsername.Text }\" đã tồn tại trong hệ thống", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                    _txtUsername.Focus();

                    _txtUsername.SelectAll();

                    return false;
                }

            }

            return true;
        }

        private void ClearAll(string key)
        {
            if (key.Contains("1")) _txtFirstname.Text = string.Empty;

            if (key.Contains("2")) _txtLastname.Text = string.Empty;

            if (key.Contains("3")) _txtUsername.Text = string.Empty;

            if (key.Contains("4")) _pwbPassword.Password = string.Empty;

            if (key.Contains("5")) _chkActive.IsChecked = false;

            if (key.Contains("6")) _txtDescription.Text = string.Empty;
        }
    }
}
