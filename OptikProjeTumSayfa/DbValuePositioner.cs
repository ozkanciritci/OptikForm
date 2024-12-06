using System;
using System.Drawing.Printing;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OptikProjeTumSayfa
{
    internal class DbValuePositioner : StudentInformationTable
    {
        string[] studentsInformationDataBase = {
                    "11111111111", "1111111111111", "11111111111", "11111111111"
                };
        int studentIndexData = 0;//data base bilgilerinin karşılığı için sayaç
        float cellHeight = tableHeight / rows;
        int studentIndex = 0; // Öğrenci bilgileri için sayaç
        private iTextSharp.text.Rectangle pageSize = PageSize.A4;
        int dvstudentIndexData = 4;



        public void CreatePdf(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(pageSize);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                // İlk tablonun üstten 3.2 cm boşlukla başlayacak
                float initialY = pageSize.Height - topMargin-cellHeight;
                bilgibas(cb, 1, initialY);
                doc.Close();
            }
        }






        private void bilgibas(PdfContentByte cb, int x, float y)
        {
            for (int row = 0; row < rows; row++)
            {
                // Satırın başlangıç Y konumu

                float currentYY = y - (row + 1) * cellHeight - cellHeight+3; //+3 mantıklı değil kaydığı için ekledim
                if (studentIndexData < studentsInformationDataBase.Length)
                {
                    cb.BeginText();

                    // Yazı tipi ve boyutu ayarlanmalı
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(bf, 10 ); // Yazı tipi: Helvetica, Boyut: 12

                    cb.SetRGBColorFill(0, 0, 0); // Siyah renk
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, studentsInformationDataBase[studentIndexData], x + 105, currentYY + cellHeight / 2 - 5, 0);
                    cb.EndText();
                    studentIndexData++;
                }

            }


            // İkinci tabloya "SINAV ADI" altına dbden gelecek isim yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla

            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "222222222", x + 5, y - (secondTableHeight / 2) - 5, 0);  // Yazıyı tablonun solunda hizala
            cb.EndText();








        }
    }
}
