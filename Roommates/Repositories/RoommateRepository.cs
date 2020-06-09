using Microsoft.Data.SqlClient;
using Roommates.Models;
using System;
using System.Collections.Generic;

namespace Roommates.Repositories
{
    class RoommateRepository: BaseRepository
    {
        public RoommateRepository(string connectionString) : base(connectionString) { }

        public List<Roommate> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT roommate.Id AS 'roomyId', FirstName, LastName, MoveInDate, RentPortion, room.Id AS 'roomId', room.Name AS 'roomName', room.MaxOccupancy AS 'roomOcc' 
                    FROM Roommate roommate JOIN Room room ON room.Id = Roommate.RoomId";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("roomyId");
                        int idValue = reader.GetInt32(idColumnPosition);

                        int firstNameColumnPosition = reader.GetOrdinal("FirstName");
                        string firstNameValue = reader.GetString(firstNameColumnPosition);

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentPortion = reader.GetInt32(rentPortionColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDate = reader.GetDateTime(moveInDateColumnPosition);

                        int roomIdColumnPosition = reader.GetOrdinal("roomId");
                        int roomIdValue = reader.GetInt32(roomIdColumnPosition);


                        int roomNameColumnPosition = reader.GetOrdinal("roomName");
                        string roomName = reader.GetString(roomNameColumnPosition);

                        int roomOcc = reader.GetOrdinal("roomOcc");
                        int roomOccValue = reader.GetInt32(roomOcc);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortion,
                            MovedInDate = moveInDate,
                            Room = new Room
                            {
                                Id = roomIdValue,
                                Name = roomName,
                                MaxOccupancy = roomOccValue
                            }
                        };

                        // ...and add that room object to our list.
                        roommates.Add(roommate);
                    }

                    // We should Close() the reader. Unfortunately, a "using" block won't work here.
                    reader.Close();

                    // Return the list of rooms who whomever called this method.
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
                    cmd.CommandText = @"SELECT roommate.Id AS 'roomyId', FirstName, LastName, MoveInDate, RentPortion, room.Id AS 'roomId', room.Name AS 'roomName', room.MaxOccupancy AS 'roomOcc' 
                    FROM Roommate roommate JOIN Room room ON room.Id = Roommate.RoomId
                    WHERE roommate.Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;
                    Room roomyRoom = null;

                    if (reader.Read())
                    {
                        roomyRoom = new Room
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("roomId")),
                            Name = reader.GetString(reader.GetOrdinal("roomName")),
                            MaxOccupancy = reader.GetInt32(reader.GetOrdinal("roomOcc"))

                        };
                        roommate = new Roommate
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("roomyId")),
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
                            RentPortion = reader.GetInt32(reader.GetOrdinal("RentPortion")),
                            MovedInDate = reader.GetDateTime(reader.GetOrdinal("MoveInDate")),
                            Room = roomyRoom
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
                    cmd.CommandText = @"SELECT roommate.Id AS 'roomyId', FirstName, LastName, MoveInDate, RentPortion, room.Id AS 'roomId', room.Name AS 'roomName', room.MaxOccupancy AS 'roomOcc' 
                    FROM Roommate roommate JOIN Room room ON room.Id = Roommate.RoomId 
                    WHERE roommate.RoomId = @roomId";
                    cmd.Parameters.AddWithValue("@roomId", roomId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Roommate> roommates = new List<Roommate>();

                    while (reader.Read())
                    {
                        int idColumnPosition = reader.GetOrdinal("roomyId");
                        int idValue = reader.GetInt32(idColumnPosition);

                        string firstNameValue = reader.GetString(reader.GetOrdinal("FirstName"));

                        int lastNameColumnPosition = reader.GetOrdinal("LastName");
                        string lastNameValue = reader.GetString(lastNameColumnPosition);

                        int rentPortionColumnPosition = reader.GetOrdinal("RentPortion");
                        int rentPortion = reader.GetInt32(rentPortionColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDate = reader.GetDateTime(moveInDateColumnPosition);

                        int roomIdColumnPosition = reader.GetOrdinal("roomId");
                        int roomIdValue = reader.GetInt32(roomIdColumnPosition);


                        int roomNameColumnPosition = reader.GetOrdinal("roomName");
                        string roomName = reader.GetString(roomNameColumnPosition);

                        int roomOcc = reader.GetOrdinal("roomOcc");
                        int roomOccValue = reader.GetInt32(roomOcc);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortion,
                            MovedInDate = moveInDate,
                            Room = new Room
                            {
                                Id = roomIdValue,
                                Name = roomName,
                                MaxOccupancy = roomOccValue
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
                                         VALUES (@Firstname, @Lastname, @RentPortion, @MovedInDate, @RoomId)";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MovedInDate", roommate.MovedInDate);
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
                                        MoveInDate = @MovedInDate,
                                        RoomId = @RoomId            
                                    WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@Firstname", roommate.Firstname);
                    cmd.Parameters.AddWithValue("@Lastname", roommate.Lastname);
                    cmd.Parameters.AddWithValue("@RentPortion", roommate.RentPortion);
                    cmd.Parameters.AddWithValue("@MovedInDate", roommate.MovedInDate);
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
