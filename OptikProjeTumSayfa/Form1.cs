
using System;
using System.Drawing;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;
using MySql.Data.MySqlClient;


namespace OptikProjeTumSayfa
{
    public partial class Form1 : Form
    {

        private DataGridView dgvStudents;
        private List<int> selectedStudentIds = new List<int>();





        public Form1()
        {



            InitializeComponent();
            SetupDataGridView();
            LoadStudents();
            this.Load += Form1_Load;

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

        private void Form1_Load(object sender, EventArgs e)
        {
            // Formun genişliğini DataGridView'in genişliğine göre ayarla
            this.Width = dgvStudents.Width + 40;  // 40 ekleyerek margin bırakıyoruz
            this.Height = dgvStudents.Height + 100; // 100 ekleyerek butonlara yer bırakıyoruz

            // Minimum boyutu belirleyerek kullanıcı küçültmesini engelle
            this.MinimumSize = new Size(this.Width, this.Height);
        }



        private void SetupDataGridView()
        {
            dgvStudents = new DataGridView
            {
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false, 
                Location = new Point(10, 50),
                Width = 1000, 
                Height = 300,
                AllowUserToAddRows = false,
                ReadOnly = true,
                
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill 
            };

            dgvStudents.CellMouseClick += DgvStudents_CellMouseClick;
            dgvStudents.CellFormatting += DgvStudents_CellFormatting;

            dgvStudents.Columns.Add("ID", "Öğrenci ID");
            dgvStudents.Columns.Add("Name", "Öğrenci Adı");
            dgvStudents.Columns.Add("StudentNumber", "Öğrenci No");
            dgvStudents.Columns.Add("TCKN", "T.C. Kimlik No");
            dgvStudents.Columns.Add("Department", "Bölüm");
            dgvStudents.Columns.Add("ExamName", "Sınav Adı");
            dgvStudents.Columns.Add("ExamDate", "Sınav Tarihi");
            dgvStudents.Columns.Add("Room", "Sınav Salonu");
            dgvStudents.Columns.Add("Seat", "Sınav Sırası");

            this.Controls.Add(dgvStudents);
        }
        private void DgvStudents_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0) // Header'a tıklamaları filtrele
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells["ID"].Value);

                // Seçimi toggle et
                if (selectedStudentIds.Contains(id))
                {
                    selectedStudentIds.Remove(id);
                    row.Selected = false;
                }
                else
                {
                    selectedStudentIds.Add(id);
                    row.Selected = true;
                }

                // Konsola yazdır
                Console.WriteLine("Seçilen ID'ler: " + string.Join(", ", selectedStudentIds));

