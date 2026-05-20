using HospitalSystem.Controllers;
using HospitalSystem.Data;
using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace HospitalSystem;

public partial class App : Application
{
    private readonly ServiceProvider _provider;

    public App()
    {
        var services = new ServiceCollection();

        services.AddScoped<HospitalDbContext>();
        services.AddScoped<IRepository<Paciente>, Repository<Paciente>>();
        services.AddScoped<IRepository<Medico>, Repository<Medico>>();
        services.AddScoped<IRepository<Enfermeiro>, Repository<Enfermeiro>>();
        services.AddScoped<IRepository<Departamento>, Repository<Departamento>>();
        services.AddScoped<ConsultaRepository>();

        services.AddScoped<PacienteController>(p =>
            new PacienteController(
                p.GetRequiredService<IRepository<Paciente>>(),
                p.GetRequiredService<HospitalDbContext>()));

        services.AddScoped<FuncionarioController>(p =>
            new FuncionarioController(
                p.GetRequiredService<IRepository<Medico>>(),
                p.GetRequiredService<IRepository<Enfermeiro>>(),
                p.GetRequiredService<HospitalDbContext>()));

        services.AddScoped<ConsultaController>(p =>
            new ConsultaController(
                p.GetRequiredService<ConsultaRepository>(),
                p.GetRequiredService<HospitalDbContext>()));

        services.AddScoped<DepartamentoController>(p =>
            new DepartamentoController(
                p.GetRequiredService<IRepository<Departamento>>(),
                p.GetRequiredService<HospitalDbContext>()));

        services.AddScoped<MainWindow>();

        _provider = services.BuildServiceProvider();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        var db = _provider.GetRequiredService<HospitalDbContext>();
        db.Database.EnsureCreated();

        var mainWindow = _provider.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }
}
