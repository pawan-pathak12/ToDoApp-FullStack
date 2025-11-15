namespace TODOAPI.Dtis
{
    public class CreateToDoDto
    {
        public string Title { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
