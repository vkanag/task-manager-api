using System.Text.Json.Serialization;

namespace TaskManagementAPI.Models
{
    public class Task
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public TaskPriority Priority { get; set; }  
        public DateTime? Deadline { get; set; }
        public bool IsCompleted { get; set; }
        public int UserId { get; set; }        

        // Define the enum for task priorities
        public enum TaskPriority
        {
            High = 1,    
            Medium = 2,   
            Low = 3   
        }

    }
}
