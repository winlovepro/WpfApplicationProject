using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApplicationProject.DataAccesses.Entities;
using WpfApplicationProject.Utilities;
using WpfApplicationProject.View;

namespace WpfApplicationProject.ViewModel
{
    public class MainViewModel: BaseViewModel
    {

        public ICommand LoadedWindowCommand { get; set; }

        public ICommand AddNewUserCommand { get; set; }

        public ICommand UpdateUserCommand { get; set; }

        public ICommand DeleteUserCommand { get; set; }

        public ICommand DoubleClickRowCommand { get; set; }

        private ObservableCollection<User> _users;

        public ObservableCollection<User> Users
        {
            get { return _users; }

            set { _users = value; OnPropertyChanged(); }
        }

        private User _selectedItem;

        public User SelectedItem
        {
            get { return _selectedItem; }

            set { _selectedItem = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            LoadedWindowCommand = new RelayCommand<Window>((x) => { return x == null ? false : true; }, (x) => { LoadedWindow(x); });

            AddNewUserCommand = new RelayCommand<Window>((x) => { return x == null ? false : true; }, (x) => { AddNewUser(x); });

            UpdateUserCommand = new RelayCommand<Window>((x) => { return x == null ? false : true; }, (x) => { UpdateUser(x); });

            DeleteUserCommand = new RelayCommand<Window>((x) => { return x == null ? false : true; }, (x) => { DeleteUser(x); });

            DoubleClickRowCommand = new RelayCommand<Window>(x => { return x == null ? false : true; }, x => { DoubleClieckRow(x); });
        }

        private void LoadedWindow(Window window)
        {

            if (window == null) return;

            window.Hide();

            LoginWindow loginWindow = new LoginWindow();

            loginWindow.ShowDialog();

            if (loginWindow.DataContext == null) return;

            LoginViewModel loginViewModel = loginWindow.DataContext as LoginViewModel;

            if(loginViewModel.IsLogin)
            {
                window.Show();

                GetAllUsers();
            }
            else
            {
                window.Close();
            }

        }

        public void GetAllUsers()
        {
            Users = new ObservableCollection<User>(DataProvider.Ins.DB.Users);
        }

        private void AddNewUser(Window window)
        {
            UserWindow userWindow = new UserWindow();

            UserViewModel viewModel = userWindow.DataContext as UserViewModel;

            viewModel.MainVM = window.DataContext as MainViewModel;

            viewModel.IsAddNewUser = true;

            userWindow.ShowDialog();
        }

        private void UpdateUser(Window window)
        {

            if(SelectedItem == null)
            {
                MessageBox.Show("Xin vui lòng chọn thông tin user cần cập nhật", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            UserWindow userWindow = new UserWindow();

            UserViewModel viewModel = userWindow.DataContext as UserViewModel;

            viewModel.MainVM = window.DataContext as MainViewModel;

            viewModel.CurrentUser = SelectedItem;

            viewModel.IsUpdateUser = true;

            userWindow.ShowDialog();

        }

        private void DeleteUser(Window window)
        {
            if(SelectedItem == null)
            {
                MessageBox.Show("Xin vui lòng chọn thông tin user cần xóa", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            var msg = MessageBox.Show($"Bạn muốn xóa User \"{ SelectedItem.Username }\" ?", "Thông Báo", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (msg != MessageBoxResult.Yes) return;

            DataProvider.Ins.DB.Users.Remove(SelectedItem);

            var result = DataProvider.Ins.DB.SaveChanges();

            if (result == 1)
            {
                GetAllUsers();

                MessageBox.Show("Xóa dữ liệu thành công", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);

                SelectedItem = null;

                return;
            }

            MessageBox.Show("Xóa dữ liệu thất bại xin vưi lòng kiểm tra lại thông tin", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DoubleClieckRow(Window window)
        {
            if (window == null || SelectedItem == null) return;

            UpdateUser(window);
        }
    }
}
