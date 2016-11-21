using System;
using System.Collections.Generic;
using RiceDoctor.InferenceEngine.Models;
using RiceDoctor.KnowledgeBase;

namespace RiceDoctor.InferenceEngine
{
    public class EngineManager
    {
        // TODO: initialize rule base
        public EngineManager(KnowledgeManager knowledgeManager)
        {
            _knowledgeManager = knowledgeManager;

            RuleBase = new RuleBase
            {
                InferenceRules = CreateMockInferenceRules(),
                RelationRules = CreateMockRelationRules()
            };

            Known = new List<Fact>();
        }

        // TODO: this is wrong (?)
        private List<Fact> Known { get; set; }

        public RuleBase RuleBase { get; private set; }

        private KnowledgeManager _knowledgeManager;

        public bool CompleteInfer(List<Fact> inputs, List<Request> outputs)
        {
            // TODO: replace this global variable
            InferenceRule Rule = null;

            foreach(Fact input in inputs)
            {
                Known.Add(input);
            }

            bool completeFlag = false; // duyet het luat chua
            List<int> usedIndexes = new List<int>();
            // luu lai danh sach cac luat da di qua va duoc ap dung de tranh lap lai

            while (!completeFlag) // neu chuat duyet het luat
            {
                bool usableFlag = false; // luat co ap dung dung khong
                int index = 0; // vi tri cua luat hien tai

                // duyet tung gia thiet trong tap luat
                foreach (InferenceRule rule in RuleBase.InferenceRules)
                {
                    IEnumerable<Fact> hypotheses = rule.Hypotheses;

                    // Tim cach luu lai nhung luat ma da duyet co trong Know nhung chua co result

                    usableFlag = CheckHypothesesInKnown(hypotheses, Known); // Kiem tra luat co ap dung duoc khong
                    if (usableFlag)
                    {
                        // Kiem tra xem luat nay da ap dung chua va dua su kien vao Known chua de tranh lap lai
                        if (!usedIndexes.Contains(index))
                        {
                            // Neu chua ap dung
                            IEnumerable<Fact> conclusions = rule.Conclusions;
                            if (CheckRequestsInConclusions(outputs, conclusions))
                            // Kiem tra xem su kien ket luan co phai la ket qua khong
                            {
                                foreach(CrispFact fact in hypotheses)
                                {
                                    Console.Write(fact.ToString() + "\t");
                                }
                                Console.Write(" -> ");
                                foreach(CrispFact fact in conclusions)
                                {
                                    Console.Write(fact.ToString() + "\t");
                                }
                                Console.WriteLine();
                            }
                            else // Su kien ket luan khong la ket qua
                            {
                                foreach (Fact conclusion in conclusions)
                                {
                                    // Them tat ca su kien ket luan vao Known
                                    Known.Add(conclusion);
                                }
                            }

                            usedIndexes.Add(index); // luat o vi tri index da duyet va cap nhat vao Known
                            break;
                        }
                    }

                    index++;
                }

                // Neu duyet het luat ma chua tim duoc ket luan
                if (index == RuleBase.InferenceRules.Count)
                {
                    completeFlag = true;
                }
   

                foreach (RelationRule rule in RuleBase.RelationRules)
                {
                    for (int i = 0; i < Known.Count; ++i)
                    {
                        Fact fact = Known[i];

                        IndividualCrispFact individualFact = fact as IndividualCrispFact;
                        if (individualFact == null) continue;

                        if (individualFact.Class == rule.Domain)
                        {
                            Tuple<IEnumerable<string>, IEnumerable<string>> result =
                                _knowledgeManager.RunQuery(rule.Query, "autogen0:" + individualFact.Individual, rule.QueryRange);
                            foreach (string rangeIndividual in result.Item2)
                            {
                                string individual = rangeIndividual.Replace("autogen0:", "");
                                Console.WriteLine(individual); // workar5ound

                                Fact rangeFact = new IndividualCrispFact
                                {
                                    Class = rule.Range,
                                    Individual = individual
                                };

                                if (!Known.Contains(rangeFact))
                                {
                                    Known.Add(rangeFact);
                                }
                            }
                        }
                        else if (individualFact.Class == rule.Range)
                        {
                            Tuple<IEnumerable<string>, IEnumerable<string>> result =
                                _knowledgeManager.RunQuery(rule.Query, rule.QueryDomain, "autogen0:" + individualFact.Individual);
                            foreach (string domainIndividual in result.Item1)
                            {
                                string individual = domainIndividual.Replace("autogen0:", "");
                                Console.WriteLine(individual);

                                Fact domainFact = new IndividualCrispFact
                                {
                                    Class = rule.Range,
                                    Individual = individual
                                };

                                if (!Known.Contains(domainFact))
                                {
                                    Known.Add(domainFact);
                                }
                            }
                        }
                    }
                }
            }

            // TODO:

            return false;
        }

        // TODO: this is also wrong (?)
        public bool IncompleteInfer(List<Fact> inputs, List<Request> outputs)
        {
            int index = 0;
            foreach (InferenceRule rule in RuleBase.InferenceRules)
            {
                // TODO: workaround, chose only one request to work with, for now
                Request request = outputs[0];

                foreach (CrispFact conclusion in rule.Conclusions)
                {
                    if (conclusion.ToInputString() == request.ToString())
                    {
                        int count = 0;
                        foreach (CrispFact hyothesis in rule.Hypotheses)
                        {
                            if (hyothesis.ToInputString() != request.ToString())
                            {
                                count++;
                            }
                        }

                        // TODO: works with count
                    }
                }
            }

            return false;
        }

