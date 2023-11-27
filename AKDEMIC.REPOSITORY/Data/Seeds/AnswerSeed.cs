using System.Collections.Generic;
using System.Linq;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.ENTITIES.Models.Intranet;


namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class AnswerSeed
    {
        public static Answer[] Seed(AkdemicContext context)
        {
            var questionsMultiple = context.Question.Where(x => x.Type == ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION).ToList();
            var questionsUnique = context.Question.Where(x => x.Type == ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION).ToList();

            var result = new List<Answer>()
                {
                    //new Answer{ Description =  "Si" , QuestionId = questionsMultiple[0].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsMultiple[0].Id },
                    //new Answer{ Description =  "Si" , QuestionId = questionsMultiple[0].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsMultiple[1].Id },
                    //new Answer{ Description =  "No sé" , QuestionId = questionsMultiple[1].Id },
                    //new Answer{ Description =  "Si" , QuestionId = questionsMultiple[2].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsMultiple[2].Id },
                    //new Answer{ Description =  "Si" , QuestionId = questionsUnique[0].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsUnique[0].Id },
                    //new Answer{ Description =  "Si" , QuestionId = questionsUnique[1].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsUnique[1].Id },
                    //new Answer{ Description =  "Si" , QuestionId = questionsUnique[2].Id },
                    //new Answer{ Description =  "No" , QuestionId = questionsUnique[2].Id }
                };
            return result.ToArray();
        }
    }
}
