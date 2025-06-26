using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using MBLite.Services;

namespace MBLite.ViewModels.Connection
{
    public class ConnectionViewModel : ViewModelBase
    {
        private bool _status = false;
        private string _port = "COM1";
        private ObservableCollection<string> _comPorts = new();
        private string _baudrate = "19200";
        private int _id = 1;
        private string _selectedPort = string.Empty;

        public bool Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }
        public string Port
        {
            get => _port;
            set => SetProperty(ref _port, value);
        }
        public ObservableCollection<string> ComPorts
        {
            get => _comPorts;
            set => SetProperty(ref _comPorts, value);
        }

        public string Baudrate
        {
            get => _baudrate;
            set => SetProperty(ref _baudrate, value);
        }
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string SelectedPort
        {
            get => _selectedPort;
            set => SetProperty(ref _selectedPort, value);
        }

        public ConnectionViewModel()
        {

        }


        //private async Task TestConnectionActionAsync()
        //{
        //    ConnectionStatus = !ConnectionStatus;
        //    OpenFileCommand.NotifyCanExecuteChanged();

        //    Connection.Id++;
        //    Connection = Connection;
        //}


    }
}
