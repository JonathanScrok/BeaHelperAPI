using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BeaHelper.BLL.Models
{
    public class Evento
    {
        public int Id_Evento { get; set; }
        public int Id_Usuario_Adm { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public string Cidade_Estado { get; set; }
        public DateTime DataPublicacao { get; set; }
        public bool SemData { get; set; }
        public bool Privado { get; set; }
        public bool UsuarioLogadoVoluntariado { get; set; }
        public bool EventoRecorrente { get; set; }
        public DateTime? DataEvento { get; set; }

    }
}
