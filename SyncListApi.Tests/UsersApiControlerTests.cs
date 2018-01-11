using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SyncList.CommonLibrary.Exceptions;
using SyncList.SyncListApi.Controllers;
using SyncList.SyncListApi.Data.Repositories.Interfaces;
using SyncList.SyncListApi.Models;
using Xunit;

namespace SyncList.SyncListApi.Tests
{
    public class UsersApiControlerTests
    {
        private UsersApiControler _controller;
        private Mock<IUsersRepository> _usersRepository;
        
        private static List<User> users = new List<User>()
        {
            new User() {Email = "test@test.test", Name = "test", Id = 1},
            new User() {Email = "test1@test.test", Name = "test1", Id = 2},
            new User() {Email = "test2@test.test", Name = "test2", Id = 3}
        };
        
        public UsersApiControlerTests()
        {
            _usersRepository = new Mock<IUsersRepository>();
            
            _controller = new UsersApiControler(_usersRepository.Object);
            _usersRepository.Setup(ur => ur.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(users);
            
            _usersRepository.Setup(ur => ur.Get(It.IsAny<int>()))
                .ReturnsAsync((int i) => users.FirstOrDefault(u => u.Id == i));

        }
        
        [Fact]
        public async Task UsersApiControler_GetAllUsers_Success()
        {
            var result = await _controller.GetAllUsers();
            
            Assert.True(result is OkObjectResult);
            var usersResult = (result as OkObjectResult).Value as List<User>;
            Assert.True(usersResult?.Count == users.Count);
            
            var sameList = true;

            for (int i = 0; i < users.Count; ++i)
            {
                sameList &= usersResult[i] == users[i];
            }

            Assert.True(sameList);
        }
        
        [Fact]
        public async Task UsersApiControler_GetAllUsers_Failed_Negative()
        {
            await Assert.ThrowsAsync<InputDataValidationException>(async() =>await _controller.GetAllUsers(-1, Int32.MaxValue));
            
            await Assert.ThrowsAsync<InputDataValidationException>(async() =>await _controller.GetAllUsers(0, -1));

        }

        [Fact]
        public async Task UsersApiControler_GetUser_Success()
        {
            var result = await _controller.GetUser(1);
            Assert.True(result is OkObjectResult);
            
            var userResult = (result as OkObjectResult).Value as User;
            var user = users.FirstOrDefault(u => u.Id == 1);
            
            Assert.NotNull(user);
            Assert.NotNull(userResult);
            
            Assert.True(userResult.Id == user.Id && userResult.Email == user.Email && userResult.Name == user.Name);
        }
        
        [Fact]
        public async Task UsersApiControler_GetUser_NotFound()
        {
            await Assert.ThrowsAsync<ResourceNotFoundException>(async() =>await _controller.GetUser(0));
        }
    }
}