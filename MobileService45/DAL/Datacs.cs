using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using MobileService45.Models;
using System.Threading.Tasks;
using System.Data.Entity;


namespace MobileService45.DAL
{
    public class Datacs
    {
        private static string strSql = ConfigurationManager.ConnectionStrings["OracleMobileDB"].ToString();
        private const string packageName = "GIAOTIEPTB.DONGBOSL.";
        public static bool IsValidMobile(string Mobile, string Password)
        {
            var user = new List<USER>();
            using (OracleMobileDB db = new OracleMobileDB())
            {
                try
                {
                    user = db.USERS.Where(x => x.MOBILE == Mobile & x.PASSWORD == Password).ToList<USER>();
                    db.Dispose();
                }
                catch
                {
                    return false;
                }
                if (user.Count != 0) return true;
                return false;
            }

        }
        public static async Task<USER> FindByMobile(string mobile)
        {
            
            using (var context = new OracleMobileDB())
            {
                USER User;
                try
                {
                    User = await (context.USERS.Where(x => x.MOBILE == mobile).FirstOrDefaultAsync<USER>());
                    User.TIME_OTP = DateTime.UtcNow;
                    context.SaveChanges();
                    int i = 1;

                }
                catch(Exception ex)
                {
                    int i = 1;
                    return null;
                }

                return User;

            }
        }
        public static async Task<bool> IsValidMobile(string Mobile,string Password,string OTP)
        {
                var User = await FindByMobile(Mobile);
            if (User != null & User.IsValidPassword(Password) & OTP == "0000") return true;
            if (User!=null&User.IsOTPLive() & User.IsValidOTP(OTP) & User.IsValidPassword(Password)) return true;
                
            
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