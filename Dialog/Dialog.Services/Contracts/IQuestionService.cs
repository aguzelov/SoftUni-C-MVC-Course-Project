using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dialog.ViewModels.Question;

namespace Dialog.Services.Contracts
{
    public interface IQuestionService
    {
        IServiceResult Add(QuestionViewModel model);

        ICollection<T> All<T>();

        Task Answer(string id);
    }
}