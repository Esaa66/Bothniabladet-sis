using System;
using System.Collections.Generic;

namespace Bothniabladet.Data
{
    //this class represents the many-to-many relationship between Image and Keyword
    public class ImageKeyword
    {
        public int ImageId { get; set; }
        public int KeywordId { get; set; }

        public Image Image { get; set; }
        public Keyword Keyword {get; set;}
    }
}
