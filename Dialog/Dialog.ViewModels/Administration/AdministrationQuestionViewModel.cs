using System;
using System.Collections.Generic;
using System.Text;
using Dialog.ViewModels.Question;

namespace Dialog.ViewModels.Administration
{
    public class AdministrationQuestionViewModel : QuestionViewModel
    {
        public string Id { get; set; }

        public bool IsAnswered { get; set; }
    }
}