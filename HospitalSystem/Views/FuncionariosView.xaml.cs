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

public partial class FuncionariosView : Window
{
    private readonly FuncionarioController _controller;

    public FuncionariosView(FuncionarioController controller)
    {
        InitializeComponent();
        _controller = controller;
        _ = CarregarGridAsync();
    }

    private async Task CarregarGridAsync()
    {
        var medicos = await _controller.CarregarMedicosAsync();
        var enfermeiros = await _controller.CarregarEnfermeirosAsync();
        var todos = medicos.Cast<Funcionario>().Concat(enfermeiros.Cast<Funcionario>());
        dgFuncionarios.ItemsSource = todos.ToList();
    }

    private async void btnSalvar_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            var tipo = (cmbTipo.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (tipo == "Médico")
            {
                var medico = new Medico
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    CRM_COREN = txtCRM.Text.Trim(),
                    EspecialidadePrincipal = txtEspecialidade.Text.Trim()
                };
                await _controller.SalvarMedicoAsync(medico);
            }
            else
            {
                var enfermeiro = new Enfermeiro
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    CRM_COREN = txtCRM.Text.Trim(),
                    Turno = (cmbTurno.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty
                };
                await _controller.SalvarEnfermeiroAsync(enfermeiro);
            }

            await CarregarGridAsync();
            LimparFormulario();
            MessageBox.Show("Funcionário salvo com sucesso!", "Sucesso",
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
        if (dgFuncionarios.SelectedItem is Medico medico)
        {
            var confirm = MessageBox.Show("Confirmar exclusão?", "Excluir",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                await _controller.ExcluirMedicoAsync(medico.Id);
                await CarregarGridAsync();
            }
        }
        else if (dgFuncionarios.SelectedItem is Enfermeiro enfermeiro)
        {
            var confirm = MessageBox.Show("Confirmar exclusão?", "Excluir",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (confirm == MessageBoxResult.Yes)
            {
                await _controller.ExcluirEnfermeiroAsync(enfermeiro.Id);
                await CarregarGridAsync();
            }
        }
    }

    private void btnLimpar_Click(object sender, RoutedEventArgs e)
        => LimparFormulario();

    private void cmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tipo = (cmbTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
        pnlEspecialidade.Visibility = tipo == "Médico" ? Visibility.Visible : Visibility.Collapsed;
        pnlTurno.Visibility = tipo == "Enfermeiro" ? Visibility.Visible : Visibility.Collapsed;
    }

    private void LimparFormulario()
    {
        txtNome.Clear();
        txtCPF.Clear();
        txtCRM.Clear();
        txtEspecialidade.Clear();
        cmbTipo.SelectedIndex = -1;
        cmbTurno.SelectedIndex = -1;
    }
}