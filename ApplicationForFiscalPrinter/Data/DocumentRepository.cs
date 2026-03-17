using ApplicationForFiscalPrinter.Data;
using Dapper;
using ApplicationForFiscalPrinter.Domain;

namespace ApplicationForFiscalPrinter.Data;

public class DocumentRepository
{
    public async Task<Document?> GetByIdAsync(int documentId)
    {
        using var connection = DbConnectionFactory.Create();

        const string sql = """
            SELECT
                d.Doc_ID,
                d.Doc_FullNumber,
                d.Doc_GrossValue,
            
                p.Del_ID,
                p.Del_DocID,
                p.Del_Name,
                p.Del_VATRate,
                p.Del_GrossValue as Del_Price,
                p.Del_Amount
            FROM Documents d
            JOIN DocElems p ON d.Doc_ID = p.Del_DocID
            WHERE d.Doc_ID = @Doc_ID 
            """;

        var documentDictionary = new Dictionary<int, Document>();

        var result = await connection.QueryAsync<Document, DocumentPositions, Document>(
            sql,
            (doc, pos) =>
            {
                if (!documentDictionary.TryGetValue(doc.Doc_ID, out var document))
                {
                    document = doc;
                    document.DocumentPositions = new List<DocumentPositions>();
                    documentDictionary.Add(document.Doc_ID, document);
                }

                document.DocumentPositions.Add(pos);
                return document;
            },
            new { Doc_ID = documentId },
            splitOn: "Del_ID"
        );

        return result.FirstOrDefault();
    }

}
