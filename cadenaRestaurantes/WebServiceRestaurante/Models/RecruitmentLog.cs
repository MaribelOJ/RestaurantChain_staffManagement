﻿using System;
using System.Collections.Generic;

namespace WebServiceRestaurante.Models;

public partial class RecruitmentLog
{
    public int IdRegistro { get; set; }

    public string Cedula { get; set; } = null!;

    public DateTime FinContrato { get; set; }

    public TimeSpan InicioJornada { get; set; }

    public long NitRestaurante { get; set; }

    public DateTime FechaCreacion { get; set; }
}
