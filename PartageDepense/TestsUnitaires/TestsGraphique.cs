using Microsoft.VisualStudio.TestTools.UnitTesting;
using PartageDepense.Model;
using System.Linq;

namespace TestsUnitaires
{
    [TestClass]
    public class TestsGraphique
    {
        private Activite _activite;
        private Participant _participant1;
        private Participant _participant2;
        private Participant _participant3;

        [TestInitialize]
        public void Setup()
        {
            // Création d'une activité de test
            _activite = new Activite("Test Activity");

            // Création des participants de test
            _participant1 = new Participant("Doe", "John");
            _participant2 = new Participant("Smith", "Jane");
            _participant3 = new Participant("Johnson", "Bob");

            // Ajout des participants à l'activité
            _activite.AjouterParticipant(_participant1);
            _activite.AjouterParticipant(_participant2);
            _activite.AjouterParticipant(_participant3);
        }

        [TestMethod]
        public void ObtenirGraphiqueSoldes_WithNoExpenses_ReturnsEmptyList()
        {
            // Act
            var result = _activite.ObtenirGraphiqueSoldes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ObtenirGraphiqueSoldes_WithExpenses_ReturnsCorrectSoldes()
        {
            // Arrange
            _activite.AjouterDepense(_participant1, "Dépense 1", 100, null);
            _activite.AjouterDepense(_participant2, "Dépense 2", 50, null);
            _activite.AjouterDepense(_participant3, "Dépense 3", 150, null);

            // Act
            var result = _activite.ObtenirGraphiqueSoldes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count); // Seuls les soldes non nuls sont inclus

            // Vérifier les soldes de chaque participant
            var solde2 = result.FirstOrDefault(r => r.Label == "Jane Smith");
            var solde3 = result.FirstOrDefault(r => r.Label == "Bob Johnson");

            Assert.IsNotNull(solde2);
            Assert.IsNotNull(solde3);

            // Le solde devrait être la différence entre ce que le participant a payé et sa part
            // Part par participant = (100 + 50 + 150) / 3 = 100
            Assert.AreEqual(-50, solde2.Value); // 50 - 100 = -50
            Assert.AreEqual(50, solde3.Value); // 150 - 100 = 50
        }

        [TestMethod]
        public void ObtenirGraphiqueDepenses_WithNoExpenses_ReturnsEmptyList()
        {
            // Act
            var result = _activite.ObtenirGraphiqueDepenses();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ObtenirGraphiqueDepenses_WithExpenses_ReturnsCorrectDepenses()
        {
            // Arrange
            _activite.AjouterDepense(_participant1, "Dépense 1", 100, null);
            _activite.AjouterDepense(_participant1, "Dépense 2", 50, null);
            _activite.AjouterDepense(_participant2, "Dépense 3", 75, null);
            _activite.AjouterDepense(_participant3, "Dépense 4", 125, null);

            // Act
            var result = _activite.ObtenirGraphiqueDepenses();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count);

            // Vérifier les dépenses de chaque participant
            var depense1 = result.FirstOrDefault(r => r.Label == "John Doe");
            var depense2 = result.FirstOrDefault(r => r.Label == "Jane Smith");
            var depense3 = result.FirstOrDefault(r => r.Label == "Bob Johnson");

            Assert.IsNotNull(depense1);
            Assert.IsNotNull(depense2);
            Assert.IsNotNull(depense3);

            // Vérifier les montants
            Assert.AreEqual(150, depense1.Value); // 100 + 50
            Assert.AreEqual(75, depense2.Value);
            Assert.AreEqual(125, depense3.Value);
        }

        [TestMethod]
        public void ObtenirGraphiqueDepenses_WithMultipleExpensesForSameParticipant_ReturnsSummedDepenses()
        {
            // Arrange
            _activite.AjouterDepense(_participant1, "Dépense 1", 100, null);
            _activite.AjouterDepense(_participant1, "Dépense 2", 50, null);
            _activite.AjouterDepense(_participant1, "Dépense 3", 75, null);

            // Act
            var result = _activite.ObtenirGraphiqueDepenses();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);

            var depense = result.First();
            Assert.AreEqual("John Doe", depense.Label);
            Assert.AreEqual(225, depense.Value); // 100 + 50 + 75
        }
    }
} 