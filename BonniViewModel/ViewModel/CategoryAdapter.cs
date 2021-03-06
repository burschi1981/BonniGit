﻿using BonnyUI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BonniModel.Interfaces;
using System.Collections.ObjectModel;
using BonnyUI;
using System.Windows.Input;
using BonnyUI.DBConnection;

namespace BonniViewModel.ViewModel
{
    /// <summary>
    /// KategorieAdapter... liefert Kategorien für Bons und Zahlungen
    /// </summary>
    public class CategoryAdapter : ViewModelBase
    {
        private IList<ICategory> _categories;

        private ObservableCollection<CategoryViewModel> _allCategories;

        private DBConnector _dbConnection;
        

        public CategoryAdapter(IList<ICategory> categories, DBConnector dbConnection)
        {
            _dbConnection = dbConnection;
            _categories = categories;
            LoadAllCategories();
        }

        public void ReloadCategories()
        {
            LoadAllCategories();
        }
        
        

        public ObservableCollection<CategoryViewModel> AllCategories
        {
            get
            {
                return _allCategories;
            }
        }

        

        private void LoadAllCategories()
        {
            _allCategories = new ObservableCollection<CategoryViewModel>();
            IList<ICategory> x = _dbConnection.GetAllCategories();
            foreach(ICategory cat in x)
            {
                // TODO: hier anpassen, evtl. Equals überschreiben
                CategoryViewModel zvm = new CategoryViewModel(cat, _categories.Contains(cat));
                _allCategories.Add(zvm);
            }
            RaisePropertyChanged("AllCategories");
        }
    }
}
