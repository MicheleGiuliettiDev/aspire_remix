using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Overflow.Services.QuestionsService.Data;
using Overflow.Services.QuestionsService.Models;

namespace QuestionService.Controllers;

[ApiController]
[Route("[controller]")]
class TagsController(QuestionDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Tag>>> GetTags()
    {
        var tags = await db.Tags.OrderBy(t => t.Name).ToListAsync();
        return Ok(tags);
    }
}
