using ApplicationForFiscalPrinter.Domain;
using ApplicationForFiscalPrinter.Dtos;

namespace ApplicationForFiscalPrinter.Mapping;

public static class FiscalReceiptMapper
{
    private const int MaxFiscalLineLength = 55;

    public static FiscalReceiptDto Map(Document document)
    {
        if (document.DocumentPositions.Count == 0)
            throw new Exception("Dokument nie zawiera pozycji");

        var lines = document.DocumentPositions.Select(p =>
        {
            if (p.Del_Amount <= 0)
                throw new Exception("Ilość musi być większa od zera");


            var unitPrice = p.Del_Price / p.Del_Amount;

            var unitPriceGrosze = (int)Math.Round(
                unitPrice * 100,
                0,
                MidpointRounding.AwayFromZero
            );

            return new FiscalReceiptLineDto
            {
                na = CutFiscalLine(p.Del_Name),
                il = p.Del_Amount,
                vt = MapVat(p.Del_VATRate),
                pr = unitPriceGrosze
            };
        }).ToList();

        var totalGrosze = lines.Sum(l =>
            (int)Math.Round(
                l.pr * l.il,
                0,
                MidpointRounding.AwayFromZero
            )
        );

        return new FiscalReceiptDto
        {
            Lines = lines,
            Summary = new FiscalReceiptSummaryDto
            {
                to = totalGrosze
            }
        };
    }

    private static string CutFiscalLine(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return string.Empty;

        var index = value.IndexOf("//", StringComparison.Ordinal);
        if (index >= 0)
            value = value.Substring(0, index);

        value = value.Trim();
        value = System.Text.RegularExpressions.Regex.Replace(value, @"\s+", " ");

        return value.Length <= MaxFiscalLineLength
            ? value
            : value.Substring(0, MaxFiscalLineLength);
    }
    private static int MapVat(int vatRate) =>
        vatRate switch
        {
            23 => 0,
            8 => 1,
            5 => 2,
            0 => 3,
            _ => throw new Exception($"Nieobsługiwana stawka VAT: {vatRate}")
        };
}
