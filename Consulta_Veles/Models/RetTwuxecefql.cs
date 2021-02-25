using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consulta_Veles.Models
{
    public class RetTwuxecefql
    {
        public class Pessoa
        {
            public int id { get; set; }
            public string cpf { get; set; }
            public string nome { get; set; }
            public string nomemae { get; set; }
            public string datanascimento { get; set; }
            public int idade { get; set; }
        }

        public class NomeDtNasc
        {
            public int statusid { get; set; }
            public string statusmensagem { get; set; }
            public string nome { get; set; }
            public string datanascimento { get; set; }
            public List<Pessoa> pessoas { get; set; }
        }
    }
}