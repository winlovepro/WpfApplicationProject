using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApplicationProject.Utilities;

namespace WpfApplicationProject.ViewModel
{
    public class ControlBarViewModel : BaseViewModel
    {

        public ICommand LoadedWindowCommand { get; set; }

        public ICommand MinimizeWindowCommand { get; set; }

        public ICommand MaximizeWindowCommand { get; set; }

        public ICommand DragMoveWindowCommand { get; set; }
        
        public ICommand CloseWindowCommand { get; set; }

        private string _windowTitle;

        public string WindowTitle 
        {
            get => _windowTitle;

            set { _windowTitle = value; OnPropertyChanged(); }
        }

        private ImageSource _windowIcon;

        public ImageSource WindowIcon
        {
            get => _windowIcon;

            set { _windowIcon = value; OnPropertyChanged(); }
        }

        private Visibility _windowIsMaximize;

        public Visibility WindowIsMaximize
        {
            get => _windowIsMaximize;

            set { _windowIsMaximize = value; OnPropertyChanged(); }
        }


        public ControlBarViewModel()
        {
            LoadedWindowCommand = new RelayCommand<UserControl>((x) => { return x == null ? false : true; }, (x) => { LoadedWindow(x); });

            MinimizeWindowCommand = new RelayCommand<UserControl>((x) => { return x == null ? false : true; }, (x) => { MinimizeWindow(x); });

            MaximizeWindowCommand = new RelayCommand<UserControl>((x) => { return x == null ? false : true; }, (x) => { MaximizeWindow(x); });

            CloseWindowCommand = new RelayCommand<UserControl>((x) => { return x == null ? false : true; }, (x) => { CloseWindow(x); });

            DragMoveWindowCommand = new RelayCommand<UserControl>((x) => { return x == null ? false : true; }, (x) => { DragMoveWindow(x); });
        }

        private void LoadedWindow(UserControl userControl)
        {
            FrameworkElement control = SystemMethods.GetWindowParent(userControl);

            if (control.GetType() == typeof(Window)) return;

            Window window = control as Window;

            if (!string.IsNullOrEmpty(window.Title)) WindowTitle = window.Title;

            if (window.Icon != null) WindowIcon = window.Icon;

            if (window.ResizeMode == ResizeMode.NoResize) WindowIsMaximize = Visibility.Collapsed;
        }

        private void MinimizeWindow(UserControl userControl)
        {
            FrameworkElement control = SystemMethods.GetWindowParent(userControl);

            if (control.GetType() == typeof(Window)) return;

            Window window = control as Window;

            window.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow(UserControl userControl)
        {
            FrameworkElement control = SystemMethods.GetWindowParent(userControl);

            if (control.GetType() == typeof(Window)) return;

            Window window = control as Window;

            if (window.WindowState != WindowState.Maximized) window.WindowState = WindowState.Maximized;
            else window.WindowState = WindowState.Normal;
        }

        private void CloseWindow(UserControl userControl)
        {
            FrameworkElement control = SystemMethods.GetWindowParent(userControl);

            if (control.GetType() == typeof(Window)) return;

            Window window = control as Window;

            window.Close();
        }

        private void DragMoveWindow(UserControl userControl)
        {
            FrameworkElement control = SystemMethods.GetWindowParent(userControl);

            if (control.GetType() == typeof(Window)) return;

            Window window = control as Window;

            window.DragMove();
        }
    }
}
