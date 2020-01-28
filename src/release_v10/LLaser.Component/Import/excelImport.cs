// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="excelImport.cs">
//   
// </copyright>
// <summary>
//   The excel import.
// </summary>
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
namespace LLaser.Component.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    // OpenXML part, just use spreadsheet (for Excel) part
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;

    using LLaser.Common.Core;
    using LLaser.Common.Extension;


    /// <summary>
    ///     The excel import.
    /// </summary>
    public class excelImport : IImport
    {
        #region Public Properties

        /// <summary>
        ///     Gets or sets the target.
        /// </summary>
        public object Target { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The import.
        /// </summary>
        /// <param name="filePath">
        /// The file path.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Import<T>(string filePath)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                Import<T>(fs);
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
        /// The import.
        /// </summary>
        /// <param name="stream">
        /// The stream.
        /// </param>
        /// <typeparam name="T">
        /// </typeparam>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Import<T>(Stream stream)
        {
            try
            {
                var models = new List<PowerTiming>();
                SharedStringItem[] sharedStringvalues = null;
                int onIndex = 0, offIndex = 0;

                // open a spreadsheet document from a stream.
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(stream, false))
                {
                    Workbook workbook = document.WorkbookPart.Workbook;
                    foreach (Sheet sheet in workbook.Descendants<Sheet>())
                    {
                        PowerTiming model = new PowerTiming(sheet.GetAttribute("name", string.Empty).Value.ToUpper());
                        models.Add(model);

                        var worksheetPart = (WorksheetPart)document.WorkbookPart.GetPartById(sheet.Id);

                        // get the shared string part and generate the shared string array
                        SharedStringTablePart sharedStringPart = document.WorkbookPart.SharedStringTablePart;
                        if (sharedStringPart != null)
                        {
                            sharedStringvalues = sharedStringPart.SharedStringTable.Elements<SharedStringItem>().ToArray();
                        }

                        Worksheet worksheet = worksheetPart.Worksheet;

                        foreach (SheetData sheetData in worksheet.Descendants<SheetData>())
                        {
                            foreach (Row row in sheetData.Descendants<Row>())
                            {
                                Cell[] cells = row.Descendants<Cell>().ToArray();
                                if (cells.Length >= 4)
                                {
                                    // read power status value
                                    if ((this.ReadCellValue(cells[0], sharedStringvalues).ToUpper() == "ON" && this.ReadCellValue(cells[3], sharedStringvalues).ToUpper() == "OFF")
                                     || (this.ReadCellValue(cells[0], sharedStringvalues).ToUpper() == "OFF" && this.ReadCellValue(cells[3], sharedStringvalues).ToUpper() == "ON"))
                                    {
                                        onIndex = this.ReadCellValue(cells[0], sharedStringvalues).ToUpper() == "ON" ? 0 : 3;
                                        offIndex = this.ReadCellValue(cells[0], sharedStringvalues).ToUpper() == "OFF" ? 0 : 3;
                                    }
                                    else if (cells.Length > 4)
                                    {
                                        // read signals
                                        if (!string.IsNullOrEmpty(this.ReadCellValue(cells[offIndex], sharedStringvalues))
                                         && !string.IsNullOrEmpty(this.ReadCellValue(cells[offIndex + 1], sharedStringvalues)))
                                        {
                                            model.OffSignals.Add(
                                                new Signal(
                                                    this.ReadCellValue(cells[offIndex], sharedStringvalues).StringToDouble(),
                                                    this.ReadCellValue(cells[offIndex + 1], sharedStringvalues).ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                                        }

                                        if (!string.IsNullOrEmpty(this.ReadCellValue(cells[onIndex], sharedStringvalues))
                                         && !string.IsNullOrEmpty(this.ReadCellValue(cells[onIndex + 1], sharedStringvalues)))
                                        {
                                            model.OnSignals.Add(
                                                new Signal(
                                                    this.ReadCellValue(cells[onIndex], sharedStringvalues).StringToDouble(),
                                                    this.ReadCellValue(cells[onIndex + 1], sharedStringvalues).ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                                        }
                                    }
                                }
                                else if (cells.Length == 2)
                                {
                                    // read signals
                                    if (!string.IsNullOrEmpty(this.ReadCellValue(cells[0], sharedStringvalues))
                                     && !string.IsNullOrEmpty(this.ReadCellValue(cells[1], sharedStringvalues)))
                                    {
                                        if (offIndex == 0)
                                        {
                                            model.OffSignals.Add(
                                                new Signal(
                                                    this.ReadCellValue(cells[0], sharedStringvalues).StringToDouble(),
                                                    this.ReadCellValue(cells[1], sharedStringvalues).ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                                        }
                                        if (onIndex == 0)
                                        {
                                            model.OnSignals.Add(
                                                new Signal(
                                                    this.ReadCellValue(cells[0], sharedStringvalues).StringToDouble(),
                                                    this.ReadCellValue(cells[1], sharedStringvalues).ToUpper().Trim() == "HIGH" ? ElectricalLevels.High : ElectricalLevels.Low));
                                        }
                                    }

                                }
                            }
                        }
                    }

                    this.Target = models;

                    // close the document
                    document.Close();
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
        /// The read cell value.
        /// </summary>
        /// <param name="cell">
        /// The cell.
        /// </param>
        /// <param name="sharedStrings">
        /// The shared strings.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string ReadCellValue(Cell cell, SharedStringItem[] sharedStrings)
        {
            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {
                int index = int.Parse(cell.CellValue.Text);

                if (sharedStrings != null && sharedStrings.Length > index)
                    return sharedStrings[index].InnerText;

                return string.Empty;
            }
            else
            {
                return cell.CellValue.Text;
            }
        }

        #endregion
    }
}