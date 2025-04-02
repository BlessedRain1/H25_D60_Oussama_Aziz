using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PartageDepense.Model;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace PartageDepense.ViewModel
{
    public partial class GestionDesActivitesVM : GestionnaireVM
    {
        public Gestionnaire _gestio { get; set; }

        [ObservableProperty]
        private string? _nomActivite;

        [ObservableProperty]
        private ObservableCollection<Activite>? _lesActivites;

        [ObservableProperty]
        private Activite? _activiteSelectionnee;

        [ObservableProperty]
        private double _progressValue;

        [ObservableProperty]
        private bool _isCardVisible;

        public virtual void ChangementActiviteSelectionne()
        {
        }

        partial void OnActiviteSelectionneeChanged(Activite? value)
        {
            if (value != null)
            {
                _activiteSelectionnee = value;
                ChangementActiviteSelectionne();
            }
        }

        public GestionDesActivitesVM(Gestionnaire gestionnaire) : base(gestionnaire)
        {
            _gestio = gestionnaire;
            _isCardVisible = false;

            if (_gestionnaire.LesActivites != null)
                _lesActivites = gestionnaire.LesActivites;
        }

        [RelayCommand]
        public void AjouterActivite()
        {
            if (NomActivite != null && _gestionnaire.AjouterActivite(NomActivite))
            {
                NomActivite = "";
                ActiviteSelectionnee = null;
                DialogHost.CloseDialogCommand.Execute(null, null);
            }
            else
            {
                MessageBox.Show("Veuillez fournir le nom de l'activité avant de l'ajouter.", "Erreur d'ajout de l'activité", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public void SupprimerActivite()
        {
            if (ActiviteSelectionnee != null)
            {
                // Afficher une boîte de dialogue de confirmation
                var result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir supprimer cette activité ?",
                    "Confirmation de suppression",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    // L'utilisateur a confirmé, procéder à la suppression
                    _gestionnaire.SupprimerActivite(ActiviteSelectionnee);
                    MessageBox.Show("Activité supprimée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                // Si l'utilisateur a cliqué sur "Non", aucune action n'est effectuée
            }
            else
            {
                // Si aucune activité n'est sélectionnée, afficher un message d'erreur
                MessageBox.Show("Aucune activité sélectionnée à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        public async Task SauvegarderDonnees()
        {
            try
            {
                IsCardVisible = true;
                ProgressValue = 0;

               

                // Simulation de la progression avant la sauvegarde réelle
                for (int i = 0; i <= 50; i++)
                {
                    await Task.Delay(15); // Plus rapide pour un effet visuel
                    ProgressValue = i;
                }

                // Exécution de la sauvegarde
                Sauvegarde nSauvegarde = new Sauvegarde();
                await nSauvegarde.EffectuerSauvegardeAsync(_gestionnaire);

                // Compléter la progression après la sauvegarde
                for (int i = 51; i <= 100; i++)
                {
                    await Task.Delay(15);
                    ProgressValue = i;
                }

                MessageBox.Show("Les données ont été sauvegardées avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde des données : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
