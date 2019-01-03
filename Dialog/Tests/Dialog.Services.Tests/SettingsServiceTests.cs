using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Administration;
using Moq;
using NUnit.Framework;

namespace Dialog.Services.Tests
{
    [TestFixture]
    public class SettingsServiceTests : BaseTests<ISettingsService>
    {
        [Test]
        public void SettingsServiceGet()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act
            var expectedSetting = this.SettingsData.First();

            var result = this.Service.Get(expectedSetting.Name);

            //Assert
            Assert.AreEqual(expectedSetting.Value, result);
        }

        [Test]
        public void SettingsServiceGetWithIncorrectName()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act

            var result = this.Service.Get(this.IncorrectTestText);

            //Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void SettingsServiceGetWithNull()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act

            var result = this.Service.Get(null);

            //Assert
            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void SettingsServiceAll()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act

            var result = this.Service.All<AdministrationSettingsViewModel>().ToList();

            //Assert
            Assert.AreEqual(this.SettingsData.Count(), result.Count);
            Assert.That(result, Is.All.InstanceOf<AdministrationSettingsViewModel>());
        }

        [Test]
        public void SettingsServiceChange()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act
            var expectedSetting = this.SettingsData.First();
            var newValue = "New Value";

            this.Service.Change(expectedSetting.Name, newValue).GetAwaiter().GetResult();

            //Assert
            Assert.AreEqual(expectedSetting.Value, newValue);
        }

        [Test]
        public void SettingsServiceChangeWithNull()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act
            var expectedSetting = this.SettingsData.First();
            var newValue = "New Value";

            this.Service.Change(expectedSetting.Name, null).GetAwaiter().GetResult();

            //Assert
            Assert.AreNotEqual(expectedSetting.Value, newValue);
        }

        [Test]
        public void SettingsServiceChangeWithIncorrectName()
        {
            //Arrange
            var settingsRepository = new Mock<IDeletableEntityRepository<Setting>>();
            settingsRepository.Setup(s => s.All())
                .Returns(this.SettingsData);

            this.Service = new SettingsService(settingsRepository.Object);

            //Act
            var expectedSetting = this.SettingsData.First();
            var newValue = "New Value";

            this.Service.Change(this.IncorrectTestText, null).GetAwaiter().GetResult();

            //Assert
            Assert.AreNotEqual(expectedSetting.Value, newValue);
        }
    }
}