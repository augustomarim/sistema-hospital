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

public class FuncionarioController
{
    private readonly IRepository<Medico> _medicoRepository;
    private readonly IRepository<Enfermeiro> _enfermeiroRepository;
    private readonly HospitalDbContext _context;

    public FuncionarioController(
        IRepository<Medico> medicoRepository,
        IRepository<Enfermeiro> enfermeiroRepository,
        HospitalDbContext context)
    {
        _medicoRepository = medicoRepository;
        _enfermeiroRepository = enfermeiroRepository;
        _context = context;
    }

    public async Task<IEnumerable<Medico>> CarregarMedicosAsync()
        => await _medicoRepository.GetAllAsync();

    public async Task<IEnumerable<Enfermeiro>> CarregarEnfermeirosAsync()
        => await _enfermeiroRepository.GetAllAsync();

    public async Task SalvarMedicoAsync(Medico medico)
    {
        ValidarFuncionario(medico);
        await ValidarCPFDuplicadoAsync(medico.CPF);
        await ValidarCRMDuplicadoAsync(medico.CRM_COREN);
        await _medicoRepository.AddAsync(medico);
    }

    public async Task SalvarEnfermeiroAsync(Enfermeiro enfermeiro)
    {
        ValidarFuncionario(enfermeiro);
        await ValidarCPFDuplicadoAsync(enfermeiro.CPF);
        await ValidarCORENDuplicadoAsync(enfermeiro.CRM_COREN);
        await _enfermeiroRepository.AddAsync(enfermeiro);
    }

    public async Task ExcluirMedicoAsync(int id)
        => await _medicoRepository.DeleteAsync(id);

    public async Task ExcluirEnfermeiroAsync(int id)
        => await _enfermeiroRepository.DeleteAsync(id);

    private static void ValidarFuncionario(Funcionario funcionario)
    {
        if (string.IsNullOrWhiteSpace(funcionario.Nome) || funcionario.Nome.Trim().Length < 3)
            throw new ArgumentException("Nome deve ter no mínimo 3 letras.");

        if (string.IsNullOrWhiteSpace(funcionario.CPF))
            throw new ArgumentException("CPF é obrigatório.");

        if (!ValidarCPF(funcionario.CPF))
            throw new ArgumentException("CPF inválido.");

        if (string.IsNullOrWhiteSpace(funcionario.Telefone))
            throw new ArgumentException("Telefone é obrigatório.");

        if (string.IsNullOrWhiteSpace(funcionario.CRM_COREN))
            throw new ArgumentException("CRM/COREN é obrigatório.");
    }

    private async Task ValidarCPFDuplicadoAsync(string cpf)
    {
        var cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
        var existe = await _context.Pacientes.AnyAsync(p => p.CPF == cpfLimpo)
                  || await _context.Set<Funcionario>().AnyAsync(f => f.CPF == cpfLimpo);
        if (existe)
            throw new ArgumentException("CPF já cadastrado.");
    }

    private async Task ValidarCRMDuplicadoAsync(string crm)
    {
        var existe = await _context.Set<Funcionario>().AnyAsync(f => f.CRM_COREN == crm);
        if (existe)
            throw new ArgumentException("CRM já cadastrado.");
    }

    private async Task ValidarCORENDuplicadoAsync(string coren)
    {
        var existe = await _context.Set<Funcionario>().AnyAsync(f => f.CRM_COREN == coren);
        if (existe)
            throw new ArgumentException("COREN já cadastrado.");
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