using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_D.Models;
using Project_D.Models;
using Project_D.Classes;

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
                _context.Department.Add(new Department { Name = "Department1" });
                _context.Department.Add(new Department { Name = "Department2" });
                _context.SaveChanges();
                //add 2 days of 2 years data per department
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 20.0,
                            GasConsumption = 10.0,
                            Date = "18/05/2020",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 0.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 25.0,
                            GasConsumption = 15.0,
                            Date = "19/05/2020",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 15.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 10.0,
                            GasConsumption = 15.0,
                            Date = "18/05/2020",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 10.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 30.0,
                            GasConsumption = 30.0,
                            Date = "19/05/2020",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 5.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 30.0,
                            GasConsumption = 20.0,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 5.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 35.0,
                            GasConsumption = 25.0,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 20.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 20.0,
                            GasConsumption = 10.0,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 15.0,
                            GasGenerated = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 25.0,
                            GasConsumption = 15.0,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 10.0,
                            GasGenerated = 0.0
                        });
                _context.SaveChanges();
            }
            if (_context.User.ToList().Count == 0)
            {
                //add 2 users (1 admin)
                _context.User.Add(new User { UserName = "admin", PassWord = "abc123", Email = "adminMail@gmail.com", IsAdmin = 1 });
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

        [HttpGet]
        [Route("/Admin")]
        public IActionResult Admin()
        {
            List<Data> data = (from d in _context.Data
                               group d by d.Date into g
                               orderby g.Key
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = g.Sum(ec => ec.EnergyConsumption),
                                   GasConsumption = g.Sum(gc => gc.GasConsumption),
                                   EnergyGenerated = g.Sum(eg => eg.EnergyGenerated)
                               }).ToList();
            List<Data> orderedData = DataWithDateTime.OrderedData(data);
            Data total = new Data
            {
                EnergyConsumption = (from d in data select d.EnergyConsumption).Sum(),
                GasConsumption = (from d in data select d.GasConsumption).Sum(),
                EnergyGenerated = (from d in data select d.EnergyGenerated).Sum(),
            };
            var tuple = Tuple.Create(_context.Department.ToList(), orderedData, total, true, new Department());
            return View(tuple);
        }

        [HttpGet]
        [Route("/Admin/{id}")]
        public IActionResult Admin(int id)
        {
            List<Data> data = (from d in _context.Data
                               where d.DepartmentID == id
                               group d by d.Date into g
                               orderby g.Key
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = g.Sum(ec => ec.EnergyConsumption),
                                   GasConsumption = g.Sum(gc => gc.GasConsumption),
                                   DepartmentID = id
                               }).ToList();
            Data total = new Data
            {
                EnergyConsumption = (from d in data where d.DepartmentID == id select d.EnergyConsumption).Sum(),
                GasConsumption = (from d in data where d.DepartmentID == id select d.GasConsumption).Sum(),
                EnergyGenerated = (from d in data where d.DepartmentID == id select d.EnergyGenerated).Sum(),
            };
            List<Data> orderedData = DataWithDateTime.OrderedData(data);
            Department current = (from d in _context.Department
                                 where d.DepartmentID == id
                                 select d).ToList()[0];
            var tuple = Tuple.Create(_context.Department.ToList(), orderedData, total, false, current);
            return View(tuple);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
