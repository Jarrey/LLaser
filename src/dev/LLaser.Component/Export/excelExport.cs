// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="excelExport.cs">
//   
// </copyright>
// <summary>
//   The excel export.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

/* OpenXML SDK is provided by Microsoft: http://www.microsoft.com/en-us/download/details.aspx?id=30425
 * 
 * Open XML is an open ECMA 376 standard and is also approved as the ISO/IEC 29500 standard 
 * that defines a set of XML schemas for representing spreadsheets, charts, presentations, 
 * and word processing documents. Microsoft Office Word 2007, Excel 2007, PowerPoint 2007, 
 * and the later versions all use Open XML as the default file format. 
 * 
 * The Open XML file formats are useful for developers because they use an open standard and are 
 * based on well-known technologies: ZIP and XML.
 * 
 * The Open XML SDK 2.5 for Microsoft Office is built on top of the System.IO.Packaging API and 
 * provides strongly typed part classes to manipulate Open XML documents. The SDK also uses 
 * the .NET Framework Language-Integrated Query (LINQ) technology to provide strongly typed object access to 
 * the XML content inside the parts of Open XML documents.
 * 
 * The Open XML SDK 2.5 simplifies the task of manipulating Open XML packages and the underlying Open XML schema 
 * elements within a package. The Open XML Application Programming Interface (API) encapsulates many common tasks 
 * that developers perform on Open XML packages, so you can perform complex operations with just a few lines of code. 
 * 
 * The tools package contains the Open XML SDK v2.5 Productivity Tool for Office and the hyperlink to documentation 
 * for the Open XML SDK 2.5. The Open XML SDK 2.5 Productivity Tool for Microsoft Office provides a number of 
 * features designed to improve your productivity and accelerate your learning while working with the SDK and 
 * Open XML files. Features include the ability to generate Open XML SDK 2.5 source code based on document content, 
 * compare source and target Open XML documents to reveal differences and to generate source code to create the 
 * target from the source, validate documents, and display documentation for the Open XML SDK 2.5 Classes, 
 * the ECMA376v1 standard, and the Microsoft Office implementation note
 * 
 * OpenXML SDK v2.5 require .Net Framework 4.0 and start to support Office 2013 documents.
 * 
 */
