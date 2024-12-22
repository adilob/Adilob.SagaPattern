
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.API.Database;
using Newsletter.API.Emails;
using Newsletter.API.Sagas;

namespace Newsletter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddDbContext<AppDbContext>(options =>
			{
				options.UseNpgsql(builder.Configuration.GetConnectionString("Database"));
			});

            builder.Services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.AddConsumers(typeof(Program).Assembly);

                busConfigurator.AddSagaStateMachine<NewsletterOnboardingSaga, NewsletterOnboardingSagaData>()
				.EntityFrameworkRepository(r =>
					{
						r.ExistingDbContext<AppDbContext>();
                        r.UsePostgres();
					});

				busConfigurator.UsingRabbitMq((context, configurator) =>
				{
					configurator.Host(builder.Configuration.GetConnectionString("RabbitMq"), host =>
					{
						host.Username("guest");
						host.Password("guest");
					});

                    configurator.UseInMemoryOutbox(context);
					configurator.ConfigureEndpoints(context);
				});
			});

			builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();

                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
				context.Database.Migrate();
			}

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
