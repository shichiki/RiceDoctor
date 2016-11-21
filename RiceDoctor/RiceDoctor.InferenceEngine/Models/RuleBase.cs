using System.Collections.Generic;

namespace RiceDoctor.InferenceEngine.Models
{
    public class RuleBase
    {
        public RuleBase()
        {
            InferenceRules = new List<InferenceRule>();
            RelationRules = new List<RelationRule>();
        }

        public List<InferenceRule> InferenceRules { get; set; }

        public List<RelationRule> RelationRules { get; set; }
    }
}