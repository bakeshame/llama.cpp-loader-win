using System.Windows;

namespace LlamaCppLoader
{
    public partial class SaveProfileDialog : Window
    {
        public string ProfileName { get; private set; } = string.Empty;

        public SaveProfileDialog()
        {
            InitializeComponent();
            ProfileNameTextBox.Focus();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProfileNameTextBox.Text))
            {
                MessageBox.Show("Please enter a profile name.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ProfileName = ProfileNameTextBox.Text.Trim();
            DialogResult = true;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
