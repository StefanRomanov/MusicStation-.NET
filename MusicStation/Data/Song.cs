using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MusicStation.Data
{
    public class Song
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Artist { get; set; }

        [Required]
        public string Title { get; set; }
        

        public string Lyrics { get; set; }

        public Genre Genre { get; set; }

        [Required]
        [Display(Name ="File")]
        public string FilePath { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}