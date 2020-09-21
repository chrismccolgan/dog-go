using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class DogRepository : IDogRepository
    {
        private readonly IConfiguration _config;

        public DogRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }


        public void AddDog(Dog newDog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Dog (Name, OwnerId, Breed, Notes, ImageUrl)
                                        OUTPUT INSERTED.ID
                                        VALUES (@Name, @OwnerId, @Breed, @Notes, @ImageUrl)";

                    cmd.Parameters.AddWithValue("@Name", newDog.Name);
                    cmd.Parameters.AddWithValue("@OwnerId", newDog.OwnerId);
                    cmd.Parameters.AddWithValue("@Breed", newDog.Breed);

                    // LOOK AT THIS
                    //  If newDog.Notes is null, we can use it as the value for the SQL Parameter.
                    //  Instead we use the special value, DbNull.Value.
                    //  This will insert NULL into the Notes column in the database.
                    if (newDog.Notes == null)
                    {
                        cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Notes", newDog.Notes);
                    }

                    // LOOK AT THIS
                    if (newDog.ImageUrl == null)
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@ImageUrl", newDog.ImageUrl);
                    }
                    // nullable columns
                    //cmd.Parameters.AddWithValue("@Notes", newDog.Notes ?? "");
                    //cmd.Parameters.AddWithValue("@ImageUrl", newDog.ImageUrl ?? "");

                    newDog.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public List<Dog> GetAllDogs()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id, d.Name, d.Breed, d.Notes, d.ImageUrl, d.OwnerId, o.Name AS OwnerName
                                        FROM Dog d
                                        LEFT JOIN Owner o ON d.OwnerId = o.Id";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        // LOOK AT THIS
                        //  Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public List<Dog> GetDogsByOwnerId(int ownerId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT d.Id, d.Name, d.Breed, d.Notes, d.ImageUrl, d.OwnerId, o.Name AS OwnerName
                                        FROM Dog d
                                        LEFT JOIN Owner o ON d.OwnerId = o.Id
                                        WHERE d.OwnerId = @ownerId";

                    cmd.Parameters.AddWithValue("@ownerId", ownerId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Dog> dogs = new List<Dog>();

                    while (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                            Owner = new Owner()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("OwnerId")),
                                Name = reader.GetString(reader.GetOrdinal("OwnerName"))
                            }
                        };

                        // Check if optional columns are null
                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        dogs.Add(dog);
                    }
                    reader.Close();
                    return dogs;
                }
            }
        }

        public void UpdateDog(Dog dog)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Dog 
                                        SET
                                            Name = @Name, 
                                            OwnerId = @OwnerId,
                                            Breed = @Breed, 
                                            Notes = @Notes, 
                                            ImageUrl = @ImageUrl
                                        WHERE Id = @Id";

                    cmd.Parameters.AddWithValue("@Name", dog.Name);
                    cmd.Parameters.AddWithValue("@OwnerId", dog.OwnerId);
                    cmd.Parameters.AddWithValue("@Breed", dog.Breed);
                    cmd.Parameters.AddWithValue("@Id", dog.Id);
                    cmd.Parameters.AddWithValue("@Notes", dog.Notes ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl ?? (object)DBNull.Value);

                    //cmd.Parameters.AddWithValue("@Notes", dog.Notes ?? "");
                    //cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl ?? "");

                    //if (dog.Notes == null)
                    //{
                    //    cmd.Parameters.AddWithValue("@Notes", DBNull.Value);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@Notes", dog.Notes);
                    //}

                    //if (dog.ImageUrl == null)
                    //{
                    //    cmd.Parameters.AddWithValue("@ImageUrl", DBNull.Value);
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@ImageUrl", dog.ImageUrl);
                    //}

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Dog GetDogById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, Name, OwnerId, Breed, Notes, ImageUrl
                        FROM Dog
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Breed = reader.GetString(reader.GetOrdinal("Breed")),
                            OwnerId = reader.GetInt32(reader.GetOrdinal("OwnerId"))
                        };

                        if (reader.IsDBNull(reader.GetOrdinal("Notes")) == false)
                        {
                            dog.Notes = reader.GetString(reader.GetOrdinal("Notes"));
                        }
                        if (reader.IsDBNull(reader.GetOrdinal("ImageUrl")) == false)
                        {
                            dog.ImageUrl = reader.GetString(reader.GetOrdinal("ImageUrl"));
                        }

                        reader.Close();
                        return dog;
                    }

                    reader.Close();
                    return null;
                }
            }
        }
        //public void DeleteOwner(int dogId)
        //{
        //    using (SqlConnection conn = Connection)
        //    {
        //        conn.Open();

        //        using (SqlCommand cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = @"
        //                    DELETE FROM Dog
        //                    WHERE Id = @Id
        //                ";

        //            cmd.Parameters.AddWithValue("@Id", dogId);

        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //}

    }
}