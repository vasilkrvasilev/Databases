using StudentSystem.Data;
using StudentSystem.Data.Migrations;
using StudentSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSystem.Client
{
    class StudentSystem
    {
        static void Main()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion
                <StudentSystemContext, Configuration>());

            var context = new StudentSystemContext();
            var course = new Course { Name = "Database" };
            var student = new Student { StudentNumber = 31150 };
            student.Name = "Pesho Petrov";
            course.Students.Add(student);
            student.Courses.Add(course);
            var homework = new Homework { SendDate = DateTime.Now, CourseId = 1, StudentId = 1 };
            var lecture = new Material { CourseId = 1 };
            student.Homeworks.Add(homework);
            course.Materials.Add(lecture);
            course.Homeworks.Add(homework);
            context.Courses.Add(course);
            context.Students.Add(student);
            context.Homeworks.Add(homework);
            context.Materials.Add(lecture);
            context.SaveChanges();
        }
    }
}
