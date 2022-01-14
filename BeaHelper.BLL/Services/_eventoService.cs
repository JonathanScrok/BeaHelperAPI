using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _eventoService
    {
        public static void CadastrarEventoBanco(Evento eventoDados)
        {
            Evento_P1 eventoCadastrar = new Evento_P1();

            eventoCadastrar.DataPublicacao = DateTime.Now;
            eventoCadastrar.DataEvento = eventoDados.DataEvento;
            eventoCadastrar.IdUsuarioAdm = eventoDados.Id_Usuario_Adm;
            eventoCadastrar.Titulo = eventoDados.Titulo;
            eventoCadastrar.Categoria = eventoDados.Categoria;
            eventoCadastrar.Descricao = eventoDados.Descricao;
            eventoCadastrar.CidadeEstado = eventoDados.Cidade_Estado;
            eventoCadastrar.SemData = eventoDados.SemData;
            eventoCadastrar.EventoRecorrente = eventoDados.EventoRecorrente;
            eventoCadastrar.Save();
        }

        public static void AtualizarEventoBanco(Evento eventoDados)
        {
            Evento_P1 eventoAtualizar = new Evento_P1(eventoDados.Id_Evento);

            eventoAtualizar.IdEvento = eventoDados.Id_Evento;
            eventoAtualizar.IdUsuarioAdm = eventoDados.Id_Usuario_Adm;
            eventoAtualizar.Titulo = eventoDados.Titulo;
            eventoAtualizar.Categoria = eventoDados.Categoria;
            eventoAtualizar.Descricao = eventoDados.Descricao;
            eventoAtualizar.CidadeEstado = eventoDados.Cidade_Estado;
            eventoAtualizar.DataPublicacao = eventoDados.DataPublicacao;
            eventoAtualizar.DataEvento = eventoDados.DataEvento;
            eventoAtualizar.SemData = eventoDados.SemData;
            eventoAtualizar.EventoRecorrente = eventoDados.EventoRecorrente;
            eventoAtualizar.Save();
        }

        public static Evento_P1 CarregaEvento(int IdEvento)
        {
            Evento_P1 evento = new Evento_P1(IdEvento);
            evento.CompleteObject();

            return evento;
        }
    }
}
