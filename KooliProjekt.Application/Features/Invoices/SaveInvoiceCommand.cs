using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommand : IRequest<OperationResult>
    {
        public int InvoiceId { get; set; }

        public int AppointmentId { get; set; }

        public int DoctorId { get; set; }

        public int UserId { get; set; }

        public DateTime IssuedAt { get; set; }

      
        public bool IsPaid { get; set; }
    }
}