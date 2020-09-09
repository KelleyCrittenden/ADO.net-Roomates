using Microsoft.Data.SqlClient;
using System;
using Roommates.Models;
using System.Collections.Generic;
using System.Text;

namespace Roommates.Repositories
{
    public class RoommateRepository : BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Id, 
                                               Firstname, 
                                               Lastname, 
                                               RentPortion, 
                                               MoveInDate 
                                        FROM Roommate";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("Id");

                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentPortionValue = reader.GetInt32(rentPortionColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDateValue = reader.GetDateTime(moveInDateColumnPosition);

                        Roommate newRoommate = new Roommate()
                        {
                            Id = idValue,
                            FirstName = firstNameValue,
                            LastName = lastNameValue,
                            RentPortion = rentPortionValue,
                            MoveInDate = moveInDateValue,
                            Room = null
                        };

                        roommates.Add(newRoommate);


                    }
                    reader.Close();

                    return roommates;
                }
            }
        }


        public Roommate GetById(int id)
        {
            using (SqlConnection conn = Connection)
            {
             
                conn.Open();
                
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"SELECT Firstname, 
                                               Lastname, 
                                               RentPortion, 
                                               MoveInDate 
                                        FROM Roommate 
                                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();


                    Roommate roommate = null;

                    if (reader.Read())
                    {

                        roommate = new Roommate()
                        {

                            Id = id,
                            FirstName = reader.GetString(reader.GetOrdinal("Firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),

                            Room = null
                        };

                    }

                    reader.Close();
                
                    return roommate;
                }
            }
        }

        public List<Roommate> GetAllWithRoom(int roomId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT Roommate.Id AS RoommateId,
                                               Roommate.FirstName,
                                               Roommate.LastName,
                                               Roommate.RentPortion,
                                               Roommate.MoveInDate,
                                               Roommate.RoomId,
                                               Room.Id As RoomId,
                                               Room.Id,
                                               Room.Name,
                                               Room.MaxOccupancy
                                        FROM Roommate
                                        LEFT JOIN Room On Roommate.Id = Room.id
                                        WHERE Room.id = @id";

                    cmd.Parameters.AddWithValue("@id", roomId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    List<Roommate> roommates = new List<Roommate>();


                    while (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("RoommateId")),
                            FirstName = reader.GetString(reader.GetOrdinal("Firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("Lastname")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MoveInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = new Room()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("RoomId")),
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy"))

                            }
                        };

                        roommates.Add(roommate);
                    }
                    reader.Close();
                    return roommates;
                }
            }

        }

        public void Insert(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {

                    cmd.CommandText = @"INSERT INTO Roommate (Firstname, Lastname, RentPortion, MoveInDate, RoomId) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@Firstname, @Lastname, @RentPortion, @MoveInDate, @RoomId)";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.LastName);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);

                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;

                }
            }
        }

        public void Update(Roommate roommate)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Roommate
                                       SET Firstname = @Firstname,
                                        Lastname = @Lastname,
                                        RentPortion = @RentPortion,
                                        MoveInDate = @MoveInDate,
                                        RoomId = @RoomId
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.FirstName);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.LastName);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MoveInDate", roommate.MoveInDate);
                    cmd.Parameters.AddWithValue("@RoomId", roommate.Room.Id);
                    cmd.Parameters.AddWithValue("@id", roommate.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}



