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
using System;
using System.Linq;
using System.Windows.Threading;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using ToastNotifications.Messages;
using SnoopyCarWPF.Data;

namespace SnoopyCarWPF
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _notificationTimer;
        private Notifier _notifier;

        public MainWindow()
        {
            InitializeComponent();
            SetupNotifier();
            SetupNotificationTimer();
        }

        private void SetupNotifier()
        {
            _notifier = new Notifier(cfg =>
            {
                cfg.PositionProvider = new WindowPositionProvider(
                    parentWindow: Application.Current.MainWindow,
                    corner: Corner.BottomRight,
                    offsetX: 10,
                    offsetY: 10);

                cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(5));

                cfg.Dispatcher = Application.Current.Dispatcher;
            });
        }

        private void SetupNotificationTimer()
        {
            _notificationTimer = new DispatcherTimer();
            _notificationTimer.Interval = TimeSpan.FromMinutes(1); // Verifica a cada minuto
            _notificationTimer.Tick += NotificationTimer_Tick;
            _notificationTimer.Start();
        }

        private void NotificationTimer_Tick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            var upcomingAppointments = DataStore.Agendamentos.Where(a =>
                a.Status == "Agendado" &&
                a.DataHora.Date == now.Date &&
                a.DataHora.Hour == now.Hour &&
                a.DataHora.Minute == now.Minute);

            foreach (var agendamento in upcomingAppointments)
            {
                _notifier.ShowInformation($"Agendamento agora: {agendamento.ClienteAgendado.Nome} - Veículo: {agendamento.ClienteAgendado.Veiculo}");
            }
        }

        private void BtnGerenciarClientes_Click(object sender, RoutedEventArgs e)
        {
            ClientesWindow clientesWindow = new ClientesWindow();
            clientesWindow.ShowDialog();
        }

        private void BtnGerenciarProdutos_Click(object sender, RoutedEventArgs e)
        {
            ProdutosWindow produtosWindow = new ProdutosWindow();
            produtosWindow.ShowDialog();
        }

        private void BtnGerenciarAgendamentos_Click(object sender, RoutedEventArgs e)
        {
            AgendamentoWindow agendamentoWindow = new AgendamentoWindow();
            agendamentoWindow.ShowDialog();
        }

        protected override void OnClosed(EventArgs e)
        {
            _notifier.Dispose();
            base.OnClosed(e);
        }
    }
}