using System.Collections.Generic;
using RiceDoctor.InferenceEngine;
using RiceDoctor.InferenceEngine.Models;
using RiceDoctor.KnowledgeBase;
using RiceDoctor.KnowledgeBase.Models;

namespace RiceDoctor.Test
{
    // TODO: replace Console program with Unit Test
    class Program
    {
        static void Main(string[] args)
        {
            KnowledgeManager knowledgeManager = new KnowledgeManager(@"C:\Users\ShichiKi\Desktop\SimpleOwlOntology.owl");
            EngineManager engineManager = new EngineManager(knowledgeManager);

            // Input nguoi dung
            List<Fact> inputs = CreateMockInputs();

            // Yeu cau nguoi dung
            List<Request> outputs = CreateMockOutputs();

            // Suy dien tien
            engineManager.CompleteInfer(inputs, outputs);

            // Hoi y kien nguoi dung va them su kien
            List<CrispFact> newFacts = engineManager.MakeFactsFromIncompleteRules(outputs);
            //List<Fact> choseFacts = new List<Fact>() {newFacts[0], newFacts[1]};
            //engine.AddNewFacts(choseFacts);

            // Suy dien tien lai voi su kien moi
            //engine.CompleteInfer(inputs, outputs);

            // Suy dien tien khong day du
            //engine.IncompleteInfer(inputs, outputs);
        }

        static List<Fact> CreateMockInputs()
        {
            return new List<Fact>
            {
                new AttributeCrispFact {Class = "La", Property = "Mau", Value = "Vang"},
                new ClassCrispFact {Class = "Re", Value = "Un"}
            };
        }

        static List<Request> CreateMockOutputs()
        {
            Term benhTerm = new Term {Name = "Benh"};
            Term sauBenhTerm = new Term {Name = "Sau_Benh"};

            Class benhClass = new Class {Term = benhTerm};
            Class sauBenhClass = new Class {Term = sauBenhTerm};

            return new List<Request>
            {
                new IndividualRequest {Class = benhClass},
                new IndividualRequest {Class = sauBenhClass}
            };
        }
    }
}