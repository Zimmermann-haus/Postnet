namespace ApplicationForFiscalPrinter.Domain;

public class Document
{
    public int Doc_ID { get; set; }
    public string Doc_FullNumber { get; set; } = string.Empty;
    public string Doc_Code { get; set; } = string.Empty;
    public int Doc_IntColumn28 { get; set; }
    public int Doc_IntColumn5 { get; set; }
    public int Doc_LeadingDocID { get; set; }
    public decimal Doc_GrossValue { get; set; }

    public List<DocumentPositions> DocumentPositions { get; set; } = new List<DocumentPositions>();

}
