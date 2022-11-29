using System.Text;
using RestSharp;

namespace BoRateUpper.Services;

internal class RateUpperService
{
    private static readonly Random Rnd = new();
    private static int _currentSuffix = 10;
    internal void RateUp (string? referer , string? cookieInfo, string? bodyRequest)
    {
        if (bodyRequest is { Length: > 1 } && bodyRequest.Contains("rating"))
        {
            var indexRating = bodyRequest.IndexOf("rating", StringComparison.CurrentCulture) - 3;
            var body = bodyRequest;
            var sb = new StringBuilder(body);
            sb.Remove(indexRating, 2);
            sb.Insert(indexRating, GenerateSuffix());
            bodyRequest = sb.ToString();
        }

        var client = new RestClient("https://www.business-gazeta.ru/comments/rate")
        {
            Timeout = -1
        };
        var request = new RestRequest(Method.POST);
        request.AddHeader("Accept", "application/json, text/javascript, */*; q=0.01");
        request.AddHeader("Accept-Language", "ru-RU,ru;q=0.9,en-US;q=0.8,en;q=0.7");
        request.AddHeader("Connection", "keep-alive");
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
        if (cookieInfo != null) request.AddHeader("Cookie", cookieInfo);
        request.AddHeader("Origin", "https://www.business-gazeta.ru");
        if (referer != null) request.AddHeader("Referer", referer);
        request.AddHeader("Sec-Fetch-Dest", "empty");
        request.AddHeader("Sec-Fetch-Mode", "cors");
        request.AddHeader("Sec-Fetch-Site", "same-origin");
        client.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/107.0.0.0 Safari/537.36";
        request.AddHeader("X-Requested-With", "XMLHttpRequest");
        request.AddHeader("sec-ch-ua", "\"Google Chrome\";v=\"107\", \"Chromium\";v=\"107\", \"Not=A?Brand\";v=\"24\"");
        request.AddHeader("sec-ch-ua-mobile", "?0");
        request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
        if (bodyRequest != null)
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", bodyRequest,
                ParameterType.RequestBody);
        var response = client.Execute(request);
        Console.WriteLine(response.Content);
    }

    internal int GenerateDelay(int min, int max) => Rnd.Next(min, max);

    internal string GenerateSuffix() 
    {
        ++_currentSuffix;
        return _currentSuffix.ToString();
    }
}