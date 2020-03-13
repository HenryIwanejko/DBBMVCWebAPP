using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DBBMVCWebApp.Models
{
    public class Game
    {
        public int GameID { get; set; }

        public string OwnerID { get; set; }

        [Required]
        [Display(Name = "Name of Game")]
        [MinLength(2), MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MinLength(2), MaxLength(300)]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Price")]
        [Range(0, 10000)]
        public decimal Price { get; set; }

        [Required]
        [FileExtensions]
        [Display(Name = "Image of the Game")]
        public byte[] GameImage { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid number")]
        public int Quantity { get; set; }
    }
}