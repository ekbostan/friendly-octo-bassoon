using System;
using System.Collections.Generic;

namespace LegalApiProxy.Models
{
    public class RecapFetchRequest
    {
        public int Id { get; set; }
        public int RequestType { get; set; } // 1 = docket, 2 = PDF, 3 = attachment
        public string PacerUsername { get; set; }
        public string PacerPassword { get; set; }
        public string Court { get; set; }
        public string DocketNumber { get; set; }
        public string PacerCaseId { get; set; }
        public string RecapDocument { get; set; }
        public bool ShowPartiesAndCounsel { get; set; }
        public DateTime? DateRangeStart { get; set; }
        public DateTime? DateRangeEnd { get; set; }
    }

    public class RecapUploadRequest
    {
        public int Id { get; set; }
        public int UploadType { get; set; } // 1 = docket, 2 = PDF
        public string FilePathLocal { get; set; }
        public string Court { get; set; }
        public string PacerCaseId { get; set; }
        public string PacerDocId { get; set; }
        public int? DocumentNumber { get; set; }
        public int? AttachmentNumber { get; set; }
        public bool Debug { get; set; }
    }

    public class RecapStatusResponse
    {
        public int Id { get; set; }
        public string Court { get; set; }
        public string Docket { get; set; }
        public string DocketEntry { get; set; }
        public string RecapDocument { get; set; }
        public int Status { get; set; } // 1 = in queue, 2 = success
        public string ErrorMessage { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
