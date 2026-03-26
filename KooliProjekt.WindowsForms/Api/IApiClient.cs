using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<UserDto>>> List(int page, int pageSize);
        Task<OperationResult> Save(SaveUserCommand command);
        Task<OperationResult> Delete(int userId);
    }
}