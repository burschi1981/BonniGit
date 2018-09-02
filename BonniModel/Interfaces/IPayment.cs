using BonniModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BonnyUI
{
    public interface IPayment : IHasCategories
    {

        double Amount { get; set; }

        string Details { get; set; }

        string PaymentType { get; set; }

        int? ID { get; set; }
    }
}
