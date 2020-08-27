using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TesteBlog.IService;

namespace TesteBlog.Service
{
  public class WebApiService<T> : IWebApiService<T>
  {

    /// <summary>
    /// Executa um método http Get de forma assíncrona
    /// </summary>
    /// <param name="URL">URL do endereço do método</param>
    /// <returns>Retorna instância da classe especificada com o retorno do método ou nulo caso o retorno seja vazio</returns>
    public virtual async Task<T> GetAsync(string URL, bool ConsideraTimeOut)
    {
      using (HttpClient httpClient = new HttpClient())
      {
        var task = await Task.Run(async () =>
        {
          using (var response = httpClient.GetAsync(URL))
          {
            if (ConsideraTimeOut && Task.WaitAny(new Task[] { response }, new TimeSpan(0, 0, 0, 5, 0)) < 0)
            {
              throw new Exception("MessageServidorIndisponivel");
            }
            else
              return await ProcessResultGetAsync(response.Result);
          }
        }).ConfigureAwait(false);

        return task;
      }
    }

    public virtual T Get(string URL)
    {

      Task<T> task = Task.Run(async () =>
      {
        return await GetAsync(URL,false);
      });
      task.Wait();
      return task.Result;
    }

    private static async Task<T> ProcessResultGetAsync(HttpResponseMessage response)
    {
      if (response.IsSuccessStatusCode)
      {
        T returnContent  = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());

        return returnContent;
      }
      else
      {
        if (response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
        {
          throw new Exception(response.ReasonPhrase);
        }
        else
        {
          return default(T);
        }
      }
    }
  }
}
