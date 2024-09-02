using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace UtilitiesRestaurante
{
	public class EmployeeRequest
	{
		[Required(ErrorMessage = "Campo requerido")]
		[MinLength(10, ErrorMessage = "Cedula no valida")]
		public string Cedula { get; set; }

		[Required(ErrorMessage = "Campo requerido")]
		[StringLength(50)]
		public string Nombre { get; set; }

		[Required(ErrorMessage = "Campo requerido")]
		[MinLength(10, ErrorMessage = "Telefono no valido")]
		public string Telefono { get; set; }

		[Required(ErrorMessage = "Campo requerido")]
		[EmailAddress(ErrorMessage = "Correo no valido")]
		public string Correo { get; set; }

		[Required(ErrorMessage = "Campo requerido")]
		public string Cargo { get; set; }

		[DataType(DataType.Date, ErrorMessage = "Formato no Valido")]
		public DateTime FinContrato { get; set; }

		public TimeSpan InicioJornada { get; set; }

		[Range(100000000, 999999999, ErrorMessage = "Nit no Valido")]
		public long NitRestaurante { get; set; }

		public string Estado { get; set; }
	}
}
