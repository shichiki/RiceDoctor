using System.Collections.Generic;

namespace RiceDoctor.InferenceEngine.Models
{
    public enum RuleType
    {
        InferenceRule,
        RelationRule
    }

    public enum InferenceRuleType
    {
        Crisp,
        CrispFuzzy,
        Fuzzy
    }

    public abstract class Rule
    {
        public abstract RuleType Type { get; }
    }

    public class InferenceRule : Rule
    {
        public InferenceRule()
        {
            Hypotheses = new List<Fact>();
            Conclusions = new List<Fact>();
        }

        public override RuleType Type => RuleType.InferenceRule;

        public InferenceRuleType InferenceType { get; set; }

        public List<Fact> Hypotheses { get; set; }

        public List<Fact> Conclusions { get; set; }

        public double CertaintyFactor { get; set; }
    }
    public class RelationRule : Rule
    {
        public override RuleType Type => RuleType.RelationRule;

        public string Domain { get; set; }

        public string Range { get; set; }

        // TODO: abstract the query to better models
        public string Query { get; set; }

        public string QueryDomain { get; set; }

        public string QueryRange { get; set; }
    }
}