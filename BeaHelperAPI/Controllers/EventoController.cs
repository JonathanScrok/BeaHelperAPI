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
        [HttpGet("{idvaga}")]
        public IActionResult GetEvento(int idvaga)
        {
            try
            {
                if (idvaga > 0)
                {
                    bool ExisteVaga = Vaga_P2.ExisteVaga(idvaga);
                    if (ExisteVaga)
                    {
                        var vaga = _vagaService.CarregaVaga(idvaga);
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

        #region Get Top 8 Evento
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

        #region Get Todos eventos
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
    }
}
