using Microsoft.Data.SqlClient;
using Roommates.Models;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    /// <summary>
    ///  This class is responsible for interacting with Room data.
    ///  It inherits from the BaseRepository class so that it can use the BaseRepository's Connection property
    /// </summary>
    public class RoomRepository : BaseRepository
    {
        /// <summary>
        ///  When new RoomRespository is instantiated, pass the connection string along to the BaseRepository
        /// </summary>
        public RoomRepository(string connectionString) : base(connectionString) { }

        // ...We'll add some methods shortly...
        /// <summary>
        ///  Get a list of all Rooms in the database
        /// </summary>
        public List<Room> GetAll()
        {
            //  We must "use" the database connection.
            //  Because a database is a shared resource (other applications may be using it too) we must
            //  be careful about how we interact with it. Specifically, we Open() connections when we need to
            //  interact with the database and we Close() them when we're finished.
            //  In C#, a "using" block ensures we correctly disconnect from a resource even if there is an error.
            //  For database connections, this means the connection will be properly closed.
            using (SqlConnection conn = Connection)
            {
                // Note, we must Open() the connection, the "using" block doesn't do that for us.
                conn.Open();

                // We must "use" commands too.
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // Here we setup the command with the SQL we want to execute before we execute it.
                    cmd.CommandText = "SELECT Id, Name, MaxOccupancy FROM Room";

                    // Execute the SQL in the database and get a "reader" that will give us access to the data.
                    SqlDataReader reader = cmd.ExecuteReader();

                    // A list to hold the rooms we retrieve from the database.
                    //creating list of rooms, populating list with rooms
                    List<Room> rooms = new List<Room>();

                    // Read() will return true if there's more data to read
                    while (reader.Read())
                    {
                        // The "ordinal" is the numeric position of the column in the query results.
                        //  For our query, "Id" has an ordinal value of 0 and "Name" is 1.
                        //Ordinal always returns an integer, the column number
                        //Order of query will effec the number of the column, grab data by passing it the column position number
                        int idColumnPosition = reader.GetOrdinal("Id");

                        // We user the reader's GetXXX methods to get the value for a particular ordinal.
                        int idValue = reader.GetInt32(idColumnPosition);

                        int nameColumnPosition = reader.GetOrdinal("Name");
                        string nameValue = reader.GetString(nameColumnPosition);

                        int maxOccupancyColumnPosition = reader.GetOrdinal("MaxOccupancy");
                        int maxOccupancy = reader.GetInt32(maxOccupancyColumnPosition);

                        // Now let's create a new room object using the data from the database.
                        Room room = new Room
                        {
                            Id = idValue,
                            Name = nameValue,
                            MaxOccupancy = maxOccupancy,
                        };

                        // ...and add that room object to our list.
                        rooms.Add(room);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
                    return rooms;
                }
            }
        }
        /// <summary>
        ///  Returns a single room with the given id.
        /// </summary>
        public Room GetById(int id)
        {
            //connectin to baserepositor(tunnel)
            using (SqlConnection conn = Connection)
            {
                //think of these as opening gates to tunnel
                conn.Open();
                //creating new commann
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    //creating a text chain 
                    //filtering using WHERE, must start with @ sign, SQL parameter
                    cmd.CommandText = "SELECT Name, MaxOccupancy FROM Room WHERE Id = @id";
                    //passing in ID value to @id someone gives us an ID, we give it to the command by adding a parameter and then that parameter gets used in the query
                    cmd.Parameters.AddWithValue("@id", id);
                    //executing command we just created
                    SqlDataReader reader = cmd.ExecuteReader();

                    Room room = null;

                    // If we only expect a single row back from the database, we don't need a while loop.
                    if (reader.Read())
                    {
                        room = new Room
                        // creating a new room object with the data we got
                        {
                            Id = id,
                            //read right to left, return a number, passed into get string, returns the value, 
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("MaxOccupancy")),
                        };
                    }

                    reader.Close();

                    return room;
                }
            }
        }
        /// <summary>
        ///  Add a new room to the database
        ///   NOTE: This method sends data to the database,
        ///   it does not get anything from the database, so there is nothing to return.
        /// </summary>
        public void Insert(Room room)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    // These SQL parameters are annoying. Why can't we use string interpolation?
                    // ... sql injection attacks!!!
                    //SQL give us an ID
                    cmd.CommandText = @"INSERT INTO Room (Name, MaxOccupancy) 
                                         OUTPUT INSERTED.Id 
                                         VALUES (@name, @maxOccupancy)";
                    //the value is going to be room.Name
                    cmd.Parameters.AddWithValue("@name", room.Name);
                    cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                    //execute this commad and return one thing, 
                    int id = (int)cmd.ExecuteScalar();

                    room.Id = id;
                }
            }
        }

            // when this method is finished we can look in the database and see the new room.

            /// <summary>
            ///  Updates the room
            /// </summary>
            public void Update(Room room)
            {
                using (SqlConnection conn = Connection)
                {
                    conn.Open();
                    using (SqlCommand cmd = conn.CreateCommand())
                    {
                        cmd.CommandText = @"UPDATE Room
                                    SET Name = @name,
                                        MaxOccupancy = @maxOccupancy
                                    WHERE Id = @id";
                        cmd.Parameters.AddWithValue("@name", room.Name);
                        cmd.Parameters.AddWithValue("@maxOccupancy", room.MaxOccupancy);
                        cmd.Parameters.AddWithValue("@id", room.Id);
                    //dont need any more data,just execute it
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        /// <summary>
        ///  Delete the room with the given id
        /// </summary>
        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Room WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
    }
