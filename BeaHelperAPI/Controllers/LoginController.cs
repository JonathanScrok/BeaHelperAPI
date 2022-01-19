using AutoMapper;
using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using BeaHelper.BLL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;


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
        /// <summary>
        /// Busca Login por Id_Login.
        /// </summary>
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
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region GET login-completo/{email}/{senha}
        /// <summary>
        /// Busca LoginCompleto por email e senha.
        /// </summary>
        [HttpGet("login-completo/{email}/{senha}")]
        public IActionResult GetLogin(string email, string senha)
        {
            try
            {
                string senhaEncoding = _encodeSenha.HashValue(senha);
                var model = Login_P1.BuscaLogin_EmailSenha(email, senhaEncoding);

                if (model.Count > 0)
                {

                    Mapper.CreateMap<Usuario_P1, UsuarioCompleto>();

                    Usuario_P1 usuario = new Usuario_P1(model[0].Email);
                    usuario.CompleteObjectEmail();

                    var usuarioCompleto = Mapper.Map<UsuarioCompleto>(usuario);

                    var Avaliacao = Avaliacao_P1.TodasAvaliacoesUsuario(usuario.IdUsuario);

                    if (Avaliacao.Count > 0)
                    {
                        double NotaSomadas = 0;
                        for (int i = 0; i < Avaliacao.Count; i++)
                        {
                            NotaSomadas += Avaliacao[i].Nota;
                        }
                        var media = NotaSomadas / Avaliacao.Count;
                        media = Math.Round(media, 1);
                        usuarioCompleto.NotaMedia = media;
                        usuarioCompleto.NuncaAvaliado = false;
                    }
                    else
                    {
                        usuarioCompleto.NuncaAvaliado = true;
                    }

                    return Ok(usuarioCompleto);
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

        #region GET existe/{email}/{senha}
        /// <summary>
        /// (bool) Busca se o Login Existe por email e senha.
        /// </summary>
        [HttpGet("existe/{email}/{senha}")]
        public IActionResult ExistenciaLogin(string email, string senha)
        {
            try
            {
                string senhaEncoding = _encodeSenha.HashValue(senha);

                var existe = Login_P1.ExisteLogin(email, senhaEncoding);
                return Ok(existe);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
                throw;
            }
        }
        #endregion

        #region PostLogin
        /// <summary>
        /// Insert de Login.
        /// </summary>
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
                        string senhaEncoding = _encodeSenha.HashValue(login.Senha);
                        login.Senha = senhaEncoding;
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
        /// <summary>
        /// Update de Login.
        /// </summary>
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
                        string senhaEncoding = _encodeSenha.HashValue(login.Senha);
                        login.Senha = senhaEncoding;
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
        /// <summary>
        /// Delete de Login por Id_Login.
        /// </summary>
        [HttpDelete("{idlogin}")]
        public IActionResult DeleteLogin(int idlogin)
        {
            try
            {
                if (idlogin > 0)
                {
                    bool ExisteLogin = Login_P1.BuscaLogin(idlogin);

                    if (ExisteLogin)
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
