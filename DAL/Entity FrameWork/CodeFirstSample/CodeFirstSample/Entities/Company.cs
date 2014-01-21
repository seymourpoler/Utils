using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class Company
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid IdCompany { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<Group> Groups { get; set; }

        public Company()
        {
            Name = string.Empty;
            Groups = new List<Group>();
        }
    }
}
