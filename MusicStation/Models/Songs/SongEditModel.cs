using MusicStation.Data;
using MusicStation.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStation.Models.Songs
{
    public class SongEditModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Change Artist")]
        public string Artist { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Change Title")]
        public string Title { get; set; }

        [Display(Name = "Change Details")]
        public string Details { get; set; }

        [Display(Name = "Change Genre")]
        public Genre Genre { get; set; }

        public string ImagePath { get; set; }

        public string UserId { get; set; }
    }
}