using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using ImageService.Infrastructure.Enums;
using ImageService.Infrastructure.Events;
using ImageService.UI.Models;
using ImageService.UI.Models.Interfaces;
using ImageService.UI.ViewModels.Interfaces;
using Prism.Commands;

namespace ImageService.UI.ViewModels
{
    public class SettingsViewModel : ISettingsViewModel
    {
        #region Properties
        public ICommand RemoveDirectoryCommand { get; set; }
        public ObservableCollection<string> VmHandlers
        {
            get
            {
                return _settingsModel.Handlers;
            }
            set
            {
                _settingsModel.Handlers = value;
                OnPropertyChanged("VmHandlers");
            }
        }
        public string SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                (RemoveDirectoryCommand as DelegateCommand<object>)?.RaiseCanExecuteChanged();
            }
        }
        public string VmOutputDirectory
        {
            get { return _settingsModel.OutputDirectory; }
            set
            {
                _settingsModel.OutputDirectory = value;
                OnPropertyChanged("VmOutputDirectory");
            }
        }

        public string VmSourceName
        {
            get { return _settingsModel.SourceName; }
            set
            {
                _settingsModel.SourceName = value;
                OnPropertyChanged("VmSourceName");
            }
        }

        public string VmLogName
        {
            get { return _settingsModel.LogName; }
            set
            {
                _settingsModel.LogName = value;
                OnPropertyChanged("VmLogName");
            }
        }
        public string VmThumbnailSize
        {
            get { return _settingsModel.ThumbnailSize; }
            set
            {
                _settingsModel.ThumbnailSize = value;
                OnPropertyChanged("VmThumbnailSize");
            }
        }

        #endregion

        #region Members
        private ISettingsModel _settingsModel;
        private string _selectedItem;
        #endregion

        #region C'tor
        /// <summary>
        /// The constructor of the class.
        /// </summary>
        public SettingsViewModel()
        {
            _settingsModel = new SettingsModel();
            RemoveDirectoryCommand = new DelegateCommand<object>(RemoveDirectoey, EnableRemoveExecution);

        }
        #endregion

        #region Methods

        /// <summary>
        /// Enable the remove command tol be executed.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        private bool EnableRemoveExecution(object obj)
        {
            return _selectedItem != null;
        }

        /// <summary>
        /// Removes the directory handler from the server.
        /// </summary>
        /// <param name="obj"></param>
        private void RemoveDirectoey(object obj)
        {
            _settingsModel.RemoveDirectory(SelectedItem);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}