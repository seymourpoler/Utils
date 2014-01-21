using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class Task
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid IdTask { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public int Minutes { get; set; }
        public Status Status { get; set; }
        
        public Task()
        {
            Name = string.Empty;
            Priority = 0;
            Minutes = 0;
            Status = null;
        }
    }
}
