﻿using System.Collections.Generic;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Users
{
    public class AppointmentsQuery : IRequest<OperationResult<IList<Appointment>>>
    {
    }
}