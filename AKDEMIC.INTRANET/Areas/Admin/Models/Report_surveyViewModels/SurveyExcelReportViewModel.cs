// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AKDEMIC.INTRANET.Areas.Admin.Models.Report_surveyViewModels
{
    public class SurveyExcelReportViewModel
    {
        public string SurveyItemTitle { get; set; }
        public bool IsLikert { get; set; }
        public List<SurveyExcelQuestionsReportViewModel> QuestionReport { get; set; }
    }

    public class SurveyExcelQuestionsReportViewModel
    {
        public string QuestionDescription { get; set; }
        public int Type { get; set; }
        public string TypeText { get; set; }
        public int TotalAnswers { get; set; }
        public List<SurveyExcelAnswersReportViewModel> AnswersReport { get; set; }
    }
    public class SurveyExcelAnswersReportViewModel
    {
        public string AnswerText { get; set; }
        public int Quantity { get; set; }
        public decimal Percentage { get; set; }
    }
}
