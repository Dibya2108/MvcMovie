using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using ViewModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Data_Access
{
    public class MovieDalSql
    {
        private string strConString = ConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;

        public MovieViewModel MapToViewModel(DataRow movieRow)
        {
            return new MovieViewModel
            {
                Id = Convert.ToInt32(movieRow["Id"]),
                Title = movieRow["Title"].ToString(),
                Price = Convert.ToDecimal(movieRow["Price"]),
                Genre = movieRow["Genre"].ToString(),
                ReleaseDate = Convert.ToDateTime(movieRow["ReleaseDate"]),
                Rating = Convert.ToInt32(movieRow["Rating"]),
                LanguageName = movieRow["LanguageName"].ToString()
            };
        }

        public DataTable GetMovies()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT Movies.*, Languages.LanguageName " +
                       "FROM Movies " +
                       "INNER JOIN Languages ON Movies.SelectedLanguageID = Languages.LanguageID WHERE IsDeleted = 0";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }


        public MovieViewModel GetMovieById(int movieId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT M.*, L.LanguageName " +
                               "FROM Movies M " +
                               "INNER JOIN Languages L ON M.SelectedLanguageID = L.LanguageID " +
                               "WHERE M.Id = @MovieId AND M.IsDeleted = 0";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@MovieId", movieId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return MapToViewModel(row);
                }

                return null;
            }
        }
        public MovieViewModel GetMovieViewModel(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT M.*, L.LanguageName " +
                               "FROM Movies M " +
                               "INNER JOIN Languages L ON M.SelectedLanguageID = L.LanguageID " +
                               "WHERE M.Id = @Id AND M.IsDeleted = 0";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var movieViewModel = MapToViewModel(row);
                    movieViewModel.SelectedLanguageId = Convert.ToInt32(row["SelectedLanguageID"]);

                    var languages = GetLanguages();
                    movieViewModel.Languages = new SelectList(languages, "Value", "Text");

                    return movieViewModel;
                }

                return null;
            }
        }
        public MovieViewModel Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string checkQuery = "SELECT M.*, L.LanguageName " +
                                   "FROM Movies M " +
                                   "INNER JOIN Languages L ON M.SelectedLanguageID = L.LanguageID " +
                                   "WHERE M.Id = @Id AND M.IsDeleted = 0";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", id);

                SqlDataAdapter da = new SqlDataAdapter(checkCmd);
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

        public bool DeleteConfirm(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string updateQuery = "UPDATE Movies SET IsDeleted = 1 WHERE Id = @MovieId";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@MovieId", id);

                int rowsAffected = updateCmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
        public IEnumerable<SelectListItem> GetLanguages()
        {
            List<SelectListItem> languages = new List<SelectListItem>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT LanguageID, LanguageName FROM Languages ORDER BY LanguageName";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var language = new SelectListItem
                    {
                        Value = reader["LanguageID"].ToString(),
                        Text = reader["LanguageName"].ToString()
                    };

                    languages.Add(language);
                }

                reader.Close();
            }

            return languages;
        }

        public List<MovieViewModel> GetAllLanguages(string searchText)
        {
            List<MovieViewModel> languageList = new List<MovieViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT LanguageID, LanguageName FROM Languages " +
                               "WHERE LanguageName LIKE @SearchText " + 
                               "ORDER BY LanguageName";

                SqlCommand cmd = new SqlCommand(query, con);

                
                cmd.Parameters.AddWithValue("@SearchText", "%" + searchText + "%");

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var language = new MovieViewModel
                    {
                        LanguageID = Convert.ToInt32(reader["LanguageID"]),
                        LanguageName = reader["LanguageName"].ToString()
                    };

                    languageList.Add(language);
                }

                reader.Close();
            }

            return languageList;
        }





        //public Movies CreateMovie(MovieViewModel movieViewModel)
        //{
        //    using (SqlConnection con = new SqlConnection(strConString))
        //    {
        //        con.Open();
        //        string createQuery = "INSERT INTO Movies (Title, Genre, ReleaseDate, Rating, Price, SelectedLanguageID, IsDeleted) " +
        //                            "VALUES (@Title, @Genre, @ReleaseDate, @Rating, @Price, @SelectedLanguageID, 0);";

        //        SqlCommand createCmd = new SqlCommand(createQuery, con);
        //        createCmd.Parameters.AddWithValue("@Title", movieViewModel.Title);
        //        createCmd.Parameters.AddWithValue("@Genre", movieViewModel.Genre);
        //        createCmd.Parameters.AddWithValue("@ReleaseDate", movieViewModel.ReleaseDate);
        //        createCmd.Parameters.AddWithValue("@Rating", movieViewModel.Rating);
        //        createCmd.Parameters.AddWithValue("@Price", movieViewModel.Price);
        //        createCmd.Parameters.AddWithValue("@SelectedLanguageID", movieViewModel.SelectedLanguageId);

        //        createCmd.ExecuteNonQuery();

        //        var newMovie = new Movies
        //        {
        //            Title = movieViewModel.Title,
        //            Genre = movieViewModel.Genre,
        //            ReleaseDate = movieViewModel.ReleaseDate,
        //            Rating = movieViewModel.Rating,
        //            Price = movieViewModel.Price,
        //            SelectedLanguageID = movieViewModel.SelectedLanguageId,
        //            IsDeleted = false
        //        };

        //        return newMovie;
        //    }
        //}

        public int InsertMovie(MovieViewModel newMovie)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string insertQuery = "INSERT INTO Movies (Title, Genre, ReleaseDate, Rating, Price, SelectedLanguageID, IsDeleted, MovieImage)" +
                    "OUTPUT INSERTED.Id  " +

                                     "VALUES (@Title, @Genre, @ReleaseDate, @Rating, @Price, @SelectedLanguageID, @IsDeleted, '/Images/Large/Movies/movie.png');";

                SqlCommand insertCmd = new SqlCommand(insertQuery, con);
                insertCmd.Parameters.AddWithValue("@Title", newMovie.Title);
                insertCmd.Parameters.AddWithValue("@Genre", newMovie.Genre);
                insertCmd.Parameters.AddWithValue("@ReleaseDate", newMovie.ReleaseDate);
                insertCmd.Parameters.AddWithValue("@Rating", newMovie.Rating);
                insertCmd.Parameters.AddWithValue("@Price", newMovie.Price);
                insertCmd.Parameters.AddWithValue("@SelectedLanguageID", newMovie.SelectedLanguageId);
                insertCmd.Parameters.AddWithValue("@IsDeleted", newMovie.IsDeleted);

                int insertedId = 0;
                using (SqlDataReader reader = insertCmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        insertedId = Convert.ToInt32(reader["Id"]);
                    }
                }

                return insertedId;
            

            //insertCmd.ExecuteNonQuery();
            }
        }

        public void UpdateMovie(MovieViewModel movieViewModel)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string updateQuery = "UPDATE Movies " +
                                    "SET Title = @Title, Genre = @Genre, ReleaseDate = @ReleaseDate, " +
                                    "Rating = @Rating, Price = @Price, SelectedLanguageID = @SelectedLanguageID " +
                                    "WHERE Id = @Id;";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@Title", movieViewModel.Title);
                updateCmd.Parameters.AddWithValue("@Genre", movieViewModel.Genre);
                updateCmd.Parameters.AddWithValue("@ReleaseDate", movieViewModel.ReleaseDate);
                updateCmd.Parameters.AddWithValue("@Rating", movieViewModel.Rating);
                updateCmd.Parameters.AddWithValue("@Price", movieViewModel.Price);
                updateCmd.Parameters.AddWithValue("@SelectedLanguageID", movieViewModel.SelectedLanguageId);
                updateCmd.Parameters.AddWithValue("@Id", movieViewModel.Id);

                int i = updateCmd.ExecuteNonQuery();
            }
        }

        public List<MovieViewModel> GetMoviesByLanguage(int? languageId)
        {
            string query = "SELECT M.*, L.LanguageName " +
                           "FROM Movies M " +
                           "INNER JOIN Languages L ON M.SelectedLanguageID = L.LanguageID " +
                           "WHERE M.IsDeleted = 0";

            if (languageId.HasValue)
            {
                query += " AND M.SelectedLanguageID = @LanguageId";
            }

            List<MovieViewModel> movieList = new List<MovieViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(query, con);

                if (languageId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@LanguageId", languageId);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    movieList.Add(MapToViewModel(row));
                }
            }

            return movieList;
        }

        public void AddLanguage(string languageName)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Languages (LanguageName) VALUES (@LanguageName)", con))
                {
                    cmd.Parameters.AddWithValue("@LanguageName", languageName);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLanguage(MovieViewModel model)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string updateQuery = "UPDATE Languages " +
                                    "SET LanguageName = @LanguageName " +
                                    "WHERE LanguageID = @LanguageID;";

                SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                updateCmd.Parameters.AddWithValue("@LanguageName", model.LanguageName);
                updateCmd.Parameters.AddWithValue("@LanguageID", model.LanguageID);

                int i = updateCmd.ExecuteNonQuery();
            }
        }

        public MovieViewModel GetLanguageViewModel(int languageId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string selectQuery = "SELECT * FROM Languages WHERE LanguageID = @LanguageID";

                using (SqlCommand cmd = new SqlCommand(selectQuery, con))
                {
                    cmd.Parameters.AddWithValue("@LanguageID", languageId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            MovieViewModel model = new MovieViewModel
                            {
                                LanguageID = (int)reader["LanguageID"],
                                LanguageName = reader["LanguageName"].ToString(),

                            };

                            return model;
                        }
                        else
                        {

                            return null;
                        }
                    }
                }
            }
        }

        public bool IsMovieNameUnique(int id, string title)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id FROM Movies WHERE Title = @Title AND Id <> @Id", connection))
                {
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return !reader.HasRows;
                    }
                }
            }
        }



        public bool IsLanguageNameUnique(int languageID, string languageName)
        {
            var existingLanguage = GetLanguageByName(languageName);

            if (existingLanguage == null)
            {
                return true;
            }
            return existingLanguage.LanguageID == languageID;
        }

        public MovieViewModel GetLanguageByName(string languageName)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT * FROM Languages WHERE LanguageName = @LanguageName", connection))
                {
                    command.Parameters.AddWithValue("@LanguageName", languageName);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            MovieViewModel language = new MovieViewModel
                            {
                                LanguageID = Convert.ToInt32(reader["LanguageID"]),
                                LanguageName = reader["LanguageName"].ToString()

                            };

                            return language;
                        }
                    }
                }
            }

            return null;
        }
        public void AddMovieToWatchlist(int movieId, int userId)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "INSERT INTO UserWatchList (UserId, MovieId) VALUES (@UserId, @MovieId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@MovieId", movieId);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<MovieViewModel> GetMoviesForKendoGrid()
        {
            List<MovieViewModel> movies = new List<MovieViewModel>();

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT M.*, L.LanguageName " +
                               "FROM Movies M " +
                               "INNER JOIN Languages L ON M.SelectedLanguageID = L.LanguageID " +
                               "WHERE M.IsDeleted = 0"; ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MovieViewModel movie = new MovieViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Genre = reader.GetString(reader.GetOrdinal("Genre")),
                                ReleaseDate = reader.GetDateTime(reader.GetOrdinal("ReleaseDate")),
                                Rating = reader.GetInt32(reader.GetOrdinal("Rating")),
                                LanguageName = reader.GetString(reader.GetOrdinal("LanguageName")),
                                Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                                MovieImage = reader.GetString(reader.GetOrdinal("MovieImage")) == null ? "" : reader.GetString(reader.GetOrdinal("MovieImage"))
                            };

                            movies.Add(movie);
                        }
                    }
                }
            }

            return movies;
        }


        public List<MovieViewModel> GetAllMoviesForKendoGrid()
        {
            List<MovieViewModel> movies = new List<MovieViewModel>();

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT M.Id, M.Title " +
                               "FROM Movies M " +
                               "WHERE M.IsDeleted = 0"; ;

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MovieViewModel movie = new MovieViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title"))
                            };

                            movies.Add(movie);
                        }
                    }
                }
            }

            return movies;
        }

        public void SaveMovieImageFilePath(int movieId, string imagePath)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string updateQuery = "UPDATE Movies SET MovieImage = @MovieImage WHERE ID = @ID";

                using (SqlCommand cmd = new SqlCommand(updateQuery, connection))
                {
                    cmd.Parameters.Add("@MovieImage", SqlDbType.VarChar).Value = imagePath;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = movieId;

                    cmd.ExecuteNonQuery();
                }
            }
            }
        public string GetMovieImageFilePath(int movieId)
        {
            string defaultImage = "/Images/Large/Movies/movie.png";
            string imagePath = defaultImage;

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                // Assuming you have a table named "Movies" with columns "MovieID" and "ImagePath"
                string selectQuery = "SELECT MovieImage FROM Movies WHERE ID = @MovieID";

                using (SqlCommand cmd = new SqlCommand(selectQuery, connection))
                {
                    cmd.Parameters.Add("@MovieID", SqlDbType.Int).Value = movieId;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the image path from the database
                            imagePath = reader["MovieImage"].ToString();
                        }
                    }
                }
            }

            return imagePath;
        }

        public bool MovieExist(int id, string title)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT Id FROM [Movies] WHERE LOWER(Title) = LOWER(@Title) AND Id <> @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Title", title);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return !reader.HasRows;
                    }
                }
            }
        }

        

        public bool LanguageExist(int languageId, string languageName)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("SELECT LanguageID FROM [Languages] WHERE LOWER(LanguageName) = LOWER(@LanguageName) AND LanguageID <> @LanguageID", connection))
                {
                    command.Parameters.AddWithValue("@LanguageID", languageId);
                    command.Parameters.AddWithValue("@LanguageName", languageName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        return !reader.HasRows;
                    }
                }
            }
        }
    }
}
