using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    public class UsersApiControllerTests
    {
        private readonly UsersApiController _controller;

        private static List<User> users = new List<User>()
        {
            new User() {Email = "test@test.test", Name = "test", Id = 1},
            new User() {Email = "test1@test.test", Name = "test1", Id = 2},
            new User() {Email = "test2@test.test", Name = "test2", Id = 3}
        };
        
        public UsersApiControllerTests()
        {
            var usersRepository = new Mock<IUsersRepository>();
            
            _controller = new UsersApiController(usersRepository.Object);
            usersRepository.Setup(ur => ur.GetAll(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(users);
            
            usersRepository.Setup(ur => ur.Get(It.IsAny<int>()))
                .ReturnsAsync((int i) => users.FirstOrDefault(u => u.Id == i));
            
            usersRepository.Setup(ur => ur.Create(It.IsAny<User>()))
                .ReturnsAsync((User u) =>
                {
                    users.Add(u);
                    return u;
                });

        }
        
        [Fact]
        public async Task UsersApiController_GetAllUsers_Success()
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
        public async Task UsersApiController_GetAllUsers_Failed_Negative()
        {
            await Assert.ThrowsAsync<InputDataValidationException>(async() =>await _controller.GetAllUsers(-1, Int32.MaxValue));
            
            await Assert.ThrowsAsync<InputDataValidationException>(async() =>await _controller.GetAllUsers(0, -1));

        }

        [Fact]
        public async Task UsersApiController_GetUser_Success()
        {
            var result = await _controller.GetUser(1);
            
            var userResult = (result as OkObjectResult)?.Value as User;
            var user = users.FirstOrDefault(u => u.Id == 1);
            
            Assert.NotNull(user);
            Assert.NotNull(userResult);
            
            Assert.True(userResult.Equals(user));
        }
        
        [Fact]
        public async Task UsersApiController_GetUser_NotFound()
        {
            await Assert.ThrowsAsync<ResourceNotFoundException>(async() =>await _controller.GetUser(0));
        }

        [Fact]
        public async Task UsersApiController_CreateUser_Success()
        {
            var newUser = new User() {Id = 4, Email = "test4@test.test", Name = "Test4"};
            var result = await _controller.CreateUser(newUser);
            var userResult = (result as OkObjectResult)?.Value as User;
            Assert.NotNull(userResult);
            Assert.True(userResult.Equals(newUser));

            var checkAdded = await _controller.GetUser(4);
            userResult = (checkAdded as OkObjectResult)?.Value as User;
            Assert.NotNull(userResult);
            Assert.True(userResult.Equals(newUser));
        }
    }
}