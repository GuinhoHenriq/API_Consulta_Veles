using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Consulta_Veles.Models
{
    public class LoginVeles
    {
        public string usuario { get; set; }
        public string senha { get; set; }
        public string cliente { get; set; }
    }
}