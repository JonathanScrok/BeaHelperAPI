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
            Usuario.Sexo = UsuarioDados.Sexo;
            Usuario.DataCadastro = DateTime.Now;
            Usuario.Save();
        }

        public static void AtualizarUsuarioBanco(Usuario UsuarioDados)
        {
            Usuario_P1 Usuario = new Usuario_P1(UsuarioDados.Id_Usuario);

            Usuario.Nome = UsuarioDados.Nome;
            Usuario.Email = UsuarioDados.Email;
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
    }
}
