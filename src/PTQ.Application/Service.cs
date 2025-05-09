using PTQ.Models.DTOs;
using PTQ.Models.Models;
using PTQ.Repositories;

namespace PTQ.Application;

public class Service : IService
{
    private readonly IRepository _repository;

    public Service(string connectionString)
    {
        _repository = new MSSQLRepository(connectionString);
    }
    
    public IEnumerable<QuizDTO> GetAllQuizzes() => _repository.GetAllQuizzes();
    public Quiz? GetQuizById(string id) => _repository.GetQuizById(id);
    public bool AddQuiz(CreateTestDTO createTestDTO) => _repository.AddQuiz(createTestDTO);
}