                // Görsel güncelleme için
                dgvStudents.Invalidate();
            }
        }





        private void DgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            selectedStudentIds.Clear();
            foreach (DataGridViewRow row in dgvStudents.SelectedRows)
            {
                if (row.Cells["ID"].Value != null)
                {
                    int id = Convert.ToInt32(row.Cells["ID"].Value);
                    selectedStudentIds.Add(id);
                }
            }
            dgvStudents.Invalidate(); // Hücreleri yeniden çiz
        }


        private void DgvStudents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells["ID"].Value);

                // Sadece selectedStudentIds listesindekileri mavi yap
                if (selectedStudentIds.Contains(id))
                {
                    row.DefaultCellStyle.BackColor = Color.Blue;
                    row.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.White;
                    row.DefaultCellStyle.ForeColor = Color.Black;
                }
            }
        }






        private void button1_Click(object sender, EventArgs e)
        {
            if (selectedStudentIds.Count == 0)
            {
                MessageBox.Show("Lütfen en az bir öğrenci seçin!");
                return;
            }

            DbConnection db = new DbConnection();

            foreach (int studentId in selectedStudentIds)
            {
                // Veritabanından öğrenci adını ve soyadını al
                string studentName = GetStudentNameById(db, studentId);
                string fileName = $"{studentName.Replace(" ", "_")}.pdf"; // Dosya adı: Ad_Soyad.pdf
                string filePath = Path.Combine(@"C:\Users\Ozkan\Desktop\", fileName);

                // Yeni bir DbValuePositioner nesnesi oluştur ve PDF üret
                DbValuePositioner studentPdf = new DbValuePositioner(studentId);
                studentPdf.CreatePdf(filePath);

                // Mevcut PDF'ye eklemeleri yap
                AddNumberedTableToExistingPdf(filePath);
                AddNotTakeExamPdf(filePath);
                AddAttentionPdf(filePath);
                AddExamplePdf(filePath);
                AddStudentInformatinTablePdf(filePath);
                AddDbInformationPdf(filePath);
            }

            MessageBox.Show($"Tüm seçilen öğrenciler için PDF'ler oluşturuldu!");
        }

        private string GetStudentNameById(DbConnection db, int studentId)
        {
            using (MySqlConnection connection = db.GetConnection())
            {
                string query = "SELECT Name FROM students WHERE StudentID = @StudentID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StudentID", studentId);
                    object result = command.ExecuteScalar();
                    return result != null ? result.ToString() : "Unknown";
                }
            }
        }




        private void LoadStudents()
        {
            DbConnection db = new DbConnection();
            List<(int, string, string, string, string, string, string, string, string)> students = db.GetStudents();

            dgvStudents.Rows.Clear();
            foreach (var student in students)
            {
                dgvStudents.Rows.Add(
                    student.Item1,  // StudentID
                    student.Item2,  // Name
                    student.Item3,  // StudentNumber
                    student.Item4,  // TCKN
                    student.Item5,  // DepartmentName
                    student.Item6,  // ExamName
                    student.Item7,  // ExamDate
                    student.Item8,  // RoomNumber
                    student.Item9   // SeatNumber
                );
            }

            if (students.Count == 0)
            {
                MessageBox.Show("Veritabanında öğrenci bulunamadı!");
            }
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

        private void AddDbInformationPdf(string filePath)
        {
            // Geçici bir dosya oluşturacağız, sonra orijinal PDF'nin üzerine yazacağız
            string tempFilePath4 = Path.Combine(Path.GetDirectoryName(filePath), "temp5.pdf");

            // Mevcut PDF'yi açın ve üstüne ekleme yapmak için PdfStamper kullanın
            using (PdfReader pdfReader = new PdfReader(filePath))
            {
                using (FileStream fs = new FileStream(tempFilePath4, FileMode.Create, FileAccess.Write))
                {
                    using (PdfStamper stamper = new PdfStamper(pdfReader, fs))
                    {
                        PdfContentByte cb = stamper.GetOverContent(1); // 1. sayfaya içerik ekliyoruz

                        // StudentInformationTable PDF'sini oluşturmak için dosya yolu
                        string studentInformationTableFilePath = Path.Combine(Path.GetDirectoryName(filePath), "temp_db.pdf");

                        // StudentInformationTable PDF'sini oluştur
                        DbValuePositioner studentInformationTable = new DbValuePositioner();
                        studentInformationTable.CreatePdf(studentInformationTableFilePath);

                        // Oluşturulan StudentInformationTable PDF'sini mevcut PDF'ye ekle
                        using (PdfReader exampleReader = new PdfReader(studentInformationTableFilePath))
                        {
                            PdfImportedPage page = stamper.GetImportedPage(exampleReader, 1); // 1. sayfayı al
                            cb.AddTemplate(page, 0, 0); // Mevcut PDF'nin üstüne ekle
                            stamper.Close();
                            exampleReader.Close();
                        }
                    }
                }
            }

       

                 // Eski dosyayı silin ve geçici dosyayı asıl dosya yapın
                File.Delete(filePath);
                File.Move(tempFilePath4, filePath);


            //MessageBox.Show($"Studentinformationtable, mevcut PDF'ye eklendi: {filePath}");

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {

        }
    }

}
