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
    internal class Attention
    {
        // Değişkenler
        private float pageWidth = 21.0f * 28.35f;                                                                           // Sayfa genişliği (A4 kağıt boyutu)
        private float pageHeight = 29.7f * 28.35f;                                                                          // Sayfa yüksekliği (A4 kağıt boyutu)
        private float tableWidth = 5.2f * 28.35f;                                                                           // Tablo genişliği 5.2 cm
        private float tableHeight = 5.61f * 28.35f;                                                                         // Tablo yüksekliği 5.61 cm
        private float titleHeight = 0.85f * 28.35f;                                                                         // Başlık yüksekliği 0.85 cm

        private float marginLeft = 14.62f * 28.35f;                                                                         // Sol kenar boşluğu 14.62 cm //x
        private float marginTop = (29.7f - 16.2f) * 28.35f;                                                                 // Üst kenar boşluğu 16.2 cm aşağıda olmalı //y

        private float circleRadius = 0.15f * 28.35f;                                                                        // Dairelerin yarıçapı 0.15 cm
        private string TableTitle = "DİKKAT";                                                                               // Tablo başlığı


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
                BaseFont bf = BaseFont.CreateFont(@"C:\Windows\Fonts\Arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                cb.SetFontAndSize(bf, 9);
                cb.SetRGBColorFill(65, 158, 220); //  mavi
                cb.SetTextMatrix(465, 355);  // X ve Y koordinatları
                cb.ShowText("DİKKAT");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(bf, 6);
                cb.SetRGBColorFill(65, 158, 220); //  mavi
                cb.SetTextMatrix(434, 338);  // X ve Y koordinatları
                cb.ShowText("TC Kimlik No veya Ögrenci No'yu");
                cb.MoveText(0, -7);
                cb.ShowText("mutlaka kodlayiniz");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(), 6);
                cb.SetRGBColorFill(65, 158, 220); //  mavi
                cb.SetTextMatrix(434, 308);  // X ve Y koordinatları
                cb.ShowText("Kitapçik türü'nü kodlamayı");
                cb.MoveText(0, -7);
                cb.ShowText("unutmayınız");
                cb.EndText();

                cb.BeginText();
                cb.SetFontAndSize(BaseFont.CreateFont(), 6);
                cb.SetRGBColorFill(65, 158, 220); //  mavi
                cb.SetTextMatrix(434, 278);  // X ve Y koordinatları
                cb.ShowText("Optik form üzerindeki tüm");
                cb.MoveText(0, -7);
                cb.ShowText("kodlamalarinizda kurşun");
                cb.MoveText(0, -8);
                cb.ShowText("kalem ve yumuşak silgi");
                cb.MoveText(0, -8);
                cb.ShowText("kullaniniz");
                cb.EndText();


                DrawTable(cb, marginLeft, marginTop, tableWidth, tableHeight, TableTitle); // Genişlik 5.2 cm, yükseklik 5.7 cm            


                doc.Close();
            }
        }
        private void DrawTable(PdfContentByte cb, float x, float y, float width, float height, string title)
        {


            // Sol siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 14);
            cb.LineTo(x, y - height - 2);
            cb.Stroke();

            // Sağ siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x + width, y - 14);
            cb.LineTo(x + width, y - height - 2);
            cb.Stroke();

            // Ust cizgi
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 14);
            cb.LineTo(x + width, y - 14);
            cb.Stroke();



            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(65, 158, 220); //  mavi 
            cb.Circle(x + 6.5, 278, circleRadius - 0.4);
            cb.Fill();
            cb.Stroke();

            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(65, 158, 220); // mavi 
            cb.Circle(x + 6.5, 308, circleRadius - 0.4);
            cb.Fill();
            cb.Stroke();

            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(65, 158, 220); // mavi 
            cb.Circle(x + 6.5, 338, circleRadius - 0.4);
            cb.Fill();
            cb.Stroke();

            // En alta düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Çizgi kalınlığı
            cb.MoveTo(x, y - height - 2);
            cb.LineTo(x + width, y - height - 2);
            cb.Stroke();



        }
    }
}