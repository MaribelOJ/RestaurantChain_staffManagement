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
		public IActionResult GetMany(string Rol, [FromQuery] string? input, [FromQuery] string? Nit)
		{

			RequestResponse<List<EmployeeRequest>> output = new RequestResponse<List<EmployeeRequest>>();


			try
			{
				using (CadenaRestaurantesContext db = new CadenaRestaurantesContext())
				{

					List<EmployeeRequest> lst = null;
					

					if (Rol == "Administrador")
					{
						if (Nit == null)
						{
							Nit = input;
							input = string.Empty;
						}


						lst = (from d in db.Workers
                               join u in db.EmploymentDetails on d.Cedula equals u.Cedula
                               where (d.Nombre.Contains(input) || EF.Functions.Like(d.Cedula, $"{input}%" )) && u.NitRestaurante == long.Parse(Nit) && !new[] { "Superadmin", "Administrador" }.Contains(d.Cargo)
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
                               join u in db.EmploymentDetails on d.Cedula equals u.Cedula
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
                                    join u in db.EmploymentDetails on d.Cedula equals u.Cedula
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

			_logger.LogInformation("en API");
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

                    EmploymentDetail detail = new EmploymentDetail();
					detail.Cedula = model.Cedula;
                    detail.FinContrato = model.FinContrato;
                    detail.InicioJornada = model.InicioJornada;
                    detail.NitRestaurante = model.NitRestaurante;
					detail.FechaCreacion = DateTime.Now;

                    db.EmploymentDetails.Add(detail);
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

		public IActionResult Edit( EmployeeRequest model)
		{
			RequestResponse<object> output = new RequestResponse<object>();
	

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

					EmploymentDetail record = db.EmploymentDetails.Where(d => d.Cedula == model.Cedula).FirstOrDefault();



					if (record != null && (record.FinContrato != model.FinContrato || record.InicioJornada != model.InicioJornada || record.NitRestaurante != model.NitRestaurante))
					{
						db.Database.ExecuteSqlRaw("EXEC addEmploymentLog @empleado_modificado, @fin_contrato, @inicio_jornada, @nit_restaurante, @estado, @fecha_creacion",
							new SqlParameter("@empleado_modificado", model.Cedula),
							new SqlParameter("@fin_contrato", record.FinContrato),
							new SqlParameter("@inicio_jornada", record.InicioJornada),
							new SqlParameter("@nit_restaurante", record.NitRestaurante),
							new SqlParameter("@estado", employee.Estado),
							new SqlParameter("@fecha_creacion", DateTime.Now));

                        record.Cedula = model.Cedula;
                        record.FinContrato = model.FinContrato;
                        record.InicioJornada = model.InicioJornada;
                        record.NitRestaurante = model.NitRestaurante;
                        record.FechaCreacion = DateTime.Now;

                        db.Entry(record).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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

					EmploymentDetail detail = db.EmploymentDetails.Where(d => d.Cedula == Cc).FirstOrDefault();

					if(employee != null)
					{
						db.Database.ExecuteSqlRaw("EXEC addEmploymentLog @empleado_modificado, @fin_contrato, @inicio_jornada, @nit_restaurante, @estado, @fecha_creacion",
							new SqlParameter("@empleado_modificado", Cc),
							new SqlParameter("@fin_contrato", detail.FinContrato),
							new SqlParameter("@inicio_jornada", detail.InicioJornada),
							new SqlParameter("@nit_restaurante", detail.NitRestaurante),
							new SqlParameter("@estado", employee.Estado),
							new SqlParameter("@fecha_creacion", DateTime.Now));

						employee.Estado = Status;

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

					var detail = db.EmploymentDetails.Where(d=>d.Cedula == Cc).ToList();
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
