using System.Collections.Generic;
using System.Windows;

namespace ETicaret.UI.Views
{
    public partial class IndirimWindow : Window
    {
        public string SecilenKategori { get; private set; }
        public int GirilenOran { get; private set; }

        public IndirimWindow(List<string> kategoriler)
        {
            InitializeComponent();
            cmbKategoriler.ItemsSource = kategoriler;
            cmbKategoriler.SelectedIndex = 0;
        }

        private void Uygula_Click(object sender, RoutedEventArgs e)
        {
            if (cmbKategoriler.SelectedItem == null)
            {
                System.Windows.MessageBox.Show("Lütfen bir kategori seçin.");
                return;
            }

            if (!int.TryParse(txtOran.Text, out int oran) || oran <= 0 || oran > 100)
            {
                System.Windows.MessageBox.Show("Lütfen geçerli bir oran girin (1-100 arası).");
                return;
            }
            SecilenKategori = cmbKategoriler.SelectedItem.ToString();
            GirilenOran = oran;

            this.DialogResult = true;
            this.Close();
        }

        private void Iptal_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}