namespace LLaser.Component.Export
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    // OpenXML part, just use spreadsheet (for Excel) part
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    using LLaser.Common.Core;

    /// <summary>
    /// The excel export.
    /// </summary>
    public class excelExport : IExport
    {
        #region Public Methods and Operators

        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Export<T>(string filePath, T obj)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
                fs.Seek(0, SeekOrigin.Begin);
                Export(fs, obj);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Dispose();
                    fs = null;
                }
            }
        }

        /// <summary>
        /// The export.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Export<T>(Stream stream, T obj)
        {
            try
            {
                var models = obj as IEnumerable<PowerTiming>;
                if (models != null)
                {
                    // create a spreadsheet document in a stream.
                    // by default, AutoSave = true, Editable = true, and Type = xlsx.
                    using (
                        SpreadsheetDocument document = SpreadsheetDocument.Create(
                            stream, SpreadsheetDocumentType.Workbook))
                    {
                        // create workbookpart and generate the content
                        WorkbookPart workbookPart = document.AddWorkbookPart();
                        var workbook = new Workbook();

                        // create sheets collection
                        var sheets = new Sheets();
                        uint sheetId = 1U;
                        foreach (PowerTiming powerTiming in models)
                        {
                            // create seperate named sheet
                            sheets.Append(
                                new Sheet
                                    {
                                        Id = "rId" + powerTiming.Name,
                                        SheetId = sheetId++,
                                        Name = powerTiming.Name
                                    });

                            // create worksheetpart and generate its content
                            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>("rId" + powerTiming.Name);
                            var worksheet = new Worksheet();

                            // add SheetData to the Worksheet
                            var sheetData = new SheetData();
                            worksheet.Append(sheetData);

                            worksheetPart.Worksheet = worksheet;

                            // generate power status row header
                            Row headerRow = this.AppendRow(sheetData, 1);
                            this.GenerateCell(headerRow, PowerStatus.On.ToString());
                            this.GenrateEmptyCell(headerRow, 2);
                            this.GenerateCell(headerRow, PowerStatus.Off.ToString());

                            // generate rows/cells with data
                            uint rowIndex = 1;
                            // Export each entity separately
                            for (int i = 0; i < Math.Max(powerTiming.OnSignals.Count, powerTiming.OffSignals.Count); i++)
                            {
                                Row row = this.AppendRow(sheetData, ++rowIndex);
                                string outputLine = string.Empty;
                                if (i < powerTiming.OnSignals.Count)
                                {
                                    this.GenerateCell(row, powerTiming.OnSignals[i].Time.ToString());
                                    this.GenerateCell(row, powerTiming.OnSignals[i].ElectricalLevel.ToString());
                                }
                                else
                                {
                                    this.GenrateEmptyCell(row, 2);
                                }

                                // generate empty cell between On mode and Off mode
                                this.GenrateEmptyCell(row);

                                if (i < powerTiming.OffSignals.Count)
                                {
                                    this.GenerateCell(row, powerTiming.OffSignals[i].Time.ToString());
                                    this.GenerateCell(row, powerTiming.OffSignals[i].ElectricalLevel.ToString());
                                }
                                else
                                {
                                    this.GenrateEmptyCell(row, 2);
                                }
                            }
                        }

                        workbook.Append(sheets);
                        workbookPart.Workbook = workbook;

                        // close the document
                        document.Close();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Append a cell (with optionally specified content and type) to the given row.
        /// </summary>
        /// <param name="row">
        /// The row to which the cell will be appended.  The cell will always be appended a the end of the row.
        /// </param>
        /// <param name="cellValue">
        /// The content of the cell (possibly blank).
        /// </param>
        /// <param name="dataType">
        /// The type of the cell content (assumed to be String).
        /// </param>
        /// <returns>
        /// The cell that was appended.
        /// </returns>
        private Cell AppendCell(Row row, string cellValue = "", CellValues dataType = CellValues.String)
        {
            var cell = new Cell();
            cell.CellReference = this.ColumnName(row.ChildElements.Count) + row.RowIndex;
            cell.DataType = dataType;
            cell.CellValue = new CellValue(cellValue);
            row.Append(cell);
            return cell;
        }

        /// <summary>
        /// Append a row to the given SheetData.
        /// </summary>
        /// <param name="sheetData">
        /// The SheetData to which the row will be appended.  The row will always be appended a the end of the SheetData.
        /// </param>
        /// <param name="rowIndex">
        /// The index of the row (one-based).
        /// </param>
        /// <returns>
        /// The row that was appended.
        /// </returns>
        private Row AppendRow(SheetData sheetData, uint rowIndex)
        {
            var row = new Row();
            row.RowIndex = rowIndex;
            sheetData.Append(row);
            return row;
        }

        /// <summary>
        /// Derive a column name (from A to ZZ) based on a zero-based column number.
        /// </summary>
        /// <param name="columnNum">
        /// The zero-based column number.
        /// </param>
        /// <returns>
        /// The column name, from A to ZZ.
        /// </returns>
        private string ColumnName(int columnNum)
        {
            columnNum = columnNum + 1;
            string result = string.Empty;
            if (columnNum <= 0)
            {
                return "A";
            }
            else if (columnNum > 16383)
            {
                return "XFD";
            }

            while (columnNum > 0)
            {
                int mod = columnNum % 26;
                if (mod == 0)
                {
                    mod = 26;
                }

                result = (char)(mod + 'A' - 1) + result;
                columnNum = (columnNum - mod) / 26;
            }

            return result;
        }

        /// <summary>
        /// The genrate empty cell.
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <param name="number">Number of empty cell want to generate</param>
        /// <returns>
        /// The <see cref="Cell"/>.
        /// </returns>
        private void GenrateEmptyCell(Row row, int number = 1)
        {
            while (number-- > 0)
            {
                this.GenerateCell(row, string.Empty);
            }
        }

        /// <summary>
        /// generate cell for current excel sheet
        /// </summary>
        /// <param name="row">
        /// the row contain current cell
        /// </param>
        /// <param name="cellData">
        /// cell data
        /// </param>
        /// <returns>
        /// The <see cref="Cell"/>.
        /// </returns>
        private Cell GenerateCell(Row row, string cellData)
        {
            Cell cell = null;
            double cellNumericValue = 0.0D;
            if (this.IsNumeric(cellData, ref cellNumericValue))
            {
                cell = this.AppendCell(row, cellNumericValue.ToString(), CellValues.Number);
            }
            else
            {
                // set cellcontent as string type
                cell = this.AppendCell(row, cellData);
            }

            return cell;
        }

        /// <summary>
        /// Determines whether a string is numeric according the the "en-US" culture info.
        /// </summary>
        /// <param name="anyString">
        /// The string.
        /// </param>
        /// <param name="outValue">
        /// The parsed result value.
        /// </param>
        /// <returns>
        /// True if the string is numeric, false otherwise.
        /// </returns>
        private bool IsNumeric(string anyString, ref double outValue)
        {
            try
            {
                if (anyString != null && anyString.Length > 0)
                {
                    // use System.Globalization.NumberStyles.Float can ignore the thousands symbol in string value
                    // and return the currect value for e.g. "12,34,567,23" as string type
                    return double.TryParse(anyString, NumberStyles.Float, null, out outValue);
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        #endregion
    }
}