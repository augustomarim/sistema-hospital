using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HospitalSystem.Controllers;
using HospitalSystem.Views;

namespace HospitalSystem;

public partial class MainWindow : Window
{
    private readonly PacienteController _pacienteController;
    private readonly FuncionarioController _funcionarioController;
    private readonly ConsultaController _consultaController;
    private readonly DepartamentoController _departamentoController;

    public MainWindow(
        PacienteController pacienteController,
        FuncionarioController funcionarioController,
        ConsultaController consultaController,
        DepartamentoController departamentoController)
    {
        InitializeComponent();
        _pacienteController = pacienteController;
        _funcionarioController = funcionarioController;
        _consultaController = consultaController;
        _departamentoController = departamentoController;
    }

    private void btnPacientes_Click(object sender, RoutedEventArgs e)
        => new PacientesView(_pacienteController).ShowDialog();

    private void btnFuncionarios_Click(object sender, RoutedEventArgs e)
        => new FuncionariosView(_funcionarioController).ShowDialog();

    private void btnConsultas_Click(object sender, RoutedEventArgs e)
        => new ConsultasView(_consultaController, _pacienteController, _funcionarioController).ShowDialog();

    private void btnDepartamentos_Click(object sender, RoutedEventArgs e)
        => new DepartamentosView(_departamentoController).ShowDialog();
}