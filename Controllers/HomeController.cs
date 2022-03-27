﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileStore.Models;

namespace MobileStore.Controllers
{
    public class HomeController: Controller
    {
        MobileContext db;
        public HomeController(MobileContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.Phones.ToList());
        }

        [HttpGet]
        public IActionResult Buy(int? id)
        {
            if (id == null) return RedirectToAction("Index");
            ViewBag.PhoneId = id;
            return View();
        }

        [HttpPost]
        public string Buy(Order order)
        {
            db.Orders.Add(order);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо, " + order.User + ", за покупку!";
        }

        [HttpGet]
        public string Add()
        {

            if (db.Blogs.FirstOrDefault(b => b.Id == 1) != null)
            {
                return "evrething is ok";
            }

            db.Blogs.Add(
            new Blog
            {
                Id = 1,
                Name = ".NET Blog",
                Posts =
                {
                    new Post
                    {
                        Id = 1,
                        Title = "Announcing the Release of EF Core 5.0",
                        Content = "Announcing the release of EF Core 5.0, a full featured cross-platform..."
                    },
                    new Post
                    {
                        Id = 2,
                        Title = "Announcing F# 5",
                        Content = "F# 5 is the latest version of F#, the functional programming language..."
                    }
                }
            });

            db.SaveChanges();

            return "added";
        }

        [HttpGet]
        public IActionResult Update(Post post)
        {
            List<Post> prepareToUpdate = new List<Post> { 
                new Post {Id = 1, Title = "Hello world", Content = "empty"}
            };

            db.Blogs.Update(
                new Blog
                {
                    Id = 1,
                    Name = ".NET Blog updated",
                    Posts = prepareToUpdate
                });


            db.SaveChanges();

            return Content("updated");   
        }


        [HttpPost("[controller]/students/")]
        public IActionResult AddStudent([FromBody]Student student)
        {
            db.Students.Add(student);

            db.SaveChanges();

            return Ok(student);
        }

        [HttpPost("[controller]/students/{id:int}")]
        public IActionResult UpdateStudents([FromBody] Student requestStudent)
        {
            var existStudent = db.Students
                                .Include(c => c.Courses)
                                .FirstOrDefault(s => s.StudentId == requestStudent.StudentId);

            existStudent.FirstName = requestStudent.FirstName;
            existStudent.LastName = requestStudent.LastName;

            // 1. remove all except the "old"
            existStudent.Courses.RemoveAll(sc => !requestStudent.Courses.Exists(
                c => c.CourseId == sc.CourseId && existStudent.StudentId == requestStudent.StudentId
               ));

            // 2. remove all ids already in database from requestStudent
            requestStudent.Courses.RemoveAll(sc => existStudent.Courses.Exists(
                c => c.CourseId == sc.CourseId && existStudent.StudentId == requestStudent.StudentId
                ));

            // 3. add all nwe courses not yet seen in the database
            existStudent.Courses.AddRange(requestStudent.Courses);

            db.SaveChanges();

            return Ok(existStudent.Courses.Count());
        }
    }
}
