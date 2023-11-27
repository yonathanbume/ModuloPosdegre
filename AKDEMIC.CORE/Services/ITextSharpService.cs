using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AKDEMIC.CORE.Services
{
    public interface ITextSharpService
    {
        void AddWatermarkToAllPages(ref byte[] filePdfByte, string watermark, float fontSize, float angle = 55, float xPosExtra = 25, float yPosExtra = 0);
        void AddImageWatermarkToAllPages(ref byte[] filePdfByte, string urlFileImage);
        byte[] AddHeaderToAllPages(byte[] original, byte[] header);
        byte[] AddPagination(byte[] bytes);
        byte[] AddPagination(byte[] bytes, BaseColor color, int fontSize, bool bold, float posX, float posY, float rotation, byte format);
        byte[] RemoveEmptyPages(byte[] bytes, int blankPdfsize = 1000);
        byte[] AddText(byte[] bytes, string text, BaseColor color, int fontSize, bool bold, float posX, float posY, float rotation);

    }
}
