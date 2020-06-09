using Microsoft.Data.SqlClient;
using Roommates.Models;


namespace Roommates.Repositories
{
    class ChoreRepository: BaseRepository
    {
        public ChoreRepository(string connectionString) : base(connectionString) { }

        public void Insert(Chore chore)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Chore (Name) 
                                        OUTPUT INSERTED.Id 
                                        VALUES (@Name)";
                    cmd.Parameters.AddWithValue("@Name", chore.Name);
                    int id = (int)cmd.ExecuteScalar();

                    chore.Id = id;
                }
            }
        }
        public Chore GetWithChoreName(string choreName)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT *
                                        FROM Chore chore
                                        WHERE chore.Name = @choreName";
                    cmd.Parameters.AddWithValue("@choreName", choreName);
                    SqlDataReader reader = cmd.ExecuteReader();

                    Chore chore = null;
                    if (reader.Read())
                    {
                        chore = new Chore
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = choreName
                        };
                    }
                    reader.Close();
                    return chore;
                }
            }
        }

    }
}
