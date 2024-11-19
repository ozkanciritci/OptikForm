using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptikProjeTumSayfa
{
    internal class AnswersTable
    {

        // Değişkenler
        private string title = "CEVAPLAR";                                                                                        // Başlık
        private int totalQuestions = 100;                                                                                         // Toplam soru sayısı
        private BaseColor titleBackgroundColor = new BaseColor(173, 216, 230);                                                    // Başlık arka plan rengi
        private BaseColor questionBackgroundColor = new BaseColor(220, 240, 255);                                                 // Soru arka plan rengi
        private float questionBackgroundOpacity = 1.0f;                                                                           // Soru arka plan rengi opaklığı
        private float titleBackgroundAlpha = 1.0f;                                                                                // Başlık arka plan saydamlığı
        private float questionBackgroundAlpha = 1.0f;                                                                             // Soru arka plan saydamlığı
        private float lineThickness = 1f;                                                                                         // Çizgi kalınlığı
        private float circleLineThickness = 0.5f;                                                                                 // Yuvarlakların çizgi kalınlığı
        private string[] choices = { "A", "B", "C", "D", "E" };                                                                   // Şıklar
        private int numberOfChoices = 5;                                                                                          // Kaç şık olacağı
        private BaseColor lineColor = BaseColor.BLACK;                                                                            // Çizgi rengi
        private BaseColor borderColor = BaseColor.BLACK;                                                                          // Çerçeve rengi
        private int questionsPerRow = 25;                                                                                         // Bir satırdaki soru sayısı
        private float titleHeight = 22.6f;                                                                                        // Başlık yüksekliği
        private float tableWidth = 343f;                                                                                          // Tablo genişliği (12.1 cm)
        private float tableHeight = (float)(11.6 * 28.35f);                                                                       // Tablo yüksekliği (10.8 cm)
        private float marginLeft = 42f;                                                                                           // Sol kenar boşluğu
        private float marginRight = 50f;                                                                                          // Sağ kenar boşluğu
        private float marginTop = (float)(16.9095*28.35);                                                                         // Üst kenar boşluğu 16.65
        private float marginBottom = 50f;                                                                                         // Alt kenar boşluğu
        private float circleRadius = 4.5f;                                                                                        // Yuvarlakların yarıçapı
        private float columnSpacing = 3f;                                                                                         // Sütunlar arası boşluk
        private float choiceSpacing = 11.5f;                                                                                      // Şıklar arası boşluk
        private BaseFont titleFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1254", BaseFont.NOT_EMBEDDED);        // Başlık yazı tipi
        private int titleFontSize = 10;                                                                                           // Başlık yazı tipi boyutu
        private BaseFont questionFont = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);          // Soru yazı tipi
        private int questionFontSize = 5;                                                                                         // Soru yazı tipi boyutu
        private float questionNumberOffsetX = 2f;                                                                                 // X ekseninde soru numarası ofseti
        private float questionNumberOffsetY = -3f;                                                                                // Y ekseninde soru numarası ofseti
        private iTextSharp.text.Rectangle pageSize = PageSize.A4;                                                                 // Sayfa boyutu

        public void CreatePdf(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(pageSize, marginLeft, marginRight, marginTop, marginBottom);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                DrawTable(cb, marginLeft, pageSize.Height - marginTop, tableWidth, tableHeight - titleHeight); // Tabloyu oluştur, başlık yüksekliği çıkarıldı

                doc.Close();
            }
        }

        private void DrawTable(PdfContentByte cb, float x, float y, float width, float height)
        {
            int columns = totalQuestions / questionsPerRow; // Sütun sayısı
            int rows = questionsPerRow;   // Satır sayısı
            float cellWidth = (width - ((columns - 1) * columnSpacing)) / columns;  // Hücre genişliği 
            float cellHeight = height / rows;  // Hücre yüksekliği, geri kalan alana eşit olarak dağılır

            // Başlığı ekle
            AddTitle(cb, x, y, width, titleHeight, title);

            y -= titleHeight; // Başlık alanı çıkartıldı

            cb.SetLineWidth(lineThickness);
            cb.SetFontAndSize(questionFont, questionFontSize);

            // "CEVAPLAR" başlığının altına çizgi çiz
            cb.SetLineWidth(lineThickness); // Çizgi kalınlığı
            cb.MoveTo(x, y);  // Başlığın altına çizgi ekle
            cb.LineTo(x + width, y);
            cb.Stroke();

            // Cevaplar için tablo hücrelerini çiz
            for (int i = 0; i < columns; i++)
            {
                for (int j = 0; j < rows; j++)
                {
                    float currentX = x + i * (cellWidth + columnSpacing); // Sütunlar arası boşluğu hesaba kat
                    float currentY = y - (j + 1) * cellHeight; // Yükseklik ayarlandı

                    // Bir mavi bir beyaz arka plan ayarla
                    if (j % 2 == 0)
                    {
                        // Opaklığı ayarlanmış soru arka plan rengini kullan
                        BaseColor colorWithOpacity = new BaseColor(
                            questionBackgroundColor.R,
                            questionBackgroundColor.G,
                            questionBackgroundColor.B,
                            (int)(questionBackgroundAlpha * 255)); // Alfa değeri (0-255)
                        cb.SetColorFill(colorWithOpacity);
                    }
                    else
                    {
                        cb.SetRGBColorFill(255, 255, 255); // Beyaz
                    }

                    // Arka planı doldur
                    cb.Rectangle(currentX, currentY, cellWidth, cellHeight);
                    cb.Fill();

                    int questionNumber = j + 1 + (i * rows); // Soru numarasını hesapla

                    // Soru numarasını yuvarlakların ortasına hizalayarak yaz
                    cb.BeginText();
                    cb.SetRGBColorFill(0, 0, 0); // Siyah renk
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, questionNumber.ToString(), currentX + questionNumberOffsetX, currentY + cellHeight / 2 + questionNumberOffsetY, 0);
                    cb.EndText();

                    // Tüm satırlar için dikdörtgenleri çiz
                    float rectX = 0.5f * 28.35f; // Dikdörtgenin X pozisyonu (kağıdın sol kenarından 0.5 cm uzaklıkta)
                    float rectY = currentY + cellHeight / 2 - (0.15f * 28.35f) / 2; // Dikdörtgenin Y pozisyonu (yüksekliğin yarısını merkezlemek için çıkardık)
                    float rectWidth = 0.4f * 28.35f; // Dikdörtgenin uzun kenarı
                    float rectHeight = 0.15f * 28.35f; // Dikdörtgenin kısa kenarı

                    cb.SetColorFill(BaseColor.BLACK);
                    cb.Rectangle(rectX, rectY, rectWidth, rectHeight);
                    cb.Fill();

                    // Şıklar A, B, C, D, E'yi çiz
                    for (int k = 0; k < numberOfChoices; k++)
                    {
                        float circleX = currentX + 30 + (k * choiceSpacing); // Şıklar arası boşluk 
                        float circleY = currentY + cellHeight / 2;

                        cb.SetLineWidth(circleLineThickness); // Yuvarlakların çizgi kalınlığını ayarla
                        cb.Circle(circleX, circleY, circleRadius);
                        cb.Stroke();

                        cb.BeginText();
                        cb.SetFontAndSize(questionFont, questionFontSize); // Şıkların yazı tipi boyutunu ayarla
                        cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, choices[k], circleX, circleY - (questionFontSize * 0.35f), 0);
                        cb.EndText();
                    }

                }

                // Sütunlar arasındaki siyah çizgi 
                if (i < columns - 1)
                {
                    cb.SetLineWidth(lineThickness); // Çizgi kalınlığını belirle
                    cb.SetRGBColorStroke(lineColor.R, lineColor.G, lineColor.B); // Çizgi rengi
                    cb.MoveTo(x + (i + 1) * (cellWidth + columnSpacing) - columnSpacing / 2, y); // Çizgiyi sütunlar arasında yerleştir
                    cb.LineTo(x + (i + 1) * (cellWidth + columnSpacing) - columnSpacing / 2, y - height);
                    cb.Stroke();
                }
            }

            // Tüm tablonun kenarlarına çizgi eklemek için
            cb.SetLineWidth(lineThickness); // Çerçeve çizgisi için çizgi kalınlığı
            cb.SetRGBColorStroke(borderColor.R, borderColor.G, borderColor.B); // Çizgi rengi
            cb.Rectangle(x, y - height, width, height);
            cb.Stroke();
        }

        private void AddTitle(PdfContentByte cb, float x, float y, float width, float height, string title)
        {
            // Başlık için arka planı çiz
            BaseColor titleColorWithOpacity = new BaseColor(
                titleBackgroundColor.R,
                titleBackgroundColor.G,
                titleBackgroundColor.B,
                (int)(titleBackgroundAlpha * 255)); // Alfa değeri (0-255)
            cb.SetColorFill(titleColorWithOpacity);
            cb.Rectangle(x, y - height, width, height);
            cb.Fill();

            // Başlık metnini yaz
            cb.SetFontAndSize(titleFont, titleFontSize);
            cb.SetRGBColorFill(0, 0, 0); // Siyah renk

            // Başlığın kenarlarını çiz
            
            cb.SetLineWidth(lineThickness); // Kenarlık kalınlığı başlık için de global değişkenle ayarlandı

            // Üst kenarlık
            cb.MoveTo(x, y);
            cb.LineTo(x + width, y);
            cb.Stroke();

            // Sol kenarlık
            cb.MoveTo(x, y);
            cb.LineTo(x, y - height);
            cb.Stroke();

            // Sağ kenarlık
            cb.MoveTo(x + width, y);
            cb.LineTo(x + width, y - height);
            cb.Stroke();

            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, title, x + width / 2, y - height / 2 - 5, 0);
            cb.EndText();
        }
    }
}
