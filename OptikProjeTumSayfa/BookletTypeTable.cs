using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;




namespace OptikProjeTumSayfa
{
    public class BookletTypeTable
    {
        //  değişkenler
        private float titleHeight = 0.9f * 28.35f;                                                        // Başlık yüksekliği
        private float marginTop = (29.7f - 10.4f) * 28.35f;                                               // Üst kenar boşluğu
        private int circleFontSize = 8;                                                                   // Yuvarlak içindeki sayılar için yazı tipi boyutu
        private float columnDistance = 1.4f * 28.35f;                                                     // Sol kenardan uzaklık
        private float rightSectionWidth = 0.9f * 28.35f;                                                  // Sağ alan genişliği
        private float lineWidth = 1f;                                                                     // Çizgi kalınlığı
        private float circleLineWidth = 0.5f;                                                             // Daire kenar çizgi kalınlığı
        private float circleRadius = 0.15f * 28.35f;                                                      // Daire yarıçapı
        private int rightsidecell = 5;                                                                    // Sağ kısımdaki hücre sayısı
        private int leftrows = 10;                                                                        // Sol kısımdaki satır sayısı
        //private string lefttableName = "KITAPCIK TURU ";
        //private string righttableName = " GOZETMEN PARAF";                                              // Tablo ismi
        //string[] tableName = { "KITAPCIK", "TURU", "GOZETMEN", "PARAF" };
       private Dictionary<string, string[]> tableNames = new Dictionary<string, string[]>
        {
            { "leftTitle", new string[] { "KİTAPÇIK", "TÜRÜ" } },
            { "rightTitle", new string[] { "GÖZETMEN", "PARAF" } }
        };

        private string[] letters = { "A", "B", "C", "D", "E" };                                           // Kitapçık türleri dizisi
        private float titleFontSize = 4;                                                                  // Başlık yazı tipi boyutu
        private float titleTextOffset = 3f;                                                               // Başlık metin ofseti

        //  renk değişkenleri
        private int[] titleBackgroundColor = { 173, 216, 230 };                                           // Başlık arka plan rengi (açık mavi)
        private int[] blueBackgroundColor = { 220, 240, 255 };                                            // Satırların açık mavi arka planı
        private int[] whiteBackgroundColor = { 255, 255, 255 };                                           // Beyaz arka plan
        private int[] blackColor = { 0, 0, 0 };                                                           // Siyah renk

        // Başlık kenarları
        private float borderLineWidth = 2f;                                                               // Sol ve sağ kenar çizgisi kalınlığı
        private float middleLineWidth = 1f;                                                               // Orta çizgi kalınlığı

        public void DrawTable(PdfContentByte cb, float x, float y, float width, float height)
        {
            float remainingHeight = height - titleHeight;
            float cellHeight = remainingHeight / leftrows;

            // 📌 **Dış Kenarlıkları Tek Seferde Çiziyoruz**
            cb.SetLineWidth(borderLineWidth);
            cb.Rectangle(x, y - height, width, height);
            cb.Stroke();

            // Başlıkları çiz
            float leftTitleX = x;
            AddTitle(cb, leftTitleX, y, columnDistance, titleHeight, tableNames["leftTitle"][0], tableNames["leftTitle"][1]);

            float rightTitleX = x + columnDistance;
            AddTitle(cb, rightTitleX, y, width - columnDistance, titleHeight, tableNames["rightTitle"][0], tableNames["rightTitle"][1]);

            // 🔹 **Başlıkların Altına Siyah Çizgi Ekliyoruz**
            float bottomTitleY = y - titleHeight;
            cb.SetLineWidth(lineWidth);
            cb.MoveTo(x, bottomTitleY);
            cb.LineTo(x + width, bottomTitleY);
            cb.Stroke(); // 📌 **Bu Çizgi Mavi Alan ile Beyaz Alanı Ayırıyor!**

            // 🔹 **İç Çizgileri Çiz**
            cb.SetLineWidth(lineWidth);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, circleFontSize);

            y -= titleHeight;

