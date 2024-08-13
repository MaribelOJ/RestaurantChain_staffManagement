using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilitiesRestaurante
{
	public class UserRequest
	{
		public string Id { get; set; }
		public string Correo { get; set; }

		public string Nombre { get; set; }
		public string Cargo { get; set; }

		public string? Nit { get; set; }
	}
}
