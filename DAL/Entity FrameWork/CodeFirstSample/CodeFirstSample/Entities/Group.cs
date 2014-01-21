using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace CodeFirstSample.Entities
{
    public class Group
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid IdGroup { get; set; }
        [Required] 
        [MaxLength(50)]
        public string Name { get; set; }
        public List<User> Users { get; set; }

        public Group()
        {
            Name = string.Empty;
            Users = new List<User>();
        }
    }
}
