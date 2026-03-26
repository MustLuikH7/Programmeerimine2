using System.Threading.Tasks;
using KooliProjekt.WindowsForms;

namespace KooliProjekt.WindowsForms.Api
{
    public interface IApiClient
    {
        Task<OperationResult<PagedResult<UserDto>>> List(int page, int pageSize);
        Task<OperationResult> Save(SaveUserCommand command);
        Task<OperationResult> Delete(int userId);
    }
}