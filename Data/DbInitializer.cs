using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SchoolSystemManagement.Models;

namespace SchoolSystemManagement.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {

            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Universities.Any())
            {
                return;   // DB has been seeded
            }

            var universities = new University[]
            {
                new University{Name="DEU",City="İzmir",Country="Turkey"}
            };
            foreach (University u in universities)
            {
                context.Universities.Add(u);
            }
            context.SaveChanges();

            var staffWorks = new StaffWork[]
            {
                new StaffWork{Type="Security"}
            };
            foreach (StaffWork sw in staffWorks)
            {
                context.StaffWorks.Add(sw);
            }
            context.SaveChanges();

            var staffs = new Staff[]
            {
                new Staff{Name="Mesut",Surname="Ozil",StaffWork=staffWorks[0],University=universities[0]}
            };
            foreach (Staff s in staffs)
            {
                context.Staffs.Add(s);
            }
            context.SaveChanges();

            var faculties = new Faculty[]
            {
                new Faculty{Name="Engineering",University=universities[0]}
            };
            foreach (Faculty f in faculties)
            {
                context.Faculties.Add(f);
            }

            var departments = new Department[]
            {
                new Department{Name="Computer Engineering",Language="English",PrimaryEducation=true,
                               SecondaryEducation=false, Faculty=faculties[0]}
            };
            foreach (Department d in departments)
            {
                context.Departments.Add(d);
            }

            var students = new Student[]
            {
                new Student{Name="Rıdvan", Surname="Özdemir", Birthday=new DateTime(1998,7,24),
                            Email="ridvan.ozdemir@ogr.deu.edu.tr", Phone="05376447972", SchoolNumber=2017510086,
                            Class=3, Department=departments[0]},

                new Student{Name="Idrishan", Surname="Parlayan", Birthday=new DateTime(1999,8,31),
                            Email="idrishan.parlayan@ogr.deu.edu.tr", Phone="05647256475", SchoolNumber=2017510066,
                            Class=3, Department=departments[0]},

                new Student{Name="Emrecan", Surname="Tan", Birthday=new DateTime(1998,4,30),
                            Email="emrecan.tan@ogr.deu.edu.tr", Phone="05434562365", SchoolNumber=2017510072,
                            Class=3, Department=departments[0]}
            };
            foreach (Student st in students)
            {
                context.Students.Add(st);
            }

            var lecturers = new Lecturer[]
            {
                new Lecturer{Name="Yalçın", Surname="Çebi", Birthday=new DateTime(1958,3,12),
                            Email="yalcin.cebi@deu.edu.tr", Phone="05325479785", Rank=Rank.Prof,
                            Salary=15000, Department=departments[0]},

                new Lecturer{Name="Alp", Surname="Kut", Birthday=new DateTime(1969,5,22),
                            Email="alp.kut@deu.edu.tr", Phone="05347246178", Rank=Rank.Prof,
                            Salary=15000, Department=departments[0]}
            };
            foreach (Lecturer l in lecturers)
            {
                context.Lecturers.Add(l);
            }

            var lessons = new Lesson[]
            {
                new Lesson{Name="Introduction of Computer Engineering",Code="CME1201",LessonPerWeek=4,
                               Credit=3, Lecturer=lecturers[0], Department=departments[0]},

                new Lesson{Name="Algorithms and Programming",Code="CME1203",LessonPerWeek=6,
                               Credit=4, Lecturer=lecturers[1], Department=departments[0]}
            };
            foreach (Lesson l in lessons)
            {
                context.Lessons.Add(l);
            }

            var studentLessons = new StudentLesson[]
            {
                new StudentLesson{Grade=Grade.CB,Student=students[0], Lesson=lessons[0]},

                new StudentLesson{Grade=Grade.AA,Student=students[0], Lesson=lessons[1]},

                new StudentLesson{Grade=Grade.BB,Student=students[1], Lesson=lessons[0]},

                new StudentLesson{Grade=Grade.CC,Student=students[1], Lesson=lessons[1]},

                new StudentLesson{Grade=Grade.BA,Student=students[2], Lesson=lessons[0]},

                new StudentLesson{Grade=Grade.AA,Student=students[2], Lesson=lessons[1]},
            };
            foreach (StudentLesson sl in studentLessons)
            {
                context.StudentLessons.Add(sl);
            }
            context.SaveChanges();
            var users = new User[]
            {
                new User{UserName="ridvan.ozdemir",Password="2017510086", UserType=0,person=context.Students.FirstOrDefault(m => m.Email=="ridvan.ozdemir@ogr.deu.edu.tr").ID},

                new User{UserName="idrishan.parlayan",Password="2017510066",UserType=0,person=context.Students.FirstOrDefault(m => m.Email=="idrishan.parlayan@ogr.deu.edu.tr").ID},

                new User{UserName="emrecan.tan",Password="2017510072",UserType=0,person=context.Students.FirstOrDefault(m => m.Email=="emrecan.tan@ogr.deu.edu.tr").ID},

                new User{UserName="yalcin.cebi",Password="123321",UserType=1,person=context.Lecturers.FirstOrDefault(m => m.Email=="yalcin.cebi@deu.edu.tr").ID},

                new User{UserName="alp.kut",Password="123321",UserType=1,person=context.Lecturers.FirstOrDefault(m => m.Email=="alp.kut@deu.edu.tr").ID},

                new User{UserName="admin",Password="admin",UserType=2}
            };
            foreach (User u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();



        }
    }
}
