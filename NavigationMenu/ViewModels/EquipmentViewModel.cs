using GalaSoft.MvvmLight.Command;
using NavigationMenu.Models;
using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows.Input;

namespace NavigationMenu.ViewModels
{
    internal class EquipmentViewModel :BaseViewModel
    {
        #region private fields

        NavigationContext _navigationContext;
        private ObservableCollection<EquipmentEntity> _equipment;
        private string _nameFilter = string.Empty;
        private int _totalEquipmentCount;
        private int _currentPage = 0;
        private const int _paginationCount = 10;

        #endregion

        #region Public Properties

        public ObservableCollection<EquipmentEntity> Equipment
        {
            get => _equipment;
            set => SetProperty(ref _equipment, value);
        }
        public string PageIndexString
        {
            get =>
                $"{_currentPage + 1} / {GetTotalPages()}";
        }

        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                SetProperty(ref _nameFilter, value);
                Load();
            }
        }
        public ICommand GoToFirstPageCommand { get; set; }
        public ICommand GoToLastPageCommand { get; set; }
        public ICommand GoToPreviousPageCommand { get; set; }
        public ICommand GoToNextPageCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        #endregion

        #region Ctor
        public EquipmentViewModel()
        {
            _navigationContext = new NavigationContext();
            Load();
            DeleteCommand = new RelayCommand<EquipmentEntity>((eq) => Delete(eq));

            GoToNextPageCommand = new DelegateCommand(() => GoToPage(_currentPage + 1));
            GoToPreviousPageCommand = new DelegateCommand(() => GoToPage(_currentPage - 1));
            GoToFirstPageCommand = new DelegateCommand(() => GoToPage(0));
            GoToLastPageCommand = new DelegateCommand(() => GoToPage(GetTotalPages() - 1));

        }
        #endregion

        #region Private Methods

        private void GoToPage(int page)
        {
            if (page < 0 || page > GetTotalPages() - 1)
                return;
            _currentPage = page;
            Load();

        }



        private void Delete(EquipmentEntity eq)
        {
            _navigationContext.EquipmentEntities.Remove(eq);
            _navigationContext.SaveChanges();
            Equipment.Remove(eq);
        }
        private void Load()
        {
            _navigationContext.EquipmentEntities.Load();
            var filteredEquipment = _navigationContext.EquipmentEntities.Local
                .Where(eq => eq.Name.Contains(NameFilter));

            _totalEquipmentCount = filteredEquipment.Count();
            filteredEquipment =
                filteredEquipment
                .Skip(_currentPage * _paginationCount)
                .Take(_paginationCount);

            Equipment = new ObservableCollection<EquipmentEntity>(filteredEquipment);
            OnPropertyChanged("PageIndexString");
        }


        private int GetTotalPages() =>
            (_totalEquipmentCount / _paginationCount)
            + ((_totalEquipmentCount % _paginationCount) != 0 ? 1 : 0);

        #endregion
    }
}
