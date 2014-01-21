using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class Project
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<User> Users { get; set; }

        public Project()
        {
            Name = string.Empty;
            Users = new List<User>();
        }
    }
}
