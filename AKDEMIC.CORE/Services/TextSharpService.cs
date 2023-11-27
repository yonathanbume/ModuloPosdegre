using AKDEMIC.CORE.Helpers;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;

namespace AKDEMIC.CORE.Services
{
    public class TextSharpService : ITextSharpService
    {
        public void AddWatermarkToAllPages(ref byte[] filePdfByte, string watermark, float fontSize, float angle = 55, float xPosExtra = 25, float yPosExtra = 0)
        {
            var baseFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.EMBEDDED);
            var gstate = new PdfGState { FillOpacity = 0.1f };
            var color = new BaseColor(0, 0, 0);

            using (var ms = new MemoryStream())
            {
                using (var reader = new PdfReader(filePdfByte))
                using (var stamper = new PdfStamper(reader, ms))
                {
                    int times = reader.NumberOfPages;
                    for (int i = 1; i <= times; i++)
                    {
                        var dc = stamper.GetOverContent(i);
                        dc.SaveState();
                        dc.SetGState(gstate);
                        dc.SetColorFill(color);
                        dc.BeginText();
                        dc.SetFontAndSize(baseFont, fontSize);
                        var ps = reader.GetPageSizeWithRotation(i);
                        var x = (ps.Right + ps.Left) / 2 + xPosExtra;
                        var y = (ps.Bottom + ps.Top) / 2 + yPosExtra;
                        dc.ShowTextAligned(Element.ALIGN_CENTER, watermark, x, y, angle);
                        dc.EndText();
                        dc.RestoreState();
                    }
                    stamper.Close();
                }

                filePdfByte = ms.ToArray();
            }
        }

        public void AddImageWatermarkToAllPages(ref byte[] filePdfByte, string urlFileImage)
        {
            try
            {
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(urlFileImage);

                var gstate = new PdfGState { FillOpacity = 0.1f };

                using (var ms = new MemoryStream())
                {
                    using (var reader = new PdfReader(filePdfByte))
                    using (var stamper = new PdfStamper(reader, ms))
                    {
                        int times = reader.NumberOfPages;
                        for (int i = 1; i <= times; i++)
                        {
                            var dc = stamper.GetOverContent(i);
                            //add image with pre-set position and size
                            dc.SetGState(gstate);
                            image.SetAbsolutePosition((PageSize.A4.Width - image.ScaledWidth) / 2, (PageSize.A4.Height - image.ScaledHeight) / 2);
                            dc.AddImage(image);
                        }
                        stamper.Close();
                    }
                    filePdfByte = ms.ToArray();
                }
            }
            catch (System.Exception) { }
        }

        public byte[] AddHeaderToAllPages(byte[] original, byte[] header)
        {
            var pdfOriginal = new PdfReader(original);
            var pdfHeader = new PdfReader(header);

            using (MemoryStream ms = new MemoryStream())
            {
                using (PdfStamper stamper = new PdfStamper(pdfOriginal, ms))
                {
                    PdfImportedPage page = stamper.GetImportedPage(pdfHeader, 1);

                    int n = pdfOriginal.NumberOfPages;
                    PdfContentByte background;
                    for (int i = 1; i <= n; i++)
                    {
                        background = stamper.GetUnderContent(i);
                        background.AddTemplate(page, 0, 0);
                    }
                    // CLose the stamper
                    stamper.Close();
                }

                return ms.ToArray();
            }
        }

        public byte[] AddPagination(byte[] bytes)
        {
            using (var reader = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader, ms))
                    {
                        int PageCount = reader.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            var bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            Font font = new Font(bf, 9, Font.NORMAL, BaseColor.BLACK);
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format("Pag. {0}/{1}", i, PageCount), font), 532, 758, 0);
                        }
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] AddPagination(byte[] bytes, BaseColor color, int fontSize, bool bold, float posX, float posY, float rotation, byte format)
        {
            using (var reader = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader, ms))
                    {
                        int PageCount = reader.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            var bf = BaseFont.CreateFont(bold ? BaseFont.HELVETICA_BOLD : BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            Font font = new Font(bf, fontSize, Font.NORMAL, color);
                            switch (format)
                            {
                                case 1:
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format("Pág. {0}/{1}", i, PageCount), font), posX, posY, rotation);
                                    break;
                                case 2:
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format("Pág. {0}", i, PageCount), font), posX, posY, rotation);
                                    break;
                                case 3:
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format("Página {0} de {1}", i, PageCount), font), posX, posY, rotation);
                                    break;
                                case 4:
                                    ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format("Page {0} of {1}", i, PageCount), font), posX, posY, rotation);
                                    break;
                            }
                        }
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] AddText(byte[] bytes, string text, BaseColor color, int fontSize, bool bold, float posX, float posY, float rotation)
        {
            using (var reader = new PdfReader(bytes))
            {
                using (var ms = new MemoryStream())
                {
                    using (var stamper = new PdfStamper(reader, ms))
                    {
                        int PageCount = reader.NumberOfPages;
                        for (int i = 1; i <= PageCount; i++)
                        {
                            var bf = BaseFont.CreateFont(bold ? BaseFont.HELVETICA_BOLD : BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                            Font font = new Font(bf, fontSize, Font.NORMAL, color);
                            ColumnText.ShowTextAligned(stamper.GetOverContent(i), Element.DIV, new Phrase(string.Format(text, i, PageCount), font), posX, posY, rotation);
                        }
                    }
                    return ms.ToArray();
                }
            }
        }

        public byte[] RemoveEmptyPages(byte[] bytes, int blankPdfsize)
        {
            // step 1: create new reader
            var r = new PdfReader(bytes);
            var raf = new RandomAccessFileOrArray(bytes);
            var document = new Document(r.GetPageSizeWithRotation(1));

            var stream = new System.IO.MemoryStream();

            // step 2: create a writer that listens to the document
            var writer = new PdfCopy(document, stream);

            // step 3: we open the document
            document.Open();

            // step 4: we add content
            PdfImportedPage page = null;

            //loop through each page and if the bs is larger than 20 than we know it is not blank.
            //if it is less than 20 than we don't include that blank page.
            for (var i = 1; i <= r.NumberOfPages; i++)
            {
                //get the page content
                byte[] bContent = r.GetPageContent(i, raf);
                var bs = new MemoryStream();

                //write the content to an output stream
                bs.Write(bContent, 0, bContent.Length);

                //add the page to the new pdf
                if (bs.Length > blankPdfsize)
                {
                    page = writer.GetImportedPage(r, i);
                    writer.AddPage(page);
                }
                bs.Close();
            }
            //close everything
            document.Close();
            writer.Close();
            raf.Close();
            r.Close();

            return stream.ToArray();
        }
    }
}
