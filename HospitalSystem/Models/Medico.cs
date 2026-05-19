using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models.Interfaces;

namespace HospitalSystem.Models;

public class Medico : Funcionario, IEspecialidade
{
    public string EspecialidadePrincipal { get; set; } = string.Empty;
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    public void Atender() => Console.WriteLine($"Dr. {Nome} atendendo.");
}