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
    public class UsuarioController : ControllerBase
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        #region Get Todos usuarios
        /// <summary>
        /// Busca todos os usuários.
        /// </summary>
        [HttpGet("todos-usuarios")]
        public IActionResult GetTodosUsuarios(int? IdUsuario = null)
        {
            try
            {
                List<UsuarioCompleto> usuarios = Usuario_P2.TodosUsuarios(IdUsuario);

                foreach (var usu in usuarios)
                {
                    var notificacoes = Notificacao_P1.BuscaIdUsuario_NotificouENotificado(usu.Id_Usuario, (int)IdUsuario);
                    if (notificacoes.Count > 0)
                        usu.JaConvidado = true;
                    else
                        usu.JaConvidado = false;

                    var Avaliacao = Avaliacao_P1.TodasAvaliacoesUsuario(usu.Id_Usuario);

                    if (Avaliacao.Count > 0)
                    {
                        double NotaSomadas = 0;
                        for (int i = 0; i < Avaliacao.Count; i++)
                        {
                            NotaSomadas += Avaliacao[i].Nota;
                        }
                        var media = NotaSomadas / Avaliacao.Count;
                        media = Math.Round(media, 1);
                        usu.NotaMedia = media;
                        usu.NuncaAvaliado = false;
                    }
                    else
                    {
                        usu.NuncaAvaliado = true;
                    }
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        #region Get usuario por Email
        /// <summary>
        /// Busca usuário por email.
        /// </summary>
        [HttpGet("{email}")]
        public IActionResult GetUsuarioEmail(string email)
        {
            try
            {
                List<Usuario> usuario = Usuario_P2.BuscaUsuario_Email(email);
                if (usuario.Count > 0)
                    return Ok(usuario);
                else
                    return NotFound();

            }
            catch (Exception ex)
            {
                return BadRequest(ex);
                throw;
            }
        }
        #endregion

        #region NovoUsuario
        /// <summary>
        /// Cadastro de novo Usuario.
        /// </summary>
        [HttpPost("cadastrar")]
        public IActionResult NovoUsuario(UsuarioCompleto login)
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
                        _usuarioService.CadastrarUsuarioCompleto(login);
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
            catch (Exception ex)
            {
                return NotFound();
                throw;
            }
        }
        #endregion

        #region PostUsuario
        /// <summary>
        /// Insert de Usuário.
        /// </summary>
        [HttpPost]
        public IActionResult PostUsuario(Usuario usuario)
        {
            try
            {
                if (usuario != null)
                {
                    bool ExisteUsuarioEmail = Usuario_P2.ExisteUsuario(usuario.Email);

                    if (!ExisteUsuarioEmail)
                    {
                        _usuarioService.CadastrarUsuarioBanco(usuario);
                        return Ok();
                    }
                    else
                    {
                        return StatusCode((int)HttpStatusCode.Forbidden, "Usuario com este email já cadastrado.");
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

        #region UpdateUsuario
        /// <summary>
        /// Update de Usuário.
        /// </summary>
        [HttpPut]
        public IActionResult UpdateUsuario(Usuario usuario)
        {
            try
            {
                if (usuario != null && usuario.Id_Usuario > 0)
                {
                    bool ExisteUsuario = Usuario_P2.ExisteUsuario(usuario.Id_Usuario);

                    if (ExisteUsuario)
                    {
                        try
                        {
                            _usuarioService.AtualizarUsuarioBanco(usuario);
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

        #region DeleteUsuario
        /// <summary>
        /// Delete de Usuário por Id_Usuario.
        /// </summary>
        [HttpDelete("{idusuario}")]
        public IActionResult DeleteUsuario(int idusuario)
        {
            try
            {
                if (idusuario > 0)
                {
                    bool ExisteUsuario = Usuario_P2.ExisteUsuario(idusuario);

                    if (ExisteUsuario)
                    {
                        try
                        {
                            Usuario_P1.Delete(idusuario);
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
