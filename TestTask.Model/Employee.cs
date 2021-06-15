using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Model
{
    [Table("Employee")]
    public class Employee
    {
        [Key]
        public int Id { get; set; } //Идентификатор
        public string Name { get; set; } //Имя
        public string Surname { get; set; } //Фамилия
        public string Patronymic { get; set; } //Отчество
        public string Email { get; set; }
        public ICollection<Project> LeaderProjects { get; set; } //1 сотрудник может быть руководителем нескольких проектов
        public ICollection<Project> EmployeeProjects { get; set; } //1 сотрудник может быть работником нескольких проектов
        public ICollection<Project> ExecutorProjects { get; set; } //1 сотрудник может быть исполнителем нескольких проектов
        //public List<EmployeeProject> EmpPrj { get; set; }
        public override string ToString()
        {
            return $"{Surname} {Name[0]}. {Patronymic[0]}.";
        }
    }
}
