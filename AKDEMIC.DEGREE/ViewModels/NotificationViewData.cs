namespace AKDEMIC.DEGREE.ViewModels
{
    public class NotificationViewData
    {
        public string Message { get; set; }
        public string Icon { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string Template { get; set; }

        public class NotificationType
        {
            public const string SUCCESS = "success";
            public const string WARNING = "warning";
            public const string INFO = "info";
            public const string ERROR = "danger";
        }

        public class DefaultIcon
        {
            public const string SUCCESS = "la la-check-circle";
            public const string WARNING = "la la-exclamation";
            public const string ERROR = "la la-exclamation-triangle";
            public const string INFO = "la la-info";
        }

        public class DefaultTitle
        {
            public const string SUCCESS = "EXITO";
            public const string ERROR = "ERROR";
            public const string WARNING = "AVISO";
            public const string INFO = "INFO";
        }
    }
}
