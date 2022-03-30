using MvcApiTicketsDpr.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Services
{
    public class ServiceApiTickets
    {
        private String UrlApi;
        private MediaTypeWithQualityHeaderValue Header;

        public ServiceApiTickets(String UrlApi)
        {
            this.UrlApi = UrlApi;
            this.Header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        public async Task<String> GetToken(String Username, String Password)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);

                LoginModel model = new LoginModel();
                model.Username = Username;
                model.Password = Password;

                String json = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                String request = "/api/auth/login";
                HttpResponseMessage response = await client.PostAsync(request, content);

                if (response.IsSuccessStatusCode)
                {
                    String data = await response.Content.ReadAsStringAsync();
                    JObject jobject = JObject.Parse(data);
                    String token = jobject.GetValue("response").ToString();

                    return token;

                }
                else
                {
                    return null;
                }

            }
        }

        private async Task<T> CallApiAsync<T>(String request)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                HttpResponseMessage response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;

                }
                else
                {
                    return default(T);
                }

            }
        }

        private async Task<T> CallApiAsync<T>(String request, String token)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);
                HttpResponseMessage response = await client.GetAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }

        public async Task<List<Ticket>> GetTicketsAsync(String token, String idUsuario)
        {
            String request = "/TicketsUsuario/" + idUsuario;
            List<Ticket> tickets = await this.CallApiAsync<List<Ticket>>(request, token);

            return tickets;
        }

        public async Task<UsuarioTicket> GetUserId(String token)
        {
            String request = "/GetUser/";
            UsuarioTicket u = await this.CallApiAsync<UsuarioTicket>(request, token);

            return u;
        }

        public async Task UploadBlobTicket(String date, String importe, String name, String url, String idUsuario, String producto, String token)
        {
            Ticket t = new Ticket();
            t.IdTicket = 200;
            t.Fecha = DateTime.Parse(date);
            t.Filename = name;
            t.IdUsuario = int.Parse(idUsuario);
            t.Importe = importe;
            t.Producto = producto;
            t.StoragePath = url;


            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                client.DefaultRequestHeaders.Add("Authorization", "bearer " + token);



                String json = JsonConvert.SerializeObject(t);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                String request = "/CreateTicket";
                HttpResponseMessage response = await client.PostAsync(request, content);


            }
        }






    }
}
