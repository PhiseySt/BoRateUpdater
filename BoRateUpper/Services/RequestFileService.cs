namespace BoRateUpper.Services;

internal class RequestFileService
{
    private const string RefererKeyword = "Referer:";
    private const string CookiesKeyword = "'Cookie: ";
    private const string BodyKeyword = "data-raw";
    private const string EndKeyword = "' \\";
    private const string FileName = "request.txt";

    public string Referer { get; }
    public string Cookies { get; }
    public string Body { get; }

    public RequestFileService()
    {
        GetValuesFromFile(out var referer, out var cookies, out var body);

        Referer = referer;
        Cookies = cookies;
        Body = body;
    }

    private void GetValuesFromFile(out string referer, out string cookies, out string body)
    {
        referer = default;
        cookies = default;
        body = default;

        using var sr = File.OpenText(FileName);
        string? currentLine;
        while ((currentLine = sr.ReadLine()) != null)
        {
            if (currentLine.Length > 0)
            {
                if (currentLine.Contains(RefererKeyword))
                {
                    var positionRefererKeyword = currentLine.IndexOf(RefererKeyword, StringComparison.CurrentCulture);
                    var positionEndKeyWord = currentLine.IndexOf(EndKeyword, StringComparison.CurrentCulture);
                    referer = currentLine.Substring(positionRefererKeyword + RefererKeyword.Length, positionEndKeyWord - positionRefererKeyword - RefererKeyword.Length).Trim();
                }

                if (currentLine.Contains(CookiesKeyword))
                {
                    var positionCookiesKeyword = currentLine.IndexOf(CookiesKeyword, StringComparison.CurrentCulture);
                    var positionEndKeyWord = currentLine.IndexOf(EndKeyword, StringComparison.CurrentCulture);
                    cookies = currentLine.Substring(positionCookiesKeyword + CookiesKeyword.Length, positionEndKeyWord - positionCookiesKeyword - CookiesKeyword.Length).Trim();
                }

                if (currentLine.Contains(BodyKeyword))
                {
                    var positionBodyKeyword = currentLine.IndexOf(BodyKeyword, StringComparison.CurrentCulture);
                    var positionEndKeyWord = currentLine.IndexOf(EndKeyword, StringComparison.CurrentCulture);
                    body = currentLine.Substring(positionBodyKeyword + BodyKeyword.Length + 1, positionEndKeyWord - positionBodyKeyword - BodyKeyword.Length).Replace("'","").Trim();
                }
            }
        }
    }
}
