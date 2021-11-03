using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using SyrusVoluntariado.Library.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _envioEmailService
    {
        public static bool EnviarCandidatoParaDonoVaga(int IdUsuarioAdm, int IdEvento, Usuario usuariovoluntatiado)
        {
            try
            {
                Usuario_P1 usuarioAdm = new Usuario_P1(IdUsuarioAdm);
                usuarioAdm.CompleteObject();

                EnviarEmail.EnviarMensagemContato(usuariovoluntatiado, usuarioAdm.Email, IdEvento);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

    }
}
