﻿using System;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace OptikProjeTumSayfa
{
    public  class StudentInformationTable
    {
        public static float secondTableHeight = 0.7f * 28.35f; // 0.7 cm = 19.845 pt
        

        public static int xgetir = 5;
        // Genel değişkenler
        private string tableTitle = "ÖĞRENCİ BİLGİLERİ";                                                                                 // Başlık
        private BaseColor titleBackgroundColor = new BaseColor(173, 216, 230);                                                           // Başlık arka plan rengi
        private float titleBackgroundOpacity = 1.0f;                                                                                     // Başlık arka plan saydamlığı
        private float borderThickness = 1f;                                                                                              // Çizgi kalınlığı
        private BaseColor borderColor = BaseColor.BLACK;                                                                                 // Çizgi rengi
        private BaseColor tableBorderColor = BaseColor.BLACK;                                                                            // Çerçeve rengi
                                                                                                                                        // Başlık yüksekliği
        public static float topMargin = 3.2f * 28.35f;                                                                                   // 3.2 cm üst kenar boşluğu
        public static float leftMargin = 1.3f * 28.35f;                                                                                  // 1.3 cm sol kenar boşluğu   
        private float logotopMargin = 2.8f * 28.35f;                                                                                     // logo 1.8 cm üst kenar boşluğu
        private float logoleftMargin = 2.5f * 28.35f;                                                                                    //logo 2.5 cm sol kenar boşluğu
        private BaseFont headerFont = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1254", BaseFont.NOT_EMBEDDED);                     // Başlık yazı tipi
        private int headerFontSize = 10;                                                                                                 // Başlık yazı tipi boyutu
        private BaseFont bodyFont = BaseFont.CreateFont(BaseFont.HELVETICA,"Cp1254", BaseFont.NOT_EMBEDDED);                             // Metin yazı tipi
        private int bodyFontSize = 8;                                                                                                    // Metin yazı tipi boyutu
        private iTextSharp.text.Rectangle pageSize = PageSize.A4;                                                                        // Sayfa boyutu
        public static BaseFont baseFont = BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);
        string[] studentInformation = {
                    "Asağıdaki yerleri büyük harflerle doldurun ve imzalayın","ADI SOYADI        :", "ÖĞRENCİ NO     :", "TC KİMLİK NO    :", "BÖLÜM / ABD     :"
                };

    
        public static int rows = 5;
        // Tablo ölçüleri (cm'yi noktaya çevirdik)
        public float tableWidth = 12.5f * 28.35f; // 12.5 cm genişlik
        public static float tableHeight = 3.5f * 28.35f; // 4.1 cm yükseklik
        public float secondTableSpacing = 0.2f * 28.35f;  // 0.2 cm = 5.67 pt
        public float headerHeight = 0.6f * 28.35f;
        public float lineX = 4.2f * 28.35f;
        public float rightTablesWidth = 5.2f * 28.35f;  // 5.2 cm genişlik
        public float rightTablesHeight = 0.8f * 28.35f;  // 0.8 cm yükseklik
        public float rightTablesSpacing = 1.425f * 28.35f;  // 1.5 cm boşluk
        public float rightTablesTextOffset = 12f; // Tablo ile metin arasındaki mesafe (12 pt = yaklaşık 0.42 cm)


        // PDF oluşturma fonksiyonu
        public void CreatePdf(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(pageSize);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                PdfContentByte cb = writer.DirectContent;

                

                // İlk tablonun üstten 3.2 cm boşlukla başlayacak
                float initialY = pageSize.Height - topMargin;
                DrawCustomTable(cb, leftMargin, initialY, tableWidth, tableHeight, studentInformation, rows);

                // Sağ tarafa 0.7 cm boşluk ve 3 yeni tablo çiz
                // Sağdaki tablolar, sol taraftaki ilk tablonun başlangıç noktasından 0.5 cm aşağıda başlasın
                DrawRightTables(cb, leftMargin + tableWidth + (0.7f * 28.35f), initialY - 4.5f - (0.5f * 28.35f));

                // Basliklari yazdırmak
                string[] texts = { "GAZİ ÜNİVERSİTESİ", "GAZİ EĞİTİM FAKÜLTESİ", "SINAV CEVAP KAĞIDI" };

                // Renk olarak siyah kullanıyoruz, 12 pt font boyutu, kalın metin istiyoruz
                BaseColor textColor = BaseColor.BLACK;
                DrawCenteredText(cb, texts, 12, textColor, true);

                // Masaüstünde bulunan resim yolu
                string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "\\logo.jpg");

                // Resmi PDF'e eklemek için bu fonksiyonu çağırıyoruz
                AddImageToPdf(cb, imagePath, logoleftMargin, pageSize.Height - logotopMargin, (2f * 28.35f), (2f * 28.35f));


                doc.Close();
            }
        }
       
     
        private void DrawCustomTable(PdfContentByte cb, float x, float y, float width, float height, string[] studentInformation, int rows)
        {

            // Başlığı ekle
            AddTableHeader(cb, x, y, width, headerHeight, tableTitle);

            y -= headerHeight; // Başlık alanı çıkarıldı



            cb.SetLineWidth(borderThickness);
            cb.SetFontAndSize(bodyFont, bodyFontSize);

            // Başlığın altına çizgi çiz
            cb.SetLineWidth(borderThickness); // Çizgi kalınlığı
            cb.MoveTo(x, y);  // Başlığın altına çizgi ekle
            cb.LineTo(x + width, y);
            cb.Stroke();

            // Hücre çizimi (dinamik olarak satır ve sütun sayısına göre ayarlandı)
            float firstColumnWidth = 9.6f * 28.35f;  // 9.6 cm
            float secondColumnWidth = 2.9f * 28.35f;  // 2.9 cm
            float cellHeight = height / rows;  // Hücre yüksekliği satır sayısına bağlı

            int studentIndex = 0; // Öğrenci bilgileri için sayaç
           

            // İlk sütun satırlara bölünecek ve her satırda ayrı hücreler olacak
            for (int row = 0; row < rows; row++)
            {
                // Satırın başlangıç Y konumu
                float currentY = y - (row + 1) * cellHeight;
               

                // İlk sütunu çiz (satır satır)
                cb.Rectangle(x, currentY, firstColumnWidth, cellHeight);
                cb.Stroke();

                // Öğrenci ismini ilk sütuna yaz
                if (studentIndex < studentInformation.Length)
                {
                    cb.BeginText();
                    cb.SetRGBColorFill(0, 0, 0); // Siyah renk
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, studentInformation[studentIndex], x + 2, currentY + cellHeight / 2 - 5, 0);
                    cb.EndText();
                    studentIndex++;

                   
                }
            }



            // İkinci sütunu tek bir hücre olarak çiz (tüm satırları kapsayacak)
            cb.Rectangle(x + firstColumnWidth, y - height, secondColumnWidth, height);
            cb.Stroke();

            // İkinci sütuna "İmza" yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla

            // İmza yazısını ortalamak için x + firstColumnWidth ile ikinci sütunun başlangıcına gidiyoruz.
            // x + firstColumnWidth + secondColumnWidth / 2 ile sütunun ortasına yerleştiriyoruz.
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, "İMZA", x + firstColumnWidth + secondColumnWidth / 2, y - 28.35f, 0);

            cb.EndText();


            // İkinci tabloyu çizmek için 0.2 cm boşluk bırak
           
            y -= (height + secondTableSpacing);

            // İkinci tabloyu çiz (yüksekliği 0.7 cm)
            
            cb.Rectangle(x, y - secondTableHeight, width, secondTableHeight);
            cb.Stroke();  // Tabloyu çiz

            // İkinci tabloya "SINAV ADI" yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla
             
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "SINAV ADI", x + 5, y - (secondTableHeight / 2) - 5, 0);  // Yazıyı tablonun solunda hizala
            cb.EndText();
            

        // İkinci tabloya ait y koordinatları
        float secondTableTopY = y;  // Tablo üst kısmı
            float secondTableBottomY = y - secondTableHeight;  // Tablo alt kısmı

            // Soldan 2.8 cm uzaklık hesaplaması (1 cm = 28.35 pt)
              // Satır çizgisinin x koordinatı

            // İkinci tabloya satır ekle (2.8 cm uzaklıkta)
            cb.MoveTo(lineX, secondTableTopY);  // Çizgiyi başlat (2. tablo üstü)
            cb.LineTo(lineX, secondTableBottomY);  // Çizgiyi bitir (2. tablo altı)
            cb.Stroke();  // Çizgiyi çiz

            // Üçüncü tabloyu çizmek için tekrar 0.2 cm boşluk bırak
            y -= (secondTableHeight + secondTableSpacing);  // Y koordinatını güncelle

            // Üçüncü tabloyu çiz (yüksekliği 0.7 cm)
            cb.Rectangle(x, y - secondTableHeight, width, secondTableHeight);
            cb.Stroke();  // Tabloyu çiz

            // Üçüncü tabloya "SINAV TARİHİ" yazısını ekle
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);  // Yazı fontu ve boyutu ayarla
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, "SINAV TARİHİ", x + 5, y - (secondTableHeight / 2) - 5, 0);  // Yazıyı tablonun solunda hizala
            cb.EndText();

            // Üçüncü tabloya ait y koordinatları
            float thirdTableTopY = y;  // Tablo üst kısmı
            float thirdTableBottomY = y - secondTableHeight;  // Tablo alt kısmı

            // Üçüncü tabloya satır ekle (2.8 cm uzaklıkta)
            cb.MoveTo(lineX, thirdTableTopY);  // Çizgiyi başlat (3. tablo üstü)
            cb.LineTo(lineX, thirdTableBottomY);  // Çizgiyi bitir (3. tablo altı)
            cb.Stroke();  // Çizgiyi çiz



        }

        // Sağ tarafta 3 tablo çizme fonksiyonu
        private void DrawRightTables(PdfContentByte cb, float x, float y)
        {
            

            // Tabloların başlıkları
            string[] titles = { "BÖLÜMÜ", "SALON NO", "SIRA NO" };

            // 3 tabloyu döngü ile çiz
            for (int i = 0; i < titles.Length; i++)
            {
                // Tablo başlığını ekle
                cb.BeginText();
                cb.SetFontAndSize(baseFont, 10);  // Yazı fontu ve boyutu ayarla
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titles[i], x + rightTablesWidth / 2, y + rightTablesTextOffset, 0);  // Metni tablo üstüne ortala
                cb.EndText();

                // Tabloyu çiz
                cb.Rectangle(x, y - rightTablesHeight, rightTablesWidth, rightTablesHeight);
                cb.Stroke();

                // Bir sonraki tabloya geçmeden önce Y koordinatını güncelle
                y -= (rightTablesHeight + rightTablesSpacing);
            }
        }



        // Başlık ekleme fonksiyonu
        private void AddTableHeader(PdfContentByte cb, float x, float y, float width, float height, string title)
        {
            // Başlık için arka planı çiz
            BaseColor headerBackgroundColor = new BaseColor(
                titleBackgroundColor.R,
                titleBackgroundColor.G,
                titleBackgroundColor.B,
                (int)(titleBackgroundOpacity * 255)); // Alfa değeri (0-255)
            cb.SetColorFill(headerBackgroundColor);
            cb.Rectangle(x, y - height, width, height);
            cb.Fill();

            // Başlık metnini yaz
            cb.SetFontAndSize(headerFont, headerFontSize);
            cb.SetRGBColorFill(0, 0, 0); // Siyah renk

            cb.BeginText();
            cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, title, x + width / 2, y - height / 2 - 5, 0);
            cb.EndText();

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


        // Metinleri üstten 1.1 cm aşağıda ve sayfanın ortasında alt alta yazdıran fonksiyon
        private void DrawCenteredText(PdfContentByte cb, string[] texts, int fontSize, BaseColor textColor, bool isBold)
        {
            float marginTop = 1.1f * 28.35f; // 1.1 cm'yi pt cinsine çeviriyoruz (28.35 pt = 1 cm)
            float pageWidth = pageSize.Width; // Sayfa genişliği
            float centerX = pageWidth / 2; // Sayfanın ortası (X ekseni)

            float y = pageSize.Height - marginTop; // İlk metnin Y koordinatı (sayfanın üstünden 1.1 cm aşağı)

            // Yazı tipi ayarı: Kalın (bold) metin kullanılıp kullanılmayacağını belirle
            BaseFont selectedFont = isBold
                ? BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, "Cp1254", BaseFont.NOT_EMBEDDED)
                : BaseFont.CreateFont(BaseFont.HELVETICA, "Cp1254", BaseFont.NOT_EMBEDDED);

            // Yazıları alt alta yazdır
            for (int i = 0; i < texts.Length; i++)
            {
                cb.BeginText();
                cb.SetFontAndSize(selectedFont, fontSize);  // Yazı tipi ve boyutu ayarla

                // Yazı rengini ayarla
                cb.SetRGBColorFill(textColor.R, textColor.G, textColor.B);  // Renk ayarı

                // Metni ortala ve yaz
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, texts[i], centerX, y, 0);

                cb.EndText();

                // Bir sonraki metin için Y koordinatını azalt (15 pt aşağı kaydırıyoruz, yazı boyutuna göre ayarlanabilir)
                y -= (fontSize + 5);  // fontSize + 5 ile satırlar arasındaki boşluğu yazı büyüklüğüne göre ayarlıyoruz
            }
        }

        // Resim ekleme fonksiyonu
        private void AddImageToPdf(PdfContentByte cb, string imagePath, float x, float y, float width, float height)
        {


            // Resim yükle
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(imagePath);

            // Resim boyutunu ayarla
            img.ScaleToFit(width, height);

            // Resmin konumunu ayarla (sol alt köşe x, y pozisyonları)
            img.SetAbsolutePosition(x, y);

            // PDF içeriğine resmi ekle
            cb.AddImage(img);
        }








    }
}
