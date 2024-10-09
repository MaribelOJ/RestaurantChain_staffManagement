using System;
using System.Collections.Generic;

namespace WebServiceRestaurante.Models;

public partial class EmploymentLog
{
    public int IdRegistro { get; set; }

    public string EmpleadoModificado { get; set; } = null!;

    public DateTime FinContrato { get; set; }

    public TimeSpan InicioJornada { get; set; }

    public long NitRestaurante { get; set; }

    public string Estado { get; set; } = null!;

    public DateTime FechaCreacion { get; set; }
}
