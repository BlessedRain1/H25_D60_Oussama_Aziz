using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using PartageDepense.Model.JSON;

namespace PartageDepense.Model
{
    public class Sauvegarde
    {
        public DateTime DateTime { get; private set; }

        public string? Description { get; private set; }

        public string? Fichier { get; private set; }

        private readonly string dossier;

        private string[] files;

        public Sauvegarde() //Générer la logique de sérialisation et désérialisation des données de l'activité à sauvegarder
        {
            DateTime = DateTime.Now;
            dossier = Path.GetFullPath(@"../../../Sauvegardes");

            // Vérifier si le dossier existe, sinon le créer
            if (!Directory.Exists(dossier))
            {
                Directory.CreateDirectory(dossier); // Crée le dossier si il n'existe pas
            }

            files = Directory.GetFiles(dossier, "*.json");
        }

        public async Task EffectuerSauvegardeAsync(Gestionnaire gestionnaire)
        {
            SauvegardeModel model = new SauvegardeModel
            {
                LesParticipants = gestionnaire.LesParticipants?.ToList(),
                LesActivites = gestionnaire.LesActivites?.ToList(),
                Date = DateTime.Now,
                Description = Description
            };

            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(model, options);

            string filePath = Path.Combine(dossier, "sauvegarde.json");

            await File.WriteAllTextAsync(filePath, jsonString);
        }

        public bool ChargerSauvegarde(Gestionnaire gestionnaire)
        {
            try
            {
                if (files.Contains(Path.Combine(dossier, "sauvegarde.json")))
                {
                    string jsonString = File.ReadAllText(Path.Combine(dossier, "sauvegarde.json"));
                    ResponseModel? model = JsonSerializer.Deserialize<ResponseModel>(jsonString);

                    if (model == null || model.lesActivites is null || model.lesParticipants is null)
                    {
                        throw new InvalidOperationException("Failed to deserialize the sauvegarde.json data.");
                    }

                    List<Participant> lesParticipants = new List<Participant>();
                    List<Activite> lesActivites = new List<Activite>();

                    if (model.lesActivites is not null) foreach (ActiviteModel a in model.lesActivites)
                        {
                            List<Participant> participantsActivite = new List<Participant>();
                            List<Depense> depensesActivite = new List<Depense>();
                            List<Remboursement> remboursementsActivite = new List<Remboursement>();
                            List<ParticipantSolde> participantsSoldes = new List<ParticipantSolde>();

                            if (a.lesParticipants is not null) foreach (ParticipantModel p in a.lesParticipants)
                                {
                                    participantsActivite.Add(new Participant(p.nom ?? "", p.prenom ?? ""));
                                }

                            if (a.lesDepenses is not null) foreach (DepenseModel d in a.lesDepenses)
                                {
                                    depensesActivite.Add(new Depense(new Participant(d.participant?.nom ?? "", d.participant?.prenom ?? ""), d.description ?? "", d.montant, d.date));
                                }

                            if (a.lesRemboursements is not null) foreach (RemboursementModel r in a.lesRemboursements)
                                {
                                    remboursementsActivite.Add(new Remboursement(r.montant, new Participant(r.participant?.nom ?? "", r.participant?.prenom ?? "")));
                                }

                            if (a.participantsSoldes is not null) foreach (ParticpantsSoldesModel ps in a.participantsSoldes)
                                {
                                    participantsSoldes.Add(new ParticipantSolde(new Participant(ps.participant?.nom ?? "", ps.participant?.prenom ?? ""), ps.solde, ps.etat ?? ""));
                                }

                            lesActivites.Add(new Activite(a.nomActivite ?? "", new ObservableCollection<Participant>(participantsActivite), new ObservableCollection<Depense>(depensesActivite), new ObservableCollection<Remboursement>(remboursementsActivite), new ObservableCollection<ParticipantSolde>(participantsSoldes)));
                        }

                    foreach (ParticipantModel p in model.lesParticipants)
                    {
                        lesParticipants.Add(new Participant(p.nom ?? "", p.prenom ?? ""));
                    }

                    gestionnaire.SetLists(lesActivites, lesParticipants);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }

    public class SauvegardeModel
    {
        public List<Participant>? LesParticipants { get; set; }

        public List<Activite>? LesActivites { get; set; }

        public DateTime Date { get; set; }

        public string? Description { get; set; }
    }
}
