using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SyrusVoluntariado.Library.Mail {
    public class Constants {

        // POP3, IMAP - Ler mensagens de e-mail
        // SMTP - Enviar e-mail

        //Autenticação - Gmail
        public readonly static string Usuario = "Beahelperteste@gmail.com";
        public readonly static string Senha = "BeHelper@202Um";

        //Servidor SMTP
        public readonly static string ServidorSMTP = "smtp.gmail.com";
        public readonly static int PortaSMTP = 587;

    }
}
