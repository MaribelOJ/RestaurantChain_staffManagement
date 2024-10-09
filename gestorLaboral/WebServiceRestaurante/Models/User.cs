using System;
using System.Collections.Generic;

namespace WebServiceRestaurante.Models;

public partial class User
{
    public int Id { get; set; }

    public string Cedula { get; set; } = null!;

    public string Clave { get; set; } = null!;

    public virtual Worker CedulaNavigation { get; set; } = null!;
}
