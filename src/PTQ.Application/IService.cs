using PTQ.Models.DTOs;
using PTQ.Models.Models;

namespace PTQ.Application;

public interface IService
{
    IEnumerable<QuizDTO> GetAllQuizzes();
    Quiz? GetQuizById(string id);
    bool AddQuiz(CreateTestDTO createTestDTO);
}