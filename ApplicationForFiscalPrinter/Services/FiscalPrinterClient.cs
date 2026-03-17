using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Serilog;
using ApplicationForFiscalPrinter.Domain;
using ApplicationForFiscalPrinter.Dtos;

namespace ApplicationForFiscalPrinter.Services;

public class FiscalPrinterClient
{
    private readonly HttpClient _httpClient;
    private readonly string _endpoint;

    public FiscalPrinterClient()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(config["FiscalPrinter:BaseUrl"]!)
        };

        _endpoint = config["FiscalPrinter:PrintEndpoint"]!;
    }

    public async Task SendAsync(FiscalReceiptDto receipt)
    {
        Log.Information("Wysyłka paragonu do drukarki fiskalnej");

        Console.WriteLine(receipt.Summary);
        foreach (var line in receipt.Lines)
        {
            Console.WriteLine(line.pr);
        }
        var response = await _httpClient.PostAsJsonAsync(_endpoint, receipt);

        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync();

            Log.Error("Błąd drukarki. Status={Status}, Body={Body}",
                response.StatusCode, body);

     

            response.EnsureSuccessStatusCode();
        }
    }
}
