using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BeaHelper.BLL.Database
{
    public class DbAcess
    {
        public static IConfiguration Configuration;

        public static string GetConnection()
        {
            //var connString = DbAcess.Configuration["ConnectionString"];

            var connString = "Data Source=mssql-65758-0.cloudclusters.net,10403;Initial Catalog=be_helper;Integrated Security=False;User Id=SuperBeaHelper;Password=B3ah3lper#2021;MultipleActiveResultSets=True";
            //var connString = "Data Source=.\\SQLEXPRESS; Initial Catalog=be_helper; User Id=sa; Password=b3ah3lper#2021;";
            return connString;
        }

    }
}
