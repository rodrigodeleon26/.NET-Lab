using DAL.IDALs;
using DAL;
using BL.IBLs;
using BL.BLs;
using DAL.DALs;

try
{
    var builder = WebApplication.CreateBuilder(args);

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
    builder.Services.AddTransient<IDAL_Personas, DAL_Personas_EF>();
    builder.Services.AddTransient<IDAL_Vehiculos, DAL_Vehiculos_EF>();

    // BLs
    builder.Services.AddTransient<IBL_Personas, BL_Personas>();
    builder.Services.AddTransient<IBL_Vehiculos, BL_Vehiculos>();

    #endregion

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    DBContext.UpdateDatabase();

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}