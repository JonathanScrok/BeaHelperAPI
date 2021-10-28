using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeaHelperAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        //200 OK
        //202 Aceito
        //204 Sem Conteúdo
        //400 Solicitação incorreta
        //404 não encontrado

        //return NotFound(); Não encontrado;
        //return BadRequest(); Status 400 contém erros
        //return Ok(); Status 200 -  operação deu certo poderá passar um objeto como parametro

        private readonly ILogger<LoginController> _logger;
        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }


        #region Get Login por ID
        [HttpGet("{idlogin}")]
        public IActionResult GetLogin(int idlogin)
        {
            try
            {
                if (idlogin > 0)
                {
                    bool ExisteLogin = Login_P1.BuscaLogin(idlogin);

                    if (ExisteLogin)
                    {
                        var login = _loginService.CarregaLogin(idlogin);
                        return Ok(login);
                    }
                    else
                    {
                        return NotFound("Login não encontrado");
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

        #region GET logincompleto/{email}/{senha}
        [HttpGet("logincompleto/{email}/{senha}")]
        public IActionResult GetLogin(string email, string senha)
        {
            try
            {
                //Login model = new Login();

                var model = Login_P1.BuscaLogin_EmailSenha(email, senha);

                if (model.Count > 0)
                {
                    return Ok(model);
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

        #region GET existe/{email}/{senha}
        [HttpGet("existe/{email}/{senha}")]
        public IActionResult ExistenciaLogin(string email, string senha)
        {
            try
            {
                //Login model = new Login();

                var existe = Login_P1.ExisteLogin(email, senha);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                return BadRequest();
                throw;
            }
        }
        #endregion

        #region PostLogin
        [HttpPost]
        public IActionResult PostLogin(Login login)
        {
            try
            {
                if (login != null)
                {
                    bool ExisteEmail = Login_P1.BuscaLogin_Email(login.Email);

                    if (!ExisteEmail)
                    {
                        _loginService.CadastrarLoginBanco(login);
                        return Ok("Success");
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, "Email já cadastrado.");
                    }
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception)
            {
                return NotFound();
                throw;
            }
        }
        #endregion

        #region UpdateLogin
        [HttpPut]
        public IActionResult UpdateLogin(Login login)
        {
            try
            {
                if (login != null && login.Id_Login > 0)
                {
                    bool ExisteLogin = Login_P1.BuscaLogin(login.Id_Login);

                    if (ExisteLogin)
                    {
                        _loginService.AtualizarLoginBanco(login);
                        return Ok();
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

        #region DeleteLogin
        [HttpDelete("{idlogin}")]
        public IActionResult DeleteUsuario(int idlogin)
        {
            try
            {
                if (idlogin > 0)
                {
                    bool ExisteUsuario = Login_P1.BuscaLogin(idlogin);

                    if (ExisteUsuario)
                    {
                        try
                        {
                            Login_P1.Delete(idlogin);
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
