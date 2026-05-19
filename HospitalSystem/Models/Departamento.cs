using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public class Departamento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Bloco { get; set; } = string.Empty;

    public ICollection<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
}