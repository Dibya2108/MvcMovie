using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Data_Access
{
    public class AccountDalSql
    {
        private string strConString = ConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;

        public bool RegisterUser(UserViewModel model)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                try
                {
                    con.Open();
                    string query = "INSERT INTO Users (Username, Password, Email, FirstName, LastName, Address, Phone, UserTypeId ) VALUES (@Username, @Password, @Email, @FirstName, @LastName, @Address, @Phone, @UserTypeId)";
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Username", model.UserName);
                        command.Parameters.AddWithValue("@Password", model.Password);
                        command.Parameters.AddWithValue("@Email", model.Email);
                        command.Parameters.AddWithValue("@FirstName", model.FirstName);
                        command.Parameters.AddWithValue("@LastName", model.LastName);
                        command.Parameters.AddWithValue("@Phone", model.Phone);
                        command.Parameters.AddWithValue("@Address", model.Address);
                        command.Parameters.AddWithValue("@UserTypeId", 2);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("SQL error during registration", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during registration", ex);
                }
                finally
                {
                    con.Close();
                }
            }

        }
        public bool AuthenticateUser(string userName, string password)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                try
                {
                    con.Open();
                    string query = "SELECT COUNT(*) FROM Users " +
                        "WHERE UserName = @Username AND Password = @Password " +
                        "AND IsDeleted = 0 AND IsActive = 1";

                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Username", userName);
                        command.Parameters.AddWithValue("@Password", password);

                        int userCount = (int)command.ExecuteScalar();

                        if (userCount > 0)
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error during LOGIN", ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return false;
        }

        public UserViewModel GetUserById(int userId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT * FROM Users WHERE UserId = @UserId";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return MapToViewModel(row);
                }

            }
            return null;
        }

        private UserViewModel MapToViewModel(DataRow movieRow)
        {
            return new UserViewModel
            {
                UserId = Convert.ToInt32(movieRow["UserId"]),
                UserName = movieRow["UserName"].ToString(),
                Password = movieRow["Password"].ToString(),
                Email = movieRow["Email"].ToString(),
                Created = Convert.ToDateTime(movieRow["Created"]),
                IsActive = Convert.ToBoolean(movieRow["IsActive"]),
                FirstName = movieRow["FirstName"].ToString(),
                LastName = movieRow["LastName"].ToString(),
                Address = movieRow["Address"].ToString(),
                Phone = Convert.ToInt32(movieRow["Phone"]),
                IsDeleted = Convert.ToBoolean(movieRow["IsDeleted"]),
                UserTypeId = Convert.ToInt32(movieRow["UserTypeId"])
            };
        }

        public int GetUserIdByUsername(string userName)
        {
            int userId = -1; // Default value if the user is not found.

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT UserId FROM Users WHERE UserName = @UserName";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserName", userName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userId = Convert.ToInt32(reader["UserId"]);
                        }
                    }
                }
            }

            return userId;
        }

        public bool UpdateUserProfile(UserViewModel user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(strConString))
                {
                    connection.Open();
                    string query = "UPDATE Users " +
        "SET UserName = @UserName, " +
        "Email = @Email, " +
        "FirstName = @FirstName, " +
        "LastName = @LastName, " +
        "Address = @Address, " +
        "Password = @Password, " +
        "Phone = @Phone " +
        "WHERE UserId = @UserId";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserName", user.UserName);
                        command.Parameters.AddWithValue("@Email", user.Email);
                        command.Parameters.AddWithValue("@FirstName", user.FirstName);
                        command.Parameters.AddWithValue("@LastName", user.LastName);
                        command.Parameters.AddWithValue("@Address", user.Address);
                        command.Parameters.AddWithValue("@Password", user.Password);
                        command.Parameters.AddWithValue("@Phone", user.Phone);
                        command.Parameters.AddWithValue("@UserId", user.UserId);

                        int rowsAffected = command.ExecuteNonQuery();

                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<WatchViewModel> GetWatchList(int userId)
        {
            List<WatchViewModel> watchList = new List<WatchViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string sqlQuery = "SELECT w.Name, w.Description, w.WatchListId, w.CreatedOn , COUNT(a.MovieId) as Count " +
      "FROM WatchList w " +
      "LEFT JOIN WatchListMovieAssociation a ON w.WatchListId = a.WatchListId " +
      "WHERE CreatedBy = @CreatedBy GROUP BY w.Name, w.Description, w.WatchListId, w.CreatedOn ;";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@CreatedBy", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WatchViewModel watch = new WatchViewModel
                            {
                                WatchListId = int.Parse(reader["WatchListId"].ToString()),
                                Name = reader["Name"].ToString(),
                                Description = reader["Description"].ToString(),
                                Count = Convert.ToInt32(reader["Count"]),
                                CreatedOn = DateTime.Parse(reader["CreatedOn"].ToString())
                                //CreatedBy = Convert.ToInt32(reader["CreatedBy"])
                            };
                            watchList.Add(watch);
                        }
                    }
                }
            }


            return watchList;
        }

        public WatchViewModel GetWatchViewModel(int watchListId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT w.Name, w.Description, w.WatchListId, COUNT(a.MovieId) as Count " +
               "FROM WatchList w " +
               "JOIN WatchListMovieAssociation a ON w.WatchListId = a.WatchListId " +
               "WHERE w.WatchListId = @WatchListId " +
               "GROUP BY w.Name, w.Description, w.WatchListId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@WatchListId", watchListId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var watchViewModel = new WatchViewModel
                    {
                        Name = row["Name"].ToString(),
                        Description = row["Description"].ToString(),
                        Count = Convert.ToInt32(row["Count"]),
                        WatchListId = Convert.ToInt32(row["WatchListId"])
                    };

                    return watchViewModel;
                }

                return null;
            }
        }


        public void AddWatchList(string watchListName, string description, int createdByUserId, List<int> movieIds)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string watchListQuery = "INSERT INTO WatchList (Name, Description, CreatedBy) VALUES (@WatchListName, @Description, @CreatedBy); SELECT SCOPE_IDENTITY();";

                using (SqlCommand watchListCmd = new SqlCommand(watchListQuery, con))
                {
                    watchListCmd.Parameters.AddWithValue("@WatchListName", watchListName);
                    watchListCmd.Parameters.AddWithValue("@Description", description);
                    watchListCmd.Parameters.AddWithValue("@CreatedBy", createdByUserId);

                    int watchListId = Convert.ToInt32(watchListCmd.ExecuteScalar());

                    if (movieIds != null && movieIds.Any())
                    {
                        string associationQuery = "INSERT INTO WatchListMovieAssociation (WatchListId, MovieId) VALUES (@WatchListId, @MovieId)";

                        foreach (int movieId in movieIds)
                        {
                            using (SqlCommand associationCmd = new SqlCommand(associationQuery, con))
                            {
                                associationCmd.Parameters.AddWithValue("@WatchListId", watchListId);
                                associationCmd.Parameters.AddWithValue("@MovieId", movieId);
                                associationCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }

        public void AddFavoriteMoviesToWatchList(int watchListId, List<int> movieIds)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                if (movieIds != null && movieIds.Any())
                {
                    string associationQuery = "INSERT INTO WatchListMovieAssociation (WatchListId, MovieId, IsFavourite) VALUES (@WatchListId, @MovieId, 1)";

                    foreach (int movieId in movieIds)
                    {
                        using (SqlCommand associationCmd = new SqlCommand(associationQuery, con))
                        {
                            associationCmd.Parameters.AddWithValue("@WatchListId", watchListId);
                            associationCmd.Parameters.AddWithValue("@MovieId", movieId);
                            associationCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        public List<MovieViewModel> GetAvailableMovies()
        {
            List<MovieViewModel> availableMovies = new List<MovieViewModel>();

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT Id, Title FROM Movies WHERE IsDeleted = 0";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create MovieViewModel and add it to the list
                            MovieViewModel movie = new MovieViewModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"].ToString()
                            };

                            availableMovies.Add(movie);
                        }
                    }
                }
            }

            return availableMovies;
        }
        

        public List<WatchViewModel> GetAllWatchListExceptFav(int userId)
        {
            List<WatchViewModel> watchList = new List<WatchViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT w.[Name], w.[Description], w.WatchListId,w.CreatedBy as UserId " +
                    "FROM WatchList w WHERE w.CreatedBy = @UserId and  w.WatchListId not in " +
                    "(select distinct WatchListId from WatchListMovieAssociation where IsFavourite = 1 )";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var watch = new WatchViewModel
                    {
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        WatchListId = Convert.ToInt32(reader["WatchListId"]),
                        CreatedBy = Convert.ToInt32(reader["UserId"])
                    };

                    watchList.Add(watch);
                }

                reader.Close();
            }

            return watchList;
        }

        public List<int> GetSelectedMovieIdsForWatchList(int watchListId)
        {
            List<int> selectedMovieIds = new List<int>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string query = "SELECT MovieId FROM WatchListMovieAssociation WHERE WatchListId = @WatchListId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@WatchListId", watchListId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int movieId = reader.GetInt32(0); // Assuming MovieId is of type int in the database
                            selectedMovieIds.Add(movieId);
                        }
                    }
                }
            }

            return selectedMovieIds;
        }

        public List<WatchViewModel> GetFavoriteMovies(int loggedInUserId)
        {
            List<WatchViewModel> favoriteMovies = new List<WatchViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string sqlQuery = "SELECT m.Id, m.Title, w.Name ,w.WatchListId, a.IsFavourite FROM WatchList w " +
                    "JOIN WatchListMovieAssociation a ON w.WatchListId = a.WatchListId " +
                    "Join Movies m On a.MovieId = m.Id " +
                    "WHERE a.IsFavourite = 1 and w.CreatedBy =@CreatedBy " +
                    "group by w.WatchListId,m.Id, m.Title, w.Name, a.IsFavourite; ";

                using (SqlCommand command = new SqlCommand(sqlQuery, con))
                {
                    command.Parameters.AddWithValue("@CreatedBy", loggedInUserId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            WatchViewModel movie = new WatchViewModel
                            {
                                MovieId = int.Parse(reader["Id"].ToString()),
                                Name = reader["Name"].ToString(),
                                Title = reader["Title"].ToString(),
                                WatchListId = int.Parse(reader["WatchListId"].ToString()),
                                IsFavourite = bool.Parse(reader["IsFavourite"].ToString())
                            };
                            favoriteMovies.Add(movie);
                        }
                    }
                }
            }

            return favoriteMovies;
        }

        public IEnumerable<MovieViewModel> GetAvailableMoviesForWatchList(int watchListId)
        {
            List<MovieViewModel> movies = new List<MovieViewModel>();

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT m.Id, m.Title " +
                    "FROM Movies m LEFT JOIN WatchListMovieAssociation wma ON m.Id = wma.MovieId " +
                    "WHERE wma.WatchListId = @WatchListId ";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    // Add parameters as needed
                    cmd.Parameters.AddWithValue("@WatchListId", watchListId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Map the data from the database to a MovieViewModel object
                            MovieViewModel movie = new MovieViewModel
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Title = reader["Title"].ToString(),
                            };

                            movies.Add(movie);
                        }
                    }
                }
            }

            return movies;
        }

        public List<int> GetFavoriteMovieIdsForWatchList(int watchListId)
        {
            List<int> favoriteMovieIds = new List<int>();

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT MovieId FROM WatchListMovieAssociation WHERE WatchListId = @WatchListId AND IsFavourite = 1";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@WatchListId", watchListId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int movieId = Convert.ToInt32(reader["MovieId"]);
                            favoriteMovieIds.Add(movieId);
                        }
                    }
                }
            }

            return favoriteMovieIds;
        }
        public void UpdateWatchList(WatchViewModel model)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();


                string query = "UPDATE WatchList SET Name = @WatchListName, Description = @Description WHERE WatchListId = @WatchListId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@WatchListId", model.WatchListId);
                    cmd.Parameters.AddWithValue("@WatchListName", model.Name);
                    cmd.Parameters.AddWithValue("@Description", model.Description);
                    cmd.ExecuteNonQuery();
                }

                string deleteAssociationsQuery = "DELETE FROM WatchListMovieAssociation WHERE WatchListId = @WatchListId";

                using (SqlCommand deleteCmd = new SqlCommand(deleteAssociationsQuery, con))
                {
                    deleteCmd.Parameters.AddWithValue("@WatchListId", model.WatchListId);
                    deleteCmd.ExecuteNonQuery();
                }

                string insertAssociationsQuery = "INSERT INTO WatchListMovieAssociation (WatchListId, MovieId) VALUES (@WatchListId, @MovieId)";

                using (SqlCommand insertCmd = new SqlCommand(insertAssociationsQuery, con))
                {
                    insertCmd.Parameters.AddWithValue("@WatchListId", model.WatchListId);

                    foreach (int movieId in model.MovieIds)
                    {
                        insertCmd.Parameters.Clear();
                        insertCmd.Parameters.AddWithValue("@WatchListId", model.WatchListId);
                        insertCmd.Parameters.AddWithValue("@MovieId", movieId);
                        insertCmd.ExecuteNonQuery();
                    }
                }
            }
        }


        public void DeleteFavourites(int watchListId)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                string clearFavoritesQuery = "DELETE FROM WatchListMovieAssociation WHERE WatchListId = @WatchListId";
                using (SqlCommand clearFavoritesCommand = new SqlCommand(clearFavoritesQuery, connection))
                {
                    clearFavoritesCommand.Parameters.AddWithValue("@WatchListId", watchListId);
                    clearFavoritesCommand.ExecuteNonQuery();
                }
            }
        }

        public int AddFavoriteMovies(WatchViewModel model)
        {
            int addedId = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(strConString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO WatchListMovieAssociation (WatchListId, MovieId, IsFavourite) VALUES (@WatchListId, @MovieId, @IsFavourite); ";

                    using (SqlCommand cmd = new SqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@WatchListId", model.WatchListId);
                        cmd.Parameters.AddWithValue("@MovieId", model.MovieId);
                        cmd.Parameters.AddWithValue("@IsFavourite", model.IsFavourite);

                        addedId = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    connection.Close();
                    return addedId;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("An error occurred while processing the request: " + ex.Message);
            }
            return addedId;
        }
        public List<WatchViewModel> GetAllWatchList(int userId)
        {
            List<WatchViewModel> watchList = new List<WatchViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT w.Name, w.Description, w.WatchListId, COUNT(a.MovieId) as Count, w.CreatedBy as UserId, w.CreatedOn " +
                   "FROM WatchList w " +
                   "Left JOIN WatchListMovieAssociation a ON w.WatchListId = a.WatchListId " +
                   "WHERE w.CreatedBy = @UserId " +
                   "GROUP BY w.Name, w.Description, w.WatchListId, w.CreatedBy, w.CreatedOn";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var watch = new WatchViewModel
                    {
                        Name = reader["Name"].ToString(),
                        Description = reader["Description"].ToString(),
                        Count = Convert.ToInt32(reader["Count"]),
                        WatchListId = Convert.ToInt32(reader["WatchListId"]),
                        CreatedBy = Convert.ToInt32(reader["UserId"]),
                        CreatedOn = Convert.ToDateTime(reader["CreatedOn"])
                    };

                    watchList.Add(watch);
                }

                reader.Close();
            }

            return watchList;
        }
        public WatchViewModel MapToWatchViewModel(DataRow row)
        {
            if (row == null)
            {
                return null;
            }

            WatchViewModel viewModel = new WatchViewModel
            {
                
                WatchListId = Convert.ToInt32(row["WatchListId"]),
                Name = row["Name"].ToString(),
                Description = row["Description"].ToString(),
                CreatedBy= Convert.ToInt32(row["CreatedBy"]),
                CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                WatchListMovieAssociationId = Convert.ToInt32(row["WatchListMovieAssociationId"]),
                MovieId = Convert.ToInt32(row["MovieId"]),
                IsFavourite = Convert.ToBoolean(row["IsFavourite"])
                
            };

            return viewModel;
        }
        public WatchViewModel Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string checkQuery = "SELECT W.*, A.WatchListMovieAssociationId, A.MovieId, A.IsFavourite " +
                   "FROM WatchList W " +
                   "INNER JOIN WatchListMovieAssociation A ON W.WatchListId = A.WatchListId " +
                   "WHERE W.WatchListId = @Id ";


                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(checkCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return MapToWatchViewModel(row);
                }
            }

            return null;
        }

        public bool DeleteConfirm(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string deleteAssociationQuery = "DELETE FROM WatchListMovieAssociation WHERE WatchListId = @Id";
                SqlCommand deleteAssociationCmd = new SqlCommand(deleteAssociationQuery, con);
                deleteAssociationCmd.Parameters.AddWithValue("@Id", id);
                deleteAssociationCmd.ExecuteNonQuery();

                
                string deleteQuery = "DELETE FROM WatchList WHERE WatchListId = @Id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@Id", id);

                int rowsAffected = deleteCmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public int GetUserTypeByUserId(int userId)
        {
            int userTypeId = -1; // Default value if the usertype is not found.

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT UserTypeId FROM Users WHERE UserId = @UserId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userTypeId = Convert.ToInt32(reader["UserTypeId"]);
                            
                        }
                    }
                }
            }

            return userTypeId;
        }
    }
}
