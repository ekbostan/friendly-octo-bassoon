using System;
using System.Collections.Generic;

namespace LegalApiProxy.Models
{
    public class Court
    {
        public string ResourceUri { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
    }

    public class Docket
    {
        public string ResourceUri { get; set; }
        public int Id { get; set; }
        public string CourtId { get; set; }
        public string CaseName { get; set; }
        public string DocketNumber { get; set; }
        public string Cause { get; set; }
        public string NatureOfSuit { get; set; }
        public DateTime? DateFiled { get; set; }
        public DateTime? DateTerminated { get; set; }
    }

    public class Cluster
    {
        public string ResourceUri { get; set; }
        public int Id { get; set; }
        public string Docket { get; set; }
        public List<string> SubOpinions { get; set; }
    }

    public class Opinion
    {
        public string ResourceUri { get; set; }
        public int Id { get; set; }
        public string Cluster { get; set; }
        public string Type { get; set; }
        public string PlainText { get; set; }
        public string HtmlWithCitations { get; set; }
    }
}
