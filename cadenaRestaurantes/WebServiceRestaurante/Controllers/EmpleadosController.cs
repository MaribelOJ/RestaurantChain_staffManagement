using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebServiceRestaurante.Models;
using UtilitiesRestaurante;
using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebServiceRestaurante.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class EmpleadosController : ControllerBase
	{
		//private readonly CadenaRestaurantesContext _dbContext;

		//public EmpleadosController(CadenaRestaurantesContext dbContext)
		//{
		//	_dbContext = dbContext;
		//}

		//[HttpGet]
		//public async Task<IActionResult> Get()
		//{
		//	var output = new RequestResponse<List<EmployeeRequest>>();
		//	var employeesList = new List<EmployeeRequest>();

		//	try
		//	{
		//		foreach(var item in await _dbContext.Empleados.ToListAsync())
		//		{
		//			employeesList.Add(new EmployeeRequest
		//			{
		//				Cedula = item.Cedula,
		//				Nombre = item.Nombre,
		//				Telefono = item.Telefono,
		//				Correo = item.Correo,
		//				Ciudad = item.Ciudad,
		//				Cargo = item.Cargo,
		//				Fin_contrato = item.FinContrato,
		//				Inicio_jornada = item.InicioJornada,
		//				Nit_restaurante = item.NitRestaurante

		//			});
		//		}
		//		output.Success = 1;
		//		output.QueryData = employeesList;

		//	}
		//	catch (Exception ex)
		//	{
		//		output.Error = ex.Message;
		//	}
		//	return Ok(output);
		//}

		private readonly ILogger<EmpleadosController> _logger;

		public EmpleadosController(ILogger<EmpleadosController> logger)
		{
			_logger = logger;
		}

		

		[HttpGet("{Rol}")]
		public IActionResult GetMany(string Rol, [FromQuery] string? input)
		{
			Console.WriteLine($"Afuera del if: {input}");
			RequestResponse<List<EmployeeRequest>> output = new RequestResponse<List<EmployeeRequest>>();

			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{

					List<EmployeeRequest> lst = null;
					

					if (Rol == "Administrador")
					{
						lst = (from d in db.Workers
                               join u in db.RecruitmentDetails on d.Cedula equals u.Cedula
                               where (d.Nombre.Contains(input) || EF.Functions.Like(d.Cedula, $"{input}%" ) || u.NitRestaurante == long.Parse(input)) && !new[] { "Superadmin", "Administrador" }.Contains(d.Cargo)
                               select new EmployeeRequest
							   {
								   Cedula = d.Cedula,
								   Nombre = d.Nombre,
								   Telefono = d.Telefono,
								   Correo = d.Correo,
								   Cargo = d.Cargo,
								   FinContrato = u.FinContrato,
								   InicioJornada = u.InicioJornada,
								   NitRestaurante = u.NitRestaurante,
								   Estado = d.Estado

								}).ToList();
					}
					else
					{

						long nitValue;
						bool isNitValid = long.TryParse(input, out nitValue);

						lst = (from d in db.Workers
                               join u in db.RecruitmentDetails on d.Cedula equals u.Cedula
                               where (d.Nombre.Contains(input) || EF.Functions.Like(d.Cedula, $"{input}%") || (isNitValid && u.NitRestaurante == nitValue)) && d.Cargo == "Administrador"
                               select new EmployeeRequest
							   { 

								   Cedula = d.Cedula,
                                   Nombre = d.Nombre,
                                   Telefono = d.Telefono,
                                   Correo = d.Correo,
                                   Cargo = d.Cargo,
								   Estado = d.Estado,
								   FinContrato = u.FinContrato,
                                   InicioJornada = u.InicioJornada,
                                   NitRestaurante = u.NitRestaurante,
								   

								}).ToList();
						_logger.LogInformation(lst[0].Nombre);


					}

					output.Success = 1;
					output.QueryData = lst;
				}

			}
			catch (Exception ex)
			{
				output.Error = ex.Message;
			}
			return Ok(output);
		}

		[HttpGet]
		public IActionResult GetOne([FromQuery] string Cc)
		{
			RequestResponse<EmployeeRequest> output = new RequestResponse<EmployeeRequest>();
			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{

					var employee = (from d in db.Workers
                                    join u in db.RecruitmentDetails on d.Cedula equals u.Cedula
                                    where d.Cedula == Cc
                                    orderby u.FechaCreacion descending
                                    select new EmployeeRequest
                                    {
                                        Cedula = d.Cedula,
                                        Nombre = d.Nombre,
                                        Telefono = d.Telefono,
                                        Correo = d.Correo,
                                        Cargo = d.Cargo,
                                        FinContrato = u.FinContrato,
                                        InicioJornada = u.InicioJornada,
                                        NitRestaurante = u.NitRestaurante,
                                        Estado = d.Estado

                                    }).FirstOrDefault();
                    output.Success = 1;
					output.QueryData = employee;
				}
			}
			catch (Exception ex)
			{
				output.Error = ex.Message;
			}
			return Ok(output);
		}

		[HttpPost("{Rol}")]
		public IActionResult Add(string Rol, EmployeeRequest model)
		{
			RequestResponse<object> output = new RequestResponse<object>();


			if (Rol == "Administrador")
			{
				if(model.Cargo == "Administrador" || model.Cargo == "Superadmin")
				{
					output.Success = 0;
					output.Error = "Insertion not allowed";
					return StatusCode(StatusCodes.Status403Forbidden, output);
				}
			}


			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{
					Worker employee = new Worker();
					employee.Cedula = model.Cedula;
					employee.Nombre = model.Nombre;
					employee.Telefono = model.Telefono;
					employee.Correo = model.Correo;
					employee.Cargo = model.Cargo;
                    employee.Estado = "Activo";
			
					
					db.Workers.Add(employee);
					db.SaveChanges();

                    RecruitmentDetail detail = new RecruitmentDetail();
					detail.Cedula = model.Cedula;
                    detail.FinContrato = model.FinContrato;
                    detail.InicioJornada = model.InicioJornada;
                    detail.NitRestaurante = model.NitRestaurante;
					detail.FechaCreacion = DateTime.Now;

                    db.RecruitmentDetails.Add(detail);
                    db.SaveChanges();

                    

					if (Rol == "Superadmin")
					{
						
						var password = Encrypt.GetSHA256("ABC123");

                        db.Database.ExecuteSqlRaw("EXEC addUser @cedula, @clave",
							new SqlParameter("@cedula", model.Cedula),
							new SqlParameter("@clave", password));
						
					}

                    output.Success = 1;

                }

			}catch(Exception ex)
			{
				output.Error = ex.Message;
			}
			return Ok(output);
		}

		[HttpPut]

		public IActionResult Edit(EmployeeRequest model)
		{

			RequestResponse<object> output = new RequestResponse<object>();
			string nit = model.NitRestaurante.ToString();
			string fecha = model.FinContrato.ToString();	
	

			try
			{
				using(CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{

					Worker employee = db.Workers.Find(model.Cedula);
					employee.Nombre= model.Nombre;
					employee.Telefono = model.Telefono;
					employee.Correo = model.Correo;
					employee.Cargo = model.Cargo;

                    db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();

					RecruitmentDetail record = (from d in db.RecruitmentDetails
												where d.Cedula == model.Cedula
												select d).FirstOrDefault();
					_logger.LogInformation((record.InicioJornada).ToString());
					_logger.LogInformation(model.InicioJornada.ToString());

					if(record != null && (record.FinContrato != model.FinContrato || record.InicioJornada != model.InicioJornada || record.NitRestaurante != model.NitRestaurante))
					{
                        RecruitmentDetail detail = db.RecruitmentDetails.Where(d=>d.Cedula == model.Cedula).FirstOrDefault();
                        detail.Cedula = model.Cedula;
                        detail.FinContrato = model.FinContrato;
                        detail.InicioJornada = model.InicioJornada;
                        detail.NitRestaurante = model.NitRestaurante;
                        detail.FechaCreacion = DateTime.Now;

                        db.Entry(detail).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        db.SaveChanges();
                    }
                    

                    output.Success = 1;
					
				}
			}
			catch(Exception ex)
			{
				output.Error = ex.Message;
			}
			return Ok(output);
		}

		[HttpPut("{Cc}")]

		public IActionResult EditStatus(string Cc, [FromQuery] string Status)
		{
			
			RequestResponse<object> output = new RequestResponse<object>();

			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{

					Worker employee = db.Workers.Find(Cc);

					employee.Estado = Status;

					db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					db.SaveChanges();
					output.Success = 1;

				}
			}
			catch (Exception ex)
			{
				output.Error = ex.Message;
			}
			return Ok(output);
		}


		[HttpDelete("{Cc}")]

		public IActionResult Delete(string Cc,[FromQuery] string Rol)
		{
			RequestResponse<object> output = new RequestResponse<object>();

			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{
					if (Rol == "Superadmin")
					{
						_logger.LogInformation("dentro de delete superadmin");
						var user = (from d in db.Users
									where d.Cedula == Cc
									select d).FirstOrDefault();

						if (user != null)
						{
							db.Remove(user);
							db.SaveChanges();
						}
					}

					var detail = db.RecruitmentDetails.Where(d=>d.Cedula == Cc).ToList();
                    db.RemoveRange(detail);
                    db.SaveChanges();

                    var employee = db.Workers.Find(Cc);
					db.Remove(employee);
					db.SaveChanges();	
					output.Success = 1;
				}
			}catch(Exception ex)
			{
				output.Error=ex.Message;
			}
			return Ok(output);
		}



	}
}
