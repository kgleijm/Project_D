﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_D.Models;
using Project_D.Models;

namespace Project_D.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly project_DContext _context;

        public HomeController(ILogger<HomeController> logger, project_DContext context)
        {
            _logger = logger;
            _context = context;

            //check if data is empty
            if (_context.Department.ToList().Count == 0 && _context.Data.ToList().Count == 0)
            {
                //add 2 departments
                _context.Department.Add( new Department { Name = "Department1" } );
                _context.Department.Add( new Department { Name = "Department2" } );
                _context.SaveChanges();
                //add 2 days of 2 years data per department
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 20.0f,
                            GasConsumption = 10.0f,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 25.0f,
                            GasConsumption = 15.0f,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 30.0f,
                            GasConsumption = 20.0f,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 35.0f,
                            GasConsumption = 25.0f,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID
                        });
                _context.SaveChanges();
            }
            if (_context.User.ToList().Count == 0)
            {
                //add 2 users (1 admin)
                _context.User.Add( new User { UserName="admin", PassWord="abc123", Email="adminMail@gmail.com", IsAdmin=1 } );
                _context.User.Add(new User { UserName = "genericUser", PassWord = "", Email = "genericEmail@gmail.com", IsAdmin = 0 });
                _context.SaveChanges();
            }
        }

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("/Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("/External")]
        public IActionResult External()
        {
            return View();
        }

        [Route("/Login")]
        public IActionResult Login()
        {
            return View();
        }

        [Route("/Admin")]
        public IActionResult Admin()
        {
            //hier moet ook opgewekte data bij
            var tuple = Tuple.Create(_context.Department.ToList(), _context.Data.ToList(), true);
            return View(tuple);
        }

        [Route("/Admin/{id}")]
        public IActionResult Admin(int id)
        {
            List<Data> data = (from d in _context.Data
                                           where d.DepartmentID == id
                                           select d).ToList();
            var tuple = Tuple.Create(_context.Department.ToList(), data, false);
            return View(tuple);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
