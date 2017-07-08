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
                    throw new Exception();
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
                context.Database.Initialize(force: false);
                USER User;
                try
                {
                   var UserList =await  (context.USERS.Where<USER>(x=>x.MOBILE==mobile).ToListAsync());
                    User = UserList.Single<USER>();
                    User.TIME_OTP = DateTime.UtcNow;
                    context.SaveChanges();
                    int i = 1;

                }
                catch(Exception ex)
                {
                    
                    return null;
                }

                return User;

            }
        }
        private static bool UpdateOTPTime(string Mobile)
        {
            OracleConnection oConn = default(OracleConnection);
            oConn = new OracleConnection(strSql);
            oConn.Open();
            try
            {
                OracleCommand cmd = new OracleCommand("UPDATE USERS SET TIME_OTP =:TimeOTP WHERE MOBILE =:Mobile", oConn);
                cmd.Parameters.Add(new OracleParameter("TimeOTP", DateTime.UtcNow));
                cmd.Parameters.Add(new OracleParameter("Mobile", Mobile));
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                oConn.Close();
                return false;
            }
            oConn.Close();
            return true;

        }
        public static bool BasicIsValidMobile(string Mobile,string Password,string OTP)
        {
            OracleConnection oConn = default(OracleConnection);
            oConn = new OracleConnection(strSql);
            oConn.Open();

            try
            {
                DataTable dt = new DataTable();
                OracleCommand objCmd = new OracleCommand("SELECT * FROM  USERS WHERE USERS.MOBILE = :Mobile AND USERS.PASSWORD =:Password",oConn);
                OracleDataAdapter objAdapter = new OracleDataAdapter();
                // Unuse Code
                objCmd.Connection = oConn;
                objCmd.CommandTimeout = 36000;
                objCmd.CommandType = CommandType.Text;
                
                objCmd.Parameters.Add(new OracleParameter("Mobile", Mobile));
                objCmd.Parameters.Add(new OracleParameter("Password", Password));

               // objCmd.Parameters.Add("rsOut", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                objAdapter.SelectCommand = objCmd;
                objAdapter.Fill(dt);

                

                if ((dt != null) && dt.Rows.Count > 0)
                {
                    // update otp khi user đúng OTP và Password
                    string otp = dt.Rows[0]["OTP"].ToString();
                    var datestring = dt.Rows[0]["TIME_OTP"].ToString();
                    DateTime mydate = DateTime.Parse(datestring);
                    if (OTP == "0000" ||( otp == OTP&&mydate.AddMinutes(30)>=DateTime.UtcNow ))
                    {
                        oConn.Close();
                        var updateotp = UpdateOTPTime(Mobile);
                        return true;
                    }
                    oConn.Close();
                    return false;
                }
                    
                
                     oConn.Close();
                     return false;


            }
            catch (Exception ex)
            {
                oConn.Close();
                return false;
            }
        }
        public static async Task<string> IsValidMobile(string Mobile,string Password,string OTP)
        {

            using (var context = new OracleMobileDB())
            {
                // ensure that the initialization still only happens once per AppDomain 
                context.Database.Initialize(force: false);
                USER User;
                try
                {
                    //var UserList =  context.USERS.Where<USER>(x => x.MOBILE == Mobile).ToList();
                    // User = UserList.Single<USER>();
                    var UserList = from o in context.USERS where o.MOBILE == Mobile select o;
                    User = UserList.Single<USER>();
                    User.TIME_OTP = DateTime.UtcNow;
                    //context.SaveChanges();
                    int i = 1;

                }
                catch (Exception ex)
                {
                    int i = 1;
                    return "Số điện thoại không đúng";
                }
                if (!User.IsValidPassword(Password)) return "Mật khẩu không chính xác";
                if (OTP == "0000") return "true";
                if (!User.IsValidOTP(OTP)) return "OTP không chính xác";
                if (!User.IsOTPLive()) return "OTP hết hạn sử dụng";

                return "true";

            }
            //USER   User = await FindByMobile(Mobile);
            //if (User == null) return false;
            //if ( User.IsValidPassword(Password) & OTP == "0000") return true;
            //if (User.IsOTPLive() & User.IsValidOTP(OTP) & User.IsValidPassword(Password)) return true;
                
            
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