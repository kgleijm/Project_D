using System;

/// <summary>
/// Summary description for Class1
/// </summary>
namespace project_D.Models
{
	public class Data
	{
		public int DataID { get; set; }
		public double EnergyConsumption { get; set; }
		public double GasConsumption { get; set; }
		public double EnergyGenerated { get; set; }
		public double GasGenerated { get; set; }
		public string Date { get; set; }
		public int DepartmentID { get; set; }
	}
}
