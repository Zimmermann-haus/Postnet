using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationForFiscalPrinter.Domain
{
    public class DocumentPositions
    {
        public int Del_ID { get; set; }
        public int Del_DocID { get; set; }
        public string Del_Name { get; set; } = string.Empty;
        public int Del_VATRate { get; set; } 
        public decimal Del_Price { get; set; }
        public int Del_Amount { get; set; }
      //  public Document? Document { get; set; }
    }
}
