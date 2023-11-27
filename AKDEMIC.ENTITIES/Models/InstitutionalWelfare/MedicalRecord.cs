using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.ENTITIES.Models.InstitutionalWelfare
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public string Religion { get; set; }
        public bool HasDrugAllergy { get; set; }
        public string DrugAllergyDescription { get; set; }
        public bool BloodTypeKnowledge { get; set; }
        public int BloodType { get; set; }
        public int RhFactor { get; set; }

        //
        public int Smoke { get; set; }
        public int Liqueur { get; set; }
        public int BloodTransfusions { get; set; }
        public int InfectiousDiseases { get; set; }
        public int CompleteVaccinations { get; set; }
        public int ChronicDiseases { get; set; }
        public int Disability { get; set; }
        public int Accidents { get; set; }
        public int Intoxications { get; set; }
        public int SurgeryOrHospitalization { get; set; }
        public int TakeMedication { get; set; }
        public int PsychologicalProblems { get; set; }
        public string OtherDiseaseDescription { get; set; }


        //
        public int Diabetes { get; set; }
        public int Obesity { get; set; }
        public int Cardiovascular { get; set; }
        public int Allergy { get; set; }
        public int Infections { get; set; }
        public int Cancer { get; set; }
        public int HomePsychologicalProblems { get; set; }
        public int AlcoholOrDrugs { get; set; }
        public int DomesticViolence { get; set; }
        public string HomeOtherDiseaseDescription { get; set; }


        //SDS
        public int SadEmotionalLevel { get; set; }
        public int FeelMorningLevel { get; set; }
        public int CryEmotionalLevel { get; set; }
        public int NightDreamProblemLevel { get; set; }
        public int SameEmotionLevel { get; set; }
        public int SexEnjoyLevel { get; set; }
        public int WeightKnowledgeLevel { get; set; }
        public int ConstipationProblemLevel { get; set; }
        public int HeartProblemLevel { get; set; }
        public int TirednessLevel { get; set; }
        public int ClearMindLevel { get; set; }
        public int RythmChangeLevel { get; set; }
        public int AgitatedLevel { get; set; }
        public int HopefulLevel { get; set; }
        public int IrritationLevel { get; set; }
        public int VelocityDecisionLevel { get; set; }
        public int UsefulLevel { get; set; }
        public int SatisfactionLevel { get; set; }
        public int SuicideLevel { get; set; }
        public int EnjoymentLevel { get; set; }
    }
}
