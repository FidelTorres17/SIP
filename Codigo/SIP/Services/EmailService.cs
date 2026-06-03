using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SIP.Models;


namespace SIP.Services
{
    public class EmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(IOptions<EmailSettings> settings)
        {
            _settings = settings.Value;
        }

        public void EnviarCorreo(
            string destino,
            string asunto,
            string cuerpoHtml)
        {
            var email = new MimeMessage();

            email.From.Add(
                new MailboxAddress(
                    _settings.FromName,
                    _settings.FromEmail
                )
            );

            email.To.Add(
                MailboxAddress.Parse(destino)
            );

            email.Subject = asunto;

            var builder = new BodyBuilder
            {
                HtmlBody = cuerpoHtml
            };

            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(
                _settings.Host,
                _settings.Port,
                SecureSocketOptions.StartTls
            );

            smtp.Authenticate(
                _settings.User,
                _settings.Password
            );

            smtp.Send(email);

            smtp.Disconnect(true);
        }

        // ==============================
        // BIENVENIDA
        // ==============================
        public void EnviarBienvenidaUsuario(
            string destino,
            string nombre,
            string usuario,
            string passwordTemporal)
        {
            string asunto = "Bienvenido a SIP";

            string html = $@"
            <div style='font-family:Segoe UI;background:#FBF6F1;padding:30px;'>

                <div style='max-width:600px;margin:auto;background:white;
                            border-radius:20px;padding:30px;
                            border:1px solid #eadfce;'>

                    <h2 style='color:#7A4A2E;'>
                        Bienvenido a SIP
                    </h2>

                    <p>
                        Hola <b>{nombre}</b>,
                    </p>

                    <p>
                        Tu cuenta ha sido creada correctamente.
                    </p>

                    <div style='background:#F3E6D8;
                                padding:18px;
                                border-radius:12px;
                                margin:20px 0;'>

                        <p><b>Usuario:</b> {usuario}</p>
                        <p><b>Contraseña temporal:</b> {passwordTemporal}</p>

                    </div>

                    <p>
                        Por seguridad cambia tu contraseña
                        después de iniciar sesión.
                    </p>

                <!--    <div style='text-align:center;margin-top:25px;'>

                        <a href='{_settings.LoginUrl}'
                           style='background:#7A4A2E;
                                  color:white;
                                  padding:14px 22px;
                                  border-radius:12px;
                                  text-decoration:none;
                                  font-weight:bold;'>

                            Ir al Login

                        </a>

                    </div> -->

                </div>

            </div>";

            EnviarCorreo(
                destino,
                asunto,
                html
            );
        }

        // ==============================
        // RECUPERAR PASSWORD
        // ==============================
        public void EnviarRecuperacion(
            string destino,
            string nombre,
            string usuario,
            string passwordTemporal)
        {
            string asunto = "Recuperación de contraseña - SIP";

            string html = $@"
            <div style='font-family:Segoe UI;background:#FBF6F1;padding:30px;'>

                <div style='max-width:600px;margin:auto;background:white;
                            border-radius:20px;padding:30px;
                            border:1px solid #eadfce;'>

                    <h2 style='color:#7A4A2E;'>
                        Recuperación de contraseña
                    </h2>

                    <p>
                        Hola <b>{nombre}</b>,
                    </p>

                    <p>
                        Se generó una nueva contraseña temporal:
                    </p>

                    <div style='background:#F3E6D8;
                                padding:18px;
                                border-radius:12px;
                                margin:20px 0;'>

                        <p><b>Usuario:</b> {usuario}</p>
                        <p><b>Password temporal:</b> {passwordTemporal}</p>

                    </div>

                <!--    <div style='text-align:center;margin-top:25px;'>

                        <a href='{_settings.LoginUrl}'
                           style='background:#7A4A2E;
                                  color:white;
                                  padding:14px 22px;
                                  border-radius:12px;
                                  text-decoration:none;
                                  font-weight:bold;'>

                            Ir al Login

                        </a>

                    </div> -->

                </div>

            </div>";

            EnviarCorreo(
                destino,
                asunto,
                html
            );
        }
    }
}
