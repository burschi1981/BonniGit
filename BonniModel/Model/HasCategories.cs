using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonniModel.Model
{
    public abstract class HasCategories
    {
        public HasCategories()
        {
            _categories = new List<ICategory>();
        }
            protected IList<ICategory> _categories;

        public IList<ICategory> Categories
        {
            get
            {
                return _categories;
            }

            set
            {
                _categories = value;
            }
        }
    }
    
}
