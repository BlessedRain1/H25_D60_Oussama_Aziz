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
using System.Windows.Navigation;
using System.Windows.Shapes;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using System.IO;
using PartageDepense.Language;

namespace PartageDepense.View
{
    /// <summary>
    /// Code-behind pour la vue GraphiqueView.
    /// Contient la logique événementielle qui ne trouve pas sa place dans le ViewModel (ex: interactions directes avec l'UI).
    /// </summary>
    public partial class GraphiqueView : UserControl
    {
        public string? CultureCode
        {
            set
            {
                LanguageManager.SetCulture(value);
                UpdateUI();
            }
        }

        public static readonly DependencyProperty CultureCodeProperty =
            DependencyProperty.Register("CultureCode", typeof(string), typeof(GraphiqueView), new PropertyMetadata(null, new PropertyChangedCallback(OnCultureCodeChanged)));

        private static void OnCultureCodeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((GraphiqueView)d).CultureCode = (string)e.NewValue;
        }

        /// <summary>
        /// Constructeur de la vue GraphiqueView.
        /// </summary>
        public GraphiqueView()
        {
            LanguageManager.SetCulture(Thread.CurrentThread.CurrentUICulture.Name);
            InitializeComponent();
            UpdateUI();
        }

        public void UpdateUI()
        {
            SelectActivityLabel.Text = LanguageManager.GetString("Sélectionnez une activité :");
            ChartTypeLabel.Text = LanguageManager.GetString("Type de graphique :");
            BalanceRadio.Content = LanguageManager.GetString("Soldes par participant");
            ExpenseRadio.Content = LanguageManager.GetString("Dépenses par participant");
            PeriodLabel.Text = LanguageManager.GetString("Période :");
            ToLabel.Text = LanguageManager.GetString("Au :");
            SortByLabel.Text = LanguageManager.GetString("Trier par :");
            ValueSortItem.Content = LanguageManager.GetString("Valeur");
            NameSortItem.Content = LanguageManager.GetString("Nom");
            AscendingLabel.Text = LanguageManager.GetString("Croissant");
            ExportChartButton.Content = LanguageManager.GetString("Exporter graphique");
        }

        /// <summary>
        /// Gère l'événement de clic sur un segment du PieChart.
        /// Met en évidence le segment sélectionné en le déplaçant légèrement.
        /// </summary>
        /// <param name="sender">Source de l'événement.</param>
        /// <param name="chartPoint">Informations sur le point du graphique cliqué.</param>
        private void Chart_OnDataClick(object sender, ChartPoint chartPoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartPoint.ChartView;
            
            // Réinitialise le déplacement de tous les segments
            foreach (PieSeries series in chart.Series)
            {
                series.PushOut = 0;
            }

            // Déplace légèrement le segment sélectionné
            var selectedSeries = (PieSeries)chartPoint.SeriesView;
            selectedSeries.PushOut = 8;

            // Obtenir le DataContext (le ViewModel)
            if (DataContext is ViewModel.GraphiqueVM viewModel)
            {
                // Obtenir le nom du participant à partir du titre de la série cliquée
                string? participantName = selectedSeries.Title;

                if (!string.IsNullOrEmpty(participantName))
                {
                    // Appeler la méthode dans le ViewModel pour afficher les dépenses individuelles
                    viewModel.GetIndividualExpenses(participantName);
                }
            }
        }

        /// <summary>
        /// Gère l'événement de clic sur le bouton "Exporter graphique".
        /// Exporte le graphique actuellement visible (BarChart ou PieChart) en image PNG.
        /// </summary>
        /// <param name="sender">Source de l'événement.</param>
        /// <param name="e">Données de l'événement.</param>
        private void ExporterGraphique_Click(object sender, RoutedEventArgs e)
        {
            RenderTargetBitmap rtb;
            if (BarChart.Visibility == Visibility.Visible)
            {
                // Export du graphique à barres : capture le conteneur parent avec une grande taille temporaire.
                FrameworkElement chart = GraphContainer;
                double originalWidth = chart.Width;
                double originalHeight = chart.Height;
                chart.Width = 1200;
                chart.Height = 600;
                chart.UpdateLayout();
                rtb = new RenderTargetBitmap(1200, 600, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
                rtb.Render(chart);
                // Restaure la taille d'origine.
                chart.Width = originalWidth;
                chart.Height = originalHeight;
            }
            else
            {
                // Export du PieChart : capture uniquement le conteneur du PieChart (PieChartContainer).
                FrameworkElement chart = PieChartContainer;
                chart.UpdateLayout();
                int width = (int)chart.ActualWidth > 0 ? (int)chart.ActualWidth : 600;
                int height = (int)chart.ActualHeight > 0 ? (int)chart.ActualHeight : 400;
                rtb = new RenderTargetBitmap(width, height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
                rtb.Render(chart);
            }

            // Ouvre une boîte de dialogue pour choisir l'emplacement et le nom du fichier PNG.
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Image PNG|*.png", // Définit le filtre de fichier pour PNG
                FileName = "graphique.png" // Nom de fichier par défaut
            };

            // Si l'utilisateur confirme l'enregistrement
            if (dlg.ShowDialog() == true)
            {
                // Enregistre l'image dans le fichier choisi par l'utilisateur.
                using (var fs = new FileStream(dlg.FileName, FileMode.Create))
                {
                    var encoder = new PngBitmapEncoder();
                    encoder.Frames.Add(BitmapFrame.Create(rtb));
                    encoder.Save(fs);
                }
                // Affiche un message de confirmation à l'utilisateur.
                MessageBox.Show(
                    "Export a été fait avec succès !",
                    "Export",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information
                );
            }
        }
    }
}
