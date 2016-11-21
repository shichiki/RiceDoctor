using RiceDoctor.KnowledgeBase.Models;

namespace RiceDoctor.InferenceEngine.Models
{
    public enum FactType
    {
        Crisp,
        Fuzzy
    }

    public enum CrispFactType
    {
        Class,
        Individual,
        Attribute
    }

    public abstract class Fact
    {
        public abstract FactType Type { get; }
    }

    public abstract class CrispFact : Fact
    {
        public override FactType Type => FactType.Crisp;

        public abstract CrispFactType CrispType { get; }

        // TODO: workaround!!!
        public abstract string ToInputString();
    }

    public class ClassCrispFact : CrispFact
    {
        public override CrispFactType CrispType => CrispFactType.Class;

        public string Class { get; set; }

        public string Value { get; set; }

        public override string ToInputString()
        {
            return Class;
        }


        public override string ToString()
        {
            return Class + "=" + Value;
        }
    }

    public class IndividualCrispFact : CrispFact
    {
        public override CrispFactType CrispType => CrispFactType.Individual;

        public string Individual { get; set; }

        public string Class { get; set; }

        public override string ToInputString()
        {
            return Class;
        }


        public override string ToString()
        {
            return Class + "=" + Individual;
        }
    }

    public class AttributeCrispFact : CrispFact
    {
        public override CrispFactType CrispType => CrispFactType.Attribute;

        public string Class { get; set; }

        public string Property { get; set; }

        public string Value { get; set; }

        public override string ToInputString()
        {
            return Class + "." + Property;
        }


        public override string ToString()
        {
            return Class + "." + Property + "=" + Value;
        }
    }

    // TODO: rewrite fuzzy class
    public class FuzzyFact : Fact
    {
        public override FactType Type => FactType.Fuzzy;

        public MembershipFunction MembershipFunction { get; set; }

        public string Class { get; set; }
    }
}