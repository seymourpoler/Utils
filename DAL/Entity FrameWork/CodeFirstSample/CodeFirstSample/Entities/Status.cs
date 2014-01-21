﻿using System;
using System.ComponentModel.DataAnnotations;

namespace CodeFirstSample.Entities
{
    public class Status
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid IdStatus { get; set; }
        public string Name { get; set; }

        public Status()
        {
            IdStatus = new Guid("00000000-0000-0000-0000-000000000001");
            Name = "Stopped";
        }
    }
}
