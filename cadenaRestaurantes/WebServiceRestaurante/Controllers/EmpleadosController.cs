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
		private readonly CadenaRestaurantesContext _db;
		private readonly ILogger<EmpleadosController> _logger;

		public EmpleadosController(ILogger<EmpleadosController> logger, CadenaRestaurantesContext dbContext)
		{
			_logger = logger;
			_db = dbContext;
		}	

		[HttpGet("{Rol}")]
		public IActionResult GetMany(string Rol, [FromQuery] string? input, [FromQuery] string? Nit)
		{

			RequestResponse<List<EmployeeRequest>> output = new RequestResponse<List<EmployeeRequest>>();


			try
			{
				
					List<EmployeeRequest>? lst = null;

					if (Nit == null)
					{
						Nit = input;
						input = string.Empty;
					}

					if (input == null)
					{
						input = string.Empty;
					}

					if (Rol == "Administrador")
					{
		
						long nitValue;
						bool isNitValid = long.TryParse(Nit, out nitValue);



						lst = (from d in _db.Workers
                               join u in _db.EmploymentDetails on d.Cedula equals u.Cedula
                               where (d.Nombre.Contains(input) || EF.Functions.Like(d.Cedula, $"{input}%" )) && (isNitValid && u.NitRestaurante == nitValue) && !new[] { "Superadmin", "Administrador" }.Contains(d.Cargo)
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

						lst = (from d in _db.Workers
                               join u in _db.EmploymentDetails on d.Cedula equals u.Cedula
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
				
				var employee = (from d in _db.Workers
                                join u in _db.EmploymentDetails on d.Cedula equals u.Cedula
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

				if (employee != null)
				{
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
				Worker employee = new Worker();
				employee.Cedula = model.Cedula;
				employee.Nombre = model.Nombre;
				employee.Telefono = model.Telefono;
				employee.Correo = model.Correo;
				employee.Cargo = model.Cargo;
                employee.Estado = "Activo";
			
					
				_db.Workers.Add(employee);
				_db.SaveChanges();

                EmploymentDetail detail = new EmploymentDetail();
				detail.Cedula = model.Cedula;
                detail.FinContrato = model.FinContrato;
                detail.InicioJornada = model.InicioJornada;
                detail.NitRestaurante = model.NitRestaurante;
				detail.FechaCreacion = DateTime.Now;

                _db.EmploymentDetails.Add(detail);
                _db.SaveChanges();

                    

				if (Rol == "Superadmin")
				{
						
					var password = Encrypt.GetSHA256("ABC123");

					_db.Database.ExecuteSqlRaw("EXEC addUser @cedula, @clave",
						new SqlParameter("@cedula", model.Cedula),
						new SqlParameter("@clave", password));
						
				}

                output.Success = 1;

                

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

				Worker? employee = _db.Workers.Find(model.Cedula);

				if (employee != null)
				{
					employee.Nombre = model.Nombre;
					employee.Telefono = model.Telefono;
					employee.Correo = model.Correo;
					employee.Cargo = model.Cargo;

					_db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					_db.SaveChanges();

					EmploymentDetail? record = _db.EmploymentDetails.Where(d => d.Cedula == model.Cedula).FirstOrDefault();



					if (record != null && (record.FinContrato != model.FinContrato || record.InicioJornada != model.InicioJornada || record.NitRestaurante != model.NitRestaurante))
					{
						_db.Database.ExecuteSqlRaw("EXEC addEmploymentLog @empleado_modificado, @fin_contrato, @inicio_jornada, @nit_restaurante, @estado, @fecha_creacion",
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

						_db.Entry(record).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
						_db.SaveChanges();
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
				Worker? employee = _db.Workers.Find(Cc);	

				if(employee != null)
				{
					EmploymentDetail? detail = _db.EmploymentDetails.Where(d => d.Cedula == Cc).FirstOrDefault();

					_db.Database.ExecuteSqlRaw("EXEC addEmploymentLog @empleado_modificado, @fin_contrato, @inicio_jornada, @nit_restaurante, @estado, @fecha_creacion",
						new SqlParameter("@empleado_modificado", Cc),
						new SqlParameter("@fin_contrato", detail?.FinContrato),
						new SqlParameter("@inicio_jornada", detail?.InicioJornada),
						new SqlParameter("@nit_restaurante", detail?.NitRestaurante),
						new SqlParameter("@estado", employee.Estado),
						new SqlParameter("@fecha_creacion", DateTime.Now));

					employee.Estado = Status;

					_db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					_db.SaveChanges();
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
				if (Rol == "Superadmin")
				{
					var user = (from d in _db.Users
								where d.Cedula == Cc
								select d).FirstOrDefault();

					if (user != null)
					{
						_db.Remove(user);
						_db.SaveChanges();
					}
				}

				var detail = _db.EmploymentDetails.Where(d=>d.Cedula == Cc).ToList();
                _db.RemoveRange(detail);
                _db.SaveChanges();

                var employee = _db.Workers.Find(Cc);
				if (employee != null)
				{
					_db.Remove(employee);
					_db.SaveChanges();
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
