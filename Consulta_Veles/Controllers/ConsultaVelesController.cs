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
using Consulta_Veles.Models;

namespace Consulta_Veles.Controllers
{
    public class ConsultaVelesController : ApiController
    {

        #region Metodo Gera Token
        /*
         *      Autor:  Guilherme Henrique - 24/02/2021
         *      Obs.: Metodo que gera token para uso das demais
         *            solicitações na API da VELES
         */
        
        public string RetornaToken()
        {
            Models.LoginVeles objToken;

            objToken = new Models.LoginVeles()
            {
                usuario = "cliente_20_r",
                senha = "nr9d9CEUfv",
                cliente = "TMKT"

            };
            string ObjLogin = JsonConvert.SerializeObject(objToken);

            var LoginPost = new RestClient(ConfigurationManager.AppSettings.Get("GeraToken").ToString());
            var requestLoginPost = new RestRequest(Method.POST);
            requestLoginPost.AddHeader("Content-Type", "application/json");
            requestLoginPost.AddJsonBody(ObjLogin);
            var responseLogin = LoginPost.Post(requestLoginPost);

            return responseLogin.Content.ToString();
        }
        #endregion

        #region Metodo Localizacpfv1
        /*
         *      Autor:  Guilherme Henrique - 24/02/2021 
         *      Obs.:   Metodo que chama a url via POST (https://api.veles.com.br/api/veles/localizacpfv1)
         *              e efetua a consulta de acordo com o CPF informado
         *              Utiliza token no formato: Bearer
         *              Retorno em string separado por "|" cada atributo do objeto
         *              e o fim com um ";"
         */

        [HttpGet]
        public HttpResponseMessage Localizacpfv1(string cpf)
        {
            Models.RetLogin.RetornoLogin objRetLogin = new RetLogin.RetornoLogin();
            Models.ConsultaCPF ObjConsCPF = new ConsultaCPF();
            Models.RetConsultaCPF ObjRetCPF = new RetConsultaCPF();

            try
            {
                string retLogin = RetornaToken().ToString();
                var retornoLogin = JsonConvert.DeserializeObject<RetLogin.RetornoLogin>(retLogin.ToString());

                objRetLogin.token = retornoLogin.token;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao Tentar gerar o Tolken de Autorização");
            }

            ObjConsCPF.documento = cpf;

            JsonConvert.SerializeObject(ObjConsCPF);

            try
            {
                var Localizacpfv1Post = new RestClient(ConfigurationManager.AppSettings.Get("LocalizaCPF").ToString());
                var requestLocalizacpfv1 = new RestRequest(Method.POST);
                requestLocalizacpfv1.AddHeader("Content-Type", "application/json");
                requestLocalizacpfv1.AddHeader("Authorization", string.Format("Bearer {0}", objRetLogin.token));
                requestLocalizacpfv1.AddJsonBody(ObjConsCPF);
                var responseLocalizacpfv1 = Localizacpfv1Post.Post(requestLocalizacpfv1);

                var retConsultaCPF = JsonConvert.DeserializeObject<RetConsultaCPF.RetConsulta>(responseLocalizacpfv1.Content.ToString());

                return Request.CreateResponse(HttpStatusCode.OK,retConsultaCPF.documento + "|" + retConsultaCPF.nome + "|" + retConsultaCPF.datanascimento + "|" + retConsultaCPF.nomemae);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Erro no retorno da Solicitação");
            }
        }
        #endregion

        #region Metodo LocalizaNomev1
        /*
         *      Autor:  Guilherme Henrique - 24/02/2021 
         *      Obs.:   Metodo que chama a url via POST (https://api.veles.com.br/api/twuxecefql/localizanomev1)
         *              e efetua a consulta de acordo com o Nome informado
         *              Utiliza token no formato: Bearer
         *              Retorno em string separado por "|" cada atributo do objeto
         *              e o fim com um ";"
         */
        [HttpGet]
        public HttpResponseMessage LocalizaNomev1(string nome)
        {
            Models.RetLogin.RetornoLogin objRetLogin = new RetLogin.RetornoLogin();
            Models.ConsultaNome objConsNome = new ConsultaNome();

            try
            {
                string retLogin = RetornaToken().ToString();
                var retornoLogin = JsonConvert.DeserializeObject<RetLogin.RetornoLogin>(retLogin.ToString());

                objRetLogin.token = retornoLogin.token;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao Tentar gerar o Tolken de Autorização");
            }

            objConsNome.nome = nome;

            JsonConvert.SerializeObject(objConsNome);
            try
            {

                var LocalizaNome = new RestClient(ConfigurationManager.AppSettings.Get("LocalizaNome").ToString());
                var requestLocalizaNome = new RestRequest(Method.POST);
                requestLocalizaNome.AddHeader("Content-Type", "application/json");
                requestLocalizaNome.AddHeader("Authorization", string.Format("Bearer {0}", objRetLogin.token));
                requestLocalizaNome.AddJsonBody(objConsNome);
                var responseLocalizaNome = LocalizaNome.Post(requestLocalizaNome);

                var retLocalizaNome = JsonConvert.DeserializeObject<Models.RetTwuxecefql.NomeDtNasc>(responseLocalizaNome.Content.ToString());

                string StringCont = string.Empty;
                for (int i = 0; i < retLocalizaNome.pessoas.Count; i++)
                {
                    StringCont += retLocalizaNome.pessoas[i].cpf + "|";
                    StringCont += retLocalizaNome.pessoas[i].nome + "|";
                    StringCont += retLocalizaNome.pessoas[i].datanascimento + "|";
                    StringCont += retLocalizaNome.pessoas[i].nomemae + ";";
                }

                return Request.CreateResponse(HttpStatusCode.OK, StringCont);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Erro no retorno da Solicitação");
            }



        }
        #endregion

