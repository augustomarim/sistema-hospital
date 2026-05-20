using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public abstract class Funcionario : Pessoa
{
    public string CRM_COREN { get; set; } = string.Empty;
    public int? DepartamentoId { get; set; }
    public Departamento? Departamento { get; set; }
}
