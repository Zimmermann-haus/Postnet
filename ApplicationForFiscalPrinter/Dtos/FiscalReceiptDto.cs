using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationForFiscalPrinter.Dtos
{
    public class FiscalReceiptDto
    {
        public List<FiscalReceiptLineDto> Lines { get; set; } = [];
        public FiscalReceiptSummaryDto Summary { get; set; } = new();
    }
}

