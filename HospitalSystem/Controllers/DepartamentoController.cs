using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;

namespace HospitalSystem.Controllers;

public class DepartamentoController
{
    private readonly IRepository<Departamento> _repository;

    public DepartamentoController(IRepository<Departamento> repository)
        => _repository = repository;

    public async Task<IEnumerable<Departamento>> CarregarTodosAsync()
        => await _repository.GetAllAsync();

    public async Task SalvarAsync(Departamento departamento)
    {
        ValidarDepartamento(departamento);
        await _repository.AddAsync(departamento);
    }

    public async Task ExcluirAsync(int id)
        => await _repository.DeleteAsync(id);

    private static void ValidarDepartamento(Departamento departamento)
    {
        if (string.IsNullOrWhiteSpace(departamento.Nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(departamento.Bloco))
            throw new ArgumentException("Bloco é obrigatório.");
    }
}
