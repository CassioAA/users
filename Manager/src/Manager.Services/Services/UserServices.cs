using AutoMapper;
using Manager.Core.Exceptions;
using Manager.Domain.Entities;
using Manager.Infra.Interfaces;
using Manager.Services.DTO;
using Manager.Services.Interfaces;

namespace Manager.Services.Services
{
    public class UserServices : IUserService {

        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserServices(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDTO> Create(UserDTO userDTO) {
 
            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if(userExists != null)
                throw new DomainExceptions("Já existe um usuário cadastrado com o email informado.");

            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userCreated = await _userRepository.Create(user);

            return _mapper.Map<UserDTO>(userCreated);
        }

        public async Task<UserDTO> Update(UserDTO userDTO) {

            var userExists = await _userRepository.GetByEmail(userDTO.Email);

            if(userExists == null)
                throw new DomainExceptions("Não existe um usuário cadastrado com o email informado.");

            // obtém a entidade a partir do DTO
            var user = _mapper.Map<User>(userDTO);
            user.Validate();

            var userUpdated = await _userRepository.Update(user);

            return _mapper.Map<UserDTO>(userUpdated);

        }

        public async Task Remove(long Id) {
            
            await _userRepository.Remove(Id);
        }

        public async Task<UserDTO> Get(long Id) {
            
            var user = await _userRepository.Get(Id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<List<UserDTO>> Get() {
            var allUsers = await _userRepository.Get();

            return _mapper.Map<List<UserDTO>>(allUsers);
        }

        public async Task<List<UserDTO>> SearchByName(string name) {
            
            var user = await _userRepository.SearchByName(name);

            return _mapper.Map<List<UserDTO>>(user);

        }

        public async Task<List<UserDTO>> SearchByEmail(string email) {

            var user = await _userRepository.SearchByEmail(email);

            return _mapper.Map<List<UserDTO>>(user);
            
        }

        public async Task<UserDTO> GetByEmail(string email) {

            var user = await _userRepository.GetByEmail(email);

            return _mapper.Map<UserDTO>(user);
            
        }
    }
}