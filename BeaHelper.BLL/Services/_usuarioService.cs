using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _usuarioService
    {
        public static void CadastrarUsuarioBanco(Usuario UsuarioDados)
        {
            Usuario_P1 Usuario = new Usuario_P1();

            Usuario.Nome = UsuarioDados.Nome;
            Usuario.Email = UsuarioDados.Email;
            Usuario.NumeroCelular = UsuarioDados.NumeroCelular;
            Usuario.Sexo = UsuarioDados.Sexo;
            Usuario.DataCadastro = DateTime.Now;
            Usuario.Save();
        }

        public static void CadastrarUsuarioCompleto(UsuarioCompleto UsuarioDados)
        {
            Usuario_P1 Usuario = new Usuario_P1();

            Usuario.Nome = UsuarioDados.Nome;
            Usuario.Email = UsuarioDados.Email;
            Usuario.NumeroCelular = UsuarioDados.NumeroCelular;
            Usuario.Sexo = UsuarioDados.Sexo;
            Usuario.DataCadastro = DateTime.Now;
            Usuario.Save();

            var UsuarioCadastrado = CarregaUsuario(UsuarioDados.Email);

            Login_P1 login = new Login_P1();

            login.Id_Usuario = UsuarioCadastrado.Id_Usuario;
            login.Email = UsuarioDados.Email;
            login.Senha = UsuarioDados.Senha;
            login.DataCadastro = DateTime.Now;
            login.Save();
        }

        public static void AtualizarUsuarioBanco(Usuario UsuarioDados)
        {
            Usuario_P1 Usuario = new Usuario_P1(UsuarioDados.Id_Usuario);

            Usuario.Nome = UsuarioDados.Nome;
            Usuario.Email = UsuarioDados.Email;
            Usuario.NumeroCelular = UsuarioDados.NumeroCelular;
            Usuario.Sexo = UsuarioDados.Sexo;
            Usuario.DataCadastro = UsuarioDados.DataCadastro;
            Usuario.Save();
        }

        public static Usuario_P1 CarregaUsuario(int IdUsuario)
        {
            Usuario_P1 Usuario = new Usuario_P1(IdUsuario);
            Usuario.CompleteObject();

            return Usuario;
        }

        public static Usuario_P1 CarregaUsuario(string email)
        {
            Usuario_P1 usuario = new Usuario_P1(email);
            usuario.CompleteObjectEmail();

            return usuario;
        }
    }
}
