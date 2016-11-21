using System;
using System.Collections.Generic;
using java.io;
using org.semanticweb.owlapi.apibinding;
using org.semanticweb.owlapi.model;
using org.swrlapi.factory;
using org.swrlapi.sqwrl;
using org.swrlapi.sqwrl.values;

namespace RiceDoctor.KnowledgeBase
{
    public class KnowledgeManager
    {
        private readonly OWLOntologyManager _ontologyManager;

        private readonly OWLOntology _ontology;

        private SQWRLQueryEngine _queryEngine;

        private int _count;

        public KnowledgeManager(string fileName)
        {
           //_ontologyManager = OWLManager.createOWLOntologyManager();
           // _ontology = _ontologyManager.loadOntologyFromOntologyDocument(new File(fileName));
           // _queryEngine = SWRLAPIFactory.createSQWRLQueryEngine(_ontology);

           // //create autogen0
           // SQWRLResult r = _queryEngine.runSQWRLQuery("q", "tbox:opd(?i) -> sqwrl:select(?i)");
        }

        public Tuple<IEnumerable<string>, IEnumerable<string>> RunQuery(string query, string domain, string range)
        {
            string completeQuery = string.Format(query, domain, range);

            List<string> domains = new List<string>();
            List<string> ranges = new List<string>();
            Tuple<IEnumerable<string>, IEnumerable<string>> result =
                new Tuple<IEnumerable<string>, IEnumerable<string>>(domains, ranges);

            SQWRLResult r = _queryEngine.runSQWRLQuery("q" + _count++, completeQuery);
            while (r.next())
            {
                if (!domain.Contains("autogen0:"))
                {
                    domain = domain.Substring(1); // xoa ?
                    SQWRLNamedIndividualResultValue domainIndividual = r.getNamedIndividual(domain);
                    string domainName = domainIndividual.getShortName();
                    domains.Add(domainName);
                }

                if (!range.Contains("autogen0:"))
                {
                    range = range.Substring(1);
                    SQWRLNamedIndividualResultValue rangeIndividual = r.getNamedIndividual(range);
                    string rangeName = rangeIndividual.getShortName();
                    ranges.Add(rangeName);
                }
            }

            return result;
        }
    }
}