using System;
using System.Collections.Generic;
using System.Text;

using HospitalSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace HospitalSystem.Data.Repositories;

public class ConsultaRepository : Repository<Consulta>
{
    public ConsultaRepository(HospitalDbContext context) : base(context) { }

    public async Task<IEnumerable<Consulta>> BuscarPorPeriodoAsync(DateTime inicio, DateTime fim)
    {
        var consultas = new List<Consulta>();

        var connection = _context.Database.GetDbConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Consultas WHERE DataHora BETWEEN @inicio AND @fim";
        command.Parameters.Add(new SqliteParameter("@inicio", inicio.ToString("yyyy-MM-dd")));
        command.Parameters.Add(new SqliteParameter("@fim", fim.ToString("yyyy-MM-dd")));

        using var result = await command.ExecuteReaderAsync();
        while (await result.ReadAsync())
        {
            consultas.Add(new Consulta
            {
                Id = result.GetInt32(0),
                DataHora = result.GetDateTime(1),
                Diagnostico = result.GetString(2),
                PacienteId = result.GetInt32(3),
                MedicoId = result.GetInt32(4)
            });
        }

        return consultas;
    }
}
