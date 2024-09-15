using System;
using System.Collections.Generic;

namespace LegalApiProxy.Models
{
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
        public string AbsoluteUrl { get; set; }
        public string AssignedTo { get; set; }
    }

    public class DocketEntry
    {
        public string ResourceUri { get; set; }
        public int EntryNumber { get; set; }
        public string Description { get; set; }
        public List<Document> RecapDocuments { get; set; }
    }

    public class Document
    {
        public string ResourceUri { get; set; }
        public string PlainText { get; set; }
        public string FilePathLocal { get; set; }
        public bool IsAvailable { get; set; }
        public int OcrStatus { get; set; }
    }

    public class Party
    {
        public string ResourceUri { get; set; }
        public string Name { get; set; }
        public List<Attorney> Attorneys { get; set; }
        public List<PartyType> PartyTypes { get; set; }
    }

    public class Attorney
    {
        public string ResourceUri { get; set; }
        public string Name { get; set; }
        public string ContactInfo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public List<Party> PartiesRepresented { get; set; }
    }

    public class PartyType
    {
        public string Docket { get; set; }
        public string Name { get; set; }
        public string DateTerminated { get; set; }
    }
}
