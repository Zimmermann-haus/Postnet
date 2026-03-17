using ApplicationForFiscalPrinter.Logging;
using ApplicationForFiscalPrinter.Services;
using Serilog;

File.WriteAllText(
    "STARTUP_TEST.log",
    $"START {DateTime.Now} ARGS={string.Join(",", args)}"
);

try
{
    SerilogConfigurator.Configure();

    Log.Information("ApplicationForFiscalPrinter - START");

    if (args.Length == 0 || !int.TryParse(args[0], out var documentId))
    {
        Log.Error("Nie podano poprawnego DocumentId");
        Environment.ExitCode = -1;
        return;
    }

    Log.Information("Start procesu dla DocumentId: {DocumentId}", documentId);

    var documentService = new DocumentService();
    await documentService.ProcessDocumentAsync(documentId);

    Log.Information("Proces zakończony poprawnie");
    Environment.ExitCode = 0;
}
catch (Exception ex)
{
    File.WriteAllText("FATAL_ERROR.log", ex.ToString());
    throw;
}
finally
{
    Log.CloseAndFlush();
}
