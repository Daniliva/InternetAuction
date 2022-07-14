using InternetAuction.BLL.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using System.Linq;
using Moq;
using InternetAuction.BLL.DTO;
using InternetAuction.DAL.Entities.MSSQL;
using InternetAuction.DAL.Contract;

namespace Imternet.Tests.BusinessTests
{
    public class UserServiceTests
    {
        [Test]
        public async Task UserService_GetAll_ReturnsAllUsers()
        {
            //arrange
            var expected = GetTestUserModels;
            var mockUnitOfWork = new Mock<IUnitOfWorkMSSQL>();

            mockUnitOfWork
                .Setup(x => x.UserRepository.GetAllAsync())
                .ReturnsAsync(GetTestUserEntities.AsEnumerable());

            var userService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await userService.GetAllAsync();

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UserService_GetById_ReturnsUserModel()
        {
            //arrange
            var expected = GetTestUserModels.First();
            var mockUnitOfWork = new Mock<IUnitOfWorkMSSQL>();

            mockUnitOfWork
                .Setup(m => m.UserRepository.GetByIdWithIncludeAsync(It.IsAny<string>()))
                .ReturnsAsync(GetTestUserEntities.First());

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await UserService.GetByIdAsync("1");

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        #region TestData

        public List<UserModel> GetTestUserModels =>
            new List<UserModel>()
            {
                // //   new UserModel { Id = "1", UserName = "Viktor", Email = "Zhuk",  PasswordHash = "1111",AvatarCurrent=new byte[0],UserRoles =new List<RoleUserModel>()},
                //    new UserModel { Id = "2", UserName = "Nassim", Email = "Taleb",  PasswordHash = "1111",AvatarCurrent=new byte[0],UserRoles =new List<RoleUserModel>()},
                //   new UserModel { Id = "3", UserName = "Desmond", Email = "Morris", PasswordHash = "1111",AvatarCurrent=new byte[0],UserRoles=new List<RoleUserModel>() },
                //   new UserModel { Id ="4", UserName = "Lebron", Email = "James", PasswordHash = "1111",AvatarCurrent=new byte[0],UserRoles=new List<RoleUserModel>() }
            };

        public List<User> GetTestPersonEntities =>
           new List<User>()
           {  new User { Id = "1", UserName = "Viktor", Email = "Zhuk",  PasswordHash = "1111",
            },
                new User { Id = "2", UserName = "Nassim", Email = "Taleb",PasswordHash = "1111"},
                new User { Id = "3", UserName = "Desmond", Email = "Morris", PasswordHash = "1111" },
                new User { Id ="4", UserName = "Lebron", Email = "James", PasswordHash = "1111" }
           };

        public List<User> GetTestUserEntities =>
            new List<User>()
            {
               new User { Id = "1", UserName = "Viktor", Email = "Zhuk",  PasswordHash = "1111" },
                new User { Id = "2", UserName = "Nassim", Email = "Taleb",PasswordHash = "1111"},
                new User { Id = "3", UserName = "Desmond", Email = "Morris", PasswordHash = "1111" },
                new User { Id ="4", UserName = "Lebron", Email = "James", PasswordHash = "1111" }
            };

        #endregion TestData
    }

    /*
        [Test]
        public async Task UserService_GetById_ReturnsUserModel()
        {
            //arrange
            var expected = GetTestUserModels.First();
            var mockUnitOfWork = new Mock<IUnitOfWork>();

            mockUnitOfWork
                .Setup(m => m.UserRepository.GetByIdWithDetailsAsync(It.IsAny<int>()))
                .ReturnsAsync(GetTestUserEntities.First());

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            var actual = await UserService.GetByIdAsync(1);

            //assert
            actual.Should().BeEquivalentTo(expected);
        }

        [Test]
        public async Task UserService_AddAsync_AddsModel()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.AddAsync(It.IsAny<User>()));

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var User = GetTestUserModels.First();

            //act
            await UserService.AddAsync(User);

            //assert
            mockUnitOfWork.Verify(x => x.UserRepository.AddAsync(It.Is<User>(x =>
                            x.Id == User.Id && x.DiscountValue == User.DiscountValue &&
                            x.Person.Surname == User.Surname && x.Person.Name == User.Name &&
                            x.Person.BirthDate == User.BirthDate)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UserService_AddAsync_ThrowsMarketExceptionWithEmptyName()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.AddAsync(It.IsAny<User>()));

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var User = GetTestUserModels.First();
            User.Name = string.Empty;

            //act
            Func<Task> act = async () => await UserService.AddAsync(User);

            //assert
            await act.Should().ThrowAsync<MarketException>();
        }

        [Test]
        public async Task UserService_AddAsync_ThrowsMarketExceptionWithNullObject()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.AddAsync(It.IsAny<User>()));

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            Func<Task> act = async () => await UserService.AddAsync(null);

            //assert
            await act.Should().ThrowAsync<MarketException>();
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(100)]
        public async Task UserService_DeleteAsync_DeletesUser(int id)
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.DeleteByIdAsync(It.IsAny<int>()));
            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());

            //act
            await UserService.DeleteAsync(id);

            //assert
            mockUnitOfWork.Verify(x => x.UserRepository.DeleteByIdAsync(id), Times.Once());
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once());
        }

        [Test]
        public async Task UserService_UpdateAsync_UpdatesUser()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.Update(It.IsAny<User>()));
            mockUnitOfWork.Setup(m => m.PersonRepository.Update(It.IsAny<Person>()));

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var User = GetTestUserModels.First();

            //act
            await UserService.UpdateAsync(User);

            //assert
            mockUnitOfWork.Verify(x => x.UserRepository.Update(It.Is<User>(x =>
                x.Id == User.Id && x.DiscountValue == User.DiscountValue &&
                x.Person.Surname == User.Surname && x.Person.Name == User.Name &&
                x.Person.BirthDate == User.BirthDate)), Times.Once);
            mockUnitOfWork.Verify(x => x.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UserService_UpdateAsync_ThrowsMarketExceptionWithEmptySurname()
        {
            //arrange
            var mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork.Setup(m => m.UserRepository.Update(It.IsAny<User>()));

            var UserService = new UserService(mockUnitOfWork.Object, UnitTestHelper.CreateMapperProfile());
            var User = GetTestUserModels.Last();
            User.Surname = null;

            //act
            Func<Task> act = async () => await UserService.UpdateAsync(User);

            //assert
            await act.Should().ThrowAsync<MarketException>();
        }
    */
}