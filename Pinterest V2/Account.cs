using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Pinterest_V2
{
     class Account
     {
          public static int UserID;
          public static int FirstLogin;

          OracleConnection connection;
          public Account()
          {
               connection = new OracleConnection(FixedValues.ordb);
               connection.Open();
          }
          public int SignUp(string Name, string Email, string Password, DateTime Birthday, byte[] ProfilePic)
          {
               int SignUpState;
               ///////////////////////////  A.6 Insert //////////////////////////////////////
               OracleCommand SignUp_cmd = new OracleCommand();
               SignUp_cmd.Connection = connection;
               SignUp_cmd.CommandText = "SignUp";
               SignUp_cmd.CommandType = CommandType.StoredProcedure;

               SignUp_cmd.Parameters.Add("name", Name);
               SignUp_cmd.Parameters.Add("email", Email);
               SignUp_cmd.Parameters.Add("password", Password);
               SignUp_cmd.Parameters.Add("day", Convert.ToInt32(Birthday.Day.ToString()));
               SignUp_cmd.Parameters.Add("month", Convert.ToInt32(Birthday.Month.ToString()));
               SignUp_cmd.Parameters.Add("year", Convert.ToInt32(Birthday.Year.ToString()));

               OracleParameter ProfilePic_Parameter = new OracleParameter();
               ProfilePic_Parameter.OracleDbType = OracleDbType.Blob;
               ProfilePic_Parameter.ParameterName = "ProfilePic";
               ProfilePic_Parameter.Value = ProfilePic;
               SignUp_cmd.Parameters.Add(ProfilePic_Parameter);

               SignUp_cmd.Parameters.Add("SignState", OracleDbType.Int32, ParameterDirection.Output);

               SignUp_cmd.ExecuteNonQuery();

               SignUpState = Convert.ToInt32(SignUp_cmd.Parameters["SignState"].Value.ToString());
               return SignUpState;
          }
     
          public int Login(string Email, string Password)
          {
               int loginState = 0;
               OracleCommand Login_cmd = new OracleCommand();

               Login_cmd.Connection = connection;
               /////////////////////////////////////////////// A. 2. select ///////////////////////////////////////////////
               Login_cmd.CommandText = "select user_id, first_login from PINTEREST_USER where email=lower(:email) and password=lower(:password)";
               Login_cmd.CommandType = CommandType.Text;
               Login_cmd.Parameters.Add("email", Email);
               Login_cmd.Parameters.Add("password", Password);

               OracleDataReader data_reader = Login_cmd.ExecuteReader();

               if (data_reader.Read())
               {
                    if (!data_reader.IsDBNull(0))
                    {
                         UserID = Convert.ToInt32(data_reader[0].ToString());
                         FirstLogin = Convert.ToInt32(data_reader[1].ToString());
                         loginState = 1;

                         //updating firstLogin in database
                         int newFirstLogin = 1;
                         /////////////////////////////////////// A.3. update //////////////////////////
                         OracleCommand setFirstLogin = new OracleCommand();
                         setFirstLogin.Connection = connection;
                         setFirstLogin.CommandText = "update PINTEREST_USER set first_login=:newFirstLogin, Login= :login where email=lower(:email) and password=lower(:password)";

                         setFirstLogin.Parameters.Add("newFirstLogin", newFirstLogin);
                         setFirstLogin.Parameters.Add("login", '1');
                         setFirstLogin.Parameters.Add("email", Email);
                         setFirstLogin.Parameters.Add("password", Password);
                         
                         setFirstLogin.CommandType = CommandType.Text;
                         setFirstLogin.ExecuteNonQuery();
                    }

                    else
                         loginState = -1;

                    return loginState;
               }

               data_reader.Close();

               loginState = -1;
               return loginState;
          }

          public bool IsEmailValid(string EmailAddress)
          {
               try
               {
                    MailAddress mail = new MailAddress(EmailAddress);
                    return true;
               }
               catch (FormatException)
               {
                    return false;
               }
          }

          // **** Read Image into Byte Array from Filesystem
          public byte[] Getimage(string filePath)
          {
               FileStream FS = new FileStream(filePath, FileMode.Open, FileAccess.Read);
               byte[] imageData = new byte[FS.Length];
               FS.Read(imageData, 0, System.Convert.ToInt32(FS.Length));
               FS.Close();

               return imageData;
          }

     }
}
