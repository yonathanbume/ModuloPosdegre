using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;


namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class QuestionSeed
    {
        public static Question[] Seed(AkdemicContext context)
        {
            var surveys = context.Survey.ToList();
            var result = new List<Question>()
                {
                    //new Question{ Description = "¿Estás satisfecho con la calidad de enseñanza de las disciplinas obligatorias?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.TEXT_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Crees que la universidad está lo suficientemente equipada para las necesidades de los estudios?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.TEXT_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Estás satisfecho/ a de estudiar en esta universidad?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Estás de acuerdo a la infraestructura de la institución?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Te sientes seguro/ a en la escuela?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.MULTIPLE_SELECTION_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Es difícil conseguir los materiales de capacitación necesarios?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Estás satisfecho con la calidad de enseñanza de las disciplinas facultativas?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION, SurveyItemId=null},
                    //new Question{ Description = "¿Cómo valorarías la complejidad / proceso de inscripción para los cursos en esta universidad?", SurveyId = surveys[0].Id , Type = ConstantHelpers.SURVEY.UNIQUE_SELECTION_QUESTION, SurveyItemId=null}
                };
            return result.ToArray();
        }
    }
}
