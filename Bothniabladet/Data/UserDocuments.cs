using System;

namespace Bothniabladet.Data
{
    public class UserDocuments
    {
        // Variables
        public bool Closed { get; set; } // Indicates if the document is closed or not

        // Links
        public int SalesDocumentId { get; set; }
        public SalesDocument SalesDocument { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
