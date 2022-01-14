using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BeaHelper.BLL.BD;
using Microsoft.Extensions.Logging;

namespace BeaHelper.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvaliarController : ControllerBase
    {
        private readonly ILogger<AvaliarController> _logger;

        [HttpGet("{nota}/{idusuarioAvaliado}/{idusuarioLogado}")]
        public IActionResult PostAvaliacao(int nota, int idusuarioAvaliado,int idusuarioLogado)
        {
            try
            {
                Avaliacao_P1 avaliacao = new Avaliacao_P1();
                avaliacao.IdUsuarioAvaliado = idusuarioAvaliado;
                avaliacao.IdUsuarioAvaliou = idusuarioLogado;
                avaliacao.Nota = nota;
                avaliacao.DataCadastro = DateTime.Now;
                avaliacao.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


    }
}
