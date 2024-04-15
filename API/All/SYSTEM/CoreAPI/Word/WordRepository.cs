using Aspose.Words;
using Aspose.Words.Drawing;
using Aspose.Words.MailMerging;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using ProfileDAL.Repositories;
using System.Data;

namespace API.All.SYSTEM.CoreAPI.Word
{
    public class WordRepository : IWordRespsitory
    {

        public async Task<byte[]> ExportFileWord(DataSet dsData, string filePath, string fileImagePath, int left, int top, int height, int width)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(filePath);
                Bookmark bookmark = doc.Range.Bookmarks["BookmarkName"];
                if (bookmark != null)
                {
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToBookmark("BookmarkName");
                    builder.Write("Data to be inserted");
                }
                doc.MailMerge.Execute(dsData.Tables[0]);
                for (int i = 1; i < dsData.Tables.Count; i++)
                {
                    doc.MailMerge.ExecuteWithRegions(dsData.Tables[i]);
                }
                if (fileImagePath != "")
                {
                    DocumentBuilder builder1 = new DocumentBuilder(doc);
                    MemoryStream imageStream = new MemoryStream(File.ReadAllBytes(fileImagePath));
                    builder1.InsertImage(imageStream, RelativeHorizontalPosition.Margin, left, RelativeVerticalPosition.Margin, top, width, height, WrapType.None);

                }
                doc.MailMerge.DeleteFields();
                doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveEmptyParagraphs;
                doc.MailMerge.CleanupOptions |= MailMergeCleanupOptions.RemoveUnusedRegions;
                foreach (Aspose.Words.Section section in doc.Sections)
                {
                    section.ClearHeadersFooters();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    doc.Save(stream, SaveFormat.Docx);
                    byte[] fileContents = stream.ToArray();
                    using (MemoryStream stream1 = new MemoryStream(fileContents))
                    {
                        using (WordprocessingDocument doc1 = WordprocessingDocument.Open(stream1, true))
                        {
                            foreach (var section in doc1.MainDocumentPart.Document.Descendants<SectionProperties>())
                            {
                                section.RemoveAllChildren<HeaderReference>();
                                section.RemoveAllChildren<FooterReference>();
                            }
                            MainDocumentPart mainPart = doc1.MainDocumentPart;
                            int currentYear = DateTime.Now.Year;
                            currentYear = 2023;

                            foreach (var textElement in mainPart.Document.Descendants<Text>())
                            {
                                if (textElement.Text.Contains("Evaluation Only. Created with Aspose.Words. Copyright 2003-"+ currentYear + " Aspose Pty Ltd."))
                                {
                                    textElement.Text = textElement.Text.Replace("Evaluation Only. Created with Aspose.Words. Copyright 2003-"+ currentYear + " Aspose Pty Ltd.", string.Empty);
                                }
                                if (textElement.Text.Contains("This document was truncated here because it was created in the Evaluation Mode."))
                                {
                                    textElement.Text = textElement.Text.Replace("This document was truncated here because it was created in the Evaluation Mode.", string.Empty);
                                }
                            }
                        }
                        stream1.Position = 0;
                        byte[] modifiedDocumentBytes = new byte[stream1.Length];
                        await stream1.ReadAsync(modifiedDocumentBytes, 0, (int)stream1.Length);
                        return modifiedDocumentBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }


        public async Task<byte[]> ExportWordNoImage(DataSet dsData, string filePath)
        {
            try
            {
                Aspose.Words.Document doc = new Aspose.Words.Document(filePath);
                Bookmark bookmark = doc.Range.Bookmarks["BookmarkName"];
                if (bookmark != null)
                {
                    DocumentBuilder builder = new DocumentBuilder(doc);
                    builder.MoveToBookmark("BookmarkName");
                    builder.Write("Data to be inserted");
                }
                doc.MailMerge.Execute(dsData.Tables[0]);
                for (int i = 1; i < dsData.Tables.Count; i++)
                {
                    doc.MailMerge.ExecuteWithRegions(dsData.Tables[i]);
                }
                doc.MailMerge.DeleteFields();
                doc.MailMerge.CleanupOptions = MailMergeCleanupOptions.RemoveEmptyParagraphs;
                doc.MailMerge.CleanupOptions |= MailMergeCleanupOptions.RemoveUnusedRegions;
                foreach (Aspose.Words.Section section in doc.Sections)
                {
                    section.ClearHeadersFooters();
                }
                using (MemoryStream stream = new MemoryStream())
                {
                    doc.Save(stream, SaveFormat.Docx);
                    byte[] fileContents = stream.ToArray();
                    using (MemoryStream stream1 = new MemoryStream(fileContents))
                    {
                        using (WordprocessingDocument doc1 = WordprocessingDocument.Open(stream1, true))
                        {
                            foreach (var section in doc1.MainDocumentPart.Document.Descendants<SectionProperties>())
                            {
                                section.RemoveAllChildren<HeaderReference>();
                                section.RemoveAllChildren<FooterReference>();
                            }
                            MainDocumentPart mainPart = doc1.MainDocumentPart;
                            int currentYear = DateTime.Now.Year;
                            currentYear = 2023;
                            foreach (var textElement in mainPart.Document.Descendants<Text>())
                            {
                                if (textElement.Text.Contains("Evaluation Only. Created with Aspose.Words. Copyright 2003-" + currentYear + " Aspose Pty Ltd."))
                                {
                                    textElement.Text = textElement.Text.Replace("Evaluation Only. Created with Aspose.Words. Copyright 2003-" + currentYear + " Aspose Pty Ltd.", string.Empty);
                                }
                                if (textElement.Text.Contains("This document was truncated here because it was created in the Evaluation Mode."))
                                {
                                    textElement.Text = textElement.Text.Replace("This document was truncated here because it was created in the Evaluation Mode.", string.Empty);
                                }
                            }
                        }
                        stream1.Position = 0;
                        byte[] modifiedDocumentBytes = new byte[stream1.Length];
                        await stream1.ReadAsync(modifiedDocumentBytes, 0, (int)stream1.Length);
                        return modifiedDocumentBytes;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

        }
    }
}
