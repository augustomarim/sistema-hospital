using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;

namespace HospitalSystem.Controllers;

public class ConsultaController
{
    private readonly ConsultaRepository _repository;

    public ConsultaController(ConsultaRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<Consulta>> CarregarTodasAsync()
        => await _repository.GetAllAsync();

    public async Task AgendarAsync(Consulta consulta)
    {
        ValidarConsulta(consulta);
        await _repository.AddAsync(consulta);
    }

    public async Task ExcluirAsync(int id)
        => await _repository.DeleteAsync(id);

    public async Task<IEnumerable<Consulta>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim)
        => await _repository.BuscarPorPeriodoAsync(inicio, fim);

    private static void ValidarConsulta(Consulta consulta)
    {
        if (consulta.MedicoId == 0)
            throw new ArgumentException("Selecione um médico.");
        if (consulta.PacienteId == 0)
            throw new ArgumentException("Selecione um paciente.");
    }
}
