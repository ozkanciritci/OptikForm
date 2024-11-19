
using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;


namespace OptikProjeTumSayfa
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Text = "PDF Tablo Oluşturucu";
            this.Width = 600;
            this.Height = 400;

            // PDF oluşturmayı tetiklemek için bir buton oluştur
            Button btnCreatePdf = new Button();
            btnCreatePdf.Text = "PDF Oluştur";
            btnCreatePdf.Location = new Point(10, 10);
            btnCreatePdf.Click += button1_Click;

            this.Controls.Add(btnCreatePdf);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Tek bir PDF dosyası için dosya yolu
            string filePath = @"C:\Users\Ozkan\Desktop\OptikForm.pdf";

            // İlk olarak AnswersTable tablosunu oluştur
            AnswersTable answersTable = new AnswersTable();
            answersTable.CreatePdf(filePath);
            //MessageBox.Show($"AnswersTable PDF oluşturuldu: {filePath}");



            // Şimdi mevcut PDF'yi açıp üzerine  ekle
            AddNumberedTableToExistingPdf(filePath);
            AddNotTakeExamPdf(filePath);
            AddAttentionPdf(filePath);
            AddExamplePdf(filePath);
            AddStudentInformatinTablePdf(filePath);
            MessageBox.Show($"optik form oluşturuldu");
        }



        private void AddNumberedTableToExistingPdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = new PdfStamper(pdfReader, fs);
                    PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                    // NumberedTable'dan tabloyu eklemek için
                    string numberedTableFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_numbered_table.pdf");

                    // NumberedTable PDF'sini oluştur ve sonra bu PDF'yi ekle
                    NumberedTable numberedTable = new NumberedTable();
                    numberedTable.CreatePdf(numberedTableFilePath);

                    // NumberedTable PDF'sini mevcut PDF'ye ekliyoruz
                    PdfReader numberedTableReader = new PdfReader(numberedTableFilePath);
                    PdfImportedPage page = stamper.GetImportedPage(numberedTableReader, 1); // 1. sayfayı al
                    cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle

                    stamper.Close();
                    numberedTableReader.Close();

                }
            }
            // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
            File.Delete(filePath);
            File.Move(tempFilePath, filePath);
           // MessageBox.Show($"NumberedTable, mevcut PDF'ye eklendi: {filePath}");
        }

        private void AddNotTakeExamPdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath1 = Path.Combine(Path.GetDirectoryName(filePath), "temp1.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath1, FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = new PdfStamper(pdfReader, fs);
                    PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                    // NotTakeExam'dan tabloyu eklemek için
                    string nottakeexamFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_nottakeexam_table.pdf");

                    // NotTakeExam PDF'sini oluştur ve sonra bu PDF'yi ekle
                    NotTakeExam notTakeExam = new NotTakeExam();
                    notTakeExam.CreatePdf(nottakeexamFilePath);

                    // NotTakeExam PDF'sini mevcut PDF'ye ekliyoruz
                    PdfReader nottakeexamReader = new PdfReader(nottakeexamFilePath);
                    PdfImportedPage page = stamper.GetImportedPage(nottakeexamReader, 1); // 1. sayfayı al
                    cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle

                    stamper.Close();
                    nottakeexamReader.Close();

                }
            }
            // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
            File.Delete(filePath);
            File.Move(tempFilePath1, filePath);


            //MessageBox.Show($"NotTakeExam, mevcut PDF'ye eklendi: {filePath}");
        }
        private void AddAttentionPdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath2 = Path.Combine(Path.GetDirectoryName(filePath), "temp2.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath2, FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = new PdfStamper(pdfReader, fs);
                    PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                    // Attention'dan tabloyu eklemek için
                    string attentionFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_Attention.pdf");

                    // Attention PDF'sini oluştur ve sonra bu PDF'yi ekle
                    Attention attention = new Attention();
                    attention.CreatePdf(attentionFilePath);

                    // Attention PDF'sini mevcut PDF'ye ekliyoruz
                    PdfReader attentionReader = new PdfReader(attentionFilePath);
                    PdfImportedPage page = stamper.GetImportedPage(attentionReader, 1); // 1. sayfayı al

                    cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle



                    stamper.Close();
                    attentionReader.Close();

                }
            }
            // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
            File.Delete(filePath);
            File.Move(tempFilePath2, filePath);


            //MessageBox.Show($"Attention, mevcut PDF'ye eklendi: {filePath}");

        }
        private void AddExamplePdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath3 = Path.Combine(Path.GetDirectoryName(filePath), "temp3.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath3, FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = new PdfStamper(pdfReader, fs);
                    PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                    // Example tabloyu eklemek için
                    string exampleFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_Example.pdf");

                    // Example PDF'sini oluştur ve sonra bu PDF'yi ekle
                    ExampleMarking example = new ExampleMarking();
                    example.CreatePdf(exampleFilePath);

                    // Example PDF'sini mevcut PDF'ye ekliyoruz
                    PdfReader exampleReader = new PdfReader(exampleFilePath);
                    PdfImportedPage page = stamper.GetImportedPage(exampleReader, 1); // 1. sayfayı al

                    cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle


                    stamper.Close();
                    exampleReader.Close();

                }
            }
            // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
            File.Delete(filePath);
            File.Move(tempFilePath3, filePath);


            //MessageBox.Show($"Example, mevcut PDF'ye eklendi: {filePath}");

        }

        private void AddStudentInformatinTablePdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath4 = Path.Combine(Path.GetDirectoryName(filePath), "temp4.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath4, FileMode.Create, FileAccess.Write))
                {
                    PdfStamper stamper = new PdfStamper(pdfReader, fs);
                    PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                    // Example tabloyu eklemek için
                    string StudentInformationTableFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_Studentinformationtable.pdf");

                    // StudentInformationTable PDF'sini oluştur ve sonra bu PDF'yi ekle
                    StudentInformationTable studentInformationTable = new StudentInformationTable();
                    studentInformationTable.CreatePdf(StudentInformationTableFilePath); // Doğru fonksiyon adını kullan

                    // Oluşturulan StudentInformationTable PDF'sini mevcut PDF'ye ekliyoruz
                    PdfReader exampleReader = new PdfReader(StudentInformationTableFilePath);
                    PdfImportedPage page = stamper.GetImportedPage(exampleReader, 1); // 1. sayfayı al

                    cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle

                    stamper.Close();
                    exampleReader.Close();


                }
            }
            // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
            File.Delete(filePath);
            File.Move(tempFilePath4, filePath);


            //MessageBox.Show($"Studentinformationtable, mevcut PDF'ye eklendi: {filePath}");

        }


    }

}
