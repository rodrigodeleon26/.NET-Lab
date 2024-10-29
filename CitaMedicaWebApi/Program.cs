using BL.BLs;
using BL.IBLs;
using DAL;
using DAL.DALs;
using DAL.IDALs;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("https://localhost:5010", "https://localhost:5011", "https://localhost:5012", "http://localhost:4200")
                       .AllowAnyHeader()
                       .AllowAnyMethod();
            });
    });
    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    /**********************************************************/
    /** Add Dependencies                                     **/
    /**********************************************************/
    #region Inyeccion de dependencias

    // DALs
    builder.Services.AddTransient<IDAL_CitasMedicas, DAL_CitasMedicas_EF>();

    // BLs
    builder.Services.AddTransient<IBL_CitasMedicas, BL_CitasMedicas>();

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    //app.UseHttpsRedirection();

    app.UseCors("AllowSpecificOrigin");
    
    app.UseAuthorization();

    app.MapControllers();

    DBContext.UpdateDatabase();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
