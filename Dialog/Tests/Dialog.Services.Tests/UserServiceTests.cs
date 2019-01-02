using System;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Dialog.Common;

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

        [Test]
        public void UserServiceApprove()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            userRepository.Setup(u => u.Update(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser currentUser) =>
                {
                    this.UserData.FirstOrDefault(u => currentUser != null && u.Id == currentUser.Id).IsApproved = true;
                });

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var actualUser = this.UserData.First();
            this.Service.Approve(actualUser.Id).GetAwaiter().GetResult();

            //Assert
            Assert.That(actualUser.IsApproved == true);
        }

        [Test]
        public void UserServiceApproveWhitIncorrectId()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            this.Service = new UserService(userRepository.Object, null);

            //Act

            this.Service.Approve(this.IncorrectTestText).GetAwaiter().GetResult();

            //Assert
        }

        [Test]
        public void UserServiceDelete()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            userRepository.Setup(u => u.Delete(It.IsAny<ApplicationUser>()))
                .Callback((ApplicationUser currentUser) =>
                {
                    var user = this.UserData.FirstOrDefault(u => currentUser != null && u.Id == currentUser.Id);
                    user.IsDeleted = true;
                    user.DeletedOn = DateTime.UtcNow;
                });

            this.Service = new UserService(userRepository.Object, null);

            //Act
            var actualUser = this.UserData.First();
            this.Service.DeleteUser(actualUser.Id).GetAwaiter().GetResult();

            //Assert
            Assert.That(actualUser.IsDeleted == true);
            Assert.NotNull(actualUser.DeletedOn);
        }

        [Test]
        public void UserServiceDeleteWhitIncorrectId()
        {
            //Arrange
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            this.Service = new UserService(userRepository.Object, null);

            //Act

            this.Service.DeleteUser(this.IncorrectTestText).GetAwaiter().GetResult();

            //Assert
        }

        [Test]
        public void UserServicePromote()
        {
            //Arrange
            var user = this.UserData.First();
            var roles = new List<string>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser currentUser, string role) =>
                {
                    roles.Add(role);
                    return new IdentityResult();
                });

            this.Service = new UserService(userRepository.Object, userManager.Object);

            //Act

            this.Service.Promote(user.Id).GetAwaiter().GetResult();

            //Assert
            Assert.That(roles.Contains(GlobalConstants.AdminRole));
            userManager.Verify(mock => mock.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UserServicePromoteWithIncorrectId()
        {
            //Arrange
            var roles = new List<string>();
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser currentUser, string role) =>
                {
                    roles.Add(role);
                    return new IdentityResult();
                });

            this.Service = new UserService(userRepository.Object, userManager.Object);

            //Act

            this.Service.Promote(this.IncorrectTestText).GetAwaiter().GetResult();

            //Assert
            Assert.That(!roles.Contains(GlobalConstants.AdminRole));
            userManager.Verify(mock => mock.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }

        [Test]
        public void UserServiceDemote()
        {
            //Arrange
            var user = this.UserData.First();
            var roles = new List<string>() { GlobalConstants.AdminRole };
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser currentUser, string role) =>
                {
                    roles.Remove(role);
                    return new IdentityResult();
                });

            this.Service = new UserService(userRepository.Object, userManager.Object);

            //Act

            this.Service.Demote(user.Id).GetAwaiter().GetResult();

            //Assert
            Assert.That(!roles.Contains(GlobalConstants.AdminRole));
            userManager.Verify(mock => mock.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void UserServiceDemoteWithIncorrectId()
        {
            //Arrange
            var user = this.UserData.First();
            var roles = new List<string>() { GlobalConstants.AdminRole };
            var userRepository = new Mock<IDeletableEntityRepository<ApplicationUser>>();
            userRepository.Setup(u => u.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var userId = id.First();
                    return this.UserData.FirstOrDefault(u => u.Id == (string)userId);
                });

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
                .ReturnsAsync((ApplicationUser currentUser, string role) =>
                {
                    roles.Remove(role);
                    return new IdentityResult();
                });

            this.Service = new UserService(userRepository.Object, userManager.Object);

            //Act

            this.Service.Demote(this.IncorrectTestText).GetAwaiter().GetResult();

            //Assert
            Assert.That(roles.Contains(GlobalConstants.AdminRole));
            userManager.Verify(mock => mock.RemoveFromRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Never);
        }
    }
}