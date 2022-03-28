﻿using System.ComponentModel.DataAnnotations;

namespace ApiNominas.DTO
{
    public class WorkerDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Surname { get; set; }

        [Required]
        public int ContractID { get; set; }
    }
}
