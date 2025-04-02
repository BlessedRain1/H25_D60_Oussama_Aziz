using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows;

namespace PartageDepense.View
{
    static class Comportements
    {
        /// <summary>
        /// Permet de valider que la touche saisie produira un résultat valide d'un nombre réel. 
        /// </summary>
        /// <param name="sender">Composant de l'interface qui a lancé la méthode.</param>
        /// <param name="e">Objet contenant les arguments du changement de contenu du composant.</param>
        static public void ValidationFormatReel(object sender, TextCompositionEventArgs e)
        {
            // Vérification de la validité du caractère saisi
            if (Regex.IsMatch(e.Text, @"[0-9]|\."))
            {
                if (sender is TextBox)
                {
                    // Déterminer le résultat final en insérant le caractère saisi à l'endroit attendu
                    TextBox textBox = (TextBox)sender;
                    string nouveauTexte = textBox.Text.Insert(textBox.CaretIndex, e.Text);

                    // Vérifier si le résultat correspond au format attendu
                    if (!double.TryParse(nouveauTexte, NumberStyles.Number, CultureInfo.InvariantCulture, out double result))
                    {
                        // Empêcher la saisie de texte non valide
                        e.Handled = true;
                    }

                    // Détecter s'il s'agit d'un point ou d'une virgule au bon endroit
                    if (nouveauTexte.Contains(",.") || nouveauTexte.Contains(".,") || nouveauTexte.Contains(".."))
                    {
                        // Annuler l'entrée de l'utilisateur
                        e.Handled = true;

                        // Positionner le curseur après le point déjà existant
                        int caretIndex = textBox.Text.IndexOf('.') + 1;
                        textBox.CaretIndex = caretIndex;
                    }
                }
            }
            else
            {
                // Empêcher la saisie de caractères non valides
                e.Handled = true;
            }
        }

        /// <summary>
        /// Détecte la frappe des touches de suppression (Delete) et de retour arrière (Backspace).
        /// </summary>
        /// <param name="sender">L'objet qui a déclenché l'événement.</param>
        /// <param name="e">Les données d'événement qui contiennent les informations sur la touche enfoncée.</param>
        static public void PreventionSuppressionSeparateurDecimale(object sender, KeyEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // Détecter la frappe de la touche de suppression (Delete)
            if (e.Key == Key.Delete)
            {
                // Vérifier si le TextBox contient un point décimal après le curseur
                int caretIndex = textBox.CaretIndex;
                if (caretIndex < textBox.Text.Length && textBox.Text[caretIndex] == '.')
                {
                    // Annuler la suppression et déplacer le curseur après le point décimal
                    e.Handled = true;
                    textBox.CaretIndex = caretIndex + 1;
                }
            }
            // Détecter la frappe de la touche de retour arrière (Backspace)
            else if (e.Key == Key.Back)
            {
                // Vérifier si le TextBox contient un point décimal avant le curseur
                int caretIndex = textBox.CaretIndex;
                if (caretIndex > 0 && textBox.Text[caretIndex - 1] == '.')
                {
                    // Annuler la suppression et déplacer le curseur avant le point décimal
                    e.Handled = true;
                    textBox.CaretIndex = caretIndex - 1;
                }
            }
        }

        /// <summary>
        /// Sélectionne tout le contenu d'un textbox de manière asynchrone de manière a s'assurer que ce soit la dernière action exécutée.
        /// </summary>
        /// <param name="sender">Composant de l'interface qui a déclenché l'événement.</param>
        /// <param name="e">Objet contenant les arguments du changement de contenu du composant.</param>
        static public async void TextBox_SelectionContenu(object sender, RoutedEventArgs e)
        {

            await Task.Delay(100); // Délai pour simuler l'opération asynchrone

            // L'exécution étant asynchrone, elle s'exécute dans un thread séparé de l'interface,
            // il faut donc manipuler les composants de l'interface sur son thread
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (sender is TextBox)
                {
                    // Sélectionne le contenu
                    ((TextBox)sender).SelectAll();

                    // Indique que l'événement a été traité
                    e.Handled = true;
                }
            });
        }
    }
}
