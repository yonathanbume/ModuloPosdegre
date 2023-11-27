using System;

namespace AKDEMIC.CORE.Helpers
{
    public class EmailHelpers
    {
        public const int PASSWORD_RECOVERY = 1;
        public static string theme = GeneralHelpers.GetInstitutionAbbreviation().ToLower();
        public static string logo_company = "/images/themes/" + theme.ToLower() + "/logo.png";
        //public static string buttonColor = ConstantHelpers.THEME_PARAMETERS.COLORS[GeneralHelpers.GetInstitutionAbbreviation()];
        public String ForgotPassword(string projectName, string url, string recoveryLink)
        {
            var h4Text = MESSAGES.RECOVERY_PASSWORD.h4Text(projectName);
            var subText1 = MESSAGES.RECOVERY_PASSWORD.subText1;
            var subText2 = MESSAGES.RECOVERY_PASSWORD.subText2;
            var btnText = MESSAGES.RECOVERY_PASSWORD.btnText;
            return EmailFormat(url, recoveryLink, projectName, h4Text, subText1, subText2, btnText);
        }

        public String NewUserRegisterNotification(string systemUrl, string projectName, string userName, string password)
        {
            string a = "<div class='example-emails margin-bottom-50'>" +
            "<div width='100%' style='background:#ebedf2; padding: 50px 20px; color: #514d6a;'>" +
                "<div style='max-width: 1000px; margin: 0px auto; font-size: 14px'>" +
                    "<table border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 20px'>" +
                        "<tr>" +
                            "<td style = 'vertical-align: top;'>" +
                                "<img src='" + systemUrl + "/images/themes/" + theme + "/logo.png' style='height:65px; margin-top:-17px' />" +
                            "</td>" +
                        "</tr>" +
                    "</table>" +
                    "<div style='padding: 40px 40px 20px 40px; background: #fff;'>" +
                        "<table border= '0' cellpadding= '0' cellspacing= '0' style= 'width: 100%;'>" +
                            "<tbody>" +
                                "<tr>" +
                                    "<td>" +
                                        "<div style='text-align: center'>" +
                                            "<h4 style='margin-bottom: 20px; color: #24222f; font-weight: 600;font-size: 24px;margin-top: 15px;'>Estimado Usuario, se ha registrado en " + projectName + "</h4>" +
                                        "</div>" +
                                        "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                            "Se ha registrado un nuevo usuario en nuestro sistema." +
                                        "</div>" +
                                        "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                            "<b> Usuario </b> : " + userName +
                                        "</div>" +
                                         "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                            "<b> Contraseña </b> : " + password +
                                        "</div>" +
                                        "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                            "Podrás acceder al sistema al hacer click en el botón que te enviamos en este correo, asi mismo te sugerimos cambiar tu contraseña una vez logeado en el sistema <b>te recordamos que este es un mensaje automático y no es necesario responder a este correo.</b> " +
                                        "</div>" +
                                        "<div style='text-align: center'>" +
                                            "<a href='" + systemUrl + "' style='display:inline-block;padding:11px 30px;margin:20px 0px 30px;font-size:15px;color:#fff;background-color:#2c2e49;border-radius:5px;text-decoration:none'>" +
                                                "Acceder al Sistema" +
                                            "</a>" +
                                        "</div>" +
                                        "<div style='text-align: center;color:#a09bb9'>" +
                                            "<p style='line-height:17px;margin:0px'> Powered by Oficina de Tecnologías de Información </p>" +
                                        "</div>" +
                                    "</td>" +
                                "</tr>" +
                            "</tbody>" +
                        "</table>" +
                    "</div>" +
                "</div>" +
            "</div>" +
        "</div>";
            return a;
        }

