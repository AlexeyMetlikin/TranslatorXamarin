using System.Collections.Generic;
using Translator.Abstract;
using System.Text.RegularExpressions;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;

namespace Translator.Model
{
    public class TranslatorApi : ITranslatorApi
    {
        public string Host { get; }

        public string API_Key { get; }

        public List<KeyValuePair<string, string>> Langs { get; }

        public TranslatorApi(string host, string key)
        {
            Host = host;
            API_Key = key;
            Langs = new List<KeyValuePair<string, string>>();
        }

        public void FillLanguages(string response)
        {
            Regex reg = new Regex("\"langs\":{");
            string languages = reg.Split(response)[1].Split('}')[0];
            foreach (var lang in languages.Split(','))
            {
                Langs.Add(new KeyValuePair<string, string>(lang.Split(':')[0].Replace("\"", ""), lang.Split(':')[1].Replace("\"", "")));
            }
        }

        public async Task<string> SendPostAync(string request, List<KeyValuePair<string, string>> pars)
        {
            if (pars != null)
            {
                FormUrlEncodedContent content = new FormUrlEncodedContent(pars);
                using (var client = new HttpClient())
                {
                    var response = await client.PostAsync(Host + request, content);
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException(response.Content.ReadAsStringAsync().Result);
                    }
                    return response.Content.ReadAsStringAsync().Result;
                }
            }

            return null;
        }
    }
}
