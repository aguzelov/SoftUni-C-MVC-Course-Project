using Dialog.Data.Common.Repositories;
using Dialog.Data.Models.Gallery;
using Dialog.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dialog.Common;
using Dialog.ViewModels.Gallery;
using NUnit.Framework.Interfaces;

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
        public void GalleryServiceRecentImages()
        {
            //Arrange
            var imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            imageRepository.Setup(i => i.All())
                .Returns(this.ImageData);

            this.Service = new GalleryService(null, imageRepository.Object);

            //Act
            var expectedCount = 8;
            var result = this.Service.RecentImages<ImageViewModel>(expectedCount).ToList();

            //Assert
            Assert.That(result.Count() <= 8);
            Assert.That(result, Is.All.InstanceOf<ImageViewModel>());
        }

        [Test]
        public void GalleryServiceGetDefaultImage()
        {
            //Arrange
            var postImage = this.ImageData.First();
            postImage.DefaultType = ImageDefaultType.BlogPost;

            var imageRepository = new Mock<IDeletableEntityRepository<Image>>();
            imageRepository.Setup(i => i.All())
                .Returns(this.ImageData);

            this.Service = new GalleryService(null, imageRepository.Object);

            //Act
            var result = this.Service.GetDefaultImage(ImageDefaultType.BlogPost);

            //Assert
            Assert.That(result.DefaultType == ImageDefaultType.BlogPost);
            Assert.AreEqual(postImage.Id, result.Id);
            Assert.That(result, Is.InstanceOf<Image>());
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
            Assert.NotNull(result);
            Assert.That(result, Is.InstanceOf<Image>());
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