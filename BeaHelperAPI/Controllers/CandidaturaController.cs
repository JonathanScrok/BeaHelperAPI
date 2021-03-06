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
    public class CandidaturaController : ControllerBase
    {
        private readonly ILogger<CandidaturaController> _logger;

        public CandidaturaController(ILogger<CandidaturaController> logger)
        {
            _logger = logger;
        }

        #region Get Candidatura por IDCandidatura
        /// <summary>
        /// Busca de Candidatura por Id_Candidatura.
        /// </summary>
        [HttpGet("{idcandidatura}")]
        public IActionResult GetCandidatura(int idcandidatura)
        {
            try
            {
                if (idcandidatura > 0)
                {
                    bool ExisteCandidatura = EventoCandidaturas_P1.ExisteCandidatura(idcandidatura);
                    if (ExisteCandidatura)
                    {
                        var evento = _candidaturaService.CarregaCandidatura(idcandidatura);
                        return Ok(evento);
                    }
                    else
                    {
                        return NotFound("Candidatura não encontrada");
                    }
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

        #region Get Candidaturas do Usuario por IdUsuario
        /// <summary>
        /// Busca Eventos Voluntariados do Usuário por IdUsuario.
        /// </summary>
        [HttpGet("/eventosvoluntariados/{IdUsuario}")]
        public IActionResult GetEventosVoluntariadosUsuario(int IdUsuario)
        {
            try
            {
                if (IdUsuario > 0)
                {
                    var listEventos = CarregaEventosCandidatados(IdUsuario);

                    if (listEventos.Count > 0)
                        return Ok(listEventos);
                    else
                        return NotFound("Nenhum evento voluntariado!");
                }
                else
                {
                    return BadRequest("Necessário IdUsuario maior que 0(zero)");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }

        private List<Evento_P1> CarregaEventosCandidatados(int IdUsuarioLogado)
        {
            List<EventoCandidatura> EventosCandidatados = EventoCandidaturas_P1.TodasCandidaturasUsuario(IdUsuarioLogado);

            List<Evento_P1> MinhasCandidaturas = new List<Evento_P1>();
            List<int> Idfvagas = new List<int>();

            for (int i = 0; i < EventosCandidatados.Count; i++)
            {
                var idf = EventosCandidatados[i].Id_Evento;
                Idfvagas.Add(idf);
            }

            foreach (var Id in Idfvagas)
            {
                Evento_P1 evento = new Evento_P1(Id);
                evento.CompleteObject();
                MinhasCandidaturas.Add(evento);
            }

            return MinhasCandidaturas;
        }

        #endregion

        #region Get Candidatura por Id_Usuario e Id_Evento
        /// <summary>
        /// Busca Candidatura pelo Id_Usuario e id_Evento.
        /// </summary>
        [HttpGet("{idusuario}/{idevento}")]
        public IActionResult GetCandidaturaIdUsuIdEvento(int idusuario, int idevento)
        {
            try
            {
                if (idusuario > 0 && idevento > 0)
                {
                    var candidatura = EventoCandidaturas_P1.CandidaturasUsuarioEvento(idusuario, idevento);
                    if (candidatura.Count > 0)
                    {
                        return Ok(candidatura);
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

        #region Get Todas Candidaturas por Id_Evento
        /// <summary>
        /// Busca todas candidaturas do id_Evento.
        /// </summary>
        [HttpGet("/todascandidaturas/{idevento}")]
        public IActionResult GetTodasCandidaturasEvento(int idevento)
        {
            try
            {
                if (idevento > 0)
                {
                    var candidatura = EventoCandidaturas_P1.TodasUsuarioCandidatadosEvento(idevento);

                    if (candidatura.Count > 0)
                    {
                        return Ok(candidatura);
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

        #region PostCandidatura
        /// <summary>
        /// Insert de Candidatura.
        /// </summary>
        [HttpPost]
        public IActionResult PostCandidatura(EventoCandidatura candidatura)
        {
            try
            {
                if (candidatura != null)
                {
                    bool ExisteCandidatura = EventoCandidaturas_P1.ExisteCandidatura(candidatura.Id_Evento, candidatura.Id_Usuario);

                    if (!ExisteCandidatura)
                    {
                        _candidaturaService.CadastrarCandidaturaBanco(candidatura);
                        return Ok();
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, "Candidatura já cadastrada.");
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

        #region UpdateCandidatura
        /// <summary>
        /// Update de Candidatura.
        /// </summary>
        [HttpPut]
        public IActionResult UpdateCandidatura(EventoCandidatura candidatura)
        {
            try
            {
                if (candidatura != null && candidatura.Id_Candidatura > 0)
                {
                    bool ExisteCandidatura = EventoCandidaturas_P1.ExisteCandidatura(candidatura.Id_Candidatura);

                    if (ExisteCandidatura)
                    {
                        try
                        {
                            _candidaturaService.AtualizarCandidaturaBanco(candidatura);
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

        #region DeleteCandidatura
        /// <summary>
        /// Delete de Candidatura por Id_Candidatura).
        /// </summary>
        [HttpDelete("{idcandidatura}")]
        public IActionResult DeleteCandidatura(int idcandidatura)
        {
            try
            {
                if (idcandidatura > 0)
                {
                    bool ExisteCandidatura = EventoCandidaturas_P1.ExisteCandidatura(idcandidatura);

                    if (ExisteCandidatura)
                    {
                        try
                        {
                            EventoCandidaturas_P1.Delete(idcandidatura);
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
