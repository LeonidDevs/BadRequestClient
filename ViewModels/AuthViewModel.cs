using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using VittaClient.Tools;
using VittaClient.Views;
using VittaClient.Services;
using VittaClient.Models;

namespace VittaClient.ViewModels
{
    public class AuthViewModel : ViewModelBase
    {
        private readonly MainWindow _mainWindow;

        private readonly ApiService _apiService;
        private string _login;
        private UserDTO _user;
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));

            }
        }

        public ICommand AuthCommand { get; set; }

        public AuthViewModel(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            _apiService = new ApiService();
            AuthCommand = new RelayCommand(async (param) => await AuthExecute());
        }

        private async Task AuthExecute()
        {
            _user = await _apiService.AuthAndGetUserIdAsynс(Login);

            if (_user != null)
            {
                _mainWindow.ChangePage(new OrdersView(_apiService, _user));
            }
            else
            {
                MessageBox.Show("Пользователь не найден. Пожалуйста, проверьте введенный логин.");
            }
        }
    }
}
