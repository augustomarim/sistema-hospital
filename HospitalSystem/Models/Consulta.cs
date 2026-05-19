using System;
using System.Collections.Generic;
using System.Text;

namespace HospitalSystem.Models;

public class Consulta
{
    public int Id { get; set; }
    public DateTime DataHora { get; set; }
    public string Diagnostico { get; set; } = string.Empty;

    public int PacienteId { get; set; }
    public Paciente? Paciente { get; set; }

    public int MedicoId { get; set; }
    public Medico? Medico { get; set; }
}