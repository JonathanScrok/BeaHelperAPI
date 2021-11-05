using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.Extensions.Logging;
using SyrusVoluntariado.Library.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _envioEmailService
    {
        private static readonly ILogger<_envioEmailService> _logger;

        public async static Task<bool> EnviarCandidatoParaDonoVaga(int IdUsuarioAdm, int IdEvento, Usuario usuariovoluntatiado)
        {
            try
            {
                Usuario_P1 usuarioAdm = new Usuario_P1(IdUsuarioAdm);
                usuarioAdm.CompleteObject();

                //EnviarEmail.EnviarMensagemContato(usuariovoluntatiado, usuarioAdm.Email, IdEvento);
                await EnviarEmail.EnviarEmailContatoAsync(usuariovoluntatiado, usuarioAdm.Email, IdEvento);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Erro service envio email:" + ex);
                return false;
            }
        }

    }
}
