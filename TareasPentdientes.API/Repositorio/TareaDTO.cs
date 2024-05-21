using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NB_JaimeJativa.Repositorio
{
    public class TareaDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaVencimiento { get; set; }

        [Required]
        public bool Completada { get; set; }

        public bool Estado { get; set; } = true;

    }
}