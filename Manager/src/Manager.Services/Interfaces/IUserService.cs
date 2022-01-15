using Manager.Services.DTO;

namespace Manager.Services.Interfaces
{
    public interface IUserService{
        Task<UserDTO> CreateAsync(UserDTO userDTO);
        Task<UserDTO> UpdateAsync(UserDTO userDTO);
        Task RemoveAsync(long Id);
        Task<UserDTO> GetAsync(long id);
        Task<List<UserDTO>> GetAllAsync();
        Task<List<UserDTO>> SearchByNameAsync(string name);
        Task<List<UserDTO>> SearchByEmailAsync(string email);
        Task<UserDTO> GetByEmailAsync(string email);
    }
}