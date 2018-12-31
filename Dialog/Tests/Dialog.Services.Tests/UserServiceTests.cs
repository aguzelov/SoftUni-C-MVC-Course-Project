﻿using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Data.Models.Blog;
using Dialog.Services.Contracts;
using Dialog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dialog.Services.Tests
{
    [TestFixture]
    public class UserServiceTests : BaseTests<IUserService>
    {
        [Test]
        public void UserServiceGetCount()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.All()).Returns(this.UserData);

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var expectedCount = this.UserData.Count();

            var result = this.Service.Count();

            //Assert
            Assert.AreEqual(expectedCount, result);
        }

        [Test]
        public void UserServiceAllUsers()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.All()).Returns(this.UserData);

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var result = this.Service.All<UserSummaryViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.UserData.Count(), result.Count);
            Assert.That(result, Is.All.AssignableFrom<UserSummaryViewModel>());
            Assert.IsTrue(result.First().CreatedOn > result.Skip(1).First().CreatedOn);
        }

        [Test]
        public void UserServiceAuthorWithPostsCount()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.All()).Returns(this.UserData);

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var result = this.Service.AuthorWithPostsCount<AuthorsWithPostsCountViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.UserData.Count(), result.Count);
            Assert.That(result, Is.All.AssignableFrom<AuthorsWithPostsCountViewModel>());
            Assert.That(result[0].Posts >= result[1].Posts);
        }

        [Test]
        public void UserServiceAuthorWithNewsCount()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.All()).Returns(this.UserData);

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var result = this.Service.AuthorWithNewsCount<AuthorsWithNewsCountViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.UserData.Count(), result.Count);
            Assert.That(result, Is.All.AssignableFrom<AuthorsWithNewsCountViewModel>());
            Assert.That(result[0].News >= result[1].News);
        }

        [Test]
        public void UserServiceGetUserRoles()
        {
            //Arrange
            var expectedRoles = new List<string>() { "Admin", "User" };

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync((string email) =>
                    {
                        return this.UserData.FirstOrDefault(u => u.Email == email);
                    });

            userManager.Setup(m => m.GetRolesAsync(It.IsAny<ApplicationUser>()))
                .ReturnsAsync(expectedRoles);

            this.Service = new UserService(null, userManager.Object);

            //Act
            var userEmail = this.UserData.First().Email;
            var roles = this.Service.GetUserRoles(userEmail).GetAwaiter().GetResult();

            var expectedResult = string.Join("; ", expectedRoles);

            //Assert
            Assert.AreEqual(expectedResult, roles);
        }
    }
}