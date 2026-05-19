using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using HospitalSystem.Controllers;
using HospitalSystem.Models;

namespace HospitalSystem.Views;

public partial class ConsultasView : Window
{
    private readonly ConsultaController _consultaController;
    private readonly PacienteController _pacienteController;
    private readonly FuncionarioController _funcionarioController;

    public ConsultasView(
        ConsultaController consultaController,
        PacienteController pacienteController,
        FuncionarioController funcionarioController)
    {
        InitializeComponent();
        _consultaController = consultaController;
        _pacienteController = pacienteController;
        _funcionarioController = funcionarioController;
        _ = CarregarDadosAsync();
    }

    private async Task CarregarDadosAsync()
    {
        cmbPaciente.ItemsSource = (await _pacienteController.CarregarTodosAsync()).ToList();
        cmbMedico.ItemsSource = (await _funcionarioController.CarregarMedicosAsync()).ToList();
        await CarregarGridAsync();
    }

    private async Task CarregarGridAsync()
    {
        var consultas = await _consultaController.CarregarTodasAsync();
        dgConsultas.ItemsSource = consultas.ToList();
    }

    private async void btnAgendar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var consulta = new Consulta
            {
                PacienteId = (cmbPaciente.SelectedItem as Paciente)?.Id ?? 0,
                MedicoId = (cmbMedico.SelectedItem as Medico)?.Id ?? 0,
                DataHora = dpData.SelectedDate ?? DateTime.Now,
                Diagnostico = txtDiagnostico.Text.Trim()
            };

            await _consultaController.AgendarAsync(consulta);
            await CarregarGridAsync();
            MessageBox.Show("Consulta agendada com sucesso!", "Sucesso",
                MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (ArgumentException ex)
        {
            MessageBox.Show(ex.Message, "Validação",
                MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private async void btnExcluir_Click(object sender, RoutedEventArgs e)
    {
        if (dgConsultas.SelectedItem is not Consulta selecionada) return;

        var confirm = MessageBox.Show("Confirmar exclusão?", "Excluir",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (confirm == MessageBoxResult.Yes)
        {
            await _consultaController.ExcluirAsync(selecionada.Id);
            await CarregarGridAsync();
        }
    }

    private async void btnBuscarPeriodo_Click(object sender, RoutedEventArgs e)
    {
        var inicio = DateTime.Now.AddMonths(-1);
        var fim = DateTime.Now;
        var consultas = await _consultaController.BuscarPorPeriodoAsync(inicio, fim);
        dgConsultas.ItemsSource = consultas.ToList();
    }
}
