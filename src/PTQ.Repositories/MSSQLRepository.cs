using Microsoft.Data.SqlClient;
using PTQ.Models.DTOs;
using PTQ.Models.Models;
using PTQ.Repositories.Exceptions;

namespace PTQ.Repositories;

public class MSSQLRepository : IRepository
{
    
    private string _connectionString;
    
    public MSSQLRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public IEnumerable<QuizDTO> GetAllQuizzes()
    {
        var quizzes = new List<QuizDTO>();

        string query = "SELECT Id, Name FROM Quiz";

        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        var quiz = new QuizDTO(reader.GetInt32(0),
                            reader.GetString(1));

                        quizzes.Add(quiz);
                    }
                }
            }
        }

        return quizzes;
    }

    public Quiz? GetQuizById(string id)
    {
        
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            
            var baseCmd = new SqlCommand("SELECT Id, Name, PotatoTeacherId, PathFile FROM Quiz WHERE Id = @Id", connection);
            baseCmd.Parameters.AddWithValue("@Id", id);
            
            using (SqlDataReader reader = baseCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        return new Quiz(reader.GetInt32(0),reader.GetString(1),
                            reader.GetInt32(2), reader.GetString(3));
                    }
                }
            }
        }
        throw new NotFoundException();
    }

    public bool AddQuiz(CreateTestDTO createTestDTO)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        using var transaction = connection.BeginTransaction();
        
        try
        {
            string checkQuery = $"SELECT COUNT(*) FROM PotatoTeacher WHERE Name = @Name";
            var checkCmd = new SqlCommand(checkQuery, connection, transaction);
            checkCmd.Parameters.AddWithValue("@Name", createTestDTO.PotatoTeacherName);
            
            PotatoTeacher? potatoTeacher = null;
            using (SqlDataReader reader = checkCmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        potatoTeacher = new PotatoTeacher(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }

            if (potatoTeacher == null)
            {
                var insertTeacherCmd = new SqlCommand(
                    "INSERT INTO PotatoTeacher (Name) VALUES (@Name)",
                    connection, transaction);
                insertTeacherCmd.Parameters.AddWithValue("@Name", createTestDTO.PotatoTeacherName);
                insertTeacherCmd.ExecuteNonQuery();
                
                using (SqlDataReader reader = checkCmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            potatoTeacher = new PotatoTeacher(reader.GetInt32(0), reader.GetString(1));
                        }
                    }
                }
            }
            
            var insertQuizCmd = new SqlCommand(
                "INSERT INTO Quiz (Name,PotatoTeacherId, PathFile) VALUES (@Name, @PotatoTeacherId, @PathFile)",
                connection, transaction);
            insertQuizCmd.Parameters.AddWithValue("@Name", createTestDTO.Name);
            insertQuizCmd.Parameters.AddWithValue("@PotatoTeacherName", potatoTeacher.Id);
            insertQuizCmd.Parameters.AddWithValue("@PathFile", createTestDTO.Path);
            insertQuizCmd.ExecuteNonQuery();
            
            transaction.Commit();
            return true;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}