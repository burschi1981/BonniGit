using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI.ViewModel
{
    public class ShopViewModel : ViewModelBase
    {
        private string _name = "Penny";
        private IShop _wrappedObject;
        private int _iD;


        public ShopViewModel(IShop shop)
        {
            this._wrappedObject = shop;
            this.Name = _wrappedObject.Name;
            this.ID = WrappedObject.ID;
        }



        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public IShop WrappedObject
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
