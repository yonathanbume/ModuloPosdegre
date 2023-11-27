using System;
using System.Collections.Generic;
using System.Text;

namespace AKDEMIC.REPOSITORY.Repositories.VisaNet.Templates
{
    public class ErrorResponseTemplate
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorResponseHeader Header { get; set; }
        public ErrorResponseData Data { get; set; }
    }

    public class ErrorResponseHeader
    {
        public string ecoreTransactionUUID { get; set; }
        public string ecoreTransactionDate { get; set; }
        public string millis { get; set; }
    }

    public class ErrorResponseData
    {
        public string CURRENCY { get; set; }
        public string TRANSACTION_DATE { get; set; }
        public string TERMINAL { get; set; }
        public string ACTION_CODE { get; set; }
        public string TRACE_NUMBER { get; set; }
        public string ECI_DESCRIPTION { get; set; }
        public string ECI { get; set; }
        public string CARD { get; set; }
        public string BRAND { get; set; }
        public string MERCHANT { get; set; }
        public string STATUS { get; set; }
        public string ADQUIRENTE { get; set; }
        public string ACTION_DESCRIPTION { get; set; }
        public string ID_UNICO { get; set; }
        public string AMOUNT { get; set; }
        public string PROCESS_CODE { get; set; }
        public string TRANSACTION_ID { get; set; }
    }
}
