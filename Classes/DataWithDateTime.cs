using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using project_D.Models;

namespace Project_D.Classes
{
    public class DataWithDateTime
	{
		public int DataID { get; set; }
		public double EnergyConsumption { get; set; }
		public double GasConsumption { get; set; }
		public double EnergyGenerated { get; set; }
		public double GasGenerated { get; set; }
		public DateTime Date { get; set; }
		public int DepartmentID { get; set; }

		static public List<Data> OrderedData (List<Data> unorderedData)
        {
			List<DataWithDateTime> unorderedWithDateTime = new List<DataWithDateTime>();
			foreach (Data dat in unorderedData)
            {
				unorderedWithDateTime.Add( new DataWithDateTime {
					EnergyConsumption = dat.EnergyConsumption,
					GasConsumption = dat.GasConsumption,
					EnergyGenerated = dat.EnergyGenerated,
					GasGenerated = dat.EnergyGenerated,
					Date = DateTime.ParseExact(dat.Date, "dd/MM/yyyy", CultureInfo.InvariantCulture),
					DepartmentID = dat.DepartmentID
				} );
            }
			List<DataWithDateTime> orderedWithDateTime = (from data in unorderedWithDateTime
														  orderby data.Date ascending
														  select data).ToList();
			List<Data> orderedData = new List<Data>();
			foreach (DataWithDateTime dat in orderedWithDateTime)
			{
				orderedData.Add(new Data
				{
					EnergyConsumption = dat.EnergyConsumption,
					GasConsumption = dat.GasConsumption,
					EnergyGenerated = dat.EnergyGenerated,
					GasGenerated = dat.EnergyGenerated,
					Date = dat.Date.ToString("dd/MM/yyyy"),
					DepartmentID = dat.DepartmentID
				});
			}
			return orderedData;
		}
	}
}
