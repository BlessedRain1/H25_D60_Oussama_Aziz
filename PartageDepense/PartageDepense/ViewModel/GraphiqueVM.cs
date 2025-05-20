using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;
using PartageDepense.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PartageDepense.ViewModel
{
    /// <summary>
    /// ViewModel responsable de la logique d'affichage des graphiques (Soldes et Dépenses) d'une activité.
    /// </summary>
    public partial class GraphiqueVM : ObservableObject
    {
        private readonly Gestionnaire _gestionnaire;

        /// <summary>
        /// Liste observable des activités disponibles pour sélection.
        /// </summary>
        [ObservableProperty]
        private ObservableCollection<Activite>? listeActivites;

        /// <summary>
        /// L'activité actuellement sélectionnée par l'utilisateur.
        /// </summary>
        [ObservableProperty]
        private Activite? activiteSelectionnee;

        /// <summary>
        /// Collection de séries de données pour le graphique à barres (Soldes).
        /// </summary>
        [ObservableProperty]
        private SeriesCollection barSeries;

        /// <summary>
        /// Collection de séries de données pour le graphique camembert (Dépenses).
        /// </summary>
        [ObservableProperty]
        private SeriesCollection pieSeries;

        /// <summary>
        /// Axes X pour le graphique (utilisé pour les labels des participants).
        /// </summary>
        [ObservableProperty]
        private AxesCollection labelsAxis;

        /// <summary>
        /// Axes Y pour le graphique (utilisé pour les montants).
        /// </summary>
        [ObservableProperty]
        private AxesCollection yAxis;

        /// <summary>
        /// Indique si le graphique des soldes est sélectionné.
        /// </summary>
        [ObservableProperty]
        private bool isSoldeSelected = true;

        /// <summary>
        /// Indique si le graphique des dépenses est sélectionné.
        /// </summary>
        [ObservableProperty]
        private bool isDepenseSelected = false;

        /// <summary>
        /// Date de début pour le filtrage des données
        /// </summary>
        [ObservableProperty]
        private DateTime dateDebut = DateTime.Now.AddMonths(-1);

        /// <summary>
        /// Date de fin pour le filtrage des données
        /// </summary>
        [ObservableProperty]
        private DateTime dateFin = DateTime.Now;

        /// <summary>
        /// Indique si le tri est croissant
        /// </summary>
        [ObservableProperty]
        private bool triCroissant = true;

        /// <summary>
        /// Indique si le tri est par valeur (false = tri par nom)
        /// </summary>
        [ObservableProperty]
        private bool triParValeur = true;

        /// <summary>
        /// Message à afficher à l'utilisateur (ex: sélectionner une activité, pas de données).
        /// </summary>
        [ObservableProperty]
        private string? messageGraphique;

        /// <summary>
        /// Résumé dynamique des données affichées sur le graphique.
        /// </summary>
        private string? _resumeGraphique;
        public string? ResumeGraphique
        {
            get => _resumeGraphique;
            set => SetProperty(ref _resumeGraphique, value);
        }

        public GraphiqueVM(Gestionnaire gestionnaire)
        {
            _gestionnaire = gestionnaire;
            ListeActivites = _gestionnaire.LesActivites;

            BarSeries = new SeriesCollection();
            PieSeries = new SeriesCollection();
            LabelsAxis = new AxesCollection();
            YAxis = new AxesCollection();

            // Initialiser les axes par défaut
            LabelsAxis.Add(new Axis
            {
                Title = "Participants",
                Labels = new List<string>()
            });

            YAxis.Add(new Axis
            {
                Title = "Montant",
                // Utilise le format monétaire de la culture UI actuelle
                LabelFormatter = value => value.ToString("C", System.Globalization.CultureInfo.CurrentUICulture)
            });

            // S'abonner aux changements de la liste des activités dans le gestionnaire
            _gestionnaire.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == nameof(_gestionnaire.LesActivites))
                {
                    ListeActivites = _gestionnaire.LesActivites;
                }
            };
        }

        /// <summary>
        /// Appelé lorsque l'activité sélectionnée change. Charge le graphique correspondant.
        /// </summary>
        partial void OnActiviteSelectionneeChanged(Activite? value)
        {
            // Charge le graphique même si la sélection est nulle (pour vider l'affichage).
            ChargerGraphique();
        }

        /// <summary>
        /// Appelé lorsque la sélection du graphique Solde change.
        /// </summary>
        partial void OnIsSoldeSelectedChanged(bool value)
        {
            if (value)
            {
                IsDepenseSelected = false; // Assure qu'un seul graphique est sélectionné
                ChargerGraphique();
            }
        }

        /// <summary>
        /// Appelé lorsque la sélection du graphique Dépense change.
        /// </summary>
        partial void OnIsDepenseSelectedChanged(bool value)
        {
            if (value)
            {
                IsSoldeSelected = false; // Assure qu'un seul graphique est sélectionné
                ChargerGraphique();
            }
        }

        /// <summary>
        /// Appelé lorsque la date de début change
        /// </summary>
        partial void OnDateDebutChanged(DateTime value)
        {
            ChargerGraphique();
        }

        /// <summary>
        /// Appelé lorsque la date de fin change
        /// </summary>
        partial void OnDateFinChanged(DateTime value)
        {
            ChargerGraphique();
        }

        /// <summary>
        /// Appelé lorsque le type de tri change
        /// </summary>
        partial void OnTriCroissantChanged(bool value)
        {
            ChargerGraphique();
        }

        /// <summary>
        /// Appelé lorsque le critère de tri change
        /// </summary>
        partial void OnTriParValeurChanged(bool value)
        {
            ChargerGraphique();
        }

        /// <summary>
        /// Méthode principale pour charger le graphique sélectionné (Soldes ou Dépenses).
        /// Contient la gestion globale des erreurs.
        /// </summary>
        public void ChargerGraphique()
        {
            // Utilise un bloc try-catch pour intercepter les erreurs potentielles lors de la génération du graphique
            try
            {
                MessageGraphique = string.Empty;
                if (ActiviteSelectionnee == null)
                {
                    BarSeries.Clear();
                    PieSeries.Clear();
                    MessageGraphique = "Veuillez sélectionner une activité.";
                    ResumeGraphique = string.Empty;
                    return;
                }

                if (IsSoldeSelected)
                    ChargerGraphiqueSoldeParticipants();
                else if (IsDepenseSelected)
                    ChargerGraphiqueDepenses();
            }
            // Capture toutes les exceptions non gérées plus spécifiquement
            catch (Exception ex)
            {
                // Log l'erreur complète (stack trace, etc.) dans un fichier pour le débogage
                // Le try-catch interne empêche l'application de crasher si le logging échoue
                try
                {
                    System.IO.File.AppendAllText("log_erreurs.txt", DateTime.Now + " : " + ex.ToString() + "\n");
                }
                catch { /* Ignore logging errors */ }

                // Affiche un message générique et convivial à l'utilisateur, sans les détails techniques de l'exception
                System.Windows.MessageBox.Show(
                    "Une erreur interne est survenue lors de la génération du graphique. Les détails ont été enregistrés dans les logs.",
                    "Erreur graphique",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Charge les données et configure le graphique à barres pour afficher les soldes des participants.
        /// </summary>
        private void ChargerGraphiqueSoldeParticipants()
        {
            if (ActiviteSelectionnee == null)
                return;

            // Obtient les données de soldes depuis l'activité
            var resultats = ActiviteSelectionnee.ObtenirGraphiqueSoldes()
                .Where(r => r.Date >= DateDebut && r.Date <= DateFin)
                .ToList();

            // Applique le tri selon les critères sélectionnés
            if (TriParValeur)
            {
                resultats = TriCroissant 
                    ? resultats.OrderBy(r => r.Value).ToList()
                    : resultats.OrderByDescending(r => r.Value).ToList();
            }
            else
            {
                resultats = TriCroissant 
                    ? resultats.OrderBy(r => r.Label).ToList()
                    : resultats.OrderByDescending(r => r.Label).ToList();
            }

            if (resultats.Count == 0)
            {
                BarSeries.Clear();
                MessageGraphique = "Aucun solde à afficher pour cette période.";
                ResumeGraphique = string.Empty;
                return;
            }
            
            BarSeries.Clear();

            // Crée une série de barres pour les soldes positifs (en vert)
            var positiveSeries = new ColumnSeries
            {
                Title = "Soldes positifs",
                Values = new ChartValues<double>(),
                DataLabels = true,
                LabelPoint = point => $"{point.Y:N2} $", // Format monétaire pour les labels
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green),
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green)
            };

            // Créer une série de barres pour les soldes négatifs (en rouge)
            var negativeSeries = new ColumnSeries
            {
                Title = "Soldes négatifs",
                Values = new ChartValues<double>(),
                DataLabels = true,
                LabelPoint = point => $"-{Math.Abs(point.Y):N2} $", // Format monétaire (sans le signe moins) pour les labels négatifs
                Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red),
                Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red)
            };

            // Parcourt les résultats pour séparer les valeurs positives et négatives dans les séries correspondantes.
            // C'est nécessaire pour appliquer des couleurs différentes aux barres.
            foreach (var resultat in resultats)
            {
                if (resultat.Value >= 0)
                {
                    positiveSeries.Values.Add(resultat.Value);
                    negativeSeries.Values.Add(0.0); // Ajoute 0 pour cette série afin de maintenir l'alignement des barres
                }
                else
                {
                    positiveSeries.Values.Add(0.0); // Ajoute 0 pour cette série
                    negativeSeries.Values.Add(Math.Abs(resultat.Value)); // Ajoute la valeur absolue pour les barres négatives
                }
            }

            // Ajoute les deux séries à la collection du graphique à barres.
            BarSeries.Add(positiveSeries);
            BarSeries.Add(negativeSeries);

            // Met à jour les labels de l'axe X avec les noms des participants.
            LabelsAxis[0].Labels = resultats.Select(r => r.Label).ToList();

            // Calcule et met à jour le résumé dynamique pour les soldes.
            ResumeGraphique = $"Participants : {resultats.Count} | Solde total : {resultats.Sum(r => r.Value):N2} $ | Solde moyen : {(resultats.Count > 0 ? resultats.Average(r => r.Value) : 0):N2} $";
        }

        /// <summary>
        /// Charge les données et configure le graphique camembert pour afficher la répartition des dépenses par participant.
        /// </summary>
        private void ChargerGraphiqueDepenses()
        {
            if (ActiviteSelectionnee == null)
                return;

            // Obtient les données de dépenses par participant depuis l'activité
            var resultats = ActiviteSelectionnee.ObtenirGraphiqueDepenses()
                .Where(r => r.Date >= DateDebut && r.Date <= DateFin)
                .GroupBy(r => r.Label)
                .Select(g => new PartageDepense.Model.GraphData(g.Key, g.Sum(x => x.Value)))
                .ToList();

            // Applique le tri selon les critères sélectionnés
            if (TriParValeur)
            {
                resultats = TriCroissant 
                    ? resultats.OrderBy(r => r.Value).ToList()
                    : resultats.OrderByDescending(r => r.Value).ToList();
            }
            else
            {
                resultats = TriCroissant 
                    ? resultats.OrderBy(r => r.Label).ToList()
                    : resultats.OrderByDescending(r => r.Label).ToList();
            }

            if (resultats.Count == 0)
            {
                PieSeries.Clear();
                MessageGraphique = "Aucune dépense à afficher pour cette période.";
                ResumeGraphique = string.Empty;
                return;
            }

            // Couleurs prédéfinies pour les segments du camembert.
            var colors = new[]
            {
                System.Windows.Media.Color.FromRgb(33, 150, 243),   // Blue
                System.Windows.Media.Color.FromRgb(244, 67, 54),    // Red
                System.Windows.Media.Color.FromRgb(255, 193, 7),    // Yellow
                System.Windows.Media.Color.FromRgb(96, 125, 139)    // Gray
            };

            PieSeries.Clear();

            // Crée une série pour chaque part du camembert (chaque participant).
            for (int i = 0; i < resultats.Count; i++)
            {
                var data = resultats[i];
                // Assigne une couleur cycliquement à chaque part.
                var color = new System.Windows.Media.SolidColorBrush(colors[i % colors.Length]);

                PieSeries.Add(new PieSeries
                {
                    Title = data.Label,
                    Values = new ChartValues<double> { data.Value },
                    DataLabels = true,
                    Fill = color,
                    // Affiche la valeur de la dépense sur chaque part.
                    LabelPoint = chartPoint => $"{chartPoint.Y}"
                });
            }

            // Calcule et met à jour le résumé dynamique pour les dépenses.
            ResumeGraphique = $"Participants : {resultats.Count} | Dépenses totales : {resultats.Sum(r => r.Value):N2} $ | Dépense moyenne : {(resultats.Count > 0 ? resultats.Average(r => r.Value) : 0):N2} $";
        }
    }
}
