using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _vagaService
    {
        public static void CadastrarVagaBanco(Vaga vagaDados)
        {
            Vaga_P1 vagaCadastrar = new Vaga_P1();

            vagaCadastrar.DataPublicacao = DateTime.Now;
            vagaCadastrar.DataEvento = vagaDados.DataEvento;
            vagaCadastrar.IdUsuarioAdm = vagaDados.Id_Usuario_Adm;
            vagaCadastrar.Titulo = vagaDados.Titulo;
            vagaCadastrar.Categoria = vagaDados.Categoria;
            vagaCadastrar.Descricao = vagaDados.Descricao;
            vagaCadastrar.CidadeEstado = vagaDados.Cidade_Estado;
            vagaCadastrar.SemData = vagaDados.SemData;
            vagaCadastrar.EventoRecorrente = vagaDados.EventoRecorrente;
            vagaCadastrar.Save();
        }

        public static void AtualizarVagaBanco(Vaga vagaDados)
        {
            Vaga_P1 vagaAtualizar = new Vaga_P1(vagaDados.Id_Vaga);

            vagaAtualizar.IdVaga = vagaDados.Id_Vaga;
            vagaAtualizar.IdUsuarioAdm = vagaDados.Id_Usuario_Adm;
            vagaAtualizar.Titulo = vagaDados.Titulo;
            vagaAtualizar.Categoria = vagaDados.Categoria;
            vagaAtualizar.Descricao = vagaDados.Descricao;
            vagaAtualizar.CidadeEstado = vagaDados.Cidade_Estado;
            vagaAtualizar.DataPublicacao = vagaDados.DataPublicacao;
            vagaAtualizar.DataEvento = vagaDados.DataEvento;
            vagaAtualizar.SemData = vagaDados.SemData;
            vagaAtualizar.EventoRecorrente = vagaDados.EventoRecorrente;
            vagaAtualizar.Save();
        }

        public static Vaga_P1 CarregaVaga(int IdVaga)
        {
            Vaga_P1 vaga = new Vaga_P1(IdVaga);
            vaga.CompleteObject();

            return vaga;
        }
    }
}
