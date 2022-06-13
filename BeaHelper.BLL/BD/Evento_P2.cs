using AutoMapper;
using BeaHelper.BLL.Database;
using BeaHelper.BLL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.BD
{
    public partial class Evento_P2
    {

        #region StringConnection
        private static string stringConnection = DbAcess.GetConnection();
        #endregion

        #region Atributos

        private int _idEvento;
        private int _idUsuarioAdm;
        private string _titulo;
        private string _categoria;
        private string _descricao;
        private string _cidadeEstado;
        private DateTime _dataPublicacao;
        private DateTime? _dataEvento;
        private bool _semData;
        private bool _eventoRecorrente;

        private bool _persisted;
        private bool _modified;

        #endregion

        #region Propriedades

        #region IdEvento
        public int IdEvento
        {
            get
            {
                return this._idEvento;
            }
            set
            {
                this._idEvento = value;
                this._modified = true;
            }
        }
        #endregion

        #region Id_Usuario_Adm
        public int Id_Usuario_Adm
        {
            get
            {
                return this._idUsuarioAdm;
            }
            set
            {
                this._idUsuarioAdm = value;
                this._modified = true;
            }
        }
        #endregion

        #region Titulo

        public string Titulo
        {
            get
            {
                return this._titulo;
            }
            set
            {
                this._titulo = value;
                this._modified = true;
            }
        }
        #endregion

        #region Categoria

        public string Categoria
        {
            get
            {
                return this._categoria;
            }
            set
            {
                this._categoria = value;
                this._modified = true;
            }
        }
        #endregion

        #region Descricao

        public string Descricao
        {
            get
            {
                return this._descricao;
            }
            set
            {
                this._descricao = value;
                this._modified = true;
            }
        }
        #endregion

        #region Cidade_Estado

        public string Cidade_Estado
        {
            get
            {
                return this._cidadeEstado;
            }
            set
            {
                this._cidadeEstado = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataPublicacao

        public DateTime DataPublicacao
        {
            get
            {
                return this._dataPublicacao;
            }
            set
            {
                this._dataPublicacao = value;
                this._modified = true;
            }
        }
        #endregion

        #region DataEvento

        public DateTime? DataEvento
        {
            get
            {
                return this._dataEvento;
            }
            set
            {
                this._dataEvento = value;
                this._modified = true;
            }
        }
        #endregion

        #region SemData
        public bool SemData
        {
            get
            {
                return this._semData;
            }
            set
            {
                this._semData = value;
                this._modified = true;
            }
        }
        #endregion

        #region EventoRecorrente
        public bool EventoRecorrente
        {
            get
            {
                return this._eventoRecorrente;
            }
            set
            {
                this._eventoRecorrente = value;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string SELECT_TODASEVENTOS = @"select * from helper.Eventos where DataEvento > GetDate() or DataEvento is NULL order by DataPublicacao asc";
        private const string SELECT_ULTIMASEVENTOS_TOP8 = @"select top 8 * from helper.Eventos WHERE DataEvento > GetDate() or DataEvento is NULL order by DataEvento asc";
        private const string SELECT_MINHASEVENTOS = @"select * from helper.Eventos WHERE Id_Usuario_Adm = @Id_Usuario_Adm";
        private const string SELECT_TITULOS = @"select Count(*) from helper.Eventos WHERE Titulo = @Titulo";
        private const string SELECT_BUSCAEVENTOID_COUNT = @"select Count(*) from helper.Eventos where Id_Evento = @Id_Evento";
        private const string SELECT_FILTROEVENTOS = @"select * from helper.Eventos WHERE Titulo like @Titulo and Descricao like @Descricao and Categoria like @Categoria and Cidade_Estado like @Cidade_Estado";
        #endregion

        #region Metodos

        #region Busca Top 8 ultimas eventos do Banco
        public static List<Evento> Top8UltimasEventos()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Evento> Eventos = new List<Evento>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_ULTIMASEVENTOS_TOP8, conn);

                Mapper.CreateMap<IDataRecord, Evento>();

                using (reader = cmd.ExecuteReader())
                {
                    Eventos = Mapper.Map<List<Evento>>(reader);
                    return Eventos;
                }
            }
            finally
            {

                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region Busca todas as Eventos do Banco
        public static List<Evento> TodasEventos()
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Evento> Eventos = new List<Evento>();

            try
            {
                conn = new SqlConnection(stringConnection);
                conn.Open();

                SqlCommand cmd = new SqlCommand(SELECT_TODASEVENTOS, conn);

                Mapper.CreateMap<IDataRecord, Evento>();

                using (reader = cmd.ExecuteReader())
                {
                    Eventos = Mapper.Map<List<Evento>>(reader);
                    return Eventos;
                }
            }
            finally
            {

                if (reader != null)
                {
                    reader.Close();
                }

                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
        #endregion

        #region MinhasEventos por ID Usuario
        public static List<Evento> MinhasEventos(int IdUsuarioAdm)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Evento> evento = new List<Evento>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Usuario_Adm", SqlDbType.Int, 4));
            parms[0].Value = IdUsuarioAdm;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_MINHASEVENTOS, conn);
            cmd.Parameters.Add(parms[0]);

            Mapper.CreateMap<IDataRecord, Evento>();

            using (reader = cmd.ExecuteReader())
            {
                evento = Mapper.Map<List<Evento>>(reader);
                return evento;
            }
        }
        #endregion

        #region BuscaTitulo
        public static bool ExisteTitulo(string titulo)
        {
            SqlConnection conn = null;
            int quantidade;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Titulo", SqlDbType.VarChar, 100));
            parms[0].Value = titulo;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_TITULOS, conn);
            cmd.Parameters.Add(parms[0]);

            quantidade = Convert.ToInt32(cmd.ExecuteScalar());

            if (quantidade > 0)
                return true;
            else
                return false;
        }
        #endregion

        #region FiltrarEventos por ID
        public static List<Evento> FiltrarEventos(string Titulo = null, string Descricao = null, string Categoria = null, string Local = null)
        {
            SqlConnection conn = null;
            SqlDataReader reader = null;
            List<Evento> eventos = new List<Evento>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Titulo", SqlDbType.VarChar, 150));
            parms.Add(new SqlParameter("@Descricao", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Categoria", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Cidade_Estado", SqlDbType.VarChar, 50));

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_FILTROEVENTOS, conn);

            foreach (var parametro in parms)
            {
                cmd.Parameters.Add(parametro);
            }

            cmd.Parameters["@Titulo"].Value = "%" + Titulo + "%";
            cmd.Parameters["@Descricao"].Value = "%" + Descricao + "%";
            cmd.Parameters["@Categoria"].Value = "%" + Categoria + "%";
            cmd.Parameters["@Cidade_Estado"].Value = "%" + Local + "%";

            Mapper.CreateMap<IDataRecord, Evento>();
            using (reader = cmd.ExecuteReader())
            {
                eventos = Mapper.Map<List<Evento>>(reader);
                return eventos;
            }

        }
        #endregion

        #region Verifica se a evento Existe
        public static bool ExisteEvento(int IdEvento)
        {
            SqlConnection conn = null;
            int quantidade;

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Evento", SqlDbType.Int, 4));
            parms[0].Value = IdEvento;

            conn = new SqlConnection(stringConnection);
            conn.Open();

            SqlCommand cmd = new SqlCommand(SELECT_BUSCAEVENTOID_COUNT, conn);
            cmd.Parameters.Add(parms[0]);

            quantidade = Convert.ToInt32(cmd.ExecuteScalar());

            if (quantidade > 0)
                return true;
            else
                return false;
        }
        #endregion

        #endregion

        #region GetParameters

        private static List<SqlParameter> GetParameters()
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Id_Evento", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Id_Usuario_Adm", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Titulo", SqlDbType.VarChar, 150));
            parms.Add(new SqlParameter("@Categoria", SqlDbType.VarChar, 100));
            parms.Add(new SqlParameter("@Descricao", SqlDbType.VarChar, 250));
            parms.Add(new SqlParameter("@Cidade_Estado", SqlDbType.VarChar, 50));
            parms.Add(new SqlParameter("@DataPublicacao", SqlDbType.DateTime, 8));
            parms.Add(new SqlParameter("@DataEvento", SqlDbType.SmallDateTime, 8));
            parms.Add(new SqlParameter("@SemData", SqlDbType.Bit));
            parms.Add(new SqlParameter("@EventoRecorrente", SqlDbType.Bit));

            return (parms);
        }
        #endregion

        #region SetParameters

        private void SetParameters(List<SqlParameter> parms)
        {
            parms[0].Value = this._idEvento;
            parms[1].Value = this._idUsuarioAdm;
            parms[2].Value = this._titulo;
            parms[3].Value = this._categoria;
            parms[4].Value = this._descricao;
            parms[5].Value = this._cidadeEstado;
            parms[6].Value = this._dataPublicacao;
            parms[7].Value = this._dataEvento;
            parms[8].Value = this._semData;
            parms[9].Value = this._eventoRecorrente;
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objUsuario">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Jonathan Scrok</remarks>
        private static bool SetInstance(IDataReader dr, Evento_P2 objEvento)
        {
            try
            {
                if (dr.Read())
                {
                    objEvento._idEvento = Convert.ToInt32(dr["Id_Evento"]);
                    objEvento._idUsuarioAdm = Convert.ToInt32(dr["Id_Usuario_Adm"]);
                    objEvento._titulo = Convert.ToString(dr["Titulo"]);
                    objEvento._descricao = Convert.ToString(dr["Descricao"]);
                    objEvento._categoria = Convert.ToString(dr["Categoria"]);
                    objEvento._cidadeEstado = Convert.ToString(dr["Cidade_Estado"]);
                    objEvento._dataPublicacao = Convert.ToDateTime(dr["DataPublicacao"]);
                    objEvento._dataEvento = Convert.ToDateTime(dr["DataEvento"]);
                    objEvento._semData = Convert.ToBoolean(dr["DataEvento"]);
                    objEvento._eventoRecorrente = Convert.ToBoolean(dr["DataEvento"]);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion
    }

}
