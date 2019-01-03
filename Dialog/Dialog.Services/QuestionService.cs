using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;
using Dialog.ViewModels.Question;

namespace Dialog.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IDeletableEntityRepository<Question> _questionRepository;

        public QuestionService(IDeletableEntityRepository<Question> questionRepository)
        {
            this._questionRepository = questionRepository;
        }

        public IServiceResult Add(QuestionViewModel model)
        {
            var result = new ServiceResult()
            {
                Success = false
            };

            if (string.IsNullOrEmpty(model.Name) ||
                string.IsNullOrEmpty(model.Email) ||
                string.IsNullOrEmpty(model.Subject) ||
                string.IsNullOrEmpty(model.Message))
            {
                result.Error = "Invalid question data!";
                return result;
            }

            var question = new Question
            {
                Name = model.Name,
                Email = model.Email,
                Subject = model.Subject,
                Message = model.Message,
                CreatedOn = DateTime.UtcNow
            };

            this._questionRepository.Add(question);
            this._questionRepository.SaveChangesAsync();

            result.Success = true;
            return result;
        }

        public ICollection<T> All<T>()
        {
            var questions = this._questionRepository.All()
                .OrderBy(q => q.CreatedOn)
                .Take(10)
                .To<T>()
                .ToList();

            return questions;
        }

        public async Task Answer(string id)
        {
            var question = await this._questionRepository.GetByIdAsync(id);

            question.IsAnswered = true;

            await this._questionRepository.SaveChangesAsync();
        }
    }
}