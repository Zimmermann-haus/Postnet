using ApplicationForFiscalPrinter.Data;
using ApplicationForFiscalPrinter.Mapping;
using Serilog;

namespace ApplicationForFiscalPrinter.Services;

public class DocumentService
{
    private readonly DocumentRepository _repository = new();
    private readonly FiscalPrinterClient _printerClient = new();

    public async Task ProcessDocumentAsync(int documentId)
    {
        Log.Information("Pobieranie dokumentu z bazy");

        var document = await _repository.GetByIdAsync(documentId);

        if (document == null)
        {
            Log.Warning("Nie znaleziono dokumentu {DocumentId}", documentId);
            throw new Exception("Dokument nie istnieje");
        }

        Log.Information("Mapowanie dokumentu na paragon fiskalny");

        var receipt = FiscalReceiptMapper.Map(document);

        Log.Information("Wysyłanie paragonu do drukarki fiskalnej");

        await _printerClient.SendAsync(receipt);
    }
}
