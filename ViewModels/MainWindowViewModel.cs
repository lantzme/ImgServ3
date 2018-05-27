using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ImageService.UI.Models.Interfaces;
using ImageService.UI.ViewModels.Interfaces;
using Prism.Commands;

namespace ImageService.UI.ViewModels
{
    public class MainWindowViewModel : IMainWindowViewModel
    {
        #region Properties
        public ICommand CloseWindowCommand { get; set; }
        public bool VmIsConnected
        {
            get { return _mainWindowModel.IsConnected; }
            set
            {
                _mainWindowModel.IsConnected = value;
                OnPropertyChanged("VmIsConnected");
            }
        }

        #endregion

        #region Members
        private IMainWindowModel _mainWindowModel;

        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class
        /// </summary>
        public MainWindowViewModel()
        {
            _mainWindowModel = new Models.MainWindowModel();
            VmIsConnected = _mainWindowModel.IsConnected;
            CloseWindowCommand = new DelegateCommand<object>(CloseWindow, CanExecuteCloseCommand);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enable the command execution.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        private bool CanExecuteCloseCommand(object obj)
        {
            return true;
        }

        /// <summary>
        /// Send a closing command to the server.
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            _mainWindowModel.CloseServerConnection();
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}