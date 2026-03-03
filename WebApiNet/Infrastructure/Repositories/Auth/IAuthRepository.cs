using WebApiNet.Core.Entities;
using WebApiNet.Infrastructure.Repositories.Base;
using WebApiNet.Presentation.Paged;

namespace WebApiNet.Infrastructure.Repositories.Auth
{
    public interface IAuthRepository : IRepository<Cliente, string>
    {
        Task<PagedResult<Cliente>> GetPagedAsync(int pageNumber, int pageSize);
        Task<Cliente> GetByEmailAsync(string email);

    }
}
