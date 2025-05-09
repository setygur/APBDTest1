using PTQ.Models.DTOs;
using PTQ.Models.Models;

namespace PTQ.Repositories;

public interface IRepository
{
    IEnumerable<QuizDTO> GetAllQuizzes();
    Quiz? GetQuizById(string id);
    bool AddQuiz(CreateTestDTO createTestDTO);
}