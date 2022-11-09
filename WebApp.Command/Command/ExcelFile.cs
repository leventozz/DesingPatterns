using ClosedXML.Excel;
using System.Data;

namespace WebApp.Command.Command
{
    public class ExcelFile<T>
    {
        public readonly List<T> _list;
        public string FileName => $"{typeof(T).Name}.xlsx";
        public string FileType => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        public ExcelFile(List<T> list)
        {
            _list = list;
        }

        public MemoryStream Create()
        {
            var wb = new XLWorkbook();
            var ds = new DataSet();

            ds.Tables.Add(GetTable());
            wb.Worksheets.Add(ds);

            var excelMemory = new MemoryStream();
            wb.SaveAs(excelMemory);

            return excelMemory;
        }

        private DataTable GetTable()
        {
            DataTable dt = new DataTable();
            var type = typeof(T);
            type.GetProperties().ToList().ForEach(p => dt.Columns.Add(p.Name, p.PropertyType));

            _list.ForEach(x =>
            {
                var values = type.GetProperties().Select(p => p.GetValue(x,null)).ToArray();
                dt.Rows.Add(values);
            });
            return dt;
        }
    }
}
