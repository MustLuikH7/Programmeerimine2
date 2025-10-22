using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    public class InvoiceItem
    {
        [Key]
        public int ItemId { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }
    }
}
