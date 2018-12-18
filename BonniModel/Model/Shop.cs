using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI.Model
{
    public class Shop
    {   
        public Shop(int iD, string name)
        {
            ID = iD;
            Name = name;
        }
        
        public string Name { get; set; }
        
        public int ID { get; set; }
    }
}
