using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consulta_Veles.Models
{
    public class RetLogin
    { 
        public class RetornoLogin
        {
            public bool autenticado { get; set; }
            public string mensagem { get; set; }
            public string criadoem { get; set; }
            public string expiraem { get; set; }
            public string token { get; set; }
            public string tokentype { get; set; }
        }
    }
}