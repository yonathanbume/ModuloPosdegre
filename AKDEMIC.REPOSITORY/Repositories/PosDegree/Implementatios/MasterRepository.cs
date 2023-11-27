using AKDEMIC.CORE.Structs;
using AKDEMIC.ENTITIES.Models.PosDegree;
using AKDEMIC.REPOSITORY.Base;
using AKDEMIC.REPOSITORY.Data;
using AKDEMIC.REPOSITORY.Repositories.PosDegree.Interfaces;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AKDEMIC.CORE.Helpers;
using AKDEMIC.CORE.Extensions;
using AKDEMIC.ENTITIES.Models.Degree;

namespace AKDEMIC.REPOSITORY.Repositories.PosDegree.Implementatios
{
    public class MasterRepository : Repository<Master>, IMasterRepository
    {
        //ace que la clase Master tenga todo los metodo de la clase Repository
        public MasterRepository(AkdemicContext context) : base(context) { }
       
        protected static void CreateHeaderRow(IXLWorksheet worksheet)
        {
            const int position = 1;
            var column = 0;

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "CODUNIV", column);

            worksheet.Column(++column).Width = 50;
            SetHeaderRowStyle(worksheet, "RAZ_SOC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MATRI_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FAC_NOM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARR_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "ESC_POST", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "EGRES_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "APEPAT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "APEMAT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "NOMBRE", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "SEXO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DOCU_TIP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DOCU_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_BACH", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "GRAD_TITU", column);

            worksheet.Column(++column).Width = 60;
            SetHeaderRowStyle(worksheet, "DEN_GRAD", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "SEG_ESP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "TRAB_INV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "NUM_CRED", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_METADATO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROG_ESTU", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_TITULO_PED", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MOD_OBT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "PROG_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_FIN_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_MOD_TIT_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_FIN_MOD_TIT_ACREDIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_SOLICIT_GRAD_TIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "FEC_TRAB_GRAD_TIT", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "TRAB_INVEST_ORIGINAL", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "MOD_EST", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "ABRE_GYT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_PAIS", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_UNIV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_REV_GRADO", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "CRIT_REV", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "RESO_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "RESO_FEC", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_FEC_ORG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_FEC_DUP", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_NUM", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "DIPL_TIP_EMI", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_LIBRO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_FOLIO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_REGISTRO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO1", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD1", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO2", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD2", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "CARGO3", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "AUTORIDAD3", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_PAIS_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_UNIV_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "PROC_GRADO_EXT", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "REG_OFICIO", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_MAT_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_INICIO_PROG", column);

            worksheet.Column(++column).Width = 20;
            SetHeaderRowStyle(worksheet, "FEC_FIN_PROG", column);

            worksheet.Column(++column).Width = 30;
            SetHeaderRowStyle(worksheet, "MOD_SUSTENTACION", column);

            worksheet.SheetView.FreezeRows(position);
        }
        protected static void SetHeaderRowStyle(IXLWorksheet worksheet, string headerName, int column)
        {
            const int position = 1;
            var fillColor = XLColor.FromArgb(0x0c618c);
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);
            var fontColor = XLColor.White;
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            const XLAlignmentHorizontalValues alignmentHorizontal = XLAlignmentHorizontalValues.Left;

