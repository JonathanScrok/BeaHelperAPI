using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models {
    public class Usuario {
        public int Id_Usuario { get; set; }
        public string Nome { get; set; }
        public int? Sexo { get; set; }
        public string Email { get; set; }
        public string NumeroCelular { get; set; }
        public bool JaConvidado { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
