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
    internal class ActivitiesViewModel:BaseViewModel
    {
        #region private fields

        NavigationContext _navigationContext;
        private ObservableCollection<Activity> _acts;
        private string _nameFilter = string.Empty;
        private int _totalActivitiesCount;
        private int _currentPage = 0;
        private const int _paginationCount = 10;

        #endregion

        #region Public Properties

        public ObservableCollection<Activity> Activities
        {
            get => _acts;
            set => SetProperty(ref _acts, value);
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
        public ICommand CreateActivityCommand { get; set; }

        #endregion

        #region Ctor
        public ActivitiesViewModel()
        {
            _navigationContext = new NavigationContext();
            Load();
            DeleteCommand = new RelayCommand<Activity>((act) => Delete(act));
            CreateActivityCommand = new RelayCommand<string>((name) => CreatNewActivity(name));

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

        private void CreatNewActivity(string name)
        {
            if (String.IsNullOrEmpty(name))
                return;
            _navigationContext.Activities.Add(
                new Activity()
                {
                    Name = name,
                    CreatedOn = DateTime.Now.ToString(),
                    CreatedBy = "Guest",
                    LastModifiedOn = DateTime.Now.ToString()
                });
            _navigationContext.SaveChanges();
            Load();
        }

        private void Delete(Activity activity)
        {
            _navigationContext.Activities.Remove(activity);
            _navigationContext.SaveChanges();
            Activities.Remove(activity);
        }
        private void Load()
        {
            _navigationContext.Activities.Load();
            var filteredActivities = _navigationContext.Activities.Local
                .Where(act => act.Name.Contains(NameFilter));

            _totalActivitiesCount = filteredActivities.Count();
            filteredActivities =
                filteredActivities
                .Skip(_currentPage * _paginationCount)
                .Take(_paginationCount);

            Activities = new ObservableCollection<Activity>(filteredActivities);
            OnPropertyChanged("PageIndexString");
        }


        private int GetTotalPages() =>
            (_totalActivitiesCount / _paginationCount)
            + ((_totalActivitiesCount % _paginationCount) != 0 ? 1 : 0);

        #endregion



    }
}
