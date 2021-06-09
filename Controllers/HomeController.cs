using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using project_D.Models;
using Project_D.Models;
using Project_D.Classes;
using System.Data;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Project_D.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly project_DContext _context;
        private IWebHostEnvironment _env;
        private DateTimeFormatInfo cultureInfo = new CultureInfo("nl-NL").DateTimeFormat;
        public HomeController(ILogger<HomeController> logger, project_DContext context, IWebHostEnvironment env)
        {
            _logger = logger;
            _context = context;
            _env = env;

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
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 25.0,
                            GasConsumption = 15.0,
                            Date = "19/05/2020",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 15.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 10.0,
                            GasConsumption = 15.0,
                            Date = "18/05/2020",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 10.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 30.0,
                            GasConsumption = 30.0,
                            Date = "19/05/2020",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 5.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 30.0,
                            GasConsumption = 20.0,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 5.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 35.0,
                            GasConsumption = 25.0,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[1].DepartmentID,
                            EnergyGenerated = 20.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 20.0,
                            GasConsumption = 10.0,
                            Date = "18/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 15.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
                        });
                _context.Data.Add(
                        new Data
                        {
                            EnergyConsumption = 25.0,
                            GasConsumption = 15.0,
                            Date = "19/05/2021",
                            DepartmentID = _context.Department.ToList()[0].DepartmentID,
                            EnergyGenerated = 10.0,
                            GasAdjustment = 0.0,
                            EnergyAdjustment = 0.0,
                            EnergyGenAdjustment = 0.0
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
            List<Data> data = (from d in _context.Data
                               group d by d.Date into g
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = (g.Sum(ec => ec.EnergyConsumption) + g.Sum(ea => ea.EnergyAdjustment)),
                                   GasConsumption = (g.Sum(gc => gc.GasConsumption) + g.Sum(ga => ga.GasAdjustment)),
                                   EnergyGenerated = g.Average(eg => eg.EnergyGenerated),
                                   EnergyGenAdjustment = g.Average(eg => eg.EnergyGenAdjustment)
                               }).ToList();
            List<DataWithDateTime> orderedData = DataWithDateTime.WithDateTime(data);
            List<DataWithDateTime> last7Days;
            if (orderedData.Count > 7)
                last7Days = orderedData.GetRange(orderedData.Count - 7, 7);
            else
                last7Days = orderedData;
            List<string> days = new List<string>();
            List<int> gasData = new List<int>();
            List<int> electricityData = new List<int>();
            foreach (var day in last7Days)
            {
                DayOfWeek dayOfWeek = day.Date.DayOfWeek;
                string abbreviation = cultureInfo.GetAbbreviatedDayName(dayOfWeek);
                days.Add(abbreviation);
                gasData.Add((int)(day.GasConsumption + day.GasAdjustment));
                electricityData.Add((int)(day.EnergyConsumption + day.EnergyAdjustment));
            }
            Tuple<string, string, string> tuple = new Tuple<string, string, string>(JsonSerializer.Serialize(days), JsonSerializer.Serialize(electricityData), JsonSerializer.Serialize(gasData));
            return View(tuple);
        }

        [Route("/{id}")]
        public IActionResult Index(int id)
        {
            return View();
        }

        [Route("/Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        [Route("/External")]
        public IActionResult External()
        {
            double total = (from d in _context.Data
                            group d by d.Date into g
                            select g.Average(eg => eg.EnergyGenerated) + g.Average(eg => eg.EnergyGenAdjustment)).Sum();

            return View(total);
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
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = (g.Sum(ec => ec.EnergyConsumption) + g.Sum(ea => ea.EnergyAdjustment)),
                                   GasConsumption = (g.Sum(gc => gc.GasConsumption) + g.Sum(ga => ga.GasAdjustment)),
                                   EnergyGenerated = g.Average(eg => eg.EnergyGenerated),
                                   EnergyGenAdjustment = g.Average(eg => eg.EnergyGenAdjustment)
                               }).ToList();
            List<Data> orderedData = DataWithDateTime.OrderedData(data);
            Data total = new Data
            {
                EnergyConsumption = (from d in data select d.EnergyConsumption).Sum() + (from d in data select d.EnergyAdjustment).Sum(),
                GasConsumption = (from d in data select d.GasConsumption).Sum() + (from d in data select d.GasAdjustment).Sum(),
                EnergyGenerated = ((from d in _context.Data group d by d.Date into g select g.Average(eg => eg.EnergyGenerated)).Sum() + 
                                    (from d in _context.Data group d by d.Date into g select g.Average(eg => eg.EnergyGenAdjustment)).Sum())
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
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = g.Sum(ec => ec.EnergyConsumption),
                                   EnergyAdjustment = g.Sum(ea => ea.EnergyAdjustment),
                                   GasConsumption = g.Sum(gc => gc.GasConsumption),
                                   GasAdjustment = g.Sum(ga => ga.GasAdjustment),
                                   DepartmentID = id
                               }).ToList();
            Data total = new Data
            {
                EnergyConsumption = (from d in data where d.DepartmentID == id select d.EnergyConsumption).Sum() + (from d in data where d.DepartmentID == id select d.EnergyAdjustment).Sum(),
                GasConsumption = (from d in data where d.DepartmentID == id select d.GasConsumption).Sum() + (from d in data where d.DepartmentID == id select d.GasAdjustment).Sum(),
            };
            List<Data> orderedData = DataWithDateTime.OrderedData(data);
            Department current = (from d in _context.Department
                                 where d.DepartmentID == id
                                 select d).ToList()[0];
            var tuple = Tuple.Create(_context.Department.ToList(), orderedData, total, false, current);
            return View(tuple);
        }

        [HttpPost]
        public IActionResult EnergyGenerationAdjustment(string[] keys, int[] adjustments)
        {
            List<Data> data = _context.Data.ToList();
            for (int i = 0; i < data.Count; i++)
            {
                int index = Array.IndexOf(keys, data[i].Date);
                if (data[i].EnergyGenAdjustment != adjustments[index])
                {
                    data[i].EnergyGenAdjustment = adjustments[index];
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Admin");
        }

        [HttpPost]
        public IActionResult EnergyAndGasAdjustments(int departmentId, string[] keys, int[] energyAdjustments, int[] gasAdjustments)
        {
            List<Data> data = _context.Data.ToList();
            for (int i = 0; i < data.Count; i++)
            {
                int index = Array.IndexOf(keys, data[i].Date);
                if (data[i].DepartmentID == departmentId && data[i].EnergyAdjustment != energyAdjustments[index])
                {
                    data[i].EnergyAdjustment = energyAdjustments[index];
                }
                if (data[i].DepartmentID == departmentId && data[i].GasAdjustment != gasAdjustments[index])
                {
                    data[i].GasAdjustment = gasAdjustments[index];
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Admin", new { id = departmentId });
        }

        /*
         Dag
        Energieverbuik
        Gasverbruik
        Oorspronkelijke opgewekte energie
        Aanpassing opgewekte energie
        Totaal opgewekte energie
         */

        [Route("/Admin/Export")]
        public FileResult ExportData()
        {
            //Create the datatable
            List<Data> data = (from d in _context.Data
                               group d by d.Date into g
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = (g.Sum(ec => ec.EnergyConsumption) + g.Sum(ea => ea.EnergyAdjustment)),
                                   GasConsumption = (g.Sum(gc => gc.GasConsumption) + g.Sum(ga => ga.GasAdjustment)),
                                   EnergyGenerated = g.Average(eg => eg.EnergyGenerated),
                                   EnergyGenAdjustment = g.Average(eg => eg.EnergyGenAdjustment)
                               }).ToList();
            List<Data> orderedData = DataWithDateTime.OrderedData(data);
            Data total = new Data
            {
                EnergyConsumption = (from d in data select d.EnergyConsumption).Sum() + (from d in data select d.EnergyAdjustment).Sum(),
                GasConsumption = (from d in data select d.GasConsumption).Sum() + (from d in data select d.GasAdjustment).Sum(),
                EnergyGenerated = ((from d in _context.Data group d by d.Date into g select g.Average(eg => eg.EnergyGenerated)).Sum() +
                                    (from d in _context.Data group d by d.Date into g select g.Average(eg => eg.EnergyGenAdjustment)).Sum())
            };


            DataTable dt = new DataTable();
            DataColumn[] columns = new DataColumn[]
            {
                new DataColumn("Dag"),
                new DataColumn("Energieverbruik"),
                new DataColumn("Gasverbruik"),
                new DataColumn("Oorspronkelijke opgewekte energie"),
                new DataColumn("Aanpassing opgewekte energie"),
                new DataColumn("Totaal opgewekte energie")
            };
            dt.Columns.AddRange(columns);

            foreach(Data d in data)
            {
                dt.Rows.Add(d.Date, d.EnergyConsumption, d.GasConsumption, d.EnergyGenerated, d.EnergyGenAdjustment, (d.EnergyGenerated + d.EnergyGenAdjustment));
            }
            dt.Rows.Add("TOTAAL:", total.EnergyConsumption, total.GasConsumption, "", "", total.EnergyGenerated);

            //convert the datatable to csv and return the file
            string path = _env.WebRootPath + ".\\Content\\Data-Totaal.csv";
            ToCSV(dt, path);

            Response.Headers.Add("Content-Disposition", "inline; filename=Data-Totaal.csv");
            return File("~/Content/Data-Totaal.csv", "tekst/csv");
        }

        /*
        Dag
        Energieverbuik
        Aanpassing energieverbruik
        Totaal energieverbruik
        Gasverbruik
        Aanpassing gasverbruik
        Totaal gasverbruik
         */

        [Route("/Admin/Export/{id}")]
        public FileResult ExportData(int id)
        {
            //Create the datatable
            List<Data> data = (from d in _context.Data
                               where d.DepartmentID == id
                               group d by d.Date into g
                               select new Data
                               {
                                   Date = g.Key,
                                   EnergyConsumption = g.Sum(ec => ec.EnergyConsumption),
                                   EnergyAdjustment = g.Sum(ea => ea.EnergyAdjustment),
                                   GasConsumption = g.Sum(gc => gc.GasConsumption),
                                   GasAdjustment = g.Sum(ga => ga.GasAdjustment),
                                   DepartmentID = id
                               }).ToList();
            Data total = new Data
            {
                EnergyConsumption = (from d in data where d.DepartmentID == id select d.EnergyConsumption).Sum() + (from d in data where d.DepartmentID == id select d.EnergyAdjustment).Sum(),
                GasConsumption = (from d in data where d.DepartmentID == id select d.GasConsumption).Sum() + (from d in data where d.DepartmentID == id select d.GasAdjustment).Sum(),
            };
            List<Data> orderedData = DataWithDateTime.OrderedData(data);


            DataTable dt = new DataTable();
            DataColumn[] columns = new DataColumn[]
            {
                new DataColumn("Dag"),
                new DataColumn("Energieverbruik"),
                new DataColumn("Aanpassing energieverbruik"),
                new DataColumn("Totaal energieverbruik"),
                new DataColumn("Gasverbruik"),
                new DataColumn("Aanpassing gasverbruik"),
                new DataColumn("Totaal gasverbruik")
            };
            dt.Columns.AddRange(columns);

            foreach (Data d in data)
            {
                dt.Rows.Add(d.Date, d.EnergyConsumption, d.EnergyAdjustment, (d.EnergyConsumption + d.EnergyAdjustment), d.GasConsumption, d.GasAdjustment, (d.GasConsumption + d.GasAdjustment));
            }
            string depName = (from d in _context.Department where d.DepartmentID == id select d.Name).ToList()[0];
            dt.Rows.Add("TOTAAL -"+ depName+":", "", "", total.EnergyConsumption, "", "", total.GasConsumption);

            //convert the datatable to csv and return the file

            string path = _env.WebRootPath + ".\\Content\\Data-" + depName + ".csv";
            ToCSV(dt, path);


            Response.Headers.Add("Content-Disposition", "inline; filename=Data-"+ depName +".csv");
            return File("~/Content/Data-" + depName + ".csv", "tekst/csv");
        }

        public static void ToCSV(DataTable dt, string path)
        {
            StreamWriter sw = new StreamWriter(path, false);
            //headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                sw.Write(dt.Columns[i]);
                if (i < dt.Columns.Count - 1)
                {
                    sw.Write(",");
                }
            }

            sw.Write(sw.NewLine);
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        string value = dr[i].ToString();
                        if (value.Contains(','))
                        {
                            value = String.Format("\"{0}\"", value);
                            sw.Write(value);
                        } else
                        {
                            sw.Write(dr[i].ToString());
                        }
                    }
                    if (i < dt.Columns.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.Write(sw.NewLine);
            }
            sw.Close();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
