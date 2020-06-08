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
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate";

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
                        int rentPortion = reader.GetInt32(rentPortionColumnPosition);

                        int moveInDateColumnPosition = reader.GetOrdinal("MoveInDate");
                        DateTime moveInDate = reader.GetDateTime(moveInDateColumnPosition);

                        int roomIdColumnPosition = reader.GetOrdinal("RoomId");
                        int roomId = reader.GetInt32(roomIdColumnPosition);

                        Roommate roommate = new Roommate
                        {
                            Id = idValue,
                            Firstname = firstNameValue,
                            Lastname = lastNameValue,
                            RentPortion = rentPortion,
                            MovedInDate = moveInDate,
                            Room = null
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
                    cmd.CommandText = "SELECT Id, FirstName, LastName, RentPortion, MoveInDate, RoomId FROM Roommate WHERE Id = @id";
                    cmd.Parameters.AddWithValue("@id", id);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Roommate roommate = null;

                    if (reader.Read())
                    {
                        roommate = new Roommate
                        {
                            Id = id,
                            Firstname = reader.GetString(reader.GetOrdinal("FirstName")),
                            Lastname = reader.GetString(reader.GetOrdinal("LastName")),
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
                    cmd.CommandText = "SELECT roommate.Id AS 'roomyId', FirstName, LastName, MoveInDate, RentPortion, room.Id AS 'roomId', room.Name AS 'roomName', room.MaxOccupancy AS 'roomOcc' FROM Roommate roommate JOIN Room room ON room.Id = Roommate.RoomId WHERE roommate.RoomId = @roomId";
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
    }
}
