using RiceDoctor.KnowledgeBase.Models;

namespace RiceDoctor.InferenceEngine.Models
{
    public enum RequestType
    {
        Individual
    }

    public abstract class Request
    {
        public abstract RequestType Type { get; }
    }

    public class IndividualRequest : Request
    {
        public override RequestType Type => RequestType.Individual;

        public Class Class { get; set; }

        public override string ToString()
        {
            return Class.Term.Name;
        }
    }
}