using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteLibrary.Models
{
    class Worker
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
    }
}
