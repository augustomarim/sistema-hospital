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
            var crmCoren = txtCrmCoren.Text.Trim();
            var especialidadeTurno = (cmbEspecialidadeTurno.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty;

            if (tipo == "Médico")
            {
                var medico = new Medico
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    Telefone = txtTelefone.Text.Trim(),
                    CRM_COREN = crmCoren,
                    EspecialidadePrincipal = especialidadeTurno
                };
                await _controller.SalvarMedicoAsync(medico);
            }
            else if (tipo == "Enfermeiro")
            {
                var enfermeiro = new Enfermeiro
                {
                    Nome = txtNome.Text.Trim(),
                    CPF = txtCPF.Text.Trim(),
                    Telefone = txtTelefone.Text.Trim(),
                    CRM_COREN = crmCoren,
                    Turno = especialidadeTurno
                };
                await _controller.SalvarEnfermeiroAsync(enfermeiro);
            }
            else
            {
                MessageBox.Show("Selecione o tipo de funcionário.", "Validação",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
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
        var confirm = MessageBox.Show("Confirmar exclusão?", "Excluir",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (confirm != MessageBoxResult.Yes) return;

        if (dgFuncionarios.SelectedItem is Medico medico)
            await _controller.ExcluirMedicoAsync(medico.Id);
        else if (dgFuncionarios.SelectedItem is Enfermeiro enfermeiro)
            await _controller.ExcluirEnfermeiroAsync(enfermeiro.Id);

        await CarregarGridAsync();
    }

    private void btnLimpar_Click(object sender, RoutedEventArgs e)
        => LimparFormulario();

    private void cmbTipo_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var tipo = (cmbTipo.SelectedItem as ComboBoxItem)?.Content.ToString();
        cmbEspecialidadeTurno.Items.Clear();

        if (tipo == "Médico")
        {
            lblCrmCoren.Text = "CRM (ex: CRM/SP-123456)";
            lblEspecialidadeTurno.Text = "Especialidade";
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Pediatria" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Cardiologia" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Neurologia" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Dermatologia" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Odontologia" });
        }
        else if (tipo == "Enfermeiro")
        {
            lblCrmCoren.Text = "COREN (ex: 123456-ES)";
            lblEspecialidadeTurno.Text = "Turno";
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Manhã" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Tarde" });
            cmbEspecialidadeTurno.Items.Add(new ComboBoxItem { Content = "Noite" });
        }
    }

    private void LimparFormulario()
    {
        txtNome.Clear();
        txtCPF.Clear();
        txtTelefone.Clear();
        txtCrmCoren.Clear();
        cmbTipo.SelectedIndex = -1;
        cmbEspecialidadeTurno.Items.Clear();
        lblCrmCoren.Text = "CRM/COREN";
        lblEspecialidadeTurno.Text = "Especialidade/Turno";
    }

    private void txtCPF_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = !char.IsDigit(e.Text[0]);

    private void txtCPF_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());

        var formatted = digits.Length switch
        {
            <= 3 => digits,
            <= 6 => $"{digits[..3]}.{digits[3..]}",
            <= 9 => $"{digits[..3]}.{digits[3..6]}.{digits[6..]}",
            _ => $"{digits[..3]}.{digits[3..6]}.{digits[6..9]}-{digits[9..Math.Min(11, digits.Length)]}"
        };

        tb.TextChanged -= txtCPF_TextChanged;
        tb.Text = formatted;
        tb.CaretIndex = formatted.Length;
        tb.TextChanged += txtCPF_TextChanged;
    }

    private void txtTelefone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = !char.IsDigit(e.Text[0]);

    private void txtTelefone_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        var digits = new string(tb.Text.Where(char.IsDigit).ToArray());

        var formatted = digits.Length switch
        {
            0 => "",
            <= 2 => $"({digits}",
            <= 7 => $"({digits[..2]}) {digits[2..]}",
            <= 11 => $"({digits[..2]}) {digits[2..7]}-{digits[7..]}",
            _ => tb.Text
        };

        tb.TextChanged -= txtTelefone_TextChanged;
        tb.Text = formatted;
        tb.CaretIndex = formatted.Length;
        tb.TextChanged += txtTelefone_TextChanged;
    }

    private void txtCrmCoren_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = false;

    private void txtCrmCoren_TextChanged(object sender, TextChangedEventArgs e)
    {
        var tb = (TextBox)sender;
        var raw = tb.Text.ToUpper();
        tb.TextChanged -= txtCrmCoren_TextChanged;
        tb.Text = raw;
        tb.CaretIndex = raw.Length;
        tb.TextChanged += txtCrmCoren_TextChanged;
    }
}