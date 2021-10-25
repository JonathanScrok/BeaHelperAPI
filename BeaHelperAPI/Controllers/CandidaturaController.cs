using BeaHelper.BLL.BD;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        #region Get Candidatura por ID
        [HttpGet("{idcandidatura}")]
        public IActionResult GetCandidatura(int idcandidatura)
        {
            try
            {
                if (idcandidatura > 0)
                {
                    bool ExisteCandidatura = VagaCandidaturas_P1.ExisteCandidatura(idcandidatura);
                    if (ExisteCandidatura)
                    {
                        var vaga = _candidaturaService.CarregaCandidatura(idcandidatura);
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

        #region Get Candidatura por Id_Usuario e Id_Evento
        [HttpGet("{idusuario}/{idevento}")]
        public IActionResult GetCandidaturaIdUsuIdVaga(int idusuario, int idevento)
        {
            try
            {
                if (idusuario > 0 && idevento > 0)
                {
                    var candidatura = VagaCandidaturas_P1.CandidaturasUsuarioVaga(idusuario, idevento);
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
                return BadRequest();
                throw;
            }
        }
        #endregion

        #region Get Todas Candidaturas por Id_Evento
        [HttpGet("/todascandidaturas/{idevento}")]
        public IActionResult GetTodasCandidaturasEvento(int idevento)
        {
            try
            {
                if (idevento > 0)
                {
                    var candidatura = VagaCandidaturas_P1.TodasUsuarioCandidatadosVaga(idevento);

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
                return BadRequest();
                throw;
            }
        }
        #endregion


    }
}
