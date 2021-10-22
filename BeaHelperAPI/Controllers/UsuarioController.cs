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
        [HttpGet("todos-usuarios")]
        public IActionResult GetTodosUsuarios()
        {
            try
            {
                List<Usuario> usuarios = Usuario_P2.TodosUsuarios();
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

        #region PostUsuario
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
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
        #endregion

        #region UpdateUsuario
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

        #region DeleteUsuario
        [HttpDelete]
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
            catch (Exception)
            {
                return BadRequest();
                throw;
            }
        }
        #endregion

    }
}
