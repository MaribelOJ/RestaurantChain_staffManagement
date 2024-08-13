using System.Net.Http.Json;
using UtilitiesRestaurante;
namespace cadenaRestaurantes.Services
{
	public class EmployeeService : IEmployees
	{
		private readonly HttpClient _http;

		public EmployeeService(HttpClient http)
		{
			_http = http;
		}

		public async Task<List<EmployeeRequest>> Lista()
		{
			RequestResponse<List<EmployeeRequest>> output = await _http.GetFromJsonAsync<RequestResponse<List<EmployeeRequest>>>("api/Empleados");

			if (output!.Success==1)
				return output.QueryData!;
			else
				throw new Exception(output.Error);
		}
		public Task<EmployeeRequest> Buscar(int Cc)
		{
			throw new NotImplementedException();
		}

		public Task<int> Editar(EmployeeRequest empleado)
		{
			throw new NotImplementedException();
		}

		public Task<bool> Eliminar(int Cc)
		{
			throw new NotImplementedException();
		}

		public Task<int> Guardar(EmployeeRequest empleado)
		{
			throw new NotImplementedException();
		}

	}
}
