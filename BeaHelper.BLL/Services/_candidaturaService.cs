using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _candidaturaService
    {
        public static void CadastrarCandidaturaBanco(EventoCandidatura candidaturaDados)
        {
            EventoCandidaturas_P1 candidatura = new EventoCandidaturas_P1();

            candidatura.IdUsuario = candidaturaDados.Id_Usuario;
            candidatura.IdEvento = candidaturaDados.Id_Evento;
            candidatura.DataCadastro = DateTime.Now;
            candidatura.Save();
        }

        public static void AtualizarCandidaturaBanco(EventoCandidatura eventoCandidatura)
        {
            EventoCandidaturas_P1 candidatura = new EventoCandidaturas_P1(eventoCandidatura.Id_Candidatura);

            candidatura.IdCandidatura = eventoCandidatura.Id_Candidatura;
            candidatura.IdUsuario = eventoCandidatura.Id_Usuario;
            candidatura.IdEvento = eventoCandidatura.Id_Evento;
            candidatura.DataCadastro = eventoCandidatura.DataCadastro;
            candidatura.Save();
        }

        public static EventoCandidaturas_P1 CarregaCandidatura(int IdLogin)
        {
            EventoCandidaturas_P1 Candidatura = new EventoCandidaturas_P1(IdLogin);
            Candidatura.CompleteObject();

            return Candidatura;
        }
    }
}
