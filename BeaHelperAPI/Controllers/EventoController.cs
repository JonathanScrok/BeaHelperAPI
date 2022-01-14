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
                    bool ExisteVaga = Vaga_P2.ExisteVaga(idevento);
                    if (ExisteVaga)
                    {
                        var vaga = _vagaService.CarregaVaga(idevento);
                        return Ok(vaga);
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
                return BadRequest();
                throw;
            }
        }
        #endregion

        #region GET existe/{idevento}
        /// <summary>
        /// (bool) Busca se o Evento existe por Id_Evento.
        /// </summary>
        [HttpGet("existe/{idevento}")]
        public IActionResult ExistenciaVaga(int idevento)
        {
            try
            {
                //Login model = new Login();

                bool ExisteVaga = Vaga_P2.ExisteVaga(idevento);
                return Ok(ExisteVaga);
            }
            catch (Exception ex)
            {
                return BadRequest();
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
                List<Vaga> vagas = Vaga_P2.Top8UltimasVagas();
                return Ok(vagas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        [HttpGet("{idevento}/{idusuarioLogado}")]
        public IActionResult ListaVoluntarios(int idevento, int idusuarioLogado)
        {

            List<VagaCandidatura> ListaUsuariosVoluntariados = VagaCandidaturas_P1.TodasUsuarioCandidatadosVaga(idevento);

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
                UsuarioCompleto.Id_Usuario = Usuario.IdUsuario;
                UsuarioCompleto.Email = Usuario.Email;
                UsuarioCompleto.Nome = Usuario.Nome;
                UsuarioCompleto.Sexo = Usuario.Sexo;

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

        #region Get Todos eventos
        /// <summary>
        /// Busca todos eventos.
        /// </summary>
        [HttpGet("todos-eventos")]
        public IActionResult GetTodosEventos()
        {
            try
            {
                List<Vaga> vagas = Vaga_P2.TodasVagas();
                return Ok(vagas);
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
                    List<Vaga> vagas = Vaga_P2.MinhasVagas(idusuarioadm);
                    if (vagas.Count > 0)
                        return Ok(vagas);
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
        public IActionResult PostEvento(Vaga vaga)
        {
            try
            {
                if (vaga != null)
                {
                    bool ExisteVagaComTitulo = Vaga_P2.ExisteTitulo(vaga.Titulo);

                    if (!ExisteVagaComTitulo)
                    {
                        _vagaService.CadastrarVagaBanco(vaga);
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
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
        #endregion

        #region UpdateEvento
        /// <summary>
        /// Update de Evento.
        /// </summary>
        [HttpPut]
        public IActionResult UpdateEvento(Vaga vaga)
        {
            try
            {
                if (vaga != null && vaga.Id_Vaga > 0)
                {
                    bool ExisteVaga = Vaga_P2.ExisteVaga(vaga.Id_Vaga);

                    if (ExisteVaga)
                    {
                        try
                        {
                            _vagaService.AtualizarVagaBanco(vaga);
                            return Ok();
                        }
                        catch (Exception)
                        {
                            return BadRequest();
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
            catch (Exception)
            {
                return BadRequest();
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
                    bool ExisteUsuario = Vaga_P2.ExisteVaga(idevento);

                    if (ExisteUsuario)
                    {
                        try
                        {
                            Vaga_P1.Delete(idevento);
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
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
        #endregion
    }
}
