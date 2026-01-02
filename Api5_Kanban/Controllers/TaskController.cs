using Api5_Kanban.Data;
using Api5_Kanban.DTOs;
using Api5_Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api5_Kanban.Controllers
{
    [ApiController]
    [Route("api")]
    public class TaskController : ControllerBase
    {
        private readonly KanbanDbContext _db;
        public TaskController(KanbanDbContext db)
        {
            _db = db;
        }
        [HttpGet("board")]
        public async Task<IActionResult> GetBoard()
        {
            var cols = await _db.Columns
                .OrderBy(c => c.Ord)
                .Select(c => new ColumnDto(c.Id, c.Name, c.Ord))
                .ToListAsync();
            var tasks= await _db.Tasks
                .OrderBy(t=>t.ColId).ThenBy(t=>t.Ord)
                .Select(t=>new TaskDto(t.Id, t.Title, t.ColId, t.Ord))
                .ToListAsync();
            return Ok(new {cols, tasks});
        }
        [HttpPost("tasks")]
        public async Task<IActionResult> CreateTask(CreateTaskDto dto)
        {
            if(!await _db.Columns.AnyAsync(c=>c.Id==dto.ColId))
            {
                return NotFound();
            }
            int maxOrd = await _db.Tasks
                .Where(t => t.ColId == dto.ColId)
                .MaxAsync(t => (int?)t.Ord) ?? 0;
            var task = new TaskItem
            {
                Title=dto.Title,
                ColId=dto.ColId,
                Ord = maxOrd+1
            };
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetBoard),
                new {id=task.Id},
                new TaskDto(task.Id, task.Title, task.ColId, task.Ord));
        }
        [HttpPost("tasks/{id}/move")]
        public async Task<IActionResult> MoveTask(int id, MoveTaskDto dto)
        {
            var task=await _db.Tasks.FindAsync(id);
            if (task==null)
            {
                return NotFound();
            }
            if(!await _db.Columns.AnyAsync(c=>c.Id==dto.ColId))
            {
                return NotFound();
            }
            var tasks = await _db.Tasks
                .Where(t=>t.ColId == dto.ColId && t.Id!=id)
                .OrderBy(t=>t.Ord)
                .ToListAsync();
            int targetOrd = Math.Clamp(dto.Ord, 1, tasks.Count + 1);
            tasks.Insert(targetOrd-1, task);
            for(int i=0; i<tasks.Count; i++)
            {
                tasks[i].Ord = i + 1;
            }
            task.ColId=dto.ColId;
            await _db.SaveChangesAsync();
            return Ok();
        }
    }
}
