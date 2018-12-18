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
        
        #region ctor
        public Category(int iD, string name)
        {
            ID = iD;
            Name = name;
        }

        #endregion


        #region public properties


        public string Name { get; set; }

        public int ID { get; set; }
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
