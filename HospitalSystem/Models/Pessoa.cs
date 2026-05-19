using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public abstract class Pessoa
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
}