using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeaHelperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificacaoController : ControllerBase
    {
        [HttpGet("notificacoes-usuario/{idusuarioLogado}")]
        public IActionResult NotificacoesUsuario(int idusuarioLogado)
        {
            try
            {
                List<Notificacao> notificacoes = new List<Notificacao>();
                if (idusuarioLogado != 0)
                {
                    notificacoes = Notificacao_P1.TodasNotificacoesUsuarioAtiva(idusuarioLogado);

                    foreach (var not in notificacoes)
                    {
                        Regex regex = new Regex(@".+evento\/visualizar\/");
                        not.Id_Evento = Convert.ToInt32(regex.Replace(not.Url_Notificacao, ""));
                    }
                }
                return Ok(notificacoes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }

        [HttpPost("notificacao-visualizada/{idNotificacao}")]
        public IActionResult NotificacaoVisualizada(int idNotificacao)
        {
            try
            {
                Notificacao_P1 notificacao = new Notificacao_P1(idNotificacao);
                notificacao.CompleteObject();
                notificacao.Flg_Visualizado = true;
                notificacao.Save();

                return Ok(notificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }

        [HttpPost("postnotificacao/{IdUsuarioNotificou}/{IdUsuarioNotificado}/{IdEvento}/{Mensagem}")]
        public IActionResult PostNotificacao(int IdUsuarioNotificou, int IdUsuarioNotificado, int IdEvento, string Mensagem)
        {
            try
            {
                Notificacao_P1 notificacao = new Notificacao_P1();
                notificacao.IdUsuarioNotificado = IdUsuarioNotificado;
                notificacao.IdUsuarioNotificou = IdUsuarioNotificou;
                notificacao.Descricao = Mensagem;
                notificacao.NotificacaoAtiva = true;
                notificacao.UrlNotificacao = "https://localhost:44394/evento/visualizar/" + IdEvento;
                notificacao.DataCadastro = DateTime.Now;
                notificacao.Save();

                return Ok(notificacao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }

        [HttpGet("quantidade-notificacao/{idUsuario}")]
        public int QtdNotificacao(int idUsuario)
        {
            if (idUsuario > 0)
                return Notificacao_P1.CountTodasNotificacoesUsuarioAtiva(idUsuario);
            else
                return 0;
        }

    }
}
