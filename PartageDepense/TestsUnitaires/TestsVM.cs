using PartageDepense.Model;
using PartageDepense.ViewModel;
using FluentAssertions;

namespace TestsUnitaires
{
    [TestClass]
    public sealed class TestsVM
    {
        Gestionnaire gestionnaire = new Gestionnaire();
        GestionDesActivitesVM? gestionDesActivitesVM;
        GestionDesParticipantsVM? gestionDesParticipantsVM;

        [TestInitialize]
        public void Setup()
        {
            gestionDesActivitesVM = new GestionDesActivitesVM(gestionnaire);
            gestionDesParticipantsVM = new GestionDesParticipantsVM(gestionnaire);

            gestionDesParticipantsVM.Nom = "Test";
            gestionDesParticipantsVM.Prenom = "Test";

            gestionDesActivitesVM.NomActivite = "Test";


            //Initialisation des participants
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();
            gestionDesParticipantsVM.AjouterParticipant();

            //Initialisation des activités
            gestionDesActivitesVM.AjouterActivite();
            gestionDesActivitesVM.AjouterActivite();
            gestionDesActivitesVM.AjouterActivite();
            gestionDesActivitesVM.AjouterActivite();

        }

        [TestMethod]
        public void AjouterParticipant()
        {
            gestionDesParticipantsVM?.LesParticipants?.Count.Should().Be(8);
        }


        [TestMethod]
        public void SupprimerParticipant()
        {
            if(gestionDesParticipantsVM is not null && gestionDesParticipantsVM.LesParticipants is not null)
                gestionDesParticipantsVM.ParticipantSelectionne = gestionDesParticipantsVM?.LesParticipants[^1];
            gestionDesParticipantsVM?.SupprimerParticipant();

            gestionnaire.LesParticipants?.Count.Should().Be(7);
        }

        [TestMethod]
        public void AjouterActivite()
        {
            gestionDesActivitesVM?.LesActivites?.Count.Should().Be(4);
        }

        [TestMethod]
        public void SupprimerActivite()
        {
            if(gestionDesActivitesVM is not null && gestionDesActivitesVM.LesActivites is not null) gestionDesActivitesVM.ActiviteSelectionnee = gestionDesActivitesVM?.LesActivites[^1];
            gestionDesActivitesVM?.SupprimerActivite();

            gestionDesActivitesVM?.LesActivites?.Count.Should().Be(3);
        }

        //[TestMethod]
        //public void AjouterSauvegarde()
        //{
        //    gestionDesSauvegardesVM.LesSauvegardes?.Count.Should().Be(2);
        //}
    }
}
