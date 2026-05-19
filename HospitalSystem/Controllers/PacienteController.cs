using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;

namespace HospitalSystem.Controllers;

public class PacienteController
{
    private readonly IRepository<Paciente> _repository;

    public PacienteController(IRepository<Paciente> repository)
        => _repository = repository;

    public async Task<IEnumerable<Paciente>> CarregarTodosAsync()
        => await _repository.GetAllAsync();

    public async Task SalvarAsync(Paciente paciente)
    {
        ValidarPaciente(paciente);
        paciente.FichaMedica = new FichaMedica { DataCriacao = DateTime.Now };
        await _repository.AddAsync(paciente);
    }

    public async Task AtualizarAsync(Paciente paciente)
    {
        ValidarPaciente(paciente);
        await _repository.UpdateAsync(paciente);
    }

    public async Task ExcluirAsync(int id)
        => await _repository.DeleteAsync(id);

    private static void ValidarPaciente(Paciente paciente)
    {
        if (string.IsNullOrWhiteSpace(paciente.Nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(paciente.CPF))
            throw new ArgumentException("CPF é obrigatório.");
    }
}
