using MusicStation.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStation.Models.Songs
{
    public class SongAddView
    {
        [Required]
        [StringLength(50)]
        public string Artist { get; set; }

        [Required]
        public string Title { get; set; }
        
        public string Lyrics { get; set; }

        public Genre Genre { get; set; }

        [Required]
        public string FilePath { get; set; }
    }
}