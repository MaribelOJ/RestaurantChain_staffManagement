using System;
using System.Collections.Generic;

namespace WebServiceRestaurante.Models;

public partial class Worker
{
    public string Cedula { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string Correo { get; set; } = null!;

    public string Cargo { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public virtual ICollection<RecruitmentDetail> RecruitmentDetails { get; set; } = new List<RecruitmentDetail>();

    public virtual User? User { get; set; }
}
