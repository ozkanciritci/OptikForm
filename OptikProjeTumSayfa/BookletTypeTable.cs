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
            float remainingHeight = height - titleHeight; // Kalan yükseklik
            float cellHeight = remainingHeight / leftrows; // Her bir hücre için kalan yükseklik

            // Başlığı ekle
            // Sol başlığı ekle (KİTAPÇIK TÜRÜ)
            float leftTitleX = x;
            AddTitle(cb, leftTitleX, y, columnDistance, titleHeight, tableNames["leftTitle"][0], tableNames["leftTitle"][1]);

            // Sağ başlığı ekle (GOZETMEN PARAF)
            float rightTitleX = x + columnDistance;
            AddTitle(cb, rightTitleX, y, width - columnDistance, titleHeight, tableNames["rightTitle"][0], tableNames["rightTitle"][1]);




            // Kenarlıklar için çizgi kalınlığını global değişkenle ayarla
            cb.SetLineWidth(lineWidth);
            BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);
            cb.SetFontAndSize(bf, circleFontSize); // Yazı tipi boyutunu artırdık

            // Tablo hücrelerini çiz (beyaz arka plan, satır çizgileri yok)
            y -= titleHeight; // Yüksekliği başlık yüksekliği kadar azalt

            // Sol kenarlık
            cb.SetLineWidth(borderLineWidth);
            cb.MoveTo(x, y);
            cb.LineTo(x, y - height + titleHeight);
            cb.Stroke();

            // Sağ kenarlık
            cb.MoveTo(x + width, y);
            cb.LineTo(x + width, y - height + titleHeight);
            cb.Stroke();

            // Üst kenarlık
            cb.MoveTo(x, y);
            cb.LineTo(x + width, y);
            cb.Stroke();

            // Sol köşeden 1.4 cm uzaklıkta dikey bir çizgi ekle
            float lineX = x + columnDistance;
            cb.SetLineWidth(lineWidth);
            cb.MoveTo(lineX, y+ titleHeight);
            cb.LineTo(lineX, y - height + titleHeight);
            cb.Stroke();

            // Sol kısmı 11 eşit parçaya böl ve her satır arasına çizgi ekle
            float leftSectionHeight = height - titleHeight;
            float rowHeight = leftSectionHeight / 11;

            int letterIndex = 0; // Harf dizisindeki ilerlemeyi izlemek için

            for (int i = 1; i <= 11; i++)
            {
                float currentY = y - i * rowHeight;

                // Satırın rengini ayarla (şeritli desen)
                bool isBlueBackground = i % 2 == 0;

                if (isBlueBackground)
                {
                    cb.SetRGBColorFill(blueBackgroundColor[0], blueBackgroundColor[1], blueBackgroundColor[2]); // Açık Mavi
                }
                else
                {
                    cb.SetRGBColorFill(whiteBackgroundColor[0], whiteBackgroundColor[1], whiteBackgroundColor[2]); // Beyaz
                }

                cb.Rectangle(x, currentY, columnDistance, rowHeight);
                cb.Fill();

                cb.SetLineWidth(lineWidth);  // Satır arası çizgiler için de global çizgi kalınlığını kullan
                cb.MoveTo(x, currentY);
                cb.LineTo(lineX, currentY); // Çizgiyi sadece sol kısım boyunca çiz
                cb.Stroke();

                // Sadece açık mavi arka plana sahip satırlara daire ve harf ekle
                if (isBlueBackground && letterIndex < letters.Length)
                {
                    // Daireyi çiz
                    float circleX = x + columnDistance / 2;
                    float circleY = currentY + rowHeight / 2;

                    cb.SetLineWidth(circleLineWidth); // Dairenin çizgi kalınlığını ayarla
                    cb.Circle(circleX, circleY, circleRadius); // Yarıçap
                    cb.Stroke();

                    // Dairenin sağ tarafına ilgili harfi ekle (sırasıyla A, B, C, D, E)
                    string letter = letters[letterIndex]; // Harfi seç
                    float textX = circleX + circleRadius; // Daireden sağa
                    float textY = circleY - (circleFontSize / 2); // Yazıyı dairenin ortasında hizalamak için küçük bir ofset

                    cb.SetRGBColorFill(blackColor[0], blackColor[1], blackColor[2]); // Siyah renk

                    cb.BeginText();
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, letter, textX, textY, 0);
                    cb.EndText();

                    letterIndex++; // Sonraki harfe geç
                }
            }

            // Sağ kısmı 5 eşit parçaya böl ve her parçanın arka planını beyaz yap
            float rightSectionHeight = height - titleHeight;
            float rightRowHeight = rightSectionHeight / rightsidecell;
            float rightX = x + width - rightSectionWidth;

            for (int i = 1; i <= 5; i++)
            {
                float currentY = y - i * rightRowHeight;

                cb.SetRGBColorFill(whiteBackgroundColor[0], whiteBackgroundColor[1], whiteBackgroundColor[2]); // Beyaz arka plan
                cb.Rectangle(rightX, currentY, rightSectionWidth, rightRowHeight);
                cb.Fill();

                cb.SetLineWidth(lineWidth);  // Satır arası çizgiler için de global çizgi kalınlığını kullan
                cb.MoveTo(rightX, currentY);
                cb.LineTo(x + width, currentY); // Çizgiyi sağ kısım boyunca çiz
                cb.Stroke();
            }

            // 1.4 cm ile 0.9 cm'lik alanları ayıran orta çizgiyi ekle
            float middleX = lineX + (rightX - lineX) / 2;
            cb.SetLineWidth(middleLineWidth); // Çizgi kalınlığını tekrar ayarla
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
        }

    }
}
