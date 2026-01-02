namespace Api5_Kanban.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ColId { get; set; }
        public int Ord {  get; set; }
    }
}