            // 🔹 **Orta Çizgi (Sol ve Sağ Tabloyu Ayıran Çizgi)**
            float lineX = x + columnDistance;
            cb.MoveTo(lineX, y + titleHeight);
            cb.LineTo(lineX, y - height + titleHeight);
            cb.Stroke();

            float leftSectionHeight = height - titleHeight;
            float rowHeight = leftSectionHeight / 11;
            int letterIndex = 0;

            for (int i = 1; i <= 11; i++)
            {
                float currentY = y - i * rowHeight;
                bool isBlueBackground = i % 2 == 0;

                if (isBlueBackground)
                    cb.SetRGBColorFill(blueBackgroundColor[0], blueBackgroundColor[1], blueBackgroundColor[2]);
                else
                    cb.SetRGBColorFill(whiteBackgroundColor[0], whiteBackgroundColor[1], whiteBackgroundColor[2]);

                cb.Rectangle(x, currentY, columnDistance, rowHeight);
                cb.Fill();

                cb.SetLineWidth(lineWidth);
                cb.MoveTo(x, currentY);
                cb.LineTo(lineX, currentY);
                cb.Stroke();

                if (isBlueBackground && letterIndex < letters.Length)
                {
                    float circleX = x + columnDistance / 2;
                    float circleY = currentY + rowHeight / 2;
                    cb.SetLineWidth(circleLineWidth);
                    cb.Circle(circleX, circleY, circleRadius);
                    cb.Stroke();

                    string letter = letters[letterIndex];
                    float textX = circleX + circleRadius;
                    float textY = circleY - (circleFontSize / 2);
                    cb.SetRGBColorFill(blackColor[0], blackColor[1], blackColor[2]);

                    cb.BeginText();
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, letter, textX, textY, 0);
                    cb.EndText();

                    letterIndex++;
                }
            }

            // 🔹 **Sağ Kısım Hücreleri**
            float rightSectionHeight = height - titleHeight;
            float rightRowHeight = rightSectionHeight / rightsidecell;
            float rightX = x + width - rightSectionWidth;

            for (int i = 1; i <= 5; i++)
            {
                float currentY = y - i * rightRowHeight;
                cb.SetRGBColorFill(whiteBackgroundColor[0], whiteBackgroundColor[1], whiteBackgroundColor[2]);
                cb.Rectangle(rightX, currentY, rightSectionWidth, rightRowHeight);
                cb.Fill();

                cb.SetLineWidth(lineWidth);
                cb.MoveTo(rightX, currentY);
                cb.LineTo(x + width, currentY);
                cb.Stroke();
            }

            // 🔹 **Orta Çizgi (Kitapçık Türü ile Gözetmen Paraf Ayrımı)**
            float middleX = lineX + (rightX - lineX) / 2;
            cb.SetLineWidth(middleLineWidth);
            cb.MoveTo(middleX, y);
            cb.LineTo(middleX, y - height + titleHeight);
            cb.Stroke();
        }





        private void AddTitle(PdfContentByte cb, float x, float y, float width, float height, string titleTop, string titleBottom)
        {
            // Başlık için arka planı çiz
            cb.SetRGBColorFill(titleBackgroundColor[0], titleBackgroundColor[1], titleBackgroundColor[2]); // Açık Mavi
            cb.Rectangle(x, y - height, width, height);
            cb.Fill();

            // Başlık metni yazı tipi ve rengi ayarla
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1254" , BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, titleFontSize);
            cb.SetRGBColorFill(blackColor[0], blackColor[1], blackColor[2]); // Siyah renk

            // İlk satır ("KİTAPÇIK") metni ortalı yazdır
            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titleTop, x + width / 2, y - (height / 3), 0); // Yüksekliğin üst kısmına hizala
            cb.EndText();

            // İkinci satır ("TÜRÜ") metni ortalı yazdır
            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titleBottom, x + width / 2, y - (2 * height / 3), 0); // Yüksekliğin alt kısmına hizala
            cb.EndText();

            // Başlığın kenarlarını çiz
            cb.SetRGBColorStroke(blackColor[0], blackColor[1], blackColor[2]); // Siyah renk
            cb.SetLineWidth(lineWidth); // Kenarlık kalınlığı başlık için de global değişkenle ayarlandı

           
        }

    }
}
