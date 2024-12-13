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
                    "11111111111", "22222222222222", "3333333333333", "444444444444444"
                };
        int studentIndexData = 0;//data base bilgilerinin karşılığı için sayaç
        float cellHeight = tableHeight / rows;
        int studentIndex = 0; // Öğrenci bilgileri için sayaç
        private iTextSharp.text.Rectangle pageSize = PageSize.A4;
        int dvstudentIndexData = 4;

        // Sağdaki Tabloların içerikleri
        string[] titles = { "AAA", "BBB", "CCC" };
        string sınavAdi = "SINAV ADI DENEME";
        string sınavTarih = "SINAV Tarihi DENEME";



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
                float initialYy = pageSize.Height - topMargin;
                bilgibas(cb, leftMargin, initialY);
                sinavAdiBas(cb, initialYy);
                sinavTarihiBas(cb, initialYy);
                rightTablesBas(cb, leftMargin + tableWidth + (0.7f * 28.35f), initialY - 4.5f - (0.5f * 28.35f));
                doc.Close();
                
            }
        }

        // Sağ tarafta 3 tablo çizme fonksiyonu
        private void rightTablesBas(PdfContentByte cb, float x, float y)
        {

            // 3 tabloyu döngü ile çiz
            for (int i = 0; i < titles.Length; i++)
            {
                // Tablo başlığını ekle
                cb.BeginText();
                cb.SetFontAndSize(baseFont, 10);  // Yazı fontu ve boyutu ayarla
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titles[i], x + rightTablesWidth / 2, y + rightTablesTextOffset-8, 0);  // Metni tablo üstüne ortala
                cb.EndText();
                // Bir sonraki tabloya geçmeden önce Y koordinatını güncelle
                y -= (rightTablesHeight + rightTablesSpacing);
            }
        }



        private void bilgibas(PdfContentByte cb, float x, float y)
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
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, studentsInformationDataBase[studentIndexData], x + 75, currentYY + cellHeight / 2 - 5, 0);
                    cb.EndText();
                    studentIndexData++;
                }
            }
        }

        private void sinavAdiBas(PdfContentByte cb, float y)
        {
            // İkinci tabloya "SINAV ADI" altına dbden gelecek isim yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla
            y -= headerHeight;
            y -= (tableHeight + secondTableSpacing);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sınavAdi,  5 + lineX, y - (secondTableHeight / 2) - 5, 0);  // Yazıyı tablonun solunda hizala
            cb.EndText();
        }

        private void sinavTarihiBas(PdfContentByte cb, float y)
        {
            // İkinci tabloya "SINAV ADI" altına dbden gelecek isim yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla
            y -= headerHeight;
            y -= (tableHeight + secondTableSpacing);
            y -= (secondTableHeight + secondTableSpacing);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sınavTarih, lineX + 5, y - (secondTableHeight / 2) - 5, 0);  // Yazıyı tablonun solunda hizala
            cb.EndText();

        }
    }
}
