using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI.Model
{
    public class Category : ICategory
    {

        #region private fields
        private string _name;
        private int _iD;
        #endregion


        #region ctor
        public Category(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        #endregion


        #region public properties


        public string Name
        {
            get { return _name; }

            set { _name = value; }
        }

        public int ID
        {
            get { return _iD; }

            set { _iD = value; }
        }
        #endregion

        public override bool Equals(object obj)
        {

            return Name.Equals(((Category)obj).Name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode()^Name.GetHashCode()^ID.GetHashCode();
        }
    }
}
