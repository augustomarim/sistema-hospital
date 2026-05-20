using HospitalSystem.Data.Repositories;
using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Controllers;

public class PacienteController
{
    private readonly IRepository<Paciente> _repository;
    private readonly HospitalDbContext _context;

    public PacienteController(IRepository<Paciente> repository, HospitalDbContext context)
    {
        _repository = repository;
        _context = context;
    }

    public async Task<IEnumerable<Paciente>> CarregarTodosAsync()
        => await _repository.GetAllAsync();

    public async Task SalvarAsync(Paciente paciente)
    {
        ValidarPaciente(paciente);
        await ValidarCPFDuplicadoAsync(paciente.CPF);
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
        if (string.IsNullOrWhiteSpace(paciente.Nome) || paciente.Nome.Trim().Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 letras.");

        if (string.IsNullOrWhiteSpace(paciente.CPF))
            throw new ArgumentException("CPF é obrigatório.");

        if (!ValidarCPF(paciente.CPF))
            throw new ArgumentException("CPF inválido.");

        if (string.IsNullOrWhiteSpace(paciente.Telefone))
            throw new ArgumentException("Telefone é obrigatório.");

        if (string.IsNullOrWhiteSpace(paciente.TipoSanguineo))
            throw new ArgumentException("Tipo sanguíneo é obrigatório.");
    }

    private async Task ValidarCPFDuplicadoAsync(string cpf)
    {
        var cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
        var existe = await _context.Pacientes.AnyAsync(p => p.CPF == cpfLimpo);
        if (existe)
            throw new ArgumentException("CPF já cadastrado.");
    }

    private static bool ValidarCPF(string cpf)
    {
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();

        if (cpf.Length != 11) return false;
        if (cpf.Distinct().Count() == 1) return false;

        var soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(cpf[i].ToString()) * (10 - i);

        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;
        if (digito1 != int.Parse(cpf[9].ToString())) return false;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(cpf[i].ToString()) * (11 - i);

        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;
        return digito2 == int.Parse(cpf[10].ToString());
    }
}