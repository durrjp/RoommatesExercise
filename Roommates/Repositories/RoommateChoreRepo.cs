using Microsoft.Data.SqlClient;
using Roommates.Models;


namespace Roommates.Repositories
{
    class RoommateChoreRepo: BaseRepository
    {
        public RoommateChoreRepo(string connectionString) : base(connectionString) { }

        public void InsertRC(Roommate roommate, Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO RoommateChore (RoommateId, ChoreID)
                                        OUTPUT INSERTED.Id 
                                        VALUES (@RoommateId, @ChoreID)";
                    cmd.Parameters.AddWithValue("@RoommateId", roommate.Id);
                    cmd.Parameters.AddWithValue("@ChoreID", chore.Id);
                    int id = (int)cmd.ExecuteScalar();

                    roommate.Id = id;
                    RoommateChore rmChore = new RoommateChore()
                    {
                        Id = id,
                        RoommateId = roommate.Id,
                        ChoreID = chore.Id
                    };
                }
            }
        }

    }
}
