using Microsoft.AspNetCore.Mvc;
using UtilitiesRestaurante;
using WebServiceRestaurante.Models;
using Microsoft.Extensions.Logging;



namespace WebServiceRestaurante.Controllers
{

	[Route("api/[controller]")]
    [ApiController]
    public class AccessController : ControllerBase
    {
		private readonly ILogger<AccessController> _logger;

		public AccessController(ILogger<AccessController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
        public async Task<IActionResult> Inicio([FromBody] LoginCredentials login)
        {
            UserRequest account_holder = new UserRequest();

            try
            {
                using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
                {
                    var password = Encrypt.GetSHA256(login.Clave);
                    RecruitmentDetail EmployeeData = null;
                   

                    var user = (from d in db.Users
                                join u in db.Workers on d.Cedula equals u.Cedula
                                where d.Clave == password && u.Correo == login.Correo 
                                select new { Empleado=u, Usuario=d}).FirstOrDefault();
                    
                    if (user == null || user.Empleado.Estado == "Inactivo")
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized);
                    }else if(user.Empleado.Cargo != "Superadmin")
                    {
                        EmployeeData = (from d in db.RecruitmentDetails 
                                        where d.Cedula == user.Empleado.Cedula 
                                        orderby d.FechaCreacion descending
                                        select d).FirstOrDefault();

                    }
                                  

                    account_holder.Id = user.Usuario.Id.ToString();
                    account_holder.Correo = user.Empleado.Correo;
					account_holder.Nombre = user.Empleado.Nombre;
					account_holder.Cargo = user.Empleado.Cargo;
                    account_holder.Nit = EmployeeData?.NitRestaurante.ToString();


					return StatusCode(StatusCodes.Status200OK, account_holder);


                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error al procesar la solicitud.");

            }



        }
    }
}
