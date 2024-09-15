using System;
using System.Collections.Generic;

namespace LegalDataModel
{
    public class LawDocument
    {
        public int DocumentId { get; set; }  
        public string Title { get; set; }    
        public string Content { get; set; }   
        public DateTime DateCreated { get; set; } 
        public DateTime DateModified { get; set; } 
        public string Summary { get; set; }  
        public Court Court { get; set; }      
        public List<Party> PartiesInvolved { get; set; } 
        public List<Citation> Citations { get; set; } 

        
        public LawDocument()
        {
            PartiesInvolved = new List<Party>();
            Citations = new List<Citation>();
        }
    }

    public class Court
    {
        public int CourtId { get; set; }       
        public string Name { get; set; }       
        public string Jurisdiction { get; set; }  
        public List<LawDocument> LawDocuments { get; set; }  
    }

    public class Party
    {
        public int PartyId { get; set; }     
        public string Name { get; set; }      
        public string Role { get; set; }     
    }

    public class Citation
    {
        public int CitationId { get; set; }  
        public string Reference { get; set; } 
    }
}
