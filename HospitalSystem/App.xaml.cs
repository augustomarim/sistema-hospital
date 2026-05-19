using System.Configuration;
using System.Data;
using System.Windows;

using HospitalSystem.Controllers;
using HospitalSystem.Data;
using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddScoped<PacienteController>();
        services.AddScoped<FuncionarioController>();
        services.AddScoped<ConsultaController>();
        services.AddScoped<DepartamentoController>();

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
