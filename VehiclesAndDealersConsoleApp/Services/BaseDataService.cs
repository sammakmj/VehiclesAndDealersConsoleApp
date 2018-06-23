using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Sammak.VnD.Services
{
    public class BaseDataService
    {
        protected HttpClient client = new HttpClient();
        const string RootUri = @"http://vautointerview.azurewebsites.net/";
        private Dictionary<string, Object> _inMemoryCache = new Dictionary<string, object>();

        public BaseDataService()
        {
            client.BaseAddress = new Uri(RootUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public async Task<T> GenericGetAsync<T>(string path) where T: class
        {
            try
            {
                T result = null;
                if (_inMemoryCache.ContainsKey(path)) {
                    Console.WriteLine($"Data from cache for: {path}");
                    result = _inMemoryCache[path] as T;
                    return result;
                }

                HttpResponseMessage response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<T>();
                    _inMemoryCache[path] = result;
                }
                else
                {
                    var errorMessage = $"Unsuccess Response for: {RootUri}{path}";
                    errorMessage += $"\nstatus code: {response.StatusCode.ToString()}";
                    throw (new Exception (errorMessage));
                }
                return result;
            }
            catch (WebException webExcp)
            {
                var errorMessage = $"WebException has been caught  for: {RootUri}{path}";
                errorMessage += $"\nwebExcp: {webExcp.ToString()}";

                WebExceptionStatus status = webExcp.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    errorMessage += $"\nThe server returned protocol error ";
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    errorMessage += $"\n{(int)httpResponse.StatusCode} - {httpResponse.StatusCode}";
                }
                throw (new Exception (errorMessage));
            }
            catch (Exception ex)
            {
                var errorMessage = $"An Exception has been caught for: {RootUri}{path}";
                var ex2 = new Exception(errorMessage, ex);
                throw ex2;
            }
        }

        public async Task<TResult> GenericPostAsync<TPayload, TResult>(string path, TPayload payload) where TPayload : class where TResult : class
        {
            TResult result = null;
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(path, payload);
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<TResult>();
                }
                else
                {
                    var errorMessage = $"Unsuccess Response for: {RootUri}{path}";
                    errorMessage += $"\nstatus code: {response.StatusCode.ToString()}";
                    throw (new Exception(errorMessage));
                }
                return result;
            }
            catch (WebException webExcp)
            {
                var errorMessage = $"WebException has been caught  for: {RootUri}{path}";
                errorMessage += $"\nwebExcp: {webExcp.ToString()}";

                WebExceptionStatus status = webExcp.Status;
                if (status == WebExceptionStatus.ProtocolError)
                {
                    errorMessage += $"\nThe server returned protocol error ";
                    HttpWebResponse httpResponse = (HttpWebResponse)webExcp.Response;
                    errorMessage += $"\n{(int)httpResponse.StatusCode} - {httpResponse.StatusCode}";
                }
                throw (new Exception(errorMessage));
            }
            //catch (Exception ex)
            //{
            //    //var errorMessage = $"An Exception has been caught for: {RootUri}{path}";
            //    //var ex2 = new Exception(errorMessage);
            //    //throw ex2;
            //}
        }
    }
}
