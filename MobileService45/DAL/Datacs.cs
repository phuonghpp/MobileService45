using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MobileService45.Models;


namespace MobileService45.DAL
{
    public class Datacs
    {
        private static string strSql = ConfigurationManager.ConnectionStrings["OracleMobileDB"].ToString();
        private const string packageName = "GIAOTIEPTB.DONGBOSL.";
        //private const string packageName = "GIAOTIEP.GIAOTIEP_GPON.";
        public static bool IsValidMobile(string Mobile, string Password)
        {
            var user = new List<USER>();
            OracleMobileDB db = new OracleMobileDB();
            try
            {
                 user = db.USERS.Where(x => x.MOBILE == Mobile & x.PASSWORD == Password).ToList();
                 db.Dispose();
            }
            catch
            {
                return false;
            }
            if (user.Count != 0) return true;
            return false;

        }
        public static DataTable GetData(string storedProcedureName, List<OracleParameter> parameters)
        {
            OracleConnection oConn = default(OracleConnection);
            oConn = new OracleConnection(strSql);
            oConn.Open();

            try
            {
                DataTable dt = new DataTable();
                OracleCommand objCmd = new OracleCommand();
                OracleDataAdapter objAdapter = new OracleDataAdapter();
                objCmd.Connection = oConn;
                objCmd.CommandTimeout = 36000;
                objCmd.CommandText = packageName + storedProcedureName;
                objCmd.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                    objCmd.Parameters.Add(parameter);

                objCmd.Parameters.Add("rsOut", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objAdapter.SelectCommand = objCmd;
                objAdapter.Fill(dt);

                oConn.Close();

                if ((dt != null) && dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

                return dt;

            }
            catch (Exception ex)
            {
                oConn.Close();
                return null;
            }
        }

        public static DataTable GetData(string package, string storedProcedureName, List<OracleParameter> parameters)
        {
            OracleConnection oConn = default(OracleConnection);
            oConn = new OracleConnection(strSql);
            oConn.Open();

            try
            {
                DataTable dt = new DataTable();
                OracleCommand objCmd = new OracleCommand();
                OracleDataAdapter objAdapter = new OracleDataAdapter();
                objCmd.Connection = oConn;
                objCmd.CommandTimeout = 36000;
                objCmd.CommandText = package + storedProcedureName;
                objCmd.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                    objCmd.Parameters.Add(parameter);

                objCmd.Parameters.Add("rsOut", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objAdapter.SelectCommand = objCmd;
                objAdapter.Fill(dt);

                oConn.Close();

                if ((dt != null) && dt.Rows.Count > 0)
                    return dt;
                else
                    return null;

                return dt;

            }
            catch (Exception ex)
            {
                oConn.Close();
                return null;
            }
        }
        public static int ExcuteData(string storedProcedureName, List<OracleParameter> parameters)
        {
            OracleConnection oConn = default(OracleConnection);
            oConn = new OracleConnection(strSql);
            oConn.Open();

            try
            {
                OracleCommand objCmd = new OracleCommand();
                objCmd.Connection = oConn;
                objCmd.CommandTimeout = 36000;
                objCmd.CommandText = packageName + storedProcedureName;
                objCmd.CommandType = CommandType.StoredProcedure;

                foreach (var parameter in parameters)
                    objCmd.Parameters.Add(parameter);

                int rs = objCmd.ExecuteNonQuery();

                oConn.Close();
                return rs;

            }
            catch (Exception ex)
            {
                oConn.Close();
                return 0;
            }
        }
    }
}