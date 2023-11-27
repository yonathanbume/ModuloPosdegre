// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using AKDEMIC.INTRANET.Services.EvaluationReportGenerator.Models;
using AKDEMIC.REPOSITORY.Repositories.Intranet.Templates.EvaluationReport;

namespace AKDEMIC.INTRANET.Services.EvaluationReportGenerator
{
    public interface IEvaluationReportGeneratorService
    {
        Task<Result> GetActEvaluationReport(Guid sectionId, int? code = null, string issueDate = null, string receptionDate = null);
        Task<Result> GetActEvaluationReportPreview(byte formatType);
        Task<Result> GetRegisterEvaluationReport(Guid sectionId);
        Task<Result> GetActEvaluationReportExtraordinaryEvaluation(Guid extraordinaryEvaluationId);
        Task<Result> GetActEvaluationReportDeferredExam(Guid deferredExamId);
        Task<Result> GetActEvaluationReportCorrectionExam(Guid correctionExamId);
    }
}
