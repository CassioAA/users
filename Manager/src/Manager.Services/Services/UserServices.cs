using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;

namespace Manager.Services.Services
{
    public class UserServices : IUserService {

        private readonly IRijndaelCryptography _rijndaelCriptography;

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserServices(
            IMapper mapper, 
            IUserRepository userRepository, 
            IRijndaelCryptography rijndaelCryptography)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _rijndaelCriptography = rijndaelCryptography;
        }

        public async Task<UserDTO> CreateAsync(UserDTO userDTO) {
 
            var userExists = await _userRepository.GetByEmailAsync(userDTO.Email);

            if(userExists != null)
                throw new DomainExceptions("Já existe um usuário cadastrado com o email informado.");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            user.ChangePassword(_rijndaelCriptography.Encrypt(user.Password));

            var userCreated = await _userRepository.CreateAsync(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<UserDTO> UpdateAsync(UserDTO userDTO) {

            var userExists = await _userRepository.GetAsync(userDTO.Id);

            if(userExists == null)
                throw new DomainExceptions("Não existe um usuário cadastrado com o email informado.");

            // obtém a entidade a partir do DTO
            var user = _mapper.Map<User>(userDTO);
            user.Validate();
            user.ChangePassword(_rijndaelCriptography.Encrypt(user.Password));

            var userUpdated = await _userRepository.UpdateAsync(user);
            
            return _mapper.Map<UserDTO>(userUpdated);

        }

        public async Task RemoveAsync(long Id) {
            
            await _userRepository.RemoveAsync(Id);
        }

        public async Task<UserDTO> GetAsync(long Id) {
            
            var user = await _userRepository.GetAsync(Id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetAllAsync() {
            var allUsers = await _userRepository.GetAllAsync();

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByNameAsync(string name) {
            
            var user = await _userRepository.SearchByNameAsync(name);

            return _mapper.Map<List<UserDTO>>(user);

        }

        public async Task<List<UserDTO>> SearchByEmailAsync(string email) {

            var user = await _userRepository.SearchByEmailAsync(email);

            return _mapper.Map<List<UserDTO>>(user);
            
        }

        public async Task<UserDTO> GetByEmailAsync(string email) {

            var user = await _userRepository.GetByEmailAsync(email);

            return _mapper.Map<UserDTO>(user);
            
        }
    }
}