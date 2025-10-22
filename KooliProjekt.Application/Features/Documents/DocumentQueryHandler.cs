﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Features.Users
{
    public class DocumentQueryHandler : IRequestHandler<DocumentsQuery, OperationResult<IList<Document>>>
    {
        private readonly ApplicationDbContext _dbContext;
        public DocumentQueryHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult<IList<Document>>> Handle(DocumentsQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<IList<Document>>();
            result.Value = await _dbContext
                .Documents
                .OrderBy(list => list.DocumentId)
                .ToListAsync();

            return result;
        }
    }
}
