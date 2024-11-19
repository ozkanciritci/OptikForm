using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptikProjeTumSayfa
{
    internal class NotTakeExam
    {
        // Değişkenler
        private float pageWidth = 21.0f * 28.35f;                                           // Sayfa genişliği (A4 kağıt boyutu)
        private float pageHeight = 29.7f * 28.35f;                                          // Sayfa yüksekliği (A4 kağıt boyutu)
        private float tableWidth = 5.2f * 28.35f;                                           // Tablo genişliği 5.2 cm
        private float tableHeight = 5.7f * 28.35f;                                          // Tablo yüksekliği 5.7 cm
        private float titleHeight = 0.85f * 28.35f;                                         // Başlık yüksekliği 0.85 cm

        private float marginLeft = 14.62f * 28.35f;                                         // Sol kenar boşluğu 14.62 cm //x
        private float marginTop = (29.7f - 10.4f) * 28.35f;                                 // Üst kenar boşluğu 10.4 cm aşağıda olmalı //y

        private float rectWidth = 0.4f * 28.35f;                                            // Dikdörtgenin genişliği 0.4 cm
        private float rectHeight = 0.15f * 28.35f;                                          // Dikdörtgenin yüksekliği 0.15 cm
        private float rectStartX = 0.5f * 28.35f;                                           // Dikdörtgenlerin başlangıç noktası (kağıdın sol kenarından 0.5 cm uzaklıkta)
        private float rectStartY = 1.55f * 28.35f;
        private float circleRadius = 0.15f * 28.35f;                                        // Dairelerin yarıçapı 0.15 cm
        private string TableTitle = "ÖĞRENCİ SINAVA GİRMEDİ";                               // Tablo başlığı


        public void CreatePdf(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // PdfContentByte nesnesini oluştur
                PdfContentByte cb = writer.DirectContent;

                // Belirli bir konuma metin ekle

                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(), 9);
                cb.SetRGBColorFill(65, 158, 220); //  mavi
                cb.SetTextMatrix(439, 490);  // X ve Y koordinatları
                cb.ShowText("SALON BAŞKANININ");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(), 9);
                cb.SetRGBColorFill(65, 158, 220); // Acik mavi
                cb.SetTextMatrix(460, 480);  // X ve Y koordinatları
                cb.ShowText("DİKKATİNE!");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(), 6);
                cb.SetRGBColorFill(65, 158, 220); // Acik mavi
                cb.SetTextMatrix(430, 465);  // X ve Y koordinatları
                cb.ShowText("Sinava girmeyen öğrencilerin cevap");
                cb.MoveText(0, -7);
                cb.ShowText("kağıtlarında 'ÖĞRENCi SINAVA'");
                cb.MoveText(0, -8);
                cb.ShowText("GİRMEDİ' bolumunu kurşun kalemle");
                cb.MoveText(0, -8);
                cb.ShowText("doldurunuz");
                cb.EndText();

                cb.BeginText();
                BaseFont bF = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED); 
                iTextSharp.text.Font f = new iTextSharp.text.Font(bF, 5f, iTextSharp.text.Font.NORMAL);

                cb.SetRGBColorFill(65, 158, 220);
                cb.SetTextMatrix(430, 420);  // X ve Y koordinatları
                cb.ShowText("Sinav sonunda, sinava giren ve");
                cb.MoveText(0, -7);
                cb.ShowText("girmeyen tüm öğrencilerin cevap");
                cb.MoveText(0, -8);
                cb.ShowText("kağıtlarında Sıra No'suna göre sıraya");
                cb.MoveText(0, -8);
                cb.ShowText("koyup birlikte paketleyiniz");
                cb.EndText();


                DrawTable(cb, marginLeft, marginTop, tableWidth, tableHeight, TableTitle); // Genişlik 5.2 cm, yükseklik 5.7 cm            


                doc.Close();
            }
        }


        private void DrawTable(PdfContentByte cb, float x, float y, float width, float height, string title)
        {

            cb.SetLineWidth(0.8f);  // Sütun kenarlıkları için inceltilmiş kenarlık
            BaseFont bf1 = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);

            // İlk boşluğun altına düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(1f); // Çizgi kalınlığı
            cb.MoveTo(x, y - titleHeight); // Çizgiyi boş satırın altına yerleştir
            cb.LineTo(x + +width, y - titleHeight);
            cb.Stroke();

            // Sol siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y);
            cb.LineTo(x, y - height + titleHeight - 23);
            cb.Stroke();

            // Sağ siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x + width, y);
            cb.LineTo(x + width, y - height + titleHeight - 23);
            cb.Stroke();

            // İkinci boşluğun altına düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Çizgi kalınlığı
            cb.MoveTo(x, y - titleHeight - 20); // Çizgiyi boş satırın altına yerleştir
            cb.LineTo(x + width, y - titleHeight - 20);
            cb.Stroke();

            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(255, 255, 255); // Beyaz
            cb.Circle(x + 2.6f * 28.35f, y - 35, circleRadius);
            cb.Stroke();

            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(65, 158, 220); // Acik mavi 
            cb.Circle(x + 6.5, y - 80, circleRadius - 0.4);
            cb.Fill();
            cb.Stroke();

            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(65, 158, 220); // Acik mavi 
            cb.Circle(x + 6.5, y - 125, circleRadius - 0.4);
            cb.Fill();
            cb.Stroke();

            // En alta düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Çizgi kalınlığı
            cb.MoveTo(x, y - height + titleHeight - 23); // Çizgiyi boş satırın altına yerleştir
            cb.LineTo(x + width, y - height + titleHeight - 23);
            cb.Stroke();

            // Başlığı ekle
            AddTitle(cb, marginLeft + 2, marginTop + 1, width, titleHeight, title, cb);


        }


        private static void AddTitle(PdfContentByte cb1, float x1, float y1, float width1, float height1, string title1, PdfContentByte cb)
        {

            // Başlık için arka planı çiz
            cb.SetRGBColorFill(173, 216, 230); // Açık Mavi
            cb.Rectangle(x1 - 2, y1 - height1, width1, height1);
            cb.Fill();

            // Başlık metnini yaz
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, 10);
            cb.SetRGBColorFill(0, 0, 0); // Siyah renk

            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, title1, x1 + width1 / 2, y1 - height1 / 2 - 3, 0);
            cb.EndText();

            // Başlığın etrafına kenarlık ekle
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Kenarlık kalınlığı başlık için
            cb.Rectangle(x1 - 2, y1 - height1, width1, height1);
            cb.Stroke();
        }
    }
}