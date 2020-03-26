using System;
namespace youth.Model
{
	public class TotalMonth

	{

		public TotalMonth(string month, int total, string act)
		{

			this.Month = month;

			this.Total = total;
			this.Act = act; 



		}

        public string Act { get; set; }
		public string Month { get; set; }

		public int Total { get; set; }



	}
}
