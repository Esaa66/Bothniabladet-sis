using System;

namespace Bothniabladet.Data
{
    public enum NewsSection
    {
        CULTURE,
        NEWS,
        INTERNATIONAL,
        ECONOMY,
        SPORTS
    }

    public class SectionEnum
    {
        public int SectionEnumId { get; set; }
        public NewsSection Name { get; set; }
    }
}
