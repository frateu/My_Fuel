﻿using System.Data.SqlClient;

namespace My_Fuel
{
    public class DBConnect
    {
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        private void connectionString()
        {
            con.ConnectionString = "data source=FRATEU; database=MyFuel; integrated security = SSPI";
        }
        public SqlDataReader commandTxt(string command)
        {
            connectionString();
            con.Open();
            com.Connection = con;
            com.CommandText = command;
            dr = com.ExecuteReader();

            return dr;
        }
    }
}
