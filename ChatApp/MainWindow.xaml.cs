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
using System.Net.Sockets;
using System.Net;
using ChatApp.NewFolder;

namespace ChatApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Start the server in the background when the window is initialized
            Task.Run(async () =>
            {
                try
                {
                    ICommunication server = new Server();
                    await server.RunAsync();  // Run server asynchronously in the background
                }
                catch (Exception ex)
                {
                    // Handle errors that occur while running the server
                    MessageBox.Show($"Error starting the server: {ex.Message}", "Server Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            });
        }

        private async void StartClient_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize the client and run asynchronously
                ICommunication client = new Client();
                await client.RunAsync();  // Wait for the client to complete its task

                // Once the client finishes, update the UI
                StatusLabel.Content = "Connected to the server!";
                StartClientButton.IsEnabled = false;  // Disable button if connection was successful
            }
            catch (Exception ex)
            {
                // Handle errors related to the client connection
                StatusLabel.Content = "Connection failed!";
                MessageBox.Show($"Error: {ex.Message}", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

}