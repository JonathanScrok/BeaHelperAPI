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
        public static void CadastrarCandidaturaBanco(VagaCandidatura candidaturaDados)
        {
            VagaCandidaturas_P1 candidatura = new VagaCandidaturas_P1();

            candidatura.IdUsuario = candidaturaDados.Id_Usuario;
            candidatura.IdVaga = candidaturaDados.Id_Vaga;
            candidatura.DataCadastro = DateTime.Now;
            candidatura.Save();
        }

        public static void AtualizarCandidaturaBanco(VagaCandidatura vagaCandidatura)
        {
            VagaCandidaturas_P1 candidatura = new VagaCandidaturas_P1();

            candidatura.IdCandidatura = vagaCandidatura.Id_Candidatura;
            candidatura.IdUsuario = vagaCandidatura.Id_Usuario;
            candidatura.IdVaga = vagaCandidatura.Id_Vaga;
            candidatura.DataCadastro = candidatura.DataCadastro;
            candidatura.Save();
        }

        public static VagaCandidaturas_P1 CarregaCandidatura(int IdLogin)
        {
            VagaCandidaturas_P1 Candidatura = new VagaCandidaturas_P1(IdLogin);
            Candidatura.CompleteObject();

            return Candidatura;
        }
    }
}
