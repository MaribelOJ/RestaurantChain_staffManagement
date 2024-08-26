using Microsoft.AspNetCore.Mvc;
using UtilitiesRestaurante;
using WebServiceRestaurante.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;



namespace WebServiceRestaurante.Controllers
{

	[Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
		private readonly ILogger<AccountController> _logger;

		public AccountController(ILogger<AccountController> logger)
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
                    EmploymentDetail EmployeeData = null;
                   

                    var user = (from d in db.Users
                                join u in db.Workers on d.Cedula equals u.Cedula
                                where d.Clave == password && u.Correo == login.Correo 
                                select new { Empleado=u, Usuario=d}).FirstOrDefault();
                    
                    if (user == null || user.Empleado.Estado == "Inactivo")
                    {
                        return StatusCode(StatusCodes.Status401Unauthorized);
                    }else if(user.Empleado.Cargo != "Superadmin")
                    {
                        EmployeeData = (from d in db.EmploymentDetails 
                                        where d.Cedula == user.Empleado.Cedula 
                                        orderby d.FechaCreacion descending
                                        select d).FirstOrDefault();
                    }
                                  

                    account_holder.Cedula = user.Usuario.Cedula;
                    account_holder.Correo = user.Empleado.Correo;
                    account_holder.Telefono = user.Empleado.Telefono;
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

        [HttpPut]

        public IActionResult EditProfileInfo(UserRequest model)
        {
            RequestResponse<object> output = new RequestResponse<object>();


            try
            {
                using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
                {

                    Worker employee = db.Workers.Find(model.Cedula);

                    if (employee != null && (employee.Nombre != model.Nombre || employee.Telefono != model.Telefono || employee.Correo != model.Correo))
                    {
                        employee.Nombre = model.Nombre;
                        employee.Telefono = model.Telefono;
                        employee.Correo = model.Correo;

                        db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();

                        output.Success = 1;
                    }

                }
            }
            catch (Exception ex)
            {
                output.Error = ex.Message;
            }
            return Ok(output);
        }

        [HttpPut("{Cc}")]
        public IActionResult EditPassword(PasswordRequest obj, string Cc)
        {
            RequestResponse<object> output = new RequestResponse<object>();


            try
            {
                using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
                {
                    var oldPassword = Encrypt.GetSHA256(obj.claveActual);
                    User account_holder = db.Users.Where(d => d.Cedula == Cc && d.Clave == oldPassword ).FirstOrDefault();

                   
                    if (account_holder != null)
                    {
                        var newPassword = Encrypt.GetSHA256(obj.claveNueva);
                        account_holder.Clave = newPassword;
                        db.Entry(account_holder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                        output.Success = 1;
                    }

                }
            }
            catch (Exception ex)
            {
                output.Error = ex.Message;
            }
            return Ok(output);
        }

    }
}