            worksheet.Column(column).Style.Alignment.Horizontal = alignmentHorizontal;
            worksheet.Cell(position, column).Value = headerName;
            worksheet.Cell(position, column).Style.Font.Bold = true;
            worksheet.Cell(position, column).Style.Font.FontColor = fontColor;
            worksheet.Cell(position, column).Style.Fill.BackgroundColor = fillColor;
            worksheet.Cell(position, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(position, column).Style.Border.OutsideBorderColor = outsideBorderColor;



        }
        protected static void SetInformationStyle(IXLWorksheet worksheet, int row, int column, string data, bool requireDateFormat = false)
        {
            const XLBorderStyleValues outsideBorder = XLBorderStyleValues.Thin;
            var outsideBorderColor = XLColor.FromArgb(0x55acd8);

            if (requireDateFormat)
            {
                worksheet.Cell(row, column).Style.DateFormat.Format = "dd/MM/yyyy";
            }

            worksheet.Cell(row, column).Value = data;
            worksheet.Cell(row, column).Style.Border.OutsideBorder = outsideBorder;
            worksheet.Cell(row, column).Style.Border.OutsideBorderColor = outsideBorderColor;
        }
      /* private async Task LoadRegistryPatternInformationSunedu(IXLWorksheet worksheet)
        {
            var row = 2;
            const int CODUNIV = 1;    //CODUNIV
            const int RAZ_SOC = 2;    //RAZ_SOC 
            const int MATRI_FEC = 3; //MATRI_FEC 
            const int FAC_NOM = 4;  //FAC_NOM
            const int CARR_PROG = 5;    //CARR_PROG
            const int ESC_POST = 6;    //ESC_POST
            const int EGRES_FEC = 7; //EGRES_FEC
            const int APEPAT = 8;     //
            const int APEMAT = 9;     // 
            const int NOMBRE = 10;    //
            const int SEXO = 11;      //
            const int DOCUTIP = 12;   //
            const int DOCU_NUM = 13;  //  
            const int PROC_BACH = 14;  //PROC_BACH
            const int GRAD_TITU = 15;   //GRAD_TITU
            const int DEN_GRAD = 16;  //DEN_GRAD
            const int SEG_ESP = 17; //SEG_ESP
            const int TRAB_INV = 18; //TRAB_INV
            const int NUM_CRED = 19; //NUM_CRED
            const int REG_METADATO = 20; //REG_METADATO
            const int PROG_ESTU = 21; //PROG_ESTU
            const int PROC_TITULO_PED = 22; //PROC_TITULO_PED
            const int MOD_OBT = 23; //MOD_OBT
            const int PROG_ACREDIT = 24;
            const int FEC_INICIO_ACREDIT = 25;
            const int FEC_FIN_ACREDIT = 26;
            const int FEC_INICIO_MOD_TIT_ACREDIT = 27;
            const int FEC_FIN_MOD_TIT_ACREDIT = 28;
            const int FEC_SOLICIT_GRAD_TIT = 29;
            const int FEC_TRAB_GRAD_TIT = 30;
            const int TRAB_INVEST_ORIGINAL = 31;
            const int MOD_EST = 32; //MOD_EST
            const int ABRE_GYT = 33; //ABRE_GYT
            const int PROC_REV_PAIS = 34; //PROC_REV_PAIS
            const int PROC_REV_UNIV = 35; //PROC_REV_UNIV
            const int PROC_REV_GRADO = 36; //PROC_REV_GRADO
            const int CRIT_REVL = 37;
            const int RESO_NUM = 38; //RESO_NUM
            const int RESO_FEC = 39; //RESO_FEC
            const int DIPL_FEC_ORG = 40; //DIPL_FEC_ORG
            const int DIPL_FEC_DUP = 41; //DIPL_FEC_DUP
            const int DIPL_NUM = 42; //DIPL_NUM
            const int DIPL_TIP_EMI = 43; //DIPL_TIP_EMI
            const int REG_LIBRO = 44; //REG_LIBRO
            const int REG_FOLIO = 45; //REG_FOLIO
            const int REG_REGISTRO = 46; //REG_REGISTRO
            const int CARGO1 = 47; //CARGO1
            const int AUTORIDAD1 = 48; //AUTORIDAD1
            const int CARGO2 = 49; //CARGO2
            const int AUTORIDAD2 = 50; //AUTORIDAD2
            const int CARGO3 = 51; //CARGO3
            const int AUTORIDAD3 = 52; //AUTORIDAD3
            const int PROC_PAIS_EXT = 53; //PROC_PAIS_EXT
            const int PROC_UNI_EXT = 54; //PROC_UNI_EXT
            const int PROC_GRADO_EXT = 55; //PROC_GRADO_EXT
            const int REG_OFICIO = 56; //REG_OFICIO
            const int FEC_MAT_PROG = 57; //FEC_MAT_PROG
            const int FEC_INICIO_PROG = 58; //FEC_INICIO_PROG
            const int FEC_FIN_PROG = 59;
            const int MOD_SUSTENTACION = 60;

            var query = _context.Masters
                .Where(x => x.UniversityCode != null && x.Status == ConstantHelpers.REGISTRY_PATTERN.STATUS.APPROVED)
                .AsQueryable();

         if (!String.IsNullOrEmpty(searchBookNumber))
            {
                query = query.Where(x => x.BookCode.ToLower().Contains(searchBookNumber.ToLower()));
            }

            if (!String.IsNullOrEmpty(dateStartFilter) && !String.IsNullOrEmpty(dateEndFilter))
            {
                var dateStartDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateStartFilter);
                var dateEndDateTime = ConvertHelpers.DatepickerToUtcDateTime(dateEndFilter);
                query = query.Where(x => (dateStartDateTime.Date <= x.CreatedAt.Value.Date) && (x.CreatedAt.Value.Date <= dateEndDateTime.Date));

            }

            var queryList = await query
                .Select(x => new 
                {
                    x.Student.User.Email,
                    x.Student.User.PhoneNumber

                })
                .ToListAsync();

            if (queryList != null)
            {
                if (queryList.Count == 0)
                {
                    throw new Exception("No existe registros");
                }

            }

            foreach (var master in queryList)
            {
                SetInformationStyle(worksheet, row, CODUNIV, master.UniversityCode);
                SetInformationStyle(worksheet, row, RAZ_SOC, master.BussinesSocialReason);
                SetInformationStyle(worksheet, row, FAC_NOM, master.FacultyName);
                SetInformationStyle(worksheet, row, CARR_PROG, master.CareerName);
            }
        }*/
       
