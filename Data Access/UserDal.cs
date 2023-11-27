﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ViewModel;

namespace Data_Access
{
    public class UserDal
    {
        private string strConString = ConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;

        public List<UserViewModel> GetAllUsers()
        {

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                DataTable dt = GetDataTable("SELECT U.*, T.UserType FROM[User] U " +
                    "JOIN[UserType] T ON U.UserTypeId = T.UserTypeId ORDER BY U.FirstName");
                List<UserViewModel> users = new List<UserViewModel>();
                foreach (DataRow dr in dt.Rows)
                {
                    users.Add(GetUserFromDataRow(dr));
                }
                return users;


            }
        }

        private UserViewModel GetUserFromDataRow(DataRow dr)
        {
            UserViewModel userView = new UserViewModel();

            userView.UserId = int.Parse(dr["UserId"].ToString());
            userView.FirstName = dr["FirstName"].ToString();
            userView.LastName = dr["LastName"].ToString();
            userView.Email = dr["Email"].ToString();
            userView.UserTypeId = int.Parse(dr["UserTypeId"].ToString()); ;
            userView.IsActive = int.Parse(dr["IsActive"].ToString());
            userView.Created = DateTime.Parse(dr["Created"].ToString());
            userView.CreatedBy = int.Parse(dr["CreatedBy"].ToString());
            userView.Password = dr["Password"].ToString();
            userView.UserType = dr["UserType"].ToString();
            userView.UserName = dr["UserName"].ToString();
            userView.Address = dr["Address"].ToString();
            userView.IsDeleted = bool.Parse(dr["IsDeleted"].ToString());

            return userView;
        }
        private DataTable GetDataTable(string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }

            return dataTable;
        }

        public UserViewModel GetUsers(int userId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string query = "SELECT U.*, T.UserType FROM[Users] U " +
                    "JOIN[UserType] T ON U.UserTypeId = T.UserTypeId  WHERE UserId = @UserId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    var userView = GetUserFromDataRow(dr);
                    userView.UserTypeId = Convert.ToInt32(dr["UserTypeId"]);
                    var users = GetUserTypes();
                    userView.UserTypes = new SelectList(users, "Value", "Text");
                    return userView;


                }

                return null;
            }
        }

        public void InsertUser(UserView user)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string insertQuery = "INSERT INTO [User] (FirstName, LastName, Email, Password, Active, CreatedBy,CreatedOn, UserTypeId) " +
                                     "VALUES (@FirstName, @LastName, @Email, @Password, @Active, @CreatedBy,@CreatedOn, @UserTypeId);";

                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                insertCmd.Parameters.AddWithValue("@LastName", user.LastName);
                insertCmd.Parameters.AddWithValue("@Email", user.Email);
                insertCmd.Parameters.AddWithValue("@Password", user.Password);
                insertCmd.Parameters.AddWithValue("@Active", user.Active);
                insertCmd.Parameters.AddWithValue("@CreatedBy", user.CreatedBy);
                insertCmd.Parameters.AddWithValue("@CreatedOn", user.CreatedOn);
                insertCmd.Parameters.AddWithValue("@UserTypeId", user.UserTypeId);

                insertCmd.ExecuteNonQuery();
            }
        }

        public void UpdateUser(UserView user)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string updateQuery = "UPDATE [User] " +
     "SET FirstName = @FirstName, LastName = @LastName, Email = @Email, " +
                    "Password = @Password, Active = @Active, " +
                    "UserTypeId = @UserTypeId " +
                    "WHERE UserId = @UserId;";




                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                updateCmd.Parameters.AddWithValue("@LastName", user.LastName);
                updateCmd.Parameters.AddWithValue("@Email", user.Email);
                updateCmd.Parameters.AddWithValue("@Password", user.Password);
                updateCmd.Parameters.AddWithValue("@Active", user.Active);
                updateCmd.Parameters.AddWithValue("@UserId", user.UserId);
                updateCmd.Parameters.AddWithValue("@UserTypeId", user.UserTypeId);

                int i = updateCmd.ExecuteNonQuery();
            }
        }

        public UserView Delete(int UserId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string checkQuery = "SELECT U.* " +
                   "FROM [User] U " +
                   "WHERE U.UserId = @Id ";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", UserId);

                SqlDataAdapter da = new SqlDataAdapter(checkCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return GetUserFromDataRow(row);
                }

            }
            return null;
        }

        public bool DeleteConfirm(int UserId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string deleteQuery = "DELETE FROM [User] WHERE UserId = @Id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@Id", UserId);

                int rowsAffected = deleteCmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }






        public IEnumerable<SelectListItem> GetUserTypes()
        {
            List<SelectListItem> userTypes = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT UserTypeId, UserType FROM UserType ORDER BY UserType ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var userType = new SelectListItem
                    {
                        Value = reader["UserTypeId"].ToString(),
                        Text = reader["UserType"].ToString()
                    };

                    userTypes.Add(userType);
                }
                reader.Close();
            }
            return userTypes;
        }

        public bool UserExist(int userId, string email)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserId FROM [User] WHERE Email = @Email AND UserId <> @UserId", connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return !reader.HasRows;
                    }
                }
            }
        }
    }
}
}
