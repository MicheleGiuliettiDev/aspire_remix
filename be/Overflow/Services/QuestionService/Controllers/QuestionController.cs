namespace QuestionService.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Overflow.Services.QuestionsService.Data;
    using Overflow.Services.QuestionsService.Models;
    using QuestionService.DTOs;

    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class QuestionController(QuestionDbContext db) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<Question>> CreateQuestion(CreateQuestionDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var name = User.FindFirstValue("name");

            if (userId is null || name is null)
                return Unauthorized("Who the heck are you?");

            var question = new Question
            {
                Title = dto.Title,
                Content = dto.Content,
                TagSlugs = dto.TagSlugs,
                AskerId = userId,
                AskerDisplayName = name,
            };

            db.Questions.Add(question);

            await db.SaveChangesAsync();

            return CreatedAtAction($"/questions/{question.Id}", question);
        }
    }
}
