using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace Data_Access
{
    public class ShowTimeDalSql
    {
        private string strConString = ConfigurationManager.ConnectionStrings["MovieDBContext"].ConnectionString;

        

        public List<ShowTimeViewModel> GetShowTimeForKendoGrid()
        {
            List<ShowTimeViewModel> show = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select S.*,M.Id, M.Title " +
                    "From ShowTime S Join " +
                    "Movies M " +
                    "On S.MovieId = M.Id " +
                    "WHERE M.IsDeleted = 0"; ;

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ShowTimeViewModel shows = new ShowTimeViewModel
                            {
                                MovieId = reader.GetInt32(reader.GetOrdinal("MovieId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate")),
                                EndDate = reader.GetDateTime(reader.GetOrdinal("EndDate")),
                                ShowTimeId = reader.GetInt32(reader.GetOrdinal("ShowTimeId")),
                                IsHousefull = reader.GetBoolean(reader.GetOrdinal("IsHousefull")),
                                FirstShowTime = reader.GetString(reader.GetOrdinal("FirstShowTime")),
                                SecondShowTime = reader.GetString(reader.GetOrdinal("SecondShowTime")),
                                ThirdShowTime = reader.GetString(reader.GetOrdinal("ThirdShowTime"))
                            };

                            show.Add(shows);
                        }
                    }
                }
            }

            return show;
        
    }

        public ShowTimeViewModel GetShowViewModel(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "SELECT S.*, M.Id, M.Title " +
                    "From ShowTime S Join " +
                    "Movies M " +  
                    "On S.MovieId = M.Id " +
                    "WHERE M.IsDeleted = 0 AND S.ShowTimeId = @id ;";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);


                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    var showTime = new ShowTimeViewModel
                    {
                        Title = row["Title"].ToString(),
                        FirstShowTime = row["FirstShowTime"].ToString(),
                        SecondShowTime= row["SecondShowTime"].ToString(),
                        ThirdShowTime = row["ThirdShowTime"].ToString(),
                        ShowTimeId = Convert.ToInt32(row["ShowTimeId"]),
                        MovieId = Convert.ToInt32(row["MovieId"]),
                        StartDate = Convert.ToDateTime(row["StartDate"]),
                        EndDate = Convert.ToDateTime(row["EndDate"]),
                        CreatedBy = Convert.ToInt32(row["CreatedBy"])
                    };

                    return showTime;
                }

                return null;
            }
        }
        public int CreateShowTime(ShowTimeViewModel showTime)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO ShowTime (MovieId, StartDate, EndDate, FirstShowTime, SecondShowTime, ThirdShowTime, CreatedBy) " +
                    "VALUES (@MovieId, @StartDate, @EndDate, @FirstShowTime, @SecondShowTime, @ThirdShowTime, @CreatedBy); ", connection))
                {
                    cmd.Parameters.AddWithValue("@MovieId", showTime.MovieId);
                    cmd.Parameters.AddWithValue("@StartDate", showTime.StartDateString);
                    cmd.Parameters.AddWithValue("@EndDate", showTime.EndDateString);
                    cmd.Parameters.AddWithValue("@FirstShowTime", showTime.FirstShowTime);
                    cmd.Parameters.AddWithValue("@SecondShowTime", showTime.SecondShowTime);
                    cmd.Parameters.AddWithValue("@ThirdShowTime", showTime.ThirdShowTime);
                    cmd.Parameters.AddWithValue("@CreatedBy", showTime.CreatedBy);

                    // Execute the query and get the newly created ShowTimeId
                    int showTimeId = Convert.ToInt32(cmd.ExecuteScalar());
                    return showTimeId;
                }
            }
        }

        public void UpdateShowTime(ShowTimeViewModel showTime)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                try
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE ShowTime SET StartDate = @StartDate, EndDate = @EndDate, " +
                        "FirstShowTime = @FirstShowTime, SecondShowTime = @SecondShowTime, ThirdShowTime = @ThirdShowTime WHERE ShowTimeId = @ShowTimeId", connection))
                    {
                        cmd.Parameters.AddWithValue("@ShowTimeId", showTime.ShowTimeId);

                        cmd.Parameters.AddWithValue("@StartDate", showTime.StartDateString);
                        cmd.Parameters.AddWithValue("@EndDate", showTime.EndDateString);
                        cmd.Parameters.AddWithValue("@FirstShowTime", showTime.FirstShowTime);
                        cmd.Parameters.AddWithValue("@SecondShowTime", showTime.SecondShowTime);
                        cmd.Parameters.AddWithValue("@ThirdShowTime", showTime.ThirdShowTime);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch(Exception ex) 
                {
                    throw new Exception("Error", ex);
                }
            }
        }

        public List<MovieViewModel> GetMovieForKendoGrid()
        {
            List<MovieViewModel> movie = new List<MovieViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select M.Id, M.Title " +
                    "From Movies M " +
                    "WHERE M.IsDeleted = 0"; ;

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            MovieViewModel movies = new MovieViewModel
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                            };

                            movie.Add(movies);
                        }
                    }
                }
            }

            return movie;

        }

        public List<ShowTimeViewModel> GetTimesForKendoGrid(int showId)
        {
            List<ShowTimeViewModel> show = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select FirstShowTime, SecondShowTime, ThirdShowTime, ShowTimeId " +
                    "From ShowTime " +
                    "Where ShowTimeId = @showId "; ;

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@showId", showId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ShowTimeViewModel shows = new ShowTimeViewModel
                            {

                                FirstShowTime = reader.GetString(reader.GetOrdinal("FirstShowTime")),
                                SecondShowTime = reader.GetString(reader.GetOrdinal("SecondShowTime")),
                                ThirdShowTime = reader.GetString(reader.GetOrdinal("ThirdShowTime")),
                                ShowTimeId = Convert.ToInt32(reader["ShowTimeId"])
                            };

                            show.Add(shows);
                        }
                    }
                }
            }

            return show;

        }


        public List<SeatViewModel> GetSeatTypes()
        {
            List<SeatViewModel> seatTypes = new List<SeatViewModel>();

            

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                string query = "SELECT SeatTypeId, TypeName, Price FROM SeatType ";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        SeatViewModel seatType = new SeatViewModel
                        {
                            SeatTypeId = Convert.ToInt32(reader["SeatTypeId"]),
                            TypeName = reader["TypeName"].ToString(),
                            Price = Convert.ToInt32(reader["Price"])
                        };
                        seatTypes.Add(seatType);
                    }
                }
            }

            return seatTypes;
        }

        public int SaveBookTicket(ShowTimeViewModel bookTicket)
        {
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    // Define the SQL query to insert the data into the BookTicket table.
                    string insertQuery = "INSERT INTO BookTicket (MovieId, ShowDate, ShowTime, SeatTypeId, PaymentStatus, PaymentAmount, NoOfTicket, UserId) " +
                        "OUTPUT INSERTED.BookTicketId " +
                                         "VALUES (@MovieId, @ShowDate, @ShowTime, @SeatTypeId, @PaymentStatus, @PaymentAmount, @NoOfTicket, @UserId)";

                    command.CommandText = insertQuery;

                    // Set the parameters for the query.
                    command.Parameters.AddWithValue("@MovieId", bookTicket.MovieId);
                    command.Parameters.AddWithValue("@ShowDate", bookTicket.ShowDateString);
                    command.Parameters.AddWithValue("@ShowTime", bookTicket.ShowTime);
                    command.Parameters.AddWithValue("@SeatTypeId", bookTicket.SeatTypeId);
                    command.Parameters.AddWithValue("@PaymentStatus", bookTicket.PaymentStatus);
                    command.Parameters.AddWithValue("@PaymentAmount", bookTicket.PaymentAmount);
                    command.Parameters.AddWithValue("@NoOfTicket", bookTicket.NoOfTicket);
                    command.Parameters.AddWithValue("@UserId", bookTicket.UserId);


                    int insertedId = 0;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            insertedId = Convert.ToInt32(reader["BookTicketId"]);
                        }
                    }

                    return insertedId;
                }
               
            }
        }


        public int CalculatePaymentAmount(int seatTypesId, int ticketQuantity)
        {
            int price = 0;

            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();
                    // Define the SQL query to retrieve the price based on the seat type.
                    string selectQuery = "SELECT Price FROM SeatType WHERE SeatTypeId = @SeatTypeId ";

                    using (SqlCommand command = new SqlCommand(selectQuery, connection))

                {
                    command.Parameters.AddWithValue("@SeatTypeId", seatTypesId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            
                            price = reader.GetInt32(0); 
                        }
                        
                        reader.Close();
                    }
                }
            }

            return price * ticketQuantity;
        }

        public int GetSeatTypesId(string seatTypeName)
        {
            int seatTypeId = 0; 
            using (SqlConnection connection = new SqlConnection(strConString))
            {
                connection.Open();

                // Construct your SQL query to retrieve the SeatTypeId based on the SeatType name
                string query = "SELECT SeatTypeId FROM SeatType WHERE TypeName = @SeatTypeName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SeatTypeName", seatTypeName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Check if a record was found and get the SeatTypeId
                            seatTypeId = reader.GetInt32(0); // Assuming SeatTypeId is an integer
                        }
                        // Close the reader
                        reader.Close();
                    }
                }
            }

            return seatTypeId;
        }

        public ShowTimeViewModel Delete(int showTimeId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string checkQuery = "SELECT S.* " +
                   "FROM ShowTime S " +               
                   "WHERE S.ShowTimeId = @Id ";

                SqlCommand checkCmd = new SqlCommand(checkQuery, con);
                checkCmd.Parameters.AddWithValue("@Id", showTimeId);

                SqlDataAdapter da = new SqlDataAdapter(checkCmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    return MapToShowViewModel(row);
                }

            }
            return null;
        }

        private ShowTimeViewModel MapToShowViewModel(DataRow row)
        {
            if (row == null)
            {
                return null;
            }

            ShowTimeViewModel viewModel = new ShowTimeViewModel
            {

                ShowTimeId = Convert.ToInt32(row["ShowTimeId"]),
                FirstShowTime = row["FirstShowTime"].ToString(),
                SecondShowTime = row["SecondShowTime"].ToString(),
                ThirdShowTime = row["ThirdShowTime"].ToString(),
                StartDate = Convert.ToDateTime(row["StartDate"]),
                EndDate = Convert.ToDateTime(row["EndDate"]),
                MovieId = Convert.ToInt32(row["MovieId"]),
                IsHousefull = Convert.ToBoolean(row["IsHousefull"])

            };

            return viewModel;
        }

        public bool DeleteConfirm(int showTimeId)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();

                string deleteQuery = "DELETE FROM ShowTime WHERE ShowTimeId = @Id";
                SqlCommand deleteCmd = new SqlCommand(deleteQuery, con);
                deleteCmd.Parameters.AddWithValue("@Id", showTimeId);

                int rowsAffected = deleteCmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }

        public List<ShowTimeViewModel> GetBookTicketForKendoGrid()
        {
            List<ShowTimeViewModel> tickets = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select M.Id, M.Title, B.BookTicketId, B.ShowDate, B.ShowTime, B.SeatTypeId, B.PaymentStatus, " +
                    "B.PaymentAmount, B.NoOfTicket, B.UserId, " +
                    "S.TypeName " +
                    "From Movies M Inner Join " +
                    "BookTicket B On M.Id = B.MovieId Inner Join " +
                    "SeatType S On B.SeatTypeId = S.SeatTypeId "; ;

                using (SqlCommand command = new SqlCommand(query, con))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ShowTimeViewModel ticket = new ShowTimeViewModel
                            {
                                MovieId = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                BookticketId = reader.GetInt32(reader.GetOrdinal("BookTicketId")),
                                SelectedDate = reader.GetDateTime(reader.GetOrdinal("ShowDate")),
                                SelectedTime = reader.GetString(reader.GetOrdinal("ShowTime")),
                                SelectedSeat = reader.GetString(reader.GetOrdinal("TypeName")),
                                PaymentStatus = reader.GetInt32(reader.GetOrdinal("PaymentStatus")),
                                PaymentAmount = reader.GetInt32(reader.GetOrdinal("PaymentAmount")),
                                NoOfTicket = reader.GetInt32(reader.GetOrdinal("NoOfTicket")),
                                UserId = reader.GetInt32(reader.GetOrdinal("UserId"))

                            };

                            tickets.Add(ticket);

                        }
                    }
                }
            }

            return tickets;

        }

        public List<ShowTimeViewModel> GetMyBookings(int userId)
        {
            List<ShowTimeViewModel> showList = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select M.Id, M.Title, B.BookTicketId, B.ShowDate, B.ShowTime, B.SeatTypeId, " +
    "B.PaymentAmount, B.NoOfTicket, B.UserId, " + 
    "S.TypeName " +
    "From Movies M Inner Join " +
    "BookTicket B On M.Id = B.MovieId Inner Join " +
    "SeatType S On B.SeatTypeId = S.SeatTypeId " +
    "where UserId =  @UserId ";


                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@UserId", userId);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var show = new ShowTimeViewModel
                    {
                        MovieId = reader.GetInt32(reader.GetOrdinal("Id")),
                        Title = reader.GetString(reader.GetOrdinal("Title")),
                        BookticketId = reader.GetInt32(reader.GetOrdinal("BookTicketId")),
                        SelectedDate = reader.GetDateTime(reader.GetOrdinal("ShowDate")),
                        SelectedTime = reader.GetString(reader.GetOrdinal("ShowTime")),
                        SelectedSeat = reader.GetString(reader.GetOrdinal("TypeName")),
                        //PaymentStatus = reader.GetInt32(reader.GetOrdinal("PaymentStatus")),
                        PaymentAmount = reader.GetInt32(reader.GetOrdinal("PaymentAmount")),
                        NoOfTicket = reader.GetInt32(reader.GetOrdinal("NoOfTicket")),
                        //UserId = reader.GetInt32(reader.GetOrdinal("UserId"))
                    };

                    showList.Add(show);
                }

                reader.Close();
            }

            return showList;
        }

        public List<ShowTimeViewModel> GetBookings()
        {
            List<ShowTimeViewModel> showList = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select distinct M.Title, B.MovieId " +
                    "From Movies M Inner Join " +
                    "BookTicket B On M.Id = B.MovieId ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var show = new ShowTimeViewModel
                    {
                        MovieId = reader.GetInt32(reader.GetOrdinal("MovieId")),
                        Title = reader.GetString(reader.GetOrdinal("Title"))
                        
                    };

                    showList.Add(show);
                }

                reader.Close();
            }

            return showList;


        }

        public List<ShowTimeViewModel> GetBookingsTime()
        {
            List<ShowTimeViewModel> showList = new List<ShowTimeViewModel>();

            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select distinct B.ShowTime, B.BookTicketId " +
                    "From BookTicket B ";

                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var show = new ShowTimeViewModel
                    {
                        BookticketId = reader.GetInt32(reader.GetOrdinal("BookTicketId")),
                        SelectedTime = reader.GetString(reader.GetOrdinal("ShowTime"))

                    };

                    showList.Add(show);
                }

                reader.Close();
            }

            return showList;
        }

        public ShowTimeViewModel GetBookingsInfo(int id)
        {
            using (SqlConnection con = new SqlConnection(strConString))
            {
                con.Open();
                string query = "Select B.* , M.Title, MovieImage, L.LanguageName " +
                    "From BookTicket B Join Movies M on B.MovieId = M.ID " +
                    "Join Languages L on L.LanguageID = M.SelectedLanguageID " +
                    "where B.BookTicketId=" + id;

                DataTable dt = GetDataTable(query);

                // Check if there are rows in the DataTable
                if (dt.Rows.Count > 0)
                {
                    ShowTimeViewModel show = GetShowBookingInfo(dt.Rows[0]);
                    return show;
                }
                else
                {
                    // Handle the case where no rows were returned, for example by returning null
                    return null;
                }
            }
        }

        private ShowTimeViewModel GetShowBookingInfo(DataRow dr)
        {
            ShowTimeViewModel show = new ShowTimeViewModel();
            show.BookticketId = int.Parse(dr["BookticketId"].ToString());
            show.MovieId = int.Parse(dr["MovieId"].ToString()); // Fix: use "MovieId" instead of "BookticketId"
            show.ShowDate = DateTime.Parse(dr["ShowDate"].ToString());
            show.ShowTime = dr["ShowTime"].ToString();
            show.SeatTypeId = int.Parse(dr["SeatTypeId"].ToString());
            show.PaymentStatus = int.Parse(dr["PaymentStatus"].ToString());
            show.PaymentAmount = int.Parse(dr["PaymentAmount"].ToString());
            show.NoOfTicket = int.Parse(dr["NoOfTicket"].ToString());
            show.UserId = int.Parse(dr["UserId"].ToString());
            show.InsertedAt = DateTime.Parse(dr["InsertedAt"].ToString());
            show.Title = dr["Title"].ToString();
            show.MovieImage = dr["MovieImage"].ToString();
            show.LanguageName = dr["LanguageName"].ToString();

            return show;
        }


        //private ShowTimeViewModel GetShowBookingInfo(DataRow dr)
        //{
        //    ShowTimeViewModel show = new ShowTimeViewModel();
        //    show.BookticketId = int.Parse(dr["BookticketId"].ToString());
        //    show.MovieId = int.Parse(dr["BookticketId"].ToString());
        //    show.ShowDate = DateTime.Parse(dr["ShowDate"].ToString());
        //    show.ShowTime = dr["ShowTime"].ToString();
        //    show.SeatTypeId = int.Parse(dr["SeatTypeId"].ToString());
        //    show.PaymentStatus = int.Parse(dr["PaymentStatus"].ToString());
        //    show.PaymentAmount = int.Parse(dr["PaymentAmount"].ToString());
        //    show.NoOfTicket = int.Parse(dr["NoOfTicket"].ToString());
        //    show.UserId= int.Parse(dr["UserId"].ToString());
        //    show.InsertedAt = DateTime.Parse(dr["InsertedAt"].ToString());
        //    show.Title = dr["Title"].ToString();
        //    show.MovieImage = dr["MovieImage"].ToString();
        //   show.LanguageName = dr["LanguageName"].ToString();

        //    return show;
        //}

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
    }
}
