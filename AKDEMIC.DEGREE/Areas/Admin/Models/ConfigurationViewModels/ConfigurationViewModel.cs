using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AKDEMIC.DEGREE.Areas.Admin.Models.ConfigurationViewModels
{
    public class ConfigurationViewModel
    {
        #region TUPAS
        [Display(Name = "Tupa para bachiller automatico")]
        public string TupaBachelorAutomatic { get; set; }
        [Display(Name = "Tupa para bachiller por solicitud")]
        public string TupaBachelorRequested { get; set; }
        [Display(Name = "Tipo tupa de bachiller")]
        public byte TupaStaticTypeBachelor { get; set; }
        [Display(Name = "Tupa para titulo profesional por sustentación de tesis")]
        public string TupaTitleDegreeSupportTesis { get; set; }
        [Display(Name = "Tupa para titulo profesional por examen de suficiencia")]
        public string TupaTitleDegreeSufficiencyExam { get; set; }
        [Display(Name = "Tupa para titulo profesional por experiencia profesional")]
        public string TupaTitleDegreeProfessionalExperience { get; set; }


        #endregion

        #region CONCEPTS
        [Display(Name = "Concepto para bachiller automatico")]
        public string ConceptBachelorAutomatic { get; set; }
        [Display(Name = "Concepto para bachiller por solicitud")]
        public string ConceptBachelorRequested { get; set; }
        [Display(Name = "Tipo concepto de bachiller")]
        public byte ConceptStaticTypeBachelor { get; set; }
        [Display(Name = "Concepto para titulo profesional por sustentación de tesis")]
        public string ConceptTitleDegreeSupportTesis { get; set; }
        [Display(Name = "Concepto para titulo profesional por examen de suficiencia")]
        public string ConceptTitleDegreeSufficiencyExam { get; set; }
        [Display(Name = "Concepto para titulo profesional por experiencia profesional")]
        public string ConceptTitleDegreeProfessionalExperience { get; set; }



        #endregion

        public bool MethodTypeRegistryPatternCreation { get; set; }
        public bool StaticTypeSystemIntegrated { get; set; }

        public string DegreeLoginBackgroundImagePath { get; set; }

        [Display(Name = "Imagen de fondo del Login")]
        public IFormFile DegreeLoginBackgroundImage { get; set; }
    }
}
