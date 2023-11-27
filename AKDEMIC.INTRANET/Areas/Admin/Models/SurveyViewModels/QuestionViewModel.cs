using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.SurveyViewModels
{
    public class QuestionViewModel
    {
        public Guid Id { get; set; }

        public Guid SurveyId { get; set; }

        [Display(Name = "Tipo de Pregunta")]
        public int Type { get; set; }

        public Guid SurveyItemId { get; set; }

        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public List<AnswerViewModel> Answers { get; set; }
    }

    public class QuestionCreateViewModel : QuestionCommonFieldsViewModel
    {
        [Required]
        public Guid SurveyItemId { get; set; }
    }

    public class QuestionCommonFieldsViewModel
    {
        [Required]
        [Display(Name = "Tipo de Pregunta")]
        public int Type { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }

        public List<AnswerCreateViewModel> Answers { get; set; }
    }

    public class QuestionEditViewModel : QuestionCommonFieldsViewModel
    {
        [Required]
        public Guid Id { get; set; }
    }

    public class LikertQuestionCreateViewModel
    {
        [Required]
        public Guid SurveyItemId { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }

    public class LikertQuestionEditViewModel
    {
        [Required]
        public Guid QuestionId { get; set; }

        [Required]
        public Guid SurveyItemId { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Description { get; set; }
    }
}
