using System;
using System.Collections.Generic;

namespace Bothniabladet.Data
{
    public class Photographer
    {
        public int PhotographerId { get; set; }
        public string PhotographerName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string Biography { get; set; }

        // Relationships
        public ICollection<Image> Images { get; set; }
    }
}

