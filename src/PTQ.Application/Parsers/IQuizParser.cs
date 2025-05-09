using PTQ.Models.DTOs;

namespace PTQ.Application.Parsers;

public interface IQuizParser
{
    Task<QuizDTO?> ParseAsync(Stream input);
}