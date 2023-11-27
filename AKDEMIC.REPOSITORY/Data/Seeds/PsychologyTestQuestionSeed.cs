using System.Collections.Generic;
using System.Linq;
using AKDEMIC.ENTITIES.Models.Intranet;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class PsychologyTestQuestionSeed
    {
        public static PsychologyTestQuestion[] Seed(AkdemicContext context)
        {
            var Categories = context.PsychologyCategories.ToArray();

            var result = new List<PsychologyTestQuestion>()
            {
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tienes dolores frecuentes de cabeza ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tienes mal apetito ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Duermes mal ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te asustas con facilidad ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Sufres temblor en las manos ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te sientes nervioso o tenso ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Sufres de mala digestión ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Eres incapaz de pensar con claridad ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te sientes triste ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Lloras con mucha frecuencia ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tienes dificultad en disfrutar tus actividades diarias ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tienes dificultad para tomar decisiones ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tienes dificultad en hacer tus tareas ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Tu rendimiento escolar se ha visto afectado ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Eres incapaz de desempeñar un papel útil en tu vida ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has perdido interés en las cosas ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te sientes aburrido ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has tenido la idea de acabar con tu vida ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te sientes cansado todo el tiempo ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Sientes que alguien ha tratado de herirte en alguna forma ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Eres una persona mucho más importante de lo que piensan los demás ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has notado interferencia o algo raro en tu pensamiento ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Oyes voces sin saber de donde vienen o que otras personas no pueden oir ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has tenido convulsiones, ataques o caídas al suelo con movimiento de brazo y piernas; con mordedura de lengua o pérdida del conocimiento ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Alguna vez le ha parecido a tu familia, amigos, tu médico o tu sacerdote que estabas bebiendo demasiado ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Alguna vez has querido dejar de beber pero no has podido ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has tenido alguna vez dificultades en los estudios a causa de la bebida, como beber en el colegio, instituto/universidad; o fallar en ellos ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Has estado en riñas o te han arrestado estando borracho ?"},
                new PsychologyTestQuestion() {  CategoryId = Categories[0].Id, Question = "¿ Te ha parecido alguna vez que bebias demasiado ?"}
            };

            return result.ToArray();
        }
    }
}
