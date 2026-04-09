using System;
using System.Collections.Generic;
using System.Text;

namespace KooliProjekt.WpfApplication
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<Users>>> List(int page, int pageSize);
        Task<OperationResult> Save(Users list);
        Task<OperationResult> Delete(int id);
    }
}