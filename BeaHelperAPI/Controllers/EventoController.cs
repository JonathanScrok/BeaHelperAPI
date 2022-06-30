using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BeaHelperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        private readonly ILogger<EventoController> _logger;

        public EventoController(ILogger<EventoController> logger)
        {
            _logger = logger;
        }

        #region Get Evento por ID
        /// <summary>
        /// Busca evento por id_Evento.
        /// </summary>
        [HttpGet("{idevento}")]
        public IActionResult GetEvento(int idevento)
        {
            try
            {
                if (idevento > 0)
                {
                    bool ExisteEvento = Evento_P2.ExisteEvento(idevento);
                    if (ExisteEvento)
                    {
                        var evento = _eventoService.CarregaEvento(idevento);
                        return Ok(evento);
                    }
                    else
                    {
                        return NotFound("Evento não encontrado");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Evento por Filtro
        /// <summary>
        /// Busca evento por filtros
        /// </summary>
        [HttpGet("/evento/filtrarevento")]
        public IActionResult GetEventoFiltrado(string Titulo, string Descricao, string Categoria, string Local)
        {
            try
            {
                List<Evento> eventos = new List<Evento>();
                eventos = Evento_P2.FiltrarEventos(Titulo, Descricao, Categoria, Local);

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region GET existe/{idevento}
        /// <summary>
        /// (bool) Busca se o Evento existe por Id_Evento.
        /// </summary>
        [HttpGet("existe/{idevento}")]
        public IActionResult ExistenciaEvento(int idevento)
        {
            try
            {
                //Login model = new Login();

                bool ExisteEvento = Evento_P2.ExisteEvento(idevento);
                return Ok(ExisteEvento);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region Get Top 8 Evento
        /// <summary>
        /// Busca Top 8 Próximos eventos.
        /// </summary>
        [HttpGet("top8")]
        public IActionResult GetTop8UltimosEventos()
        {
            try
            {
                List<Evento> eventos = Evento_P2.Top8UltimasEventos();
                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        #region Lista de voluntarios do evento
        /// <summary>
        /// Lista de Voluntarios do evento
        /// </summary>
        [HttpGet("Listavoluntarios-evento/{idevento}/{idusuarioLogado}")]
        public IActionResult ListaVoluntarios(int idevento, int idusuarioLogado)
        {

            List<EventoCandidatura> ListaUsuariosVoluntariados = EventoCandidaturas_P1.TodasUsuarioCandidatadosEvento(idevento);
            if (ListaUsuariosVoluntariados.Count > 0)
            {


                List<UsuarioCompleto> voluntariosCompleto = new List<UsuarioCompleto>();
                List<int> IdfVoluntarios = new List<int>();

                //Lista todos ID dos usuários candidatados
                for (int i = 0; i < ListaUsuariosVoluntariados.Count; i++)
                {
                    var idf = ListaUsuariosVoluntariados[i].Id_Usuario;
                    IdfVoluntarios.Add(idf);
                }

                foreach (var IdUsu in IdfVoluntarios)
                {
                    UsuarioCompleto UsuarioCompleto = new UsuarioCompleto();

                    Usuario_P1 Usuario = new Usuario_P1(IdUsu);
                    Usuario.CompleteObject();

                    var JaAvaliado = Avaliacao_P1.BuscaIdUsuario_AvaliouEAvaliado(IdUsu, idusuarioLogado);

                    var Avaliacao = Avaliacao_P1.TodasAvaliacoesUsuario(IdUsu);
                    UsuarioCompleto.Id_Usuario = Usuario.Id_Usuario;
                    UsuarioCompleto.Email = Usuario.Email;
                    UsuarioCompleto.Nome = Usuario.Nome;
                    UsuarioCompleto.Sexo = Usuario.Sexo;
                    UsuarioCompleto.NumeroCelular = Usuario.NumeroCelular;

                    if (JaAvaliado.Count > 0)
                    {
                        UsuarioCompleto.UsuarioLogadoAvaliou = true;
                    }
                    else
                    {
                        UsuarioCompleto.UsuarioLogadoAvaliou = false;
                    }

                    if (Avaliacao.Count > 0)
                    {
                        double NotaSomadas = 0;
                        for (int i = 0; i < Avaliacao.Count; i++)
                        {
                            NotaSomadas += Avaliacao[i].Nota;
                        }
                        var media = NotaSomadas / Avaliacao.Count;
                        media = Math.Round(media, 1);
                        UsuarioCompleto.NotaMedia = media;
                        UsuarioCompleto.NuncaAvaliado = false;
                    }
                    else
                    {
                        UsuarioCompleto.NuncaAvaliado = true;
                    }

                    voluntariosCompleto.Add(UsuarioCompleto);
                }
                return Ok(voluntariosCompleto);
            }
            else
            {
                return NotFound("Nenhum Voluntário voluntariado!");
            }
        }
        #endregion

        #region Get Todos eventos
        /// <summary>
        /// Busca todos eventos.
        /// </summary>
        [HttpGet("todos-eventos/{idusuariologado}")]
        public IActionResult GetTodosEventos(int idusuariologado)
        {
            try
            {
                List<Evento> eventos = Evento_P2.TodasEventos();
                if (idusuariologado > 0)
                {
                    foreach (var evento in eventos)
                    {
                        if (evento.Id_Evento > 0)
                        {
                            var candidatura = EventoCandidaturas_P1.CandidaturasUsuarioEvento(idusuariologado, evento.Id_Evento);
                            if (candidatura.Count > 0)
                            {
                                evento.UsuarioLogadoVoluntariado = true;
                            }
                        }
                    }
                }

                return Ok(eventos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        #region Get Meus eventos
        /// <summary>
        /// Busca eventos do Usuário por Id_Usuario_Adm.
        /// </summary>
        [HttpGet("meus-eventos/{idusuarioadm}")]
        public IActionResult GetMeusEventos(int idusuarioadm)
        {
            try
            {
                if (idusuarioadm > 0)
                {
                    List<Evento> eventos = Evento_P2.MinhasEventos(idusuarioadm);
                    if (eventos.Count > 0)
                        return Ok(eventos);
                    else
                        return NotFound();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        #region PostEvento
        /// <summary>
        /// Insert de Evento.
        /// </summary>
        [HttpPost]
        public IActionResult PostEvento(Evento evento)
        {
            try
            {
                if (evento != null)
                {
                    bool ExisteEventoComTitulo = Evento_P2.ExisteTitulo(evento.Titulo);

                    if (!ExisteEventoComTitulo)
                    {
                        _eventoService.CadastrarEventoBanco(evento);
                        return Ok();
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, "Titulo já cadastrado.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region UpdateEvento
        /// <summary>
        /// Update de Evento.
        /// </summary>
        [HttpPut]
        public IActionResult UpdateEvento(Evento evento)
        {
            try
            {
                if (evento != null && evento.Id_Evento > 0)
                {
                    bool ExisteEvento = Evento_P2.ExisteEvento(evento.Id_Evento);

                    if (ExisteEvento)
                    {
                        try
                        {
                            _eventoService.AtualizarEventoBanco(evento);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex.Message);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region DeleteEvento
        /// <summary>
        /// Delete de Evento por Id_Evento.
        /// </summary>
        [HttpDelete("{idevento}")]
        public IActionResult DeleteUsuario(int idevento)
        {
            try
            {
                if (idevento > 0)
                {
                    bool ExisteUsuario = Evento_P2.ExisteEvento(idevento);

                    if (ExisteUsuario)
                    {
                        try
                        {
                            Evento_P1.Delete(idevento);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            return BadRequest(ex);
                        }
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion
    }
}
