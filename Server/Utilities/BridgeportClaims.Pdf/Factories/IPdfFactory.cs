using System;
using System.Collections.Generic;
using System.Data;

namespace BridgeportClaims.Pdf.Factories
{
    public interface IPdfFactory
    {
        bool MergePdf(IEnumerable<Uri> fileNames, string targetPdf);
        string GeneratePdf(DataTable dt);
    }
}