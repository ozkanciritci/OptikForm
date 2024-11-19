using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OptikProjeTumSayfa
{
    public class NumberedTable
    {
        // Değişkenler
        private float pageWidth = 21.0f * 28.35f;                                                       // Sayfa genişliği (A4 kağıt boyutu)
        private float pageHeight = 29.7f * 28.35f;                                                      // Sayfa yüksekliği (A4 kağıt boyutu)
        private float tableWidth = 4.6f * 28.35f;                                                       // Tablo genişliği
        private float tableHeight = 5.6f * 28.35f;                                                      // Tablo yüksekliği
        private float titleHeight = 0.9f * 28.35f;                                                      // Başlık yüksekliği
        private float firstRowHeight = 0.4f * 28.35f;                                                   // İlk satır boşluğu
        private float marginLeft = 1.5f * 28.35f;                                                       // Sol kenar boşluğu
        private float marginTop = (29.7f - 10.4f) * 28.35f;                                             // Üst kenar boşluğu
        private float rectWidth = 0.4f * 28.35f;                                                        // Dikdörtgen genişliği
        private float rectHeight = 0.15f * 28.35f;                                                      // Dikdörtgen yüksekliği
        private float rectStartX = 0.5f * 28.35f;                                                       // Dikdörtgenlerin başlangıç noktası
        private int circleFontSize = 5;                                                                 // Yuvarlak içindeki sayılar için yazı tipi boyutu
        private float circleRadius = 0.15f * 28.35f;                                                    // Dairelerin yarıçapı
        private float tableSpacing = 0.4f * 28.35f;                                                     // Tablolar arası boşluk
        private float borderLineWidth = 0.5f;                                                           // Çizgi kalınlığı
        private float titleFontSize = 10f;                                                              // Başlık yazı tipi boyutu
        private float titleTextOffset = 3f;                                                             // Başlık metni ofseti
        private int titleBackgroundColorR = 173;                                                        // Başlık arka plan rengi (Kırmızı bileşeni)
        private int titleBackgroundColorG = 216;                                                        // Başlık arka plan rengi (Yeşil bileşeni)
        private int titleBackgroundColorB = 230;                                                        // Başlık arka plan rengi (Mavi bileşeni)
        private int blackColorR = 0;                                                                    // Siyah renk (Kırmızı bileşeni)
        private int blackColorG = 0;                                                                    // Siyah renk (Yeşil bileşeni)
        private int blackColorB = 0;                                                                    // Siyah renk (Mavi bileşeni)
        private int blueBackgroundColorR = 220;                                                         // Açık mavi arka plan rengi (Kırmızı bileşeni)
        private int blueBackgroundColorG = 240;                                                         // Açık mavi arka plan rengi (Yeşil bileşeni)
        private int blueBackgroundColorB = 255;                                                         // Açık mavi arka plan rengi (Mavi bileşeni)
        private int whiteBackgroundColorR = 255;                                                        // Beyaz arka plan rengi (Kırmızı bileşeni)
        private int whiteBackgroundColorG = 255;                                                        // Beyaz arka plan rengi (Yeşil bileşeni)
        private int whiteBackgroundColorB = 255;                                                        // Beyaz arka plan rengi (Mavi bileşeni)
        private string firstTableTitle = "TC KİMLİK NO";                                                // İlk tablo başlığı
        private string secondTableTitle = "ÖĞRENCİ NO";                                                 // İkinci tablo başlığı
        private float bookletTypeTableWidth = 2.3f * 28.35f;                                            // Booklet Type Table genişliği

        public void CreatePdf(string filePath)
        {
            // Belirtilen dosya yolunda bir PDF dosyası oluştur
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                // İlk tabloyu çiz
                DrawTable(cb, marginLeft, marginTop, tableWidth, tableHeight, firstTableTitle);

                // İkinci tabloyu çiz
                DrawTable(cb, marginLeft + tableWidth + tableSpacing, marginTop, tableWidth, tableHeight, secondTableTitle);

                // Üçüncü tabloyu çiz (Booklet Type Table)
                BookletTypeTable bookletTypeTable = new BookletTypeTable();
                bookletTypeTable.DrawTable(cb, marginLeft + 2 * tableWidth + 2 * tableSpacing, marginTop, bookletTypeTableWidth, tableHeight);

                doc.Close();
            }
        }

        private void DrawTable(PdfContentByte cb, float x, float y, float width, float height, string title)
        {
            int columns = 12; // Sütun sayısı
            int rows = 10;    // Satır sayısı
            float remainingHeight = height - titleHeight - firstRowHeight; // Kalan yükseklik
            float cellHeight = remainingHeight / rows; // Her bir hücre için kalan yükseklik

            // Başlığı ekle
            AddTitle(cb, x, y, width, titleHeight, title);

            cb.SetLineWidth(borderLineWidth);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, circleFontSize);

            // İlk boşluğu çiz
            y -= titleHeight; // Yüksekliği başlık yüksekliği kadar azalt
            for (int i = 0; i < columns; i++)
            {
                if (i % 2 == 0)
                {
                    cb.SetRGBColorFill(blueBackgroundColorR, blueBackgroundColorG, blueBackgroundColorB);
                }
                else
                {
                    cb.SetRGBColorFill(whiteBackgroundColorR, whiteBackgroundColorG, whiteBackgroundColorB);
                }
                cb.Rectangle(x + i * (width / columns), y - firstRowHeight, width / columns, firstRowHeight);
                cb.Fill();
            }

            // İlk boşluğun altına düz bir çizgi çiz
            cb.SetRGBColorStroke(blackColorR, blackColorG, blackColorB);
            cb.SetLineWidth(borderLineWidth);
            cb.MoveTo(x, y - firstRowHeight);
            cb.LineTo(x + width, y - firstRowHeight);
            cb.Stroke();

            // Sol siyah kenarlığı ekle
            cb.SetLineWidth(borderLineWidth);
            cb.MoveTo(x, y);
            cb.LineTo(x, y - height + titleHeight);
            cb.Stroke();

            // Sağ siyah kenarlığı ekle
            cb.SetLineWidth(borderLineWidth);
            cb.MoveTo(x + width, y);
            cb.LineTo(x + width, y - height + titleHeight + firstRowHeight);
            cb.Stroke();

            // Tablo sütunlarını ve yuvarlak hücreleri çizme
            y -= firstRowHeight;
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    if (i % 2 == 0)
                    {
                        cb.SetRGBColorFill(blueBackgroundColorR, blueBackgroundColorG, blueBackgroundColorB);
                    }
                    else
                    {
                        cb.SetRGBColorFill(whiteBackgroundColorR, whiteBackgroundColorG, whiteBackgroundColorB);
                    }
                    cb.Rectangle(x + i * (width / columns), y - (j + 1) * cellHeight, width / columns, cellHeight);
                    cb.Fill();

                    // Hücre için dairesel çerçeveyi çiz
                    cb.SetLineWidth(borderLineWidth);
                    cb.Circle(x + i * (width / columns) + (width / columns) / 2, y - (j + 1) * cellHeight + cellHeight / 2, circleRadius);
                    cb.Stroke();

                    // Numarayı dairenin içine ortalayarak yaz
                    cb.BeginText();
                    cb.SetRGBColorFill(blackColorR, blackColorG, blackColorB);
                    cb.SetFontAndSize(bf, circleFontSize);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, j.ToString(), x + i * (width / columns) + (width / columns) / 2, y - (j + 1) * cellHeight + cellHeight / 2 - (circleFontSize * 0.35f), 0);
                    cb.EndText();
                }

                // Sütunlar arasında siyah çizgi çiz
                cb.SetRGBColorStroke(blackColorR, blackColorG, blackColorB);
                cb.SetLineWidth(borderLineWidth);
                cb.MoveTo(x + (i + 1) * (width / columns), y + firstRowHeight);
                cb.LineTo(x + (i + 1) * (width / columns), y - height + titleHeight + firstRowHeight);
                cb.Stroke();
            }

            // Üst ve alt kenarlıkları ekle
            cb.SetLineWidth(borderLineWidth);
            cb.MoveTo(x, y + firstRowHeight);
            cb.LineTo(x + width, y + firstRowHeight);
            cb.Stroke();

            cb.MoveTo(x, y - height + titleHeight + firstRowHeight);
            cb.LineTo(x + width, y - height + titleHeight + firstRowHeight);
            cb.Stroke();

            // Sol tarafa dikdörtgenleri çiz
            DrawRectangles(cb, x + width + 10, y, rows, cellHeight);
        }

        private void DrawRectangles(PdfContentByte cb, float x, float y, int rows, float cellHeight)
        {
            for (int j = 0; j < rows + 9; j++)
            {
                float rectY = y - (j + 1) * cellHeight + cellHeight / 2 - rectHeight / 2 + 36.86f;

                cb.SetColorFill(BaseColor.BLACK);
                cb.Rectangle(rectStartX, rectY, rectWidth, rectHeight);
                cb.Fill();
            }
        }

        private void AddTitle(PdfContentByte cb, float x, float y, float width, float height, string title)
        {
            // Başlık için arka planı çiz
            cb.SetRGBColorFill(titleBackgroundColorR, titleBackgroundColorG, titleBackgroundColorB);
            cb.Rectangle(x, y - height, width, height);
            cb.Fill();

            // Başlık metnini yaz
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1254", BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, titleFontSize);
            cb.SetRGBColorFill(blackColorR, blackColorG, blackColorB);

            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, title, x + width / 2, y - height / 2 - titleTextOffset, 0);
            cb.EndText();

            // Başlığın etrafına kenarlık ekle
            cb.SetRGBColorStroke(blackColorR, blackColorG, blackColorB);
            cb.SetLineWidth(borderLineWidth);
            cb.Rectangle(x, y - height, width, height);
            cb.Stroke();
        }
    }
}
