using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public class Enfermeiro : Funcionario
{
    public string Turno { get; set; } = string.Empty;

    public void AdministrarMedicamento()
        => Console.WriteLine($"Enfermeiro {Nome} administrando medicamento.");
}
