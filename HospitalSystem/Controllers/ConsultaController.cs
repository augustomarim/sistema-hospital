using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Controllers;

using HospitalSystem.Data;
using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using Microsoft.EntityFrameworkCore;

public class ConsultaController
{
    private readonly ConsultaRepository _repository;
    private readonly HospitalDbContext _context;

    public ConsultaController(ConsultaRepository repository, HospitalDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Consulta>> CarregarTodasAsync()
        => await _repository.GetAllAsync();

    public async Task AgendarAsync(Consulta consulta)
    {
        ValidarConsulta(consulta);
        await ValidarConsultaDuplicadaAsync(consulta);
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

        if (consulta.DataHora < DateTime.Now)
            throw new ArgumentException("Data da consulta não pode ser no passado.");
    }

    private async Task ValidarConsultaDuplicadaAsync(Consulta consulta)
    {
        var existe = await _context.Consultas.AnyAsync(c =>
            c.MedicoId == consulta.MedicoId &&
            c.PacienteId == consulta.PacienteId &&
            c.DataHora.Date == consulta.DataHora.Date);

        if (existe)
            throw new ArgumentException("Já existe uma consulta para este paciente com este médico nesta data.");
    }
}