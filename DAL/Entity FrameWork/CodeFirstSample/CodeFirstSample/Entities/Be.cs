using System;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class Be
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid IdBe { get; set; }
        public string Name { get; set; }

        public Be()
        {
            IdBe = new Guid("00000000-0000-0000-0000-000000000001");
            Name = "Stopped";
        }
    }
}
