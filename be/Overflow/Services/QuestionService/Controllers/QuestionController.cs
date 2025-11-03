namespace QuestionService.Controllers
{
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
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
            var validTags = await db.Tags.Where(t => dto.TagSlugs.Contains(t.Slug)).ToListAsync();

            var missing = dto.TagSlugs.Except(validTags.Select(t => t.Slug).ToList()).ToList();

            if (missing.Count > 0)
            {
                return BadRequest($"The following tags do not exist: {string.Join(", ", missing)}");
            }

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

        public async Task<ActionResult<List<Question>>> GetQuestions(string? tag)
        {
            var query = db.Questions.AsQueryable();

            if (!string.IsNullOrWhiteSpace(tag))
            {
                query = query.Where(q => q.TagSlugs.Contains(tag));
            }

            return await query.OrderByDescending(x => x.CreatedAt).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestionById(string id)
        {
            var question = await db.Questions.FindAsync(id);

            if (question is null)
            {
                return NotFound();
            }

            await db
                .Questions.Where(q => q.Id == id)
                .ExecuteUpdateAsync(setters =>
                    setters.SetProperty(q => q.ViewCount, q => q.ViewCount + 1)
                );

            return question;
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult<Question>> UpdateQuestion(string id, CreateQuestionDto dto)
        {
            var question = await db.Questions.FindAsync(id);

            if (question is null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null || question.AskerId != userId)
            {
                return Forbid("You are not allowed to update this question.");
            }

            var validTags = await db.Tags.Where(t => dto.TagSlugs.Contains(t.Slug)).ToListAsync();

            var missing = dto.TagSlugs.Except(validTags.Select(t => t.Slug).ToList()).ToList();

            if (missing.Count > 0)
            {
                return BadRequest($"The following tags do not exist: {string.Join(", ", missing)}");
            }

            question.Title = dto.Title;
            question.Content = dto.Content;
            question.TagSlugs = dto.TagSlugs;
            question.UpdatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return question;
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteQuestion(string id)
        {
            var question = await db.Questions.FindAsync(id);

            if (question is null)
            {
                return NotFound();
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId is null || question.AskerId != userId)
            {
                return Forbid("You are not allowed to delete this question.");
            }

            db.Questions.Remove(question);
            await db.SaveChangesAsync();

            return NoContent();
        }
    }
}
