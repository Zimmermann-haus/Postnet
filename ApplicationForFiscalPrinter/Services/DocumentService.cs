using ApplicationForFiscalPrinter.Data;
using ApplicationForFiscalPrinter.Mapping;
using Serilog;
using System.Linq;

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

        if (document.Doc_Code != "Faktura sprzedaży" && document.Doc_Code != "Faktura sprzedaży zaliczkowa")
        {
            Log.Warning("Dokument niepodlegający fiskalizacji {DocumentId}", documentId);
            throw new Exception("Dokument niepodlega fiskalizacji");
        }

        if (document.Doc_IntColumn5 == 2652)
        {
            Log.Warning("Dokument zafiskalizowany {DocumentId}", documentId);
            throw new Exception("Dokument jest już zafiskalizowany");
        }

            Log.Information("Mapowanie dokumentu na paragon fiskalny");

        if (document.Doc_IntColumn28 == 2670 &&
            string.Equals(document.Doc_Code, "Faktura sprzedaży zaliczkowa", StringComparison.OrdinalIgnoreCase))
        {
            if (document.Doc_LeadingDocID != 0)
            {
                var related = await _repository.GetRelatedPositionsByLeadingIdAsync(document.Doc_LeadingDocID, document.Doc_ID);

                foreach (var pos in document.DocumentPositions)
                {
                    var advanceSum = related
                        .Where(r => r.Del_ItmID == pos.Del_ItmID && r.Del_ErpSeqNo == pos.Del_ErpSeqNo)
                        .Sum(r => r.Del_Price);

                    pos.Del_Price -= advanceSum;
                    if (pos.Del_Price < 0) pos.Del_Price = 0;
                }
            }
        }

        var receipt = FiscalReceiptMapper.Map(document);

        Log.Information("Generowanie paragonu fiskalnego (wysyłka do drukarki wyłączona)");


        await _printerClient.SendAsync(receipt);
    }
}
