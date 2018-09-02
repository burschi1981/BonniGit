using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonniModel.Interfaces
{
    public interface IHasCategories
    {
        IList<ICategory> Categories { get; set; }
    }
}
