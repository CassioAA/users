using Manager.Infra.Interfaces;
using Manager.Services.Interfaces;
using Manager.Services.Services;
using Manager.Services.DTO;
using Manager.Domain.Entities;
using Manager.Tests.Configurations;
using Manager.Tests.Fixtures;
using Manager.Core.Exceptions;
using Moq;
using AutoMapper;
using EscNet.Cryptography.Interfaces;
using Xunit;
using Bogus.DataSets;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using FluentAssertions;
using Bogus;

namespace Manager.Tests.Projects.Services
{
    public class UserServicesTests
    {
        // Subject Under Tests
        private readonly IUserService _sut;

        private readonly IMapper _mapper;

        // Mocks (simulations)
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRijndaelCryptography> _rijndaelCryptographyMock;

        public UserServicesTests()
        {
            _mapper = AutoMapperConfiguration.GetConfiguration();
            _userRepositoryMock = new Mock<IUserRepository>();
            _rijndaelCryptographyMock = new Mock<IRijndaelCryptography>();

            _sut = new UserServices(
                mapper: _mapper,
                userRepository: _userRepositoryMock.Object,
                rijndaelCryptography: _rijndaelCryptographyMock.Object
            );
        }

        #region Create

        // displayName - test identification on output
        [Fact(DisplayName = "Create Valid User")]
        [Trait("Category", "Services")]
        // MethodName_Condition_ExpectedResult
        public async Task Create_WhenUserIsValid_ReturnsUserDTO()
        {
            // Arrange - preparation
            var userToCreate = UserFixture.CreateValidUserDTO();

            // Bogus generates fake data. In this case, a set of words
            var encryptedPassword = new Lorem().Sentence();

            var userCreated = _mapper.Map<User>(userToCreate);
            userCreated.ChangePassword(encryptedPassword);

            // simulations setup
            // IsAny - allows the use of any string
            _userRepositoryMock.Setup( x => x.GetByEmailAsync(It.IsAny<string>()))
            // the logic of the method to be tested throws an exception if the return is not null
                .ReturnsAsync(() => null);

            _rijndaelCryptographyMock.Setup( x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup( x => x.CreateAsync(It.IsAny<User>()))
                .ReturnsAsync(() => userCreated);

            // Act - execution
            var result = await _sut.CreateAsync(userToCreate);

            // Assert - comparison
            // equivalent to Assert.Equal, but the intention of the comparison becomes clearer
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userCreated));
        }


        [Fact(DisplayName = "Create When User Exists")]
        [Trait("Category", "Services")]
        public void Create_WhenUserExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userToCreate = UserFixture.CreateValidUserDTO();
            var userExists = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
             .ReturnsAsync(() => userExists);

            // Act
            // Delegate
            Func<Task<UserDTO>> act = async () => {
                return await _sut.CreateAsync(userToCreate);
            };