        public async Task DownloadExcel(IXLWorksheet worksheet)
        {
           CreateHeaderRow(worksheet);
         // await LoadRegistryPatternInformationSunedu(worksheet);
        }
        public async Task<List<Master>> GetAll()
        {
            return await _context.Masters.ToListAsync();
        }
        public async Task<DataTablesStructs.ReturnedData<object>> GetMasterDataTable(DataTablesStructs.SentParameters parameters1, string search)
        {
            var query = _context.Masters.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.Nombre.ToLower().Contains(search.Trim().ToLower()));
            }
            var recorFilter = await query.CountAsync();

            var data = await query.Skip(parameters1.PagingFirstRecord)
                .Take(parameters1.RecordsPerDraw).Select(x => new {
                    x.id,
                    x.Nombre,
                    x.Duracion,
                    x.Creditos,
                    x.Descripcion

                }).ToListAsync();
            var recordTotal = data.Count();
            return new DataTablesStructs.ReturnedData<object>
            {
                Data = data,
                DrawCounter = parameters1.DrawCounter,
                RecordsFiltered = recorFilter,
                RecordsTotal = recordTotal
            };
        }

        public async Task<Master> Get(Guid id)
        {
            var entity = await _context.Masters.FindAsync(id);
            return entity;
            
        }
        override
        public async Task Delete(Master maestria)
        {
            _context.Masters.Remove(maestria);
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateAsync(Master entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /*    public async Task Update( Master model)
           {
               var master = new Master
               {
                   Nombre = model.Nombre,
                   Duracion = model.Duracion,
                   Creditos = model.Creditos,
                   Descripcion = model.Descripcion,
               };

               _context.Masters.ToList; 
               await _context.SaveChangesAsync();
           }*/
    }
  
}
