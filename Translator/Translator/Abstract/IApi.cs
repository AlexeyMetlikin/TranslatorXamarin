
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Translator.Abstract
{
    public interface IApi
    {
        // Наименование сервера
        string Host { get; }

        // Функция для отправки POST-запроса на сервер
        Task<string> SendPostAync(string request, List<KeyValuePair<string, string>> pars);
    }
}
