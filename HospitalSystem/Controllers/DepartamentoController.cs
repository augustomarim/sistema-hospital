using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Controllers;

using HospitalSystem.Data;
using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

public class DepartamentoController
{
    private readonly IRepository<Departamento> _repository;
    private readonly HospitalDbContext _context;

    public DepartamentoController(IRepository<Departamento> repository, HospitalDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Departamento>> CarregarTodosAsync()
        => await _repository.GetAllAsync();

    public async Task SalvarAsync(Departamento departamento)
    {
        ValidarDepartamento(departamento);
        await ValidarNomeDuplicadoAsync(departamento.Nome);
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

    private async Task ValidarNomeDuplicadoAsync(string nome)
    {
        var existe = await _context.Departamentos.AnyAsync(d => d.Nome == nome);
        if (existe)
            throw new ArgumentException("Departamento já cadastrado.");
    }
}