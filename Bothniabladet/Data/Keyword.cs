using System;
using System.Collections.Generic;

namespace Bothniabladet.Data
{
    public class Keyword
    {
        /*VARIABLES*/
        public int KeywordId { get; set; }
        public string Word { get; set; }

        /*LINKS*/
        public ICollection<ImageKeyword> KeywordLink { get; set; }

        /*METHODS*/

    }
}
