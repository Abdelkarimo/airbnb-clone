using BLL.Services.Abstractions;
using BLL.Services.Impelementation;
using PL.Hubs;

namespace PL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //Team Member: Abdallah Assem
            // Date: 10-11-2025
            // Descriprion: Add SignalR and Chat Service / Configure CORS for Angular
            // Add SignalR and Chat Service
            builder.Services.AddSignalR();

            

            
            
            
            builder.Services.AddScoped<IChatService, ChatService>();

            // Configure CORS for Angular
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AngularClient", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });
            var app = builder.Build();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Use CORS
            app.UseCors("AngularClient");

            app.UseAuthorization();


            app.MapControllers();

            // Map SignalR Hub
            app.MapHub<ChatHub>("/chatHub");
            
            ///////////End of Team Member: Abdallah Assem            
        
            app.Run();
        }
    }
}
