using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Consulta_Veles.Models
{
    public class RetConsultaCPF
    {
        public class Endereco
        {
            public int id { get; set; }
            public string tipo { get; set; }
            public object titulo { get; set; }
            public string logradouro { get; set; }
            public string numero { get; set; }
            public string complemento { get; set; }
            public string bairro { get; set; }
            public string cidade { get; set; }
            public string uf { get; set; }
            public string cep { get; set; }
        }

        public class Telefone
        {
            public int id { get; set; }
            public string ddd { get; set; }
            public string telefone { get; set; }
            public string prefixo { get; set; }
            public string sufixo { get; set; }
            public bool procon { get; set; }
        }

        public class Email
        {
            public int id { get; set; }
            public string email { get; set; }
            public string nomeemail { get; set; }
            public string provedoremail { get; set; }
            public string dominioemail { get; set; }
        }

        public class RetConsulta
        {
            public int statusid { get; set; }
            public string statusmensagem { get; set; }
            public string documento { get; set; }
            public string nome { get; set; }
            public string nomemae { get; set; }
            public object nomepai { get; set; }
            public string datanascimento { get; set; }
            public int idade { get; set; }
            public string sexo { get; set; }
            public List<Endereco> enderecos { get; set; }
            public List<Telefone> telefones { get; set; }
            public List<Email> emails { get; set; }
        }
    }
}