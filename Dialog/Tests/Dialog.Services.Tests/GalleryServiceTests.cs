using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using NUnit.Framework;

namespace Dialog.Services.Tests
{
    public class GalleryServiceTests : BaseTests<IGalleryService>
    {
        [Test]
        public void GalleryServiceCount()
        {
            //Arrange
            var imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            imageRepository.Setup(i => i.All())
                .Returns(this.ImageData);

            this.Service = new GalleryService(null, imageRepository.Object);

            //Act
            var expectedCount = this.ImageData.Count();
            var actualCount = this.Service.Count();

            //Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [Test]
        public void GalleryServiceAll()
        {
            //Arrange
            var imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            imageRepository.Setup(i => i.All())
                .Returns(this.ImageData);

            this.Service = new GalleryService(null, imageRepository.Object);

            //Act
            var expectedCount = this.ImageData.Count();
            var result = this.Service.All();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
            Assert.That(result, Is.All.InstanceOf<Image>());
        }

        [Test]
        public void GalleryServiceUpload()
        {
            //Arrange
            var cloudinaryService = new Mock<ICloudinaryService>();
            cloudinaryService
                .Setup(c => c.Upload(It.IsAny<IFormFile>(), It.IsAny<string>()))
                .Returns(new CloudinaryServiceUploadResult
                {
                    PublicId = "PublicId",
                    Uri = new Uri("http://www.uri.com/"),
                    SecureUri = new Uri("http://www.secureUri.com/")
                });

            var imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            imageRepository.Setup(i => i.All())
                .Returns(this.ImageData);

            this.Service = new GalleryService(cloudinaryService.Object, imageRepository.Object);

            //Act
            var files = GenerateFiles();
            var result = this.Service.Upload(files);

            //Assert
            Assert.AreEqual(files.Count, result.Count);
            Assert.That(result, Is.All.InstanceOf<Image>());
        }

        private ICollection<IFormFile> GenerateFiles()
        {
            var files = new List<IFormFile>();

            for (int i = 0; i < 10; i++)
            {
                var fileMock = new Mock<IFormFile>();
                //Setup mock file using a memory stream
                var content = "content";
                var fileName = "fileName." + i;
                var ms = new MemoryStream();
                var writer = new StreamWriter(ms);
                writer.Write(content);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
                fileMock.Setup(_ => _.FileName).Returns(fileName);
                fileMock.Setup(_ => _.Length).Returns(ms.Length);
                fileMock.Setup(_ => _.ContentDisposition).Returns($"inline; filename={fileName}");

                var file = fileMock.Object;
                files.Add(file);
            }

            return files;
        }
    }
}