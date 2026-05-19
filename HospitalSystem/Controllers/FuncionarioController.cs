using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models;
using HospitalSystem.Models.Interfaces;

namespace HospitalSystem.Controllers;

public class FuncionarioController
{
    private readonly IRepository<Medico> _medicoRepository;
    private readonly IRepository<Enfermeiro> _enfermeiroRepository;

    public FuncionarioController(
        IRepository<Medico> medicoRepository,
        IRepository<Enfermeiro> enfermeiroRepository)
    {
        _medicoRepository = medicoRepository;
        _enfermeiroRepository = enfermeiroRepository;
    }

    public async Task<IEnumerable<Medico>> CarregarMedicosAsync()
        => await _medicoRepository.GetAllAsync();

    public async Task<IEnumerable<Enfermeiro>> CarregarEnfermeirosAsync()
        => await _enfermeiroRepository.GetAllAsync();

    public async Task SalvarMedicoAsync(Medico medico)
    {
        ValidarFuncionario(medico);
        await _medicoRepository.AddAsync(medico);
    }

    public async Task SalvarEnfermeiroAsync(Enfermeiro enfermeiro)
    {
        ValidarFuncionario(enfermeiro);
        await _enfermeiroRepository.AddAsync(enfermeiro);
    }

    public async Task ExcluirMedicoAsync(int id)
        => await _medicoRepository.DeleteAsync(id);

    public async Task ExcluirEnfermeiroAsync(int id)
        => await _enfermeiroRepository.DeleteAsync(id);

    private static void ValidarFuncionario(Funcionario funcionario)
    {
        if (string.IsNullOrWhiteSpace(funcionario.Nome))
            throw new ArgumentException("Nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(funcionario.CRM_COREN))
            throw new ArgumentException("CRM/COREN é obrigatório.");
    }
}
