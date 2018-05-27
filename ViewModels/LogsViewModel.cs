using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ImageService.Logging;
using ImageService.UI.Models;
using ImageService.UI.Models.Interfaces;
using ImageService.UI.ViewModels.Interfaces;

namespace ImageService.UI.ViewModels
{
    public class LogsViewModel : ILogsViewModel
    {
        #region Properties
        public ObservableCollection<LogMessage> VmLogArchive
        {
            get { return _logs; }
            set
            {
                _logs = value;
                OnPropertyChanged("VmLogArchive");
            }
        }
        #endregion

        #region Members
        private ObservableCollection<LogMessage> _logs;
        private ILogsModel _logsModel;
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public LogsViewModel()
        {
            _logsModel = new LogsModel();
            VmLogArchive = _logsModel.LogMessegesArchive;
        }
        #endregion

        #region Methods
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}