using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationForFiscalPrinter.Dtos
{
    public class FiscalReceiptLineDto
    {
        public string na { get; set; } = string.Empty; 
        public decimal il { get; set; }  
        public int vt { get; set; }   
        public int pr { get; set; }   
    }
}
