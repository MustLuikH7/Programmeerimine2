using System;

namespace KooliProjekt.Application.Dto
{
    public class InvoiceItemDetailsDto
    {
        public int ItemId { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public int Amount { get; set; }

        public InvoiceDto Invoice { get; set; }
    }
}
