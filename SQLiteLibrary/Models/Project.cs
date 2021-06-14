using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteLibrary.Models
{
    class Project
    {
        public string Name { get; set; }
        public string CompanyCustomer { get; set; }
        public string CompanyExecutor { get; set; }
        public Worker Worker { get; set; }
        public Worker Leader { get; set; }
        public Worker Executor { get; set; }
        public DateTime Start { get; set; }
        public DateTime Finish { get; set; }
        public Priority Priority { get; set; }
        public List<Worker> Workers { get; set; } = new List<Worker>();
    }
    public enum Priority : byte
    {
        Low = 1,
        Middle,
        High
    }
}
