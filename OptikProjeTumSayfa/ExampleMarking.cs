using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Ocsp;
using System.Xml.Linq;

namespace OptikProjeTumSayfa
{
    internal class ExampleMarking
    {
        // Değişkenler
        private float pageWidth = 21.0f * 28.35f;                                                   // Sayfa genişliği (A4 kağıt boyutu)
        private float pageHeight = 29.7f * 28.35f;                                                  // Sayfa yüksekliği (A4 kağıt boyutu)
        private float tableWidth = 5.2f * 28.35f;                                                   // Tablo genişliği 5.2 cm
        private float tableHeight = 3f * 28.35f;                                                    // Tablo yüksekliği 3 cm
        private float titleHeight = 0.85f * 28.35f;                                                 // Başlık yüksekliği 0.85 cm

        private float marginLeft = 14.62f * 28.35f;                                                 // Sol kenar boşluğu 14.62 cm
        private float marginTop = (29.7f - 16.2f) * 28.35f;                                         // Üst kenar boşluğu 21.72 cm aşağıda olmalı

        private float circleRadius = 0.15f * 28.35f;                                                // Dairelerin yarıçapı 0.15 cm
        private string TableTitle = "";                                                             // Tablo başlığı
        private string secondTableTitle = "Yanlis Kodlamalar";                                      // Tablo başlığı

        public void CreatePdf(string filePath)
        {



            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.Read))
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

                cb.SetRGBColorFill(65, 158, 220); // Mavi
                cb.SetTextMatrix(445, 160);  // X ve Y koordinatları
                cb.ShowText("ÖRNEK");
                cb.EndText();

                cb.BeginText();
                BaseFont bF = BaseFont.CreateFont("C:\\windows\\fonts\\arial.ttf", "windows-1254", true);
                iTextSharp.text.Font f = new iTextSharp.text.Font(bF, 7f, iTextSharp.text.Font.NORMAL);
                cb.SetRGBColorFill(0, 0, 0); // Siyah
                cb.SetTextMatrix(440, 192);  // X ve Y koordinatları
                cb.ShowText("Cevaplarınızı aşağıdaki");
                cb.MoveText(0, -8);
                cb.ShowText("örnekteki gibi işaretleyiniz");
                cb.EndText();



                // İlk tabloyu çiz
                DrawTable(cb, marginLeft, marginTop, tableWidth, tableHeight, TableTitle); // Genişlik 5.2 cm, yükseklik 5.7 cm 

                // İkinci tabloyu çiz
                DrawTable1(cb, marginLeft, marginTop + 3, tableWidth, tableHeight - 2, secondTableTitle);

                doc.Close();
            }

        }

        private void DrawTable(PdfContentByte cb, float x, float y, float width, float height, string title)
        {




            // Sol siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 175);
            cb.LineTo(x, y - 250);
            cb.Stroke();

            // Sağ siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x + width, y - 175);
            cb.LineTo(x + width, y - 250);
            cb.Stroke();

            // Ust cizgi
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 175);
            cb.LineTo(x + width, y - 175);
            cb.Stroke();

            // Proje klasöründeki resim dosyasının yolunu dinamik olarak ayarlayın
            string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "El.jpg");
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);
            // Resmin konumunu ayarlayın (x, y koordinatları)
            img.ScaleAbsolute(40, 40);
            img.SetAbsolutePosition(498, 138);

            // Resmi cb nesnesi ile PDF sayfasına ekleyin
            cb.AddImage(img);


            // Cember ciz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.5f); // Çizgi kalınlığı
            cb.SetRGBColorFill(0, 0, 0); //  siyah
            cb.Circle(x + 85, 158, circleRadius);
            cb.Fill();
            cb.Stroke();



            // En alta düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Çizgi kalınlığı
            cb.MoveTo(x, y - 250);
            cb.LineTo(x + width, y - 250);
            cb.Stroke();



        }
        private void DrawTable1(PdfContentByte cb, float x, float y, float width, float height, string title)
        {
            // Sol siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 270);
            cb.LineTo(x, y - 330);
            cb.Stroke();

            // Sağ siyah kenarlığı ekle
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x + width, y - 270);
            cb.LineTo(x + width, y - 330);
            cb.Stroke();

            // Ust cizgi
            cb.SetLineWidth(0.8f);
            cb.MoveTo(x, y - 270);
            cb.LineTo(x + width, y - 270);
            cb.Stroke();

            // En alta düz bir çizgi çiz
            cb.SetRGBColorStroke(0, 0, 0); // Siyah renk
            cb.SetLineWidth(0.8f); // Çizgi kalınlığı
            cb.MoveTo(x, y - 330);
            cb.LineTo(x + width, y - 330);
            cb.Stroke();


            // Belirli bir konuma metin ekle
            cb.BeginText();
            cb.SetFontAndSize(BaseFont.CreateFont(), 9);
            cb.SetRGBColorFill(65, 158, 220); // Mavi
            cb.SetTextMatrix(435, 95);  // X ve Y koordinatları
            cb.ShowText("YANLIS KODLAMALAR");
            cb.EndText();

            // Proje klasöründeki resim dosyasının yolunu dinamik olarak ayarlayın
            string imagePath1 = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Optik.jpg");
            iTextSharp.text.Image img1 = iTextSharp.text.Image.GetInstance(imagePath1);
            // Resmin konumunu ayarlayın (x, y koordinatları)
            img1.ScaleAbsolute(70, 30);
            img1.SetAbsolutePosition(448, 57);

            // Resmi cb nesnesi ile PDF sayfasına ekleyin
            cb.AddImage(img1);



        }
    }
}