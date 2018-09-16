//using BonnyUI.ViewModel;
using System.Windows.Input;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
//using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using BonnyUI.DBConnection;
using BonnyUI.Model;
using BonniDB;

namespace BonnyUI.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        // Viewmodel zur Verwaltung der Shops
        private ShopAdminViewModel _shopAdmin;

        // Viewmodel zur Verwaltung der Kategorien
        private CategoryAdminViewModel _categoryAdmin;


        // Viewmodel zur Verwaltung der Bons
        private BonAdminViewModel _bonAdmin;

        // Das Datenprojekt selbst. Enthält Shops und Bons
        private Project _project;

        // Die Verbindung zur DB
        private IDBConnector _DBConnection;

        #endregion
        
        #region Konstruktoren
        /// <summary>
        /// Wird bei Start verwendet. Ruft einfach nur den zweiten Konstruktor auf
        /// </summary>
        public MainViewModel()
        {
            // Elemente aus dB laden
            _DBConnection = new DBConnector();
            
            LoadProject();

            // Admin für Kategorien laden und mit Kategorien befüllen
            _categoryAdmin = new CategoryAdminViewModel(_project.Categories, _DBConnection);
            _categoryAdmin.PropertyChanged -= ShopsChanged;
            _categoryAdmin.PropertyChanged += ShopsChanged;

            // Admin für Shop befüllen und mit Geschäften befüllen
            _shopAdmin = new ShopAdminViewModel(_project.Shops, _DBConnection);
            _shopAdmin.PropertyChanged -= ShopsChanged;
            _shopAdmin.PropertyChanged += ShopsChanged;

            // BonAdmin laden, enthält ShopAdmin und Projekt
            _bonAdmin = new BonAdminViewModel(_project, _shopAdmin, _DBConnection);
        }
        #endregion



        #region public properties and Commands

        public ShopAdminViewModel ShopAdmin
        {
            get { return _shopAdmin; }
        }

        public CategoryAdminViewModel CategoryAdmin
        {
            get { return _categoryAdmin; }
        }


        public BonAdminViewModel BonAdmin
        {
            get { return _bonAdmin; }
        }
        #endregion


        #region public voids
        // Commands:
        // neues Geschäft
        public void CancelViewClosing()
        {
            // Hier Abfrage, ob Änderungen und Ähnliches gespeichert werden sollen, siehe Originalprojekt
        }

        #endregion

        #region private voids and delegates

        /// <summary>
        /// Delegat für den ShopAdmin.
        /// Wird alarmiert, wenn sich in den ShopDates etwas verändert hat und informiert die Bons darüber, dass sie möglicherweise die Shop-Daten neu laden müssen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShopsChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Shops") || e.PropertyName.Equals("Categories"))
                BonAdmin.ReloadShopsInBons();
        }
        
        

        // Erstellt ein neues Projekt zur Datenhaltung und befüllt Shops und Bons
        private void LoadProject()
        {   
            _project = new Project();


            // TODO: Laden der Daten in eigene Factory ausladen
            
            // Laden der Geschäfte
            _project.Shops = _DBConnection.GetAllShops();

            // Laden der Kategorien
            _project.Categories = _DBConnection.GetAllCategories();

            // Laden der Bons
            _project.Bons = _DBConnection.GetAllBons();
        }
        #endregion
    }
}