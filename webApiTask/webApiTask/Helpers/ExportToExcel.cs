using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using Models;
using Models.DTO;
using System.IO;

namespace webApiTask.Helpers
{
    public class ExportToExcel
    {
        public void WriteToExcel(List<ListTagDTO> myList)
        {
            var wb = new XLWorkbook();
            var ws = wb.Worksheets.Add("Sample Sheet");


            var lastColIndexer = 1;
            foreach (var list in myList)
            {

                var items = list.Items.ToList();
                var tags = string.Join(",", list.Tags.Select(n => n.Name));


                ws.Cell(1, lastColIndexer).Value = list.Name;

                ws.Range(1, lastColIndexer, 1, lastColIndexer + 1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


                ws.Cell(2, lastColIndexer).Value = tags;
                ws.Range(2, lastColIndexer, 2, lastColIndexer + 1).Merge().Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                var rowIndexer = 3;

                foreach (var t in items)
                {

                    ws.Cell(rowIndexer, lastColIndexer).Value = t.Text;

                    var isCompleted = t.IsCompleted;
                    ws.Cell(rowIndexer, lastColIndexer).Style.Fill.BackgroundColor = isCompleted ? XLColor.Green : XLColor.Red;


                    ws.Cell(rowIndexer, lastColIndexer + 1).SetDataValidation().List("\"True,False\"");
                    ws.Cell(rowIndexer, lastColIndexer + 1).SetDataValidation().IgnoreBlanks = true;
                    ws.Cell(rowIndexer, lastColIndexer + 1).SetDataValidation().InCellDropdown = true;
                    ws.Cell(rowIndexer, lastColIndexer + 1).Value = isCompleted;

                    rowIndexer++;
                }

                lastColIndexer += 2;

            }


            string fileName = "ToDoLists.xlsx";
            var stream = GetStream(wb);

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Buffer = true;
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
            HttpContext.Current.Response.BinaryWrite(stream.ToArray());
            HttpContext.Current.Response.End();
        }

        public MemoryStream GetStream(XLWorkbook xlBook)
        {
            var fs = new MemoryStream();
            xlBook.SaveAs(fs);
            fs.Position = 0;
            return fs;
        }

    }
}