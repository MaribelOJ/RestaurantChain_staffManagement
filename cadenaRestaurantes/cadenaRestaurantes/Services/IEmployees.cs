using UtilitiesRestaurante;

namespace cadenaRestaurantes.Services
{
	public interface IEmployees
	{
		Task<List<EmployeeRequest>> Lista();
		Task<EmployeeRequest> Buscar(int Cc);
		Task<int> Guardar(EmployeeRequest empleado);
		Task<int> Editar(EmployeeRequest empleado);
		Task<bool> Eliminar(int Cc);
	}
}
