using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Command
{
    public class CreatePdfTableActionCommand<T> : ITableActionCommand
    {
        private readonly PdfFile<T> _pdfFile;

        public CreatePdfTableActionCommand(PdfFile<T> pdfFile)
        {
            _pdfFile = pdfFile;
        }

        public IActionResult Execute()
        {
            var pdfMs = _pdfFile.Create();
            return new FileContentResult(pdfMs.ToArray(), _pdfFile.FileType) { FileDownloadName = _pdfFile.FileName };
        }
    }
}
