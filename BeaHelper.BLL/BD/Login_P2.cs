using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.BD
{
    public partial class Login_P2
    {

        //#region Busca Login por EMAIL e SENHA
        //public static bool ExisteLogin(string Email, string Senha)
        //{
        //    SqlConnection conn = null;
        //    SqlDataReader reader = null;
        //    int quantidade;
        //    bool existe;

        //    List<SqlParameter> parms = new List<SqlParameter>();
        //    parms.Add(new SqlParameter("@Email", SqlDbType.VarChar, 100));
        //    parms.Add(new SqlParameter("@Senha", SqlDbType.VarChar, 100));
        //    parms[0].Value = Email;
        //    parms[1].Value = Senha;

        //    conn = new SqlConnection(stringConnection);
        //    conn.Open();

        //    SqlCommand cmd = new SqlCommand(SELECT_BUSCALOGIN_EMAILSENHA, conn);
        //    cmd.Parameters.Add(parms[0]);
        //    cmd.Parameters.Add(parms[1]);

        //    quantidade = Convert.ToInt32(cmd.ExecuteScalar());

        //    if (quantidade > 0)
        //        return true;
        //    else
        //        return false;

        //}
        //#endregion
    }
}
