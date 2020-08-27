using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TesteBlog.IService
{
  public interface IWebApiService<T>
  {

    /// <summary>
    /// Executa um método http Get de forma assíncrona considerando TimeOut de 5 segundos
    /// </summary>
    /// <param name="URL">URL do endereço do método</param>
    /// <returns>Retorna instância da classe especificada com o retorno do método ou nulo caso o retorno seja vazio</returns>
    Task<T> GetAsync(string URL, bool ConsideraTimeOut);

    /// <summary>
    /// Executa um método http Get de forma síncrona
    /// </summary>
    /// <param name="URL">URL do endereço do método</param>
    /// <returns>Retorna instância da classe especificada com o retorno do método ou nulo caso o retorno seja vazio</returns>
    T Get(string URL);
  }
}
