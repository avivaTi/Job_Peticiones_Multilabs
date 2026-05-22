using Aviva.Multilabs.Peticiones.Demonio.Data;
using Aviva.Multilabs.Peticiones.Demonio.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("Arrancando Demonio Multilabs");
            return base.StartAsync(cancellation);
        }
        public override Task StopAsync(CancellationToken cancellation)
        {
            _logger.LogInformation("Parando Demonio Multilabs");
            return base.StopAsync(cancellation);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                //await Task.Delay(1000, stoppingToken);

                _logger.LogInformation("El demonio Multilabs sigue ejecutandose correctamente");
                await procesarPeticiones();
                await Task.Delay(60000, stoppingToken);
            }
        }

        private async Task procesarPeticiones()
        {
            DataMultilabs data = new DataMultilabs();
            List<Lista_Peticiones_Multilabs> listaPeticiones = new List<Lista_Peticiones_Multilabs>();
            //var jsonPresupuesto = (dynamic)null;
            listaPeticiones = data.listaPeticionesMultilabs();
            int cantidad = listaPeticiones.Count();
            _logger.LogInformation($" Cantidad de registros { cantidad } ");
            int contador = 1;

            foreach (var registro in listaPeticiones)
            {
                int cantidadRegistros = obtenerDatosMultilabs(registro.Fecha, registro.DNI);

                if (cantidadRegistros == 1)
                {
                    //Registrar en tabla de UNILABS
                    _logger.LogInformation($" {contador} : En multilabs para orden { registro.DNI } fecha { registro.Fecha } centroId {registro.SedeId} ");
                    data.grabarPeticionMultilabs(registro,"R",2);
                }
                else
                {
                    _logger.LogInformation($" {contador} : No está en multilabs para orden { registro.DNI } fecha { registro.Fecha } centroId {registro.SedeId} ");
                    data.grabarPeticionMultilabs(registro, "P", 1);
                }

                contador++;
            }
        }

        public int obtenerDatosMultilabs(string fechaAtencion, string documento)
        {
            try
            {
                //Lista_Peticiones_Multilabs item = new Lista_Peticiones_Multilabs();
                //DataMultilabs data = new DataMultilabs();
                int cantidadRegistros = 0;

                string urlBase = "https://api.multilab.com.pe/tp/";
                string endpointDetalle = "summary";
                string uri = $"{urlBase}{endpointDetalle}";

                string token = getAccessToken();

                var body = new
                {
                    from = DateTime.Now.ToString("yyyy-MM-dd"),
                    to = DateTime.Now.ToString("yyyy-MM-dd")
                };

                string json = JsonConvert.SerializeObject(body);


                _logger.LogInformation($" JsonCreate para obtener SUMMARY Multilabs");
                HttpClient client = new HttpClient();

                HttpContent contentCreate = new StringContent(json, Encoding.UTF8, "application/json");

                HttpRequestMessage requestCreate = new HttpRequestMessage(HttpMethod.Post, uri);
                requestCreate.Headers.Add("Authorization", "Bearer " + token);
                requestCreate.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                requestCreate.Content = contentCreate;

                HttpResponseMessage response = client.SendAsync(requestCreate).Result;
                string res = response.Content.ReadAsStringAsync().Result;

                DateTime fechaConvertida = DateTime.ParseExact(fechaAtencion,  "dd/MM/yyyy",  CultureInfo.InvariantCulture  );

                string fechaFormateada = fechaConvertida.ToString("yyyy-MM-dd");

                Root data2 = JsonConvert.DeserializeObject<Root>(res);

                var resultado = data2.atenciones.FirstOrDefault(x => (x.fecha_atencion ?? "").Trim() == fechaFormateada.Trim() && (x.doc_identidad ?? "").Trim() == documento.Trim() );

                if (resultado == null)
                {
                    cantidadRegistros = 0;
                }
                else
                {
                    cantidadRegistros = 1;
                }

                return cantidadRegistros;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public string getAccessToken()
        {

            //string username = _configuration.GetSection("LoggingSalesForce").GetValue<string>("username");
            string url = "https://api.multilab.com.pe/tp/auth/token";

            string id = "LZH729pTrFQpx78TfCgYXjA46";
            string secret = "dNJi7Fut6TcU9nQaJ4ASQ2uAdgCcKCR9JUyYi2ke3wt63";


            HttpClient client = new HttpClient();
            Dictionary<string, string> data = new Dictionary<string, string>
            {
                { "id", id },
                { "secret", secret }
            };

            var jsonResponse = default(string);
            HttpContent content = new FormUrlEncodedContent(data);
            var response = client.PostAsync(url, content).Result;
            jsonResponse = response.Content.ReadAsStringAsync().Result;
            string token = "";

            if (response.IsSuccessStatusCode)
            {
                HttpContent cont = response.Content;
                string jsonContent = cont.ReadAsStringAsync().Result;

                dynamic jsonObj = JsonConvert.DeserializeObject(jsonContent);

                token = (string)jsonObj["token"];

                _logger.LogInformation($" Mi acces_token Multilabs ----->  {token}");

            }
            else
            {
                _logger.LogInformation($" Error en acces_token ----->  {response.ReasonPhrase}");
            }


            return token;

        }

        
    }
}
