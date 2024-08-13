using static System.Net.Mime.MediaTypeNames;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;


namespace WebServiceRestaurante
{
	public class Program
	{
		private readonly string MiCors = "MiCors";
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			Program p = new Program();

			// Add services to the container.
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(name: p.MiCors,
					builder =>
					{
						builder.WithOrigins("*");
					}
					);
			});

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			// Add time format
			builder.Services.AddSwaggerGen(opt =>
			{
				opt.MapType<TimeSpan>(() => new OpenApiSchema
				{
					Type = "string",
					Format = "time",
					Example = new OpenApiString(DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss"))
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1"));
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
			}

			app.UseCors(p.MiCors);

			app.UseAuthorization();

			app.MapControllers();

			app.Run();
		}
	}
}
