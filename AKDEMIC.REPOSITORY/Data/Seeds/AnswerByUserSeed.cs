using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AnswerByUserSeed
    {
        public static AnswerByUser[] Seed(AkdemicContext context)
        {
            var answers = context.Answer.ToList();
            var questionsMultiple = context.Question.Where(x => x.Type == ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION).ToList();
            var questionsUnique = context.Question.Where(x => x.Type == ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION).ToList();
            var questionsText = context.Question.Where(x => x.Type == ConstantHelpers.SURVEY.TEXT_QUESTION).ToList();
            var surveyusers = context.SurveyUsers.ToList();           

             var result = new List<AnswerByUser>()
                 {
                     //new AnswerByUser{ Description = "UserAnswerText1",  QuestionId = questionsText[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ Description = "UserAnswerText2",  QuestionId = questionsText[1].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[0].Id,  QuestionId = questionsMultiple[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[0].Id,  QuestionId = questionsMultiple[0].Id, SurveyUserId = surveyusers[1].Id  },
                     //new AnswerByUser{ AnswerId = answers[1].Id,  QuestionId = questionsMultiple[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[2].Id,  QuestionId = questionsMultiple[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[3].Id,  QuestionId = questionsMultiple[1].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[4].Id,  QuestionId = questionsMultiple[1].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[4].Id,  QuestionId = questionsMultiple[1].Id, SurveyUserId = surveyusers[1].Id  },
                     //new AnswerByUser{ AnswerId = answers[5].Id,  QuestionId = questionsMultiple[2].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[6].Id,  QuestionId = questionsMultiple[2].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[7].Id,  QuestionId = questionsUnique[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[7].Id,  QuestionId = questionsUnique[0].Id, SurveyUserId = surveyusers[1].Id  },
                     //new AnswerByUser{ AnswerId = answers[8].Id,  QuestionId = questionsUnique[0].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[8].Id,  QuestionId = questionsUnique[0].Id, SurveyUserId = surveyusers[1].Id  },
                     //new AnswerByUser{ AnswerId = answers[9].Id,  QuestionId = questionsUnique[1].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[10].Id,  QuestionId = questionsUnique[1].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[11].Id,  QuestionId = questionsUnique[2].Id, SurveyUserId = surveyusers[0].Id  },
                     //new AnswerByUser{ AnswerId = answers[12].Id,  QuestionId = questionsUnique[2].Id, SurveyUserId = surveyusers[0].Id  }
                 };
             return result.ToArray();
         }
    }
}
