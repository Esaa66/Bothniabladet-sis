using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bothniabladet.Data
{
    public class Image
    {
        /*VARIABLES*/
        public int ImageId { get; set; }
        public string ImageTitle { get; set; }
        public byte[] ImageData { get; set; }
        public int BasePrice { get; set; }
        public DateTime Issue { get; set; }
        public bool IsDeleted { get; set; }
        public byte[] Thumbnail { get; set; } = new byte[0];



        /*LINKS*/
        public ICollection<EditedImage> EditedImages { get; set; }
        public ImageLicense ImageLicense { get; set; }     //1-1 unidirectional(?) to the image license object
        public ImageMetaData ImageMetaData { get; set; }    //1-1 unidirectional(?) to the metadata object
        public NewsSection Section { get; set; }        //using the enum
        public ICollection<ImageKeyword> KeywordLink { get; set; }
        public ICollection<ShoppingCart> ShoppingCart { get; set; }  // Many to many relationship with User 

        /*METHODS*/

        /*Convenience variables and methods*/
        //ADD THE METHOD IN THE CONFIG
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; }

        // Add this inside the Image class
        [NotMapped]
        public string ImageDataURL
        {
            get
            {
                if (ImageData != null)
                {
                    var base64 = Convert.ToBase64String(ImageData);
                    return $"data:image/jpg;base64,{base64}";
                }
                else
                {
                    return string.Empty; // Return an empty string or any default value when ImageData is null
                }
            }
        }
    }
}
