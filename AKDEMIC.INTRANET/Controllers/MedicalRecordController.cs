using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.CORE.Services;
using AKDEMIC.ENTITIES.Models.Generals;
using AKDEMIC.ENTITIES.Models.InstitutionalWelfare;
using AKDEMIC.INTRANET.ViewModels.MedicalRecordViewModels;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.SERVICE.Services.Generals.Interfaces;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AKDEMIC.INTRANET.Controllers
{
    [Authorize(Roles = CORE.Helpers.ConstantHelpers.ROLES.STUDENTS)]
    [Route("ficha-medica")]
    public class MedicalRecordController : BaseController
    {
        private readonly IStudentService _studentService;
        private IWebHostEnvironment _hostingEnvironment;
        private readonly IConverter _dinkConverter;
        private readonly IViewRenderService _viewRenderService;

        public MedicalRecordController(UserManager<ApplicationUser> userManager,
            IConverter dinkConverter,
            IStudentService studentService,
            IViewRenderService viewRenderService,
            IWebHostEnvironment environment,
            AkdemicContext context) : base(context, userManager)
        {
            _studentService = studentService;
            _hostingEnvironment = environment;
            _dinkConverter = dinkConverter;
            _viewRenderService = viewRenderService;
        }

        /// <summary>
        /// Obtiene la vista inicial de la ficha medica
        /// </summary>
        /// <returns>Retorna una vista</returns>
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var student = await _studentService.GetStudentByUser(userId);
            var model = new MedicalRecordViewModel();
            model.FullName = student.User.FullName;
            model.Age = student.User.Age.ToString();
            model.Sex = AKDEMIC.CORE.Helpers.ConstantHelpers.SEX.VALUES.ContainsKey(student.User.Sex) ? AKDEMIC.CORE.Helpers.ConstantHelpers.SEX.VALUES[student.User.Sex] : "--";
            model.Phone = student.User.PhoneNumber;
            model.Dni = student.User.Dni;
            model.UserName = student.User.UserName;
            model.Career = student.Career.Name;
            model.StudentId = student.Id;
            if (student.MedicalRecordId.HasValue)
            {
                model.Religion = student.MedicalRecord.Religion;
                model.HasDrugAllergy = student.MedicalRecord.HasDrugAllergy;
                model.DrugAllergyDescription = student.MedicalRecord.DrugAllergyDescription;
                model.BloodTypeKnowledge = student.MedicalRecord.BloodTypeKnowledge;
                model.BloodType = student.MedicalRecord.BloodType;
                model.RhFactor = student.MedicalRecord.RhFactor;

                model.Smoke = student.MedicalRecord.Smoke;
                model.Liqueur = student.MedicalRecord.Liqueur;
                model.BloodTransfusions = student.MedicalRecord.BloodTransfusions;
                model.InfectiousDiseases = student.MedicalRecord.InfectiousDiseases;
                model.CompleteVaccinations = student.MedicalRecord.CompleteVaccinations;
                model.ChronicDiseases = student.MedicalRecord.ChronicDiseases;
                model.Disability = student.MedicalRecord.Disability;
                model.Accidents = student.MedicalRecord.Accidents;
                model.Intoxications = student.MedicalRecord.Intoxications;
                model.SurgeryOrHospitalization = student.MedicalRecord.SurgeryOrHospitalization;
                model.TakeMedication = student.MedicalRecord.TakeMedication;
                model.PsychologicalProblems = student.MedicalRecord.PsychologicalProblems;
                model.OtherDiseaseDescription = student.MedicalRecord.OtherDiseaseDescription;

                model.Diabetes = student.MedicalRecord.Diabetes;
                model.Obesity = student.MedicalRecord.Obesity;
                model.Cardiovascular = student.MedicalRecord.Cardiovascular;
                model.Allergy = student.MedicalRecord.Allergy;
                model.Infections = student.MedicalRecord.Infections;
                model.Cancer = student.MedicalRecord.Cancer;
                model.HomePsychologicalProblems = student.MedicalRecord.HomePsychologicalProblems;
                model.AlcoholOrDrugs = student.MedicalRecord.AlcoholOrDrugs;
                model.DomesticViolence = student.MedicalRecord.DomesticViolence;
                model.HomeOtherDiseaseDescription = student.MedicalRecord.HomeOtherDiseaseDescription;

                model.SadEmotionalLevel = student.MedicalRecord.SadEmotionalLevel;
                model.FeelMorningLevel = student.MedicalRecord.FeelMorningLevel;
                model.CryEmotionalLevel = student.MedicalRecord.CryEmotionalLevel;
                model.NightDreamProblemLevel = student.MedicalRecord.NightDreamProblemLevel;
                model.SameEmotionLevel = student.MedicalRecord.SameEmotionLevel;
                model.SexEnjoyLevel = student.MedicalRecord.SexEnjoyLevel;
                model.WeightKnowledgeLevel = student.MedicalRecord.WeightKnowledgeLevel;
                model.ConstipationProblemLevel = student.MedicalRecord.ConstipationProblemLevel;
                model.HeartProblemLevel = student.MedicalRecord.HeartProblemLevel;
                model.TirednessLevel = student.MedicalRecord.TirednessLevel;
                model.ClearMindLevel = student.MedicalRecord.ClearMindLevel;
                model.RythmChangeLevel = student.MedicalRecord.RythmChangeLevel;
                model.AgitatedLevel = student.MedicalRecord.AgitatedLevel;
                model.HopefulLevel = student.MedicalRecord.HopefulLevel;
                model.IrritationLevel = student.MedicalRecord.IrritationLevel;
                model.VelocityDecisionLevel = student.MedicalRecord.VelocityDecisionLevel;
                model.UsefulLevel = student.MedicalRecord.UsefulLevel;
                model.SatisfactionLevel = student.MedicalRecord.SatisfactionLevel;
                model.SuicideLevel = student.MedicalRecord.SuicideLevel;
                model.EnjoymentLevel = student.MedicalRecord.EnjoymentLevel;
            }
            return View(model);
        }

        /// <summary>
        /// Guarda la información de la ficha medica
        /// </summary>
        /// <param name="model">Modelo que contiene la información de la ficha medica</param>
        /// <returns>Retorna un Codigo de Estado HTTP 200 o Codigo de Estado HTTP 400</returns>
        [HttpPost("guardar")]
        public async Task<IActionResult> Save(MedicalRecordViewModel model)
        {
            var student = await _studentService.GetWithIncludes(model.StudentId);
            var medicalRecord = new MedicalRecord();
            if (student.MedicalRecordId.HasValue)
            {
                student.MedicalRecord.Religion = model.Religion;
                student.MedicalRecord.HasDrugAllergy = model.HasDrugAllergy;
                student.MedicalRecord.DrugAllergyDescription = model.DrugAllergyDescription;
                student.MedicalRecord.BloodTypeKnowledge = model.BloodTypeKnowledge;
                student.MedicalRecord.BloodType = model.BloodType;
                student.MedicalRecord.RhFactor = model.RhFactor;

                student.MedicalRecord.Smoke = model.Smoke;
                student.MedicalRecord.Liqueur = model.Liqueur;
                student.MedicalRecord.BloodTransfusions = model.BloodTransfusions;
                student.MedicalRecord.InfectiousDiseases = model.InfectiousDiseases;
                student.MedicalRecord.CompleteVaccinations = model.CompleteVaccinations;
                student.MedicalRecord.ChronicDiseases = model.ChronicDiseases;
                student.MedicalRecord.Disability = model.Disability;
                student.MedicalRecord.Accidents = model.Accidents;
                student.MedicalRecord.Intoxications = model.Intoxications;
                student.MedicalRecord.SurgeryOrHospitalization = model.SurgeryOrHospitalization;
                student.MedicalRecord.TakeMedication = model.TakeMedication;
                student.MedicalRecord.PsychologicalProblems = model.PsychologicalProblems;
                student.MedicalRecord.OtherDiseaseDescription = model.OtherDiseaseDescription;

                student.MedicalRecord.Diabetes = model.Diabetes;
                student.MedicalRecord.Obesity = model.Obesity;
                student.MedicalRecord.Cardiovascular = model.Cardiovascular;
                student.MedicalRecord.Allergy = model.Allergy;
                student.MedicalRecord.Infections = model.Infections;
                student.MedicalRecord.Cancer = model.Cancer;
                student.MedicalRecord.HomePsychologicalProblems = model.HomePsychologicalProblems;
                student.MedicalRecord.AlcoholOrDrugs = model.AlcoholOrDrugs;
                student.MedicalRecord.DomesticViolence = model.DomesticViolence;
                student.MedicalRecord.HomeOtherDiseaseDescription = model.HomeOtherDiseaseDescription;

                student.MedicalRecord.SadEmotionalLevel = model.SadEmotionalLevel;
                student.MedicalRecord.FeelMorningLevel = model.FeelMorningLevel;
                student.MedicalRecord.CryEmotionalLevel = model.CryEmotionalLevel;
                student.MedicalRecord.NightDreamProblemLevel = model.NightDreamProblemLevel;
                student.MedicalRecord.SameEmotionLevel = model.SameEmotionLevel;
                student.MedicalRecord.SexEnjoyLevel = model.SexEnjoyLevel;
                student.MedicalRecord.WeightKnowledgeLevel = model.WeightKnowledgeLevel;
                student.MedicalRecord.ConstipationProblemLevel = model.ConstipationProblemLevel;
                student.MedicalRecord.HeartProblemLevel = model.HeartProblemLevel;
                student.MedicalRecord.TirednessLevel = model.TirednessLevel;
                student.MedicalRecord.ClearMindLevel = model.ClearMindLevel;
                student.MedicalRecord.RythmChangeLevel = model.RythmChangeLevel;
                student.MedicalRecord.AgitatedLevel = model.AgitatedLevel;
                student.MedicalRecord.HopefulLevel = model.HopefulLevel;
                student.MedicalRecord.IrritationLevel = model.IrritationLevel;
                student.MedicalRecord.VelocityDecisionLevel = model.VelocityDecisionLevel;
                student.MedicalRecord.UsefulLevel = model.UsefulLevel;
                student.MedicalRecord.SatisfactionLevel = model.SatisfactionLevel;
                student.MedicalRecord.SuicideLevel = model.SuicideLevel;
                student.MedicalRecord.EnjoymentLevel = model.EnjoymentLevel;
            }
            else
            {
                medicalRecord.Religion = model.Religion;
                medicalRecord.HasDrugAllergy = model.HasDrugAllergy;
                medicalRecord.DrugAllergyDescription = model.DrugAllergyDescription;
                medicalRecord.BloodTypeKnowledge = model.BloodTypeKnowledge;
                medicalRecord.BloodType = model.BloodType;
                medicalRecord.RhFactor = model.RhFactor;

                medicalRecord.Smoke = model.Smoke;
                medicalRecord.Liqueur = model.Liqueur;
                medicalRecord.BloodTransfusions = model.BloodTransfusions;
                medicalRecord.InfectiousDiseases = model.InfectiousDiseases;
                medicalRecord.CompleteVaccinations = model.CompleteVaccinations;
                medicalRecord.ChronicDiseases = model.ChronicDiseases;
                medicalRecord.Disability = model.Disability;
                medicalRecord.Accidents = model.Accidents;
                medicalRecord.Intoxications = model.Intoxications;
                medicalRecord.SurgeryOrHospitalization = model.SurgeryOrHospitalization;
                medicalRecord.TakeMedication = model.TakeMedication;
                medicalRecord.PsychologicalProblems = model.PsychologicalProblems;
                medicalRecord.OtherDiseaseDescription = model.OtherDiseaseDescription;

                medicalRecord.Diabetes = model.Diabetes;
                medicalRecord.Obesity = model.Obesity;
                medicalRecord.Cardiovascular = model.Cardiovascular;
                medicalRecord.Allergy = model.Allergy;
                medicalRecord.Infections = model.Infections;
                medicalRecord.Cancer = model.Cancer;
                medicalRecord.HomePsychologicalProblems = model.HomePsychologicalProblems;
                medicalRecord.AlcoholOrDrugs = model.AlcoholOrDrugs;
                medicalRecord.DomesticViolence = model.DomesticViolence;
                medicalRecord.HomeOtherDiseaseDescription = model.HomeOtherDiseaseDescription;

                medicalRecord.SadEmotionalLevel = model.SadEmotionalLevel;
                medicalRecord.FeelMorningLevel = model.FeelMorningLevel;
                medicalRecord.CryEmotionalLevel = model.CryEmotionalLevel;
                medicalRecord.NightDreamProblemLevel = model.NightDreamProblemLevel;
                medicalRecord.SameEmotionLevel = model.SameEmotionLevel;
                medicalRecord.SexEnjoyLevel = model.SexEnjoyLevel;
                medicalRecord.WeightKnowledgeLevel = model.WeightKnowledgeLevel;
                medicalRecord.ConstipationProblemLevel = model.ConstipationProblemLevel;
                medicalRecord.HeartProblemLevel = model.HeartProblemLevel;
                medicalRecord.TirednessLevel = model.TirednessLevel;
                medicalRecord.ClearMindLevel = model.ClearMindLevel;
                medicalRecord.RythmChangeLevel = model.RythmChangeLevel;
                medicalRecord.AgitatedLevel = model.AgitatedLevel;
                medicalRecord.HopefulLevel = model.HopefulLevel;
                medicalRecord.IrritationLevel = model.IrritationLevel;
                medicalRecord.VelocityDecisionLevel = model.VelocityDecisionLevel;
                medicalRecord.UsefulLevel = model.UsefulLevel;
                medicalRecord.SatisfactionLevel = model.SatisfactionLevel;
                medicalRecord.SuicideLevel = model.SuicideLevel;
                medicalRecord.EnjoymentLevel = model.EnjoymentLevel;
                await _context.MedicalRecords.AddAsync(medicalRecord);
                student.MedicalRecord = medicalRecord;
            }
            await _studentService.Update(student);
            var url = Url.Action("PrintMedicalRecord", "MedicalRecord", new { studentId = student.Id });
            return Ok(url);
        }

        /// <summary>
        /// Obtiene la constancia de la ficha medica del estudiante
        /// </summary>
        /// <param name="studentId">Identificador del estudiante</param>
        /// <returns>Retorna un archivo PDF</returns>
        [HttpGet("imprimir/{studentId}")]
        public async Task<IActionResult> PrintMedicalRecord(Guid studentId)
        {
            var student = await _studentService.GetWithIncludes(studentId);
            var model = new MedicalRecordViewModel();

            var executeQR = GenerarQR(studentId);
            model.ImageQR = String.Format("data:image/png;base64,{0}", Convert.ToBase64String(executeQR)); 

            model.FullName = student.User.FullName;
            model.Age = student.User.Age.ToString();
            model.Sex = AKDEMIC.CORE.Helpers.ConstantHelpers.SEX.VALUES.ContainsKey(student.User.Sex) ? AKDEMIC.CORE.Helpers.ConstantHelpers.SEX.VALUES[student.User.Sex] : "--";
            model.Phone = student.User.PhoneNumber;
            model.Dni = student.User.Dni;
            model.UserName = student.User.UserName;
            model.Career = student.Career.Name;     

            model.Religion = student.MedicalRecord.Religion;
            model.HasDrugAllergy = student.MedicalRecord.HasDrugAllergy;
            model.DrugAllergyDescription = student.MedicalRecord.DrugAllergyDescription;
            model.BloodTypeKnowledge = student.MedicalRecord.BloodTypeKnowledge;
            model.BloodType = student.MedicalRecord.BloodType;
            model.RhFactor = student.MedicalRecord.RhFactor;

            model.Smoke = student.MedicalRecord.Smoke;
            model.Liqueur = student.MedicalRecord.Liqueur;
            model.BloodTransfusions = student.MedicalRecord.BloodTransfusions;
            model.InfectiousDiseases = student.MedicalRecord.InfectiousDiseases;
            model.CompleteVaccinations = student.MedicalRecord.CompleteVaccinations;
            model.ChronicDiseases = student.MedicalRecord.ChronicDiseases;
            model.Disability = student.MedicalRecord.Disability;
            model.Accidents = student.MedicalRecord.Accidents;
            model.Intoxications = student.MedicalRecord.Intoxications;
            model.SurgeryOrHospitalization = student.MedicalRecord.SurgeryOrHospitalization;
            model.TakeMedication = student.MedicalRecord.TakeMedication;
            model.PsychologicalProblems = student.MedicalRecord.PsychologicalProblems;
            model.OtherDiseaseDescription = student.MedicalRecord.OtherDiseaseDescription;

            model.Diabetes = student.MedicalRecord.Diabetes;
            model.Obesity = student.MedicalRecord.Obesity;
            model.Cardiovascular = student.MedicalRecord.Cardiovascular;
            model.Allergy = student.MedicalRecord.Allergy;
            model.Infections = student.MedicalRecord.Infections;
            model.Cancer = student.MedicalRecord.Cancer;
            model.HomePsychologicalProblems = student.MedicalRecord.HomePsychologicalProblems;
            model.AlcoholOrDrugs = student.MedicalRecord.AlcoholOrDrugs;
            model.DomesticViolence = student.MedicalRecord.DomesticViolence;
            model.HomeOtherDiseaseDescription = student.MedicalRecord.HomeOtherDiseaseDescription;

            model.SadEmotionalLevel = student.MedicalRecord.SadEmotionalLevel;
            model.FeelMorningLevel = student.MedicalRecord.FeelMorningLevel;
            model.CryEmotionalLevel = student.MedicalRecord.CryEmotionalLevel;
            model.NightDreamProblemLevel = student.MedicalRecord.NightDreamProblemLevel;
            model.SameEmotionLevel = student.MedicalRecord.SameEmotionLevel;
            model.SexEnjoyLevel = student.MedicalRecord.SexEnjoyLevel;
            model.WeightKnowledgeLevel = student.MedicalRecord.WeightKnowledgeLevel;
            model.ConstipationProblemLevel = student.MedicalRecord.ConstipationProblemLevel;
            model.HeartProblemLevel = student.MedicalRecord.HeartProblemLevel;
            model.TirednessLevel = student.MedicalRecord.TirednessLevel;
            model.ClearMindLevel = student.MedicalRecord.ClearMindLevel;
            model.RythmChangeLevel = student.MedicalRecord.RythmChangeLevel;
            model.AgitatedLevel = student.MedicalRecord.AgitatedLevel;
            model.HopefulLevel = student.MedicalRecord.HopefulLevel;
            model.IrritationLevel = student.MedicalRecord.IrritationLevel;
            model.VelocityDecisionLevel = student.MedicalRecord.VelocityDecisionLevel;
            model.UsefulLevel = student.MedicalRecord.UsefulLevel;
            model.SatisfactionLevel = student.MedicalRecord.SatisfactionLevel;
            model.SuicideLevel = student.MedicalRecord.SuicideLevel;
            model.EnjoymentLevel = student.MedicalRecord.EnjoymentLevel;

            var viewToString = await _viewRenderService.RenderToStringAsync("/Views/Pdf/PrintMedicalReport.cshtml", model);
            var cssPath = Path.Combine(_hostingEnvironment.WebRootPath, @"css/views/studentinformation/printsocioeconomicrecord.css");

            var globalSettings = new DinkToPdf.GlobalSettings
            {
                ColorMode = DinkToPdf.ColorMode.Color,
                Orientation = DinkToPdf.Orientation.Portrait,
                PaperSize = DinkToPdf.PaperKind.A4,
                Margins = new DinkToPdf.MarginSettings { Top = 10, Bottom = 0, Left = 10, Right = 10 }
            };

            var objectSettings = new DinkToPdf.ObjectSettings
            {
                PagesCount = true,
                HtmlContent = viewToString,
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = cssPath },
                FooterSettings = {
                    FontName = "Arial",
                    FontSize = 9,
                    Line = false,
                    Left = $"Fecha : {DateTime.UtcNow.ToLocalDateTimeFormat()}",
                    Center = "",
                    Right = "Pág: [page]/[toPage]"
                }
            };

            var pdf = new DinkToPdf.HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            var fileByte = _dinkConverter.Convert(pdf);

            HttpContext.Response.Headers["Set-Cookie"] = "fileDownload=true; path=/";
            HttpContext.Response.Headers["Content-Disposition"] = $"attachment; filename= ficha_medica_{student.User.UserName}.pdf";
            return File(fileByte, "application/octet-stream");
        }

        private byte[] GenerarQR(Guid studentId)
        {
            QRCoder.QRCodeGenerator qrGenerator = new QRCoder.QRCodeGenerator();
            var URLAbsolute = $"Ficha_medica_{studentId}";
            //URLAbsolute = HttpContext.Request.Host + "/ficha-socioeconomica/ver/" + studentId;
            QRCoder.QRCodeData qrCodeData = qrGenerator.CreateQrCode(URLAbsolute, QRCoder.QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.PngByteQRCode(qrCodeData);
            //var stream = new MemoryStream();
            //qrCode.GetGraphic(5).Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            //return stream.ToArray();
            return qrCode.GetGraphic(5);
        }
    }
}
