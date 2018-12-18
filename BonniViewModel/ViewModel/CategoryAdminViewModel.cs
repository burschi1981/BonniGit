using BonniModel.Interfaces;
using BonnyUI.DBConnection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BonnyUI.ViewModel
{
    /// <summary>
    /// Viewmodel der Category-Admin
    /// Hinzufügen und Ändern von Kategorien
    /// </summary>
    public class CategoryAdminViewModel:ViewModelBase
    {
        #region private fields
        private int _currentID;

        private ObservableCollection<CategoryViewModel> _allCategories;
        
        private string _currentCategoryName;

        private DBConnector _DBConnection;

        private IList<ICategory> _categories;

        private CategoryViewModel _currentCategory;
        #endregion

        #region Konstruktoren
        public CategoryAdminViewModel(IList<ICategory> categorys, DBConnector dbConnection)
        {
             _categories = categorys;
            SaveCategoryDatesAsNewCommand = new ActionCommand(SaveCategoryDatesAsNew, CanUpdateCategoryDates);
            UpdateCategoryDatesCommand = new ActionCommand(UpdateCategoryDates, CanUpdateCategoryDates);
            _DBConnection = dbConnection;
            ReloadCategoryViewModels();
        }
        #endregion
        
        #region public Properties

        public ICommand SaveCategoryDatesAsNewCommand { get; private set; }
        public ICommand UpdateCategoryDatesCommand { get; private set; }





        public string CurrentCategoryName
        {
            get
            {
                return _currentCategoryName;
            }
            set
            {
                _currentCategoryName = value;
            }
        }

        public IList<ICategory> Categories
        {
            get { return _categories; }
            set
            {
                _categories = value;
            }
        }

        public ObservableCollection<CategoryViewModel> AllCategories
        {
            get { return _allCategories; }
        }

        public CategoryViewModel CurrentCategory
        {
            get { return _currentCategory; }
            set
            {
                _currentCategory = value;
                if (_currentCategory != null)
                {
                    _currentCategoryName = _currentCategory.Name;
                    _currentID = _currentCategory.ID;
                }
                else
                {                 
                    _currentCategoryName = "";
                }
                RaisePropertyChanged("CurrentCategoryName");
                RaisePropertyChanged("CurrentCategory");
            }
        }


        #endregion
        
        #region public voids

        public void ReloadCategoryViewModels()
        {
            // Geschäfte laden:
            ObservableCollection<CategoryViewModel> obs = new ObservableCollection<CategoryViewModel>();
            foreach (ICategory category in _categories)
                obs.Add(new CategoryViewModel(category, false));
            _allCategories = obs;

            CurrentCategory = GetCurrentCategory();
            if (CurrentCategory == null && AllCategories.Count > 0)
                CurrentCategory = AllCategories[0];

            RaisePropertyChanged("CurrentCategory");
            RaisePropertyChanged("AllCategories");
        }

        #endregion

        #region private voids


        private CategoryViewModel GetCurrentCategory()
        {
            var retval = _allCategories.Where(x => x.ID.Equals(_currentID)).FirstOrDefault();
            return retval;
        }

        private void UpdateCategoryDates(object obj)
        {
            try
            {
                _currentCategory.WrappedObject.Name = _currentCategoryName;
                
                _currentCategory.Name = _currentCategoryName;
                
                _DBConnection.ChangeCategory(CurrentCategory.WrappedObject);
                
                ReloadCategoryViewModels();
                
                RaisePropertyChanged("Categories");
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Ändern der Daten des Geschäfts", ex);
            }
        }

        private void SaveCategoryDatesAsNew(object obj)
        {
            try
            {
                _currentID = _DBConnection.AddCategory(CurrentCategoryName);
                Categories = _DBConnection.GetAllCategories();
                ReloadCategoryViewModels();
                RaisePropertyChanged("Categories");
            }
            catch (Exception ex)
            {
                throw new Exception("Fehler beim Anlegen des neuen Geschäfts", ex);
            }
        }



        private bool CanUpdateCategoryDates(object obj)
        {
            bool retval = true;
            if (_currentCategoryName == null || _currentCategoryName == "" || CategoryAlreadyExists())
                retval = false;
            return retval;
        }

        






        private bool CategoryAlreadyExists()
        {
            bool retval = false;
            if (_allCategories != null)
            {   
                if (_allCategories.Where(x => x.Name.Equals(CurrentCategoryName)).FirstOrDefault() != null)
                    retval = true;
            }
            return retval;
        }

        #endregion

        
    }
}
