using HospitalSystem.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

nnamespace HospitalSystem.Models;

public class Paciente : Pessoa
{
    public string TipoSanguineo { get; set; } = string.Empty;

    public FichaMedica? FichaMedica { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();

    public void AgendarConsulta()
        => Console.WriteLine($"Paciente {Nome} agendando consulta.");
}
