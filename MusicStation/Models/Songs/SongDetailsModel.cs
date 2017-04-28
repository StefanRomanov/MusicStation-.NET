using MusicStation.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStation.Models.Songs
{
    public class SongDetailsModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Artist { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        public string Details { get; set; }

        public Genre Genre { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [Display(Name = "File")]
        public string FilePath { get; set; }

        public User User { get; set; }
    }
}