using System.Collections.Generic;
using RiceDoctor.InferenceEngine;
using RiceDoctor.InferenceEngine.Models;
using RiceDoctor.KnowledgeBase.Models;
using RiceDoctor.KnowledgeBase;

namespace RiceDoctor.Test
{
    // TODO: replace Console program with Unit Test
    class Program
    {
        static void Main(string[] args)
       {
            KnowledgeManager knowledgeManager = new KnowledgeManager(@"C:\Users\ShichiKi\AppData\Roaming\Skype\My Skype Received Files\SimpleOwlOntology(5).owl");
            EngineManager engineManager = new EngineManager(knowledgeManager);
            // Input nguoi dung
            List<Fact> inputs = CreateMockInputs();

            // Yeu cau nguoi dung
            List<Request> outputs = CreateMockRequests();

            // Suy dien tien
            engineManager.CompleteInfer(inputs, outputs);

            // Hoi y kien nguoi dung va them su kien
            List<CrispFact> newFacts = engineManager.MakeFactsFromIncompleteRules(outputs);
            List<Fact> choseFacts = new List<Fact>() {newFacts[0]}; // workaround, chon dai 1 sk dau tien trong tap su kien moi bo sung cho luat ko day du
            engineManager.AddNewFacts(choseFacts);

            // Suy dien tien lai voi su kien moi
            engineManager.CompleteInfer(inputs, outputs);

            // Suy dien tien khong day du
            engineManager.IncompleteInfer(inputs, outputs);
        }

        static List<Fact> CreateMockInputs()
        {
            return new List<Fact>
            {
                new AttributeCrispFact {Class = "La", Property = "Mau", Value = "Vang"},
                new ClassCrispFact {Class = "Re", Value = "Un"},
               new IndividualCrispFact {Class="BenhHai", Individual="DaoOn" }
            };
        }

        // TODO : remake Request class
        static List<Request> CreateMockRequests()
        {
            Term benhTerm = new Term {Name = "Benh" };
            Term sauBenhTerm = new Term {Name = "Sau_Benh" };
            Term namTerm = new Term { Name = "Nam" }; // nguyen nhan gay benh la gi?

            Class benhClass = new Class {Term = benhTerm};
            Class sauBenhClass = new Class {Term = sauBenhTerm};
            Class namClass = new Class { Term = namTerm };

            return new List<Request>
            {
                new IndividualRequest {Class = benhClass},
                new IndividualRequest {Class = sauBenhClass},
                new IndividualRequest {Class=namClass }
            };
        }
    }
}