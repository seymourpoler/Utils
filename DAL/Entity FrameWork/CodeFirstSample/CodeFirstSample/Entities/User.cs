using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid IdUser { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public List<Task> Tasks { get; set; }

        public User()
        {
            Name = string.Empty;
            Login = String.Empty;
            Password = string.Empty;
            Tasks = new List<Task>();
        }
    }
}
