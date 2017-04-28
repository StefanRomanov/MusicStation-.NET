﻿using System;
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
        [StringLength(50)]
        public string Title { get; set; }

        public string Details { get; set; }

        public Genre Genre { get; set; }

        public string ImagePath { get; set; }

        [Required]
        [MaxLength(52428800)]
        [Display(Name ="File")]
        public string FilePath { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }
    }
}