using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dialog.Common;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Administration;
using Dialog.ViewModels.Question;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Dialog.Services.Tests
{
    [TestFixture]
    public class QuestionServiceTests : BaseTests<IQuestionService>
    {
        [Test]
        public void QuestionServiceAdd()
        {
            //Arrange
            var questionRepository = new Mock<IDeletableEntityRepository<Question>>();
            questionRepository.Setup(q => q.Add(It.IsAny<Question>()))
                .Callback((Question question) =>
                {
                    var data = this.QuestionsData.ToList();
                    data.Add(question);

                    this.QuestionsData = data.AsQueryable();
                });

            this.Service = new QuestionService(questionRepository.Object);

            //Act
            var model = new QuestionViewModel
            {
                Name = "Name",
                Email = "Email",
                Subject = "Subject",
                Message = "Message"
            };

            var expectedCount = this.QuestionsData.Count() + 1;

            var result = this.Service.Add(model);

            //Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual(expectedCount, this.QuestionsData.Count());
            Assert.That(this.QuestionsData.Last().Name == model.Name);
        }

        [Test]
        public void QuestionServiceAddWithIncorrectModel()
        {
            //Arrange
            var questionRepository = new Mock<IDeletableEntityRepository<Question>>();
            questionRepository.Setup(q => q.Add(It.IsAny<Question>()))
                .Callback((Question question) =>
                {
                    var data = this.QuestionsData.ToList();
                    data.Add(question);

                    this.QuestionsData = data.AsQueryable();
                });

            this.Service = new QuestionService(questionRepository.Object);

            //Act
            var model = new QuestionViewModel();
            var expectedErrorMsg = GlobalConstants.ModelIsEmpty;
            var expectedCount = this.QuestionsData.Count();
            var result = this.Service.Add(model);

            //Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(expectedErrorMsg, result.Error);
            Assert.AreEqual(expectedCount, this.QuestionsData.Count());
        }

        [Test]
        public void QuestionServiceAnswer()
        {
            //Arrange
            var questionRepository = new Mock<IDeletableEntityRepository<Question>>();
            questionRepository.Setup(q => q.GetByIdAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] id) =>
                {
                    var idStr = id.First().ToString();
                    var question = this.QuestionsData.FirstOrDefault(q => q.Id == idStr);

                    return question;
                });

            questionRepository.Setup(q => q.SaveChangesAsync())
                .ReturnsAsync(1);

            this.Service = new QuestionService(questionRepository.Object);

            //Act
            var expectedquestion = this.QuestionsData.First();

            this.Service.Answer(expectedquestion.Id).GetAwaiter().GetResult();

            //Assert
            Assert.IsTrue(expectedquestion.IsAnswered);
            questionRepository.Verify(mock => mock.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void QuestionServiceAll()
        {
            //Arrange
            var questionRepository = new Mock<IDeletableEntityRepository<Question>>();
            questionRepository.Setup(q => q.All())
                .Returns(this.QuestionsData);

            this.Service = new QuestionService(questionRepository.Object);

            //Act

            var result = this.Service.All<AdministrationQuestionViewModel>().ToList();

            //Assert
            Assert.IsTrue(result.Count <= 10);
            Assert.That(result, Is.All.InstanceOf<AdministrationQuestionViewModel>());
        }
    }
}