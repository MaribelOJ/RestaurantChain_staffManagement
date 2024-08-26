using UtilitiesRestaurante;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace cadenaRestaurantes.AccessControllers
{
	public class AuthenticationFilter : AuthenticationStateProvider
	{
		private readonly ISessionStorageService _sessionStorage;
	
		private ClaimsPrincipal _sinInformacion = new ClaimsPrincipal(new ClaimsIdentity());
		public AuthenticationFilter(ISessionStorageService sessionStorage)
		{
			_sessionStorage = sessionStorage;
		}

		public async Task UpdateAuthenticationState(UserRequest? loggedUser)
		{
			ClaimsPrincipal claimsPrincipal;

			if (loggedUser != null)
			{

				var claims = new List<Claim>
				{
					new Claim(ClaimTypes.NameIdentifier, loggedUser.Cedula),
					new Claim(ClaimTypes.Name, loggedUser.Nombre),
					new Claim(ClaimTypes.HomePhone, loggedUser.Telefono),
					new Claim(ClaimTypes.Email, loggedUser.Correo),
					new Claim(ClaimTypes.Role, loggedUser.Cargo)
				};

				if (!string.IsNullOrEmpty(loggedUser.Nit))
				{
					claims.Add(new Claim(ClaimTypes.SerialNumber, loggedUser.Nit));
				}

				claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));

				await _sessionStorage.StoreSession("loggedUser", loggedUser);
			}
			else
			{
				claimsPrincipal = _sinInformacion;
				await _sessionStorage.RemoveItemAsync("loggedUser");
			}

			NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));

		}

		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
			var loggedUser = await _sessionStorage.GetSession<UserRequest>("loggedUser");

			if (loggedUser == null)
				return await Task.FromResult(new AuthenticationState(_sinInformacion));		

			var claims = new List<Claim>
{
				new Claim(ClaimTypes.NameIdentifier,loggedUser.Cedula),
				new Claim(ClaimTypes.Name, loggedUser.Nombre),
				new Claim(ClaimTypes.HomePhone, loggedUser.Telefono),
				new Claim(ClaimTypes.Email, loggedUser.Correo),
				new Claim(ClaimTypes.Role, loggedUser.Cargo)
			};

			if (!string.IsNullOrEmpty(loggedUser.Nit))
			{
				claims.Add(new Claim(ClaimTypes.SerialNumber, loggedUser.Nit));
			}

			var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "JwtAuth"));


			return await Task.FromResult(new AuthenticationState(claimPrincipal));
		}
	}
}
