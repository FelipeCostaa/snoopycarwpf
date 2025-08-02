using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System;
using System.Collections.Generic;
using SnoopyCarWPF.Data;
using SnoopyCarWPF.Models;

namespace SnoopyCarWPF
{
    public partial class AgendamentoWindow : Window
    {
        public AgendamentoWindow()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            // Carrega clientes no ComboBox
            ClienteComboBox.ItemsSource = DataStore.Clientes;
            ClienteComboBox.DisplayMemberPath = "Nome";

            // Carrega horários no ComboBox
            HoraComboBox.ItemsSource = GerarHorarios();

            // Seta a data de hoje como padrão
            DataDatePicker.SelectedDate = DateTime.Today;

            // Carrega agendamentos no DataGrid
            AgendamentosDataGrid.ItemsSource = DataStore.Agendamentos;
        }

        private List<string> GerarHorarios()
        {
            var horarios = new List<string>();
            for (int h = 8; h < 18; h++) // Das 8h às 17h
            {
                horarios.Add($"{h:00}:00");
                horarios.Add($"{h:00}:30");
            }
            return horarios;
        }

        private void BtnAgendar_Click(object sender, RoutedEventArgs e)
        {
            if (ClienteComboBox.SelectedItem == null || DataDatePicker.SelectedDate == null || HoraComboBox.SelectedItem == null)
            {
                MessageBox.Show("Por favor, preencha todos os campos.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var cliente = ClienteComboBox.SelectedItem as Cliente;
            var data = DataDatePicker.SelectedDate.Value;
            var hora = TimeSpan.Parse(HoraComboBox.SelectedItem.ToString());

            var dataHoraAgendamento = data.Add(hora);

            var novoAgendamento = new Agendamento
            {
                ClienteAgendado = cliente,
                DataHora = dataHoraAgendamento,
                Status = "Agendado"
            };

            DataStore.Agendamentos.Add(novoAgendamento);
            MessageBox.Show("Agendamento realizado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnConcluir_Click(object sender, RoutedEventArgs e)
        {
            var agendamentoSelecionado = AgendamentosDataGrid.SelectedItem as Agendamento;
            if (agendamentoSelecionado != null)
            {
                agendamentoSelecionado.Status = "Concluído";
            }
            else
            {
                MessageBox.Show("Selecione um agendamento para marcar como concluído.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e)
        {
            var agendamentoSelecionado = AgendamentosDataGrid.SelectedItem as Agendamento;
            if (agendamentoSelecionado != null)
            {
                agendamentoSelecionado.Status = "Cancelado";
            }
            else
            {
                MessageBox.Show("Selecione um agendamento para cancelar.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
