using System.Threading.Tasks;
using Manager.Domain.Entities;
using System.Collections.Generic;

namespace Manager.Infra.Interfaces{

    public interface IUserRepository : IBaseRepository<User> {

        Task<User>  GetByEmailAsync(string email);

        Task<List<User>>  SearchByEmailAsync(string email);

        Task<List<User>>  SearchByNameAsync(string name);

    }
}