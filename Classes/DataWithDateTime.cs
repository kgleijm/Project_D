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
		public double EnergyConsumption { get; set; }
		public double GasConsumption { get; set; }
		public double EnergyGenerated { get; set; }
		public double EnergyGenAdjustment { get; set; }
		public double EnergyAdjustment { get; set; }
		public double GasAdjustment { get; set; }
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
					GasAdjustment = dat.GasAdjustment,
					EnergyAdjustment = dat.EnergyAdjustment,
					EnergyGenAdjustment = dat.EnergyGenAdjustment,
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
					GasAdjustment = dat.GasAdjustment,
					EnergyAdjustment = dat.EnergyAdjustment,
					EnergyGenAdjustment = dat.EnergyGenAdjustment,
					Date = dat.Date.ToString("dd/MM/yyyy").Replace('-', '/'),
					DepartmentID = dat.DepartmentID
				});
			}
			return orderedData;
		}
	}
}
