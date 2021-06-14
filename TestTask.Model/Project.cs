using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTask.Model
{
    [Table("Project")]
    public class Project
    {
        [Key]
        public int Id                         { get; set; } //Идентификатор
        public string Name                    { get; set; } //Имя
        public string CompanyCustomer         { get; set; } //Компания-заказчик
        public string CompanyExecutor         { get; set; } //Компания-исполнитель
        public int? EmployeeId                 { get; set; }
        public Employee Employee              { get; set; } //Сотрудник
        public int? LeaderId                   { get; set; }
        public Employee Leader                { get; set; } //Руководитель
        public ICollection<Employee> Executors { get; set; } //Исполнитель
        public DateTime Start                 { get; set; } //Дата начала проекта
        public DateTime Finish                { get; set; } //Дата окончания проекта
        [Column(TypeName = "int")]                          
        public Priority Priority              { get; set; } //Приоритет

        //public List<EmployeeProject> EmpPrj { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