        private List<InferenceRule> CreateMockInferenceRules()
        {
            // nham !!!!
            return new List<InferenceRule>
            {
                new InferenceRule
                {
                    InferenceType = InferenceRuleType.Crisp,
                    CertaintyFactor = 1.0,
                    Hypotheses = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Re", Value = "Un"}
                    },
                    Conclusions = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Nuoc", Value = "Nhieu"},
                        new ClassCrispFact {Class = "Hat", Value = "Lep"}
                    }
                },
                new InferenceRule
                {
                    InferenceType = InferenceRuleType.Crisp,
                    CertaintyFactor = 1.0,
                    Hypotheses = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Re", Value = "Un"},
                        new ClassCrispFact {Class = "Than", Value = "Cao"}
                    },
                    Conclusions = new List<Fact>
                    {
                        new IndividualCrispFact {Class = "Sau_Benh", Individual = "Sau_Xanh"},
                    }
                },
                new InferenceRule
                {
                    InferenceType = InferenceRuleType.Crisp,
                    CertaintyFactor = 1.0,
                    Hypotheses = new List<Fact>
                    {
                        new AttributeCrispFact {Class = "La", Property = "Mau", Value = "Vang"},
                        new ClassCrispFact {Class = "Nuoc", Value = "Nhieu"}
                    },
                    Conclusions = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Hat", Value = "Lep"}
                    }
                },
                new InferenceRule
                {
                    InferenceType = InferenceRuleType.Crisp,
                    CertaintyFactor = 1.0,
                    Hypotheses = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Than", Value = "Cao"},
                        new AttributeCrispFact {Class = "La", Property = "Mau", Value = "Xanh"},
                        new ClassCrispFact {Class = "Re", Value = "Un"}
                    },
                    Conclusions = new List<Fact>
                    {
                        new IndividualCrispFact {Class = "Benh", Individual = "Nam"}
                    }
                },
                new InferenceRule
                {
                    InferenceType = InferenceRuleType.Crisp,
                    CertaintyFactor = 1.0,
                    Hypotheses = new List<Fact>
                    {
                        new ClassCrispFact {Class = "Than", Value = "Cao"},
                        new AttributeCrispFact {Class = "La", Property = "Mau", Value = "Xanh"},
                        new ClassCrispFact {Class = "Re", Value = "Un"}
                    },
                    Conclusions = new List<Fact>
                    {
                        new IndividualCrispFact {Class = "Benh", Individual = "Nam_Lun"}
                    }
                }
            };
        }

        private List<RelationRule> CreateMockRelationRules()
        {
            return new List<RelationRule>
            {
                new RelationRule
                {
                    Domain = "Thuoc",
                    Range = "Benh",
                    Query = "autogen0:chuaBenh({0}, {1}) -> sqwrl:select({0}, {1})",
                    QueryDomain = "?thuoc",
                    QueryRange = "?benh"
                },
                new RelationRule
                {
                    Domain = "BenhHai",
                    Range = "Nam",
                    Query = "autogen0:gayBoi({0}, {1}) -> sqwrl:select({0}, {1})",
                    QueryDomain = "?benhHai",
                    QueryRange = "?nam"
                }
            };
        }

        private bool CheckHypothesesInKnown(IEnumerable<Fact> hypotheses, IEnumerable<Fact> known)
        {
            int length = 0;
            int count = 0;

            // Kiem tra xem cac luat co ap dung duoc khong
            // TODO: support fuzzy facts
            foreach (CrispFact hypothesis in hypotheses)
            {
                length++;

                foreach (CrispFact k in known)
                {
                    if (hypothesis.ToString() == k.ToString())
                    {
                        count++;
                    }
                }
            }

            return count == length;
        }

        private bool CheckRequestsInConclusions(IEnumerable<Request> requests, IEnumerable<Fact> conclusions)
        {
            foreach (IndividualRequest request in requests)
            {
                foreach (CrispFact conclusion in conclusions)
                {
                    Console.Write(request.ToString() + "\t");
                    Console.WriteLine(conclusion.ToInputString());
                    if (request.ToString() == conclusion.ToInputString())
                    {
                        // TODO: something clearly wrong
                        return true;
                    }
                }
            }

            return false;
        }

        // TODO: refactor loops
        public List<CrispFact> MakeFactsFromIncompleteRules(IEnumerable<Request> requests)
        {
            List<CrispFact> newFacts = new List<CrispFact>();

            foreach (InferenceRule rule in RuleBase.InferenceRules)
            {
                foreach (CrispFact conclusion in rule.Conclusions)
                {
                    foreach (IndividualRequest request in requests)
                    {
                        if (conclusion.ToInputString() == request.ToString())
                        {
                            foreach (CrispFact hyothesis in rule.Hypotheses)
                            {
                                bool existedFact = false;
                                foreach (CrispFact f in Known)
                                {
                                    if (f.ToString() == hyothesis.ToString())
                                    {
                                        existedFact = true;
                                        break;
                                    }
                                }

                                if (!existedFact)
                                {
                                    //// this condition is wrong
                                    //if (!newFacts.Contains(hyothesis))
                                    //{
                                    //    newFacts.Add(hyothesis);
                                    //}

                                    // true Contains method
                                    bool isNewFact = true;
                                    foreach (CrispFact newFact in newFacts)
                                    {
                                        if (newFact.ToInputString() == hyothesis.ToInputString())
                                        {
                                            isNewFact = false;
                                            break;
                                        }
                                    }

                                    if (isNewFact)
                                    {
                                        newFacts.Add(hyothesis);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return newFacts;
        }

        public void AddNewFacts(IEnumerable<Fact> facts)
        {
            foreach (Fact fact in facts)
            {
                Known.Add(fact);
            }
        }
    }
}