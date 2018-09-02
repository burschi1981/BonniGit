using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI.ViewModel
{
    public class CategoryViewModel : ViewModelBase
    {
        private string _name = "neue Kategorie";
        private ICategory _wrappedObject;
        private int _iD;
        private bool _isSelected;


        public CategoryViewModel(ICategory category, bool isSelected)
        {
            this._wrappedObject = category;
            this.Name = _wrappedObject.Name;
            this.ID = WrappedObject.ID;
            _isSelected = isSelected;
        }


        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }


        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public ICategory WrappedObject
        {
            get { return _wrappedObject; }
            set { _wrappedObject = value; }
        }



        public int ID
        {
            get { return _iD; }
            set { _iD = value; }
        }


    }
}