        #region Metodo LocalizaNomeDtNasc
        /*
         *      Autor:  Guilherme Henrique - 24/02/2021 
         *      Obs.:   Metodo que chama a url via POST (https://api.veles.com.br/api/twuxecefql/localizanomedatanascimentov1)
         *              e efetua a consulta de acordo com o Nome e data de nascimento informado
         *              Utiliza token no formato: Bearer
         *              Retorno em string separado por "|" cada atributo do objeto
         *              e o fim com um ";"
         */
        [HttpGet]
        public HttpResponseMessage LocalizaNomeDtNasc(string nome, string dtNasc)
        {
            Models.RetLogin.RetornoLogin objRetLogin = new RetLogin.RetornoLogin();
            Models.ConsultaNomeDtNasc objNomeDtNasc = new ConsultaNomeDtNasc();

            try
            {
                string retLogin = RetornaToken().ToString();
                var retornoLogin = JsonConvert.DeserializeObject<RetLogin.RetornoLogin>(retLogin.ToString());

                objRetLogin.token = retornoLogin.token;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao Tentar gerar o Tolken de Autorização");
            }

            objNomeDtNasc.nome = nome;
            objNomeDtNasc.datanascimento = dtNasc;

            JsonConvert.SerializeObject(objNomeDtNasc);

            try
            {

                var LocalizaNomeDtNascPost = new RestClient(ConfigurationManager.AppSettings.Get("LocalizaNomeDtNasc").ToString());
                var requestLocalizaNomeDtNasc = new RestRequest(Method.POST);
                requestLocalizaNomeDtNasc.AddHeader("Content-Type", "application/json");
                requestLocalizaNomeDtNasc.AddHeader("Authorization", string.Format("Bearer {0}", objRetLogin.token));
                requestLocalizaNomeDtNasc.AddJsonBody(objNomeDtNasc);
                var responseLocalizaNomeDtNasc = LocalizaNomeDtNascPost.Post(requestLocalizaNomeDtNasc);

                var retLocalizaNomeDtNasc = JsonConvert.DeserializeObject<Models.RetTwuxecefql.NomeDtNasc>(responseLocalizaNomeDtNasc.Content.ToString());

                string StringCont = string.Empty;
                for (int i = 0; i < retLocalizaNomeDtNasc.pessoas.Count; i++)
                {
                    StringCont += retLocalizaNomeDtNasc.pessoas[i].cpf + "|";
                    StringCont += retLocalizaNomeDtNasc.pessoas[i].nome + "|";
                    StringCont += retLocalizaNomeDtNasc.pessoas[i].datanascimento + "|";
                    StringCont += retLocalizaNomeDtNasc.pessoas[i].nomemae + ";";
                }

                return Request.CreateResponse(HttpStatusCode.OK, StringCont);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Erro no retorno da Solicitação");
            }
        }
        #endregion

        #region Metodo LocalizaNomeNomeMae
        /*
         *      Autor:  Guilherme Henrique - 24/02/2021 
         *      Obs.:   Metodo que chama a url via POST (https://api.veles.com.br/api/twuxecefql/localizanomenomemaev1)
         *              e efetua a consulta de acordo com o Nome e nome da mae informado
         *              Utiliza token no formato: Bearer
         *              Retorno em string separado por "|" cada atributo do objeto
         *              e o fim com um ";"
         */
        [HttpGet]
        public HttpResponseMessage LocalizaNomeNomeMae(string nome, string nomeMae)
        {
            Models.RetLogin.RetornoLogin objRetLogin = new RetLogin.RetornoLogin();
            Models.ConsultaNomeNomeMae objNomeNomeMae = new ConsultaNomeNomeMae();

            try
            {
                string retLogin = RetornaToken().ToString();
                var retornoLogin = JsonConvert.DeserializeObject<RetLogin.RetornoLogin>(retLogin.ToString());

                objRetLogin.token = retornoLogin.token;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Erro ao Tentar gerar o Tolken de Autorização");
            }

            objNomeNomeMae.nome = nome;
            objNomeNomeMae.nomemae = nomeMae;

            JsonConvert.SerializeObject(objNomeNomeMae);

            try
            {

                var LocalizaNomeNomeMaePost = new RestClient(ConfigurationManager.AppSettings.Get("LocalizaNomeNomeMae").ToString());
                var requestLocalizaNomeNomeMae = new RestRequest(Method.POST);
                requestLocalizaNomeNomeMae.AddHeader("Content-Type", "application/json");
                requestLocalizaNomeNomeMae.AddHeader("Authorization", string.Format("Bearer {0}", objRetLogin.token));
                requestLocalizaNomeNomeMae.AddJsonBody(objNomeNomeMae);
                var responseLocalizaNomeDtNasc = LocalizaNomeNomeMaePost.Post(requestLocalizaNomeNomeMae);

                var retLocalizaNomeNomeMae = JsonConvert.DeserializeObject<Models.RetTwuxecefql.NomeDtNasc>(responseLocalizaNomeDtNasc.Content.ToString());

                string StringCont = string.Empty;
                for (int i = 0; i < retLocalizaNomeNomeMae.pessoas.Count; i++)
                {
                    StringCont += retLocalizaNomeNomeMae.pessoas[i].cpf + "|";
                    StringCont += retLocalizaNomeNomeMae.pessoas[i].nome + "|";
                    StringCont += retLocalizaNomeNomeMae.pessoas[i].datanascimento + "|";
                    StringCont += retLocalizaNomeNomeMae.pessoas[i].nomemae + ";";
                }

                return Request.CreateResponse(HttpStatusCode.OK, StringCont);
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.OK, "Erro no retorno da Solicitação");
            }

        }
        #endregion
    }
}
