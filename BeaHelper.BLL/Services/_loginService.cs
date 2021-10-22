using BeaHelper.BLL.BD;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Services
{
    public class _loginService
    {
        public static void CadastrarLoginBanco(Login loginDados)
        {
            Login_P1 login = new Login_P1();

            login.IdUsuario = loginDados.Id_Usuario;
            login.Email = loginDados.Email;
            login.Senha = loginDados.Senha;
            login.DataCadastro = DateTime.Now;
            login.Save();
        }

        public static void AtualizarLoginBanco(Login loginDados)
        {
            Login_P1 login = new Login_P1(loginDados.Id_Login);

            login.IdLogin = loginDados.Id_Login;
            login.IdUsuario = loginDados.Id_Usuario;
            login.Email = loginDados.Email;
            login.Senha = loginDados.Senha;
            login.DataCadastro = DateTime.Now;
            login.Save();
        }

        public static Login_P1 CarregaLogin(int IdLogin)
        {
            Login_P1 login = new Login_P1(IdLogin);
            login.CompleteObject();

            return login;
        }
    }
}
