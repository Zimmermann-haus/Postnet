namespace ApplicationForFiscalPrinter.Domain;

public class Document
{
    public int Doc_ID { get; set; }
    public string Doc_FullNumber { get; set; } = string.Empty;
    public decimal Doc_GrossValue { get; set; }

    public List<DocumentPositions> DocumentPositions { get; set; } = [];

}
