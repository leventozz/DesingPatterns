using Microsoft.AspNetCore.Mvc;

namespace WebApp.Command.Command
{
    public class CreateExcelTableActionCommand<T> : ITableActionCommand
    {
        private readonly ExcelFile<T> _excelFile;

        public CreateExcelTableActionCommand(ExcelFile<T> excelFile)
        {
            _excelFile = excelFile;
        }

        public IActionResult Execute()
        {
            var excelMs = _excelFile.Create();
            return new FileContentResult(excelMs.ToArray(), _excelFile.FileType) { FileDownloadName = _excelFile.FileName };
        }
    }
}