            // Act
            act.Should().ThrowAsync<DomainExceptions>()
            .WithMessage("Já existe um usuário cadastrado com o email informado.");
        }

        [Fact(DisplayName = "Create When User is Invalid")]
        [Trait("Category", "Services")]
        public void Create_WhenUserIsInvalid_ReturnsEmptyOptional()
        {
            // Arrange
            var userToCreate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(() => null);

            // Act
            Func<Task<UserDTO>> act = async () => {
                return await _sut.CreateAsync(userToCreate);
            };

            // Act
            act.Should()
                .ThrowAsync<DomainExceptions>();
        }

        #endregion

        #region Update

        [Fact(DisplayName = "Update Valid User")]
        [Trait("Category", "Services")]
        public async Task Update_WhenUserIsValid_ReturnsUserDTO()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateValidUserDTO();
            var userUpdated = _mapper.Map<User>(userToUpdate);

            var encryptedPassword = new Lorem().Sentence();

            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            _rijndaelCryptographyMock.Setup(x => x.Encrypt(It.IsAny<string>()))
                .Returns(encryptedPassword);

            _userRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync(() => userUpdated);

            // Act
            var result = await _sut.UpdateAsync(userToUpdate);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userUpdated));
        }

        [Fact(DisplayName = "Update When User Not Exists")]
        [Trait("Category", "Services")]
        public void Update_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userToUpdate = UserFixture.CreateValidUserDTO();

            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            // Act
            Func<Task<UserDTO>> act = async () => {
                return await _sut.CreateAsync(userToUpdate);
            };

            // Act
            act.Should().ThrowAsync<DomainExceptions>()
                .WithMessage("Não existe um usuário com o Id informado.");
        }

        [Fact(DisplayName = "Update When User is Invalid")]
        [Trait("Category", "Services")]
        public void Update_WhenUserIsInvalid_ReturnsEmptyOptional()
        {
            // Arrange
            var oldUser = UserFixture.CreateValidUser();
            var userToUpdate = UserFixture.CreateInvalidUserDTO();

            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(() => oldUser);

            // Act
            Func<Task<UserDTO>> act = async () => {
                return await _sut.CreateAsync(userToUpdate);
            };

            // Act
            act.Should().ThrowAsync<DomainExceptions>()
                .WithMessage("Não existe um usuário com o Id informado.");
        }

        #endregion

        #region Remove

        [Fact(DisplayName = "Remove User")]
        [Trait("Category", "Services")]
        public async Task Remove_WhenUserExists_RemoveUser()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.RemoveAsync(It.IsAny<int>()))
                // checks the invoke of the remove method with the random Id
                .Verifiable();

            // Act
            await _sut.RemoveAsync(userId);

            // Assert
            // checks if the remove method was invoked at least once
            _userRepositoryMock.Verify(x => x.RemoveAsync(userId), Times.Once);
        }

        #endregion

        #region Get

        [Fact(DisplayName = "Get By Id")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetAsync(userId))
                .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.GetAsync(userId);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Id When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetById_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userId = new Randomizer().Int(0, 1000);

            _userRepositoryMock.Setup(x => x.GetAsync(userId))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAsync(userId);

            // Assert
            result.Should()
                .Be(null);
        }

        [Fact(DisplayName = "Get By Email")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserExists_ReturnsUserDTO()
        {
            // Arrange
            var userEmail = new Internet().Email();
            var userFound = UserFixture.CreateValidUser();

            _userRepositoryMock.Setup(x => x.GetByEmailAsync(userEmail))
                .ReturnsAsync(() => userFound);

            // Act
            var result = await _sut.GetByEmailAsync(userEmail);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<UserDTO>(userFound));
        }

        [Fact(DisplayName = "Get By Email When User Not Exists")]
        [Trait("Category", "Services")]
        public async Task GetByEmail_WhenUserNotExists_ReturnsEmptyOptional()
        {
            // Arrange
            var userEmail = new Internet().Email();

            _userRepositoryMock.Setup(x => x.GetAsync(It.IsAny<long>()))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetByEmailAsync(userEmail);

            // Assert
            result.Should()
                .Be(null);
        }

        [Fact(DisplayName = "Get All Users")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenUsersExists_ReturnsAListOfUserDTO()
        {
            // Arrange
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Get All Users When None User Found")]
        [Trait("Category", "Services")]
        public async Task GetAllUsers_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange

            _userRepositoryMock.Setup(x => x.GetAllAsync())
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.GetAllAsync();

            // Assert
            result.Should()
                .BeEmpty();
        }

        #endregion

        #region Search

        [Fact(DisplayName = "Search By Name")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByNameAsync(nameToSearch))
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByNameAsync(nameToSearch);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Name When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByName_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var nameToSearch = new Name().FirstName();

            _userRepositoryMock.Setup(x => x.SearchByNameAsync(nameToSearch))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByNameAsync(nameToSearch);

            // Assert
            result.Should()
                .BeEmpty();
        }

        [Fact(DisplayName = "Search By Email")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenAnyUserFound_ReturnsAListOfUserDTO()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();
            var usersFound = UserFixture.CreateListValidUser();

            _userRepositoryMock.Setup(x => x.SearchByEmailAsync(emailSoSearch))
                .ReturnsAsync(() => usersFound);

            // Act
            var result = await _sut.SearchByEmailAsync(emailSoSearch);

            // Assert
            result.Should()
                .BeEquivalentTo(_mapper.Map<List<UserDTO>>(usersFound));
        }

        [Fact(DisplayName = "Search By Email When None User Found")]
        [Trait("Category", "Services")]
        public async Task SearchByEmail_WhenNoneUserFound_ReturnsEmptyList()
        {
            // Arrange
            var emailSoSearch = new Internet().Email();

            _userRepositoryMock.Setup(x => x.SearchByEmailAsync(emailSoSearch))
                .ReturnsAsync(() => null);

            // Act
            var result = await _sut.SearchByEmailAsync(emailSoSearch);

            // Assert
            result.Should()
                .BeEmpty();
        }

        #endregion
    }


}