        public string EmailFormat(string systemUrl, string callbackUrl, string projectName, string h4Text, string subText1, string subText2, string btnText)
        {
            string a = "<div class='example-emails margin-bottom-50'>" +
          "<div width='100%' style='background:#ebedf2; padding: 50px 20px; color: #514d6a;'>" +
              "<div style='max-width: 1000px; margin: 0px auto; font-size: 14px'>" +
                  "<table border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 20px'>" +
                      "<tr>" +
                          "<td style = 'vertical-align: top;'>" +
                              "<img src='" + Flurl.Url.Combine(systemUrl, logo_company) + "' style='height:65px; margin-top:-17px' />" +
                          "</td>" +
                      "</tr>" +
                  "</table>" +
                  "<div style='padding: 40px 40px 20px 40px; background: #fff;'>" +
                      "<table border= '0' cellpadding= '0' cellspacing= '0' style= 'width: 100%;'>" +
                          "<tbody>" +
                              "<tr>" +
                                  "<td>" +
                                      "<div style='text-align: center'>" +
                                          "<h4 style='margin-bottom: 20px; color: #24222f; font-weight: 600;font-size: 24px;margin-top: 15px;'>" +
                                                h4Text +//"Estimado Usuario, has solicitado un cambio de contraseña en el sistema de " + projectName + 
                                          "</h4>" +
                                      "</div>" +
                                      "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                          subText1 + //"Si no fuiste tú, comunicate con la oficina de tecnología de la Universidad." +
                                      "</div>" +
                                      "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                          subText2 +//"Podrás recuperar tu contraseña al hacer click en el botón que te enviamos en este correo, <b>te recordamos que este es un mensaje automático y no es necesario responder a este correo.</b> " +
                                      "</div>" +
                                      "<div style='text-align: center'>" +
                                          "<a href='" + callbackUrl + "' style='display:inline-block;padding:11px 30px;margin:20px 0px 30px;font-size:15px;color:#fff;background-color:#2c2e49;border-radius:5px;text-decoration:none'>" +
                                              btnText/*"Recuperar Contraseña"*/ +
                                          "</a>" +
                                      "</div>" +
                                      "<div style='text-align: center;color:#a09bb9'>" +
                                      //"<p style='line-height:17px;margin:0px'> Powered by Oficina de Tecnologías de Información </p>" +
                                      "</div>" +
                                  "</td>" +
                              "</tr>" +
                          "</tbody>" +
                      "</table>" +
                  "</div>" +
              "</div>" +
          "</div>" +
      "</div>";
            return a;
        }

        public string ConfirmEmail(string systemUrl, string callbackUrl, string projectName)
        {
            string a = "<div class='example-emails margin-bottom-50'>" +
        "<div width='100%' style='background:#ebedf2; padding: 50px 20px; color: #514d6a;'>" +
            "<div style='max-width: 1000px; margin: 0px auto; font-size: 14px'>" +
                "<table border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 20px'>" +
                    "<tr>" +
                        "<td style = 'vertical-align: top;'>" +
                            "<img src='" + Flurl.Url.Combine(systemUrl, logo_company) + "' style='height:65px; margin-top:-17px' />" +
                        "</td>" +
                    "</tr>" +
                "</table>" +
                "<div style='padding: 40px 40px 20px 40px; background: #fff;'>" +
                    "<table border= '0' cellpadding= '0' cellspacing= '0' style= 'width: 100%;'>" +
                        "<tbody>" +
                            "<tr>" +
                                "<td>" +
                                    "<div style='text-align: center'>" +
                                        "<h4 style='margin-bottom: 20px; color: #24222f; font-weight: 600;font-size: 24px;margin-top: 15px;'>" +
                                              "Estimado Usuario"+
                                        "</h4>" +
                                    "</div>" +
                                    "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                        "Para completar su registro en el sistema de " + projectName + " deberá confirmar su correo ingresando al siguiente enlace." +
                                    "</div>" +
                                    "<div style='text-align: center'>" +
                                        "<a href='" + callbackUrl + "' style='display:inline-block;padding:11px 30px;margin:20px 0px 30px;font-size:15px;color:#fff;background-color:#2c2e49;border-radius:5px;text-decoration:none'>" +
                                            "Confimar Correo" +
                                        "</a>" +
                                    "</div>" +
                                    "<div style='text-align: center;color:#a09bb9'>" +
                                    //"<p style='line-height:17px;margin:0px'> Powered by Oficina de Tecnologías de Información </p>" +
                                    "</div>" +
                                "</td>" +
                            "</tr>" +
                        "</tbody>" +
                    "</table>" +
                "</div>" +
            "</div>" +
        "</div>" +
    "</div>";
            return a;
        }

        public string EmailConfirmationCodeFormat(string systemUrl, string code)
        {
            string a = "<!DOCTYPE html><html>" +
                "<head><meta charset='utf-8'/></head><body>"+
                "<div class='example-emails margin-bottom-50'>" +
          "<div width='100%' style='background:#ebedf2; padding: 50px 20px; color: #514d6a;'>" +
              "<div style='max-width: 1000px; margin: 0px auto; font-size: 14px'>" +
                  "<table border='0' cellpadding='0' cellspacing='0' style='width: 100%; margin-bottom: 20px'>" +
                      "<tr>" +
                          "<td style = 'vertical-align: top;'>" +
                              "<img src='" + Flurl.Url.Combine(systemUrl, logo_company) + "' style='height:65px; margin-top:-17px' />" +
                          "</td>" +
                      "</tr>" +
                  "</table>" +
                  "<div style='padding: 40px 40px 20px 40px; background: #fff;'>" +
                      "<table border= '0' cellpadding= '0' cellspacing= '0' style= 'width: 100%;'>" +
                          "<tbody>" +
                              "<tr>" +
                                  "<td>" +
                                      "<div style='text-align: center'>" +
                                          "<h4 style='margin-bottom: 20px; color: #24222f; font-weight: 600;font-size: 24px;margin-top: 15px;'>" +
                                                "Necesitas verificar tu dirección de correo electrónico para poder iniciar sesión" +
                                          "</h4>" +
                                      "</div>" +
                                      "<div style='text-align: center ; margin-top:27px ; margin-bottom:20px'>" +
                                        "Por favor ingrese el código" +
                                      "</div>" +
                                      "<div style='text-align: center'>" +
                                          "<h2>"+ code +"<h2>" +
                                      "</div>" +
                                      "<div style='text-align: center;color:#a09bb9'>" +
                                      //"<p style='line-height:17px;margin:0px'> Powered by Oficina de Tecnologías de Información </p>" +
                                      "</div>" +
                                  "</td>" +
                              "</tr>" +
                          "</tbody>" +
                      "</table>" +
                  "</div>" +
              "</div>" +
          "</div>" +
      "</div>" +
      "</body></html>";
            return a;
        }

        public static class MESSAGES
        {
            public static class RECOVERY_PASSWORD
            {
                public static string h4Text(string projectName)
                {
                    return string.Format("Estimado Usuario, has solicitado un cambio de contraseña en el sistema de {0}", projectName);
                }

                public static string subText1 = "Si no fuiste tú, comunicate con la oficina de tecnología de la Universidad.";
                public static string subText2 = "Podrás recuperar tu contraseña al hacer click en el botón que te enviamos en este correo, <b>te recordamos que este es un mensaje automático y no es necesario responder a este correo.</b> ";
                public static string btnText = "Recuperar Contraseña";
            }
            public static class SURVEY_EMAIL
            {
                public static string h4Text(string member)
                {
                    return string.Format("Hola, {0}, ¡Tienes una encuesta nueva!", member);
                }

                public static string subText1(string projectName)
                {
                    return "Te invitamos a responder la encuesta en el sistema de " + projectName + ".";
                }

                public static string subText2 = "Podrás ingresar al sistema a travéz del siguiente enlace.</b> ";
                public static string btnText = "Responder Encuesta";
            }
        }
    }
}
