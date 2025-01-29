using System;
using System.Collections.Generic;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;

namespace OptikProjeTumSayfa
{
    internal class DbValuePositioner : StudentInformationTable
    {
        private string[] studentsInformationDataBase;
        private string[] titles;
        private string sınavAdi;
        private string sınavTarih;
        private readonly iTextSharp.text.Rectangle pageSize = PageSize.A4;
        private int StudentID = 6; // Varsayılan bir öğrenci ID'si, gerektiğinde değiştirebilirsiniz.


        private int studentIndexData = 0; // Öğrenci bilgileri için sayaç

        // Sabit değerler
        private float cellHeight; // Her bir hücrenin yüksekliği
        

        // Constructor
        public DbValuePositioner()
        {
            // Hücre yüksekliğini hesapla
            cellHeight = tableHeight / rows;
        }

        private void FetchDataFromDatabase()
        {
            DbConnection db = new DbConnection();
            using (MySqlConnection connection = db.GetConnection())
            {
                Console.WriteLine("Kullanılan StudentID: " + StudentID); // Debug için

                studentsInformationDataBase = FetchStudentData(connection);
                titles = FetchTitles(connection);

                sınavAdi = FetchSingleValue(connection, @"
            SELECT ExamName 
            FROM examdetails 
            WHERE ID = (SELECT ExampleDetailsID FROM students WHERE StudentID = @StudentID);");

                sınavTarih = FetchSingleValue(connection, @"
            SELECT ExamDate 
            FROM examdetails 
            WHERE ID = (SELECT ExampleDetailsID FROM students WHERE StudentID = @StudentID);");
            }
        }


        private string[] FetchStudentData(MySqlConnection connection)
        {
            List<string> studentData = new List<string>();
            string query = @"
            SELECT s.Name, 
                   s.StudentNumber, 
                   s.TCKN, 
                   d.DepartmentName 
            FROM students s
            LEFT JOIN departments d ON s.DepartmentID = d.DepartmentID
            WHERE s.StudentID = @StudentID;";


            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", StudentID); // Kullanıcı ID'sini parametre olarak ekleyin

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        studentData.Add(reader.GetString("Name")); // Ad Soyad
                        studentData.Add(reader.GetString("StudentNumber")); // Öğrenci No
                        studentData.Add(reader.GetString("TCKN")); // TC Kimlik No
                        studentData.Add(reader.GetString("DepartmentName")); // Bölüm
                    }
                }
            }


            return studentData.ToArray();
        }

        private string[] FetchTitles(MySqlConnection connection)
        {
            List<string> titleData = new List<string>();

            string query = @"
            SELECT e.DepartmentName AS ExamDepartment, 
                   e.RoomNumber AS ExamRoom, 
                   e.SeatNumber AS ExamSeat
            FROM examtitles e
            JOIN students s ON e.ID = s.ExampleTitleID
            WHERE s.StudentID = @StudentID;";  // ExampleTitleID ile eşleşen verileri getir

            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", StudentID); // Global StudentID değişkenini kullan

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        titleData.Add(reader.GetString("ExamDepartment")); // Bölüm
                        titleData.Add(reader.GetInt32("ExamRoom").ToString()); // Salon No
                        titleData.Add(reader.GetInt32("ExamSeat").ToString()); // Sıra No
                    }
                }
            }

            return titleData.ToArray();
        }



        private string FetchSingleValue(MySqlConnection connection, string query)
        {
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StudentID", StudentID); // Eksik parametreyi ekledik

                object result = command.ExecuteScalar();
                return result != null ? result.ToString() : string.Empty;
            }
        }


        public void CreatePdf(string filePath)
        {
            FetchDataFromDatabase();

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Document doc = new Document(pageSize);
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;

                float initialY = pageSize.Height - topMargin - cellHeight;
                float initialYy = pageSize.Height - topMargin;

                bilgibas(cb, leftMargin, initialY);
                sinavAdiBas(cb, initialYy);
                sinavTarihiBas(cb, initialYy);
                rightTablesBas(cb, leftMargin + tableWidth + (0.7f * 28.35f), initialY - 4.5f - (0.5f * 28.35f));

                doc.Close();
            }
        }

        private void rightTablesBas(PdfContentByte cb, float x, float y)
        {
            for (int i = 0; i < titles.Length; i++)
            {
                cb.BeginText();
                cb.SetFontAndSize(baseFont, 10);
                cb.ShowTextAligned(PdfContentByte.ALIGN_CENTER, titles[i], x + rightTablesWidth / 2, y + rightTablesTextOffset - 8, 0);
                cb.EndText();
                y -= (rightTablesHeight + rightTablesSpacing);
            }
        }

        private void bilgibas(PdfContentByte cb, float x, float y)
        {
            for (int row = 0; row < rows; row++)
            {
                float currentYY = y - (row + 1) * cellHeight - cellHeight + 3;

                if (studentIndexData < studentsInformationDataBase.Length)
                {
                    cb.BeginText();
                    BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    cb.SetFontAndSize(bf, 10);
                    cb.SetRGBColorFill(0, 0, 0);
                    cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, studentsInformationDataBase[studentIndexData], x + 75, currentYY + cellHeight / 2 - 5, 0);
                    cb.EndText();
                    studentIndexData++;
                }
            }
        }

        private void sinavAdiBas(PdfContentByte cb, float y)
        {
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            y -= headerHeight;
            y -= (tableHeight + secondTableSpacing);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sınavAdi, 5 + lineX, y - (secondTableHeight / 2) - 5, 0);
            cb.EndText();
        }

        private void sinavTarihiBas(PdfContentByte cb, float y)
        {
            cb.BeginText();
            cb.SetFontAndSize(baseFont, 8);
            y -= headerHeight;
            y -= (tableHeight + secondTableSpacing);
            y -= (secondTableHeight + secondTableSpacing);
            cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, sınavTarih, lineX + 5, y - (secondTableHeight / 2) - 5, 0);
            cb.EndText();
        }
    }
}
