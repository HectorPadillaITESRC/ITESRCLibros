﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITESRCLibrosMAUI.Models.DTOs
{
    public class LibrosDTO
    {
        public int? Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Autor { get; set; } = null!;
        public string Portada { get; set; } = null!;
        public bool? Eliminado { get; set; }
    }
}
