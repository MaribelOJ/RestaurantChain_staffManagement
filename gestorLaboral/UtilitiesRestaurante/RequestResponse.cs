using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesRestaurante
{
	public class RequestResponse<dataType>
	{
		public int Success { get; set; }
		public string Error { get; set; }
		public dataType QueryData { get; set; }

		public RequestResponse()
		{

			this.Success = 0;
		}
	}
}
