using _05.Picture;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Picture
{
    static void Main()
    {
        SqlConnection dbCon = new SqlConnection(Settings.Default.dbConString);
        dbCon.Open();
        using (dbCon)
        {
            SqlCommand cmdSelect = new SqlCommand(
                "SELECT Picture FROM Categories", dbCon);
            SqlDataReader reader = cmdSelect.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    byte[] image = (byte[])reader["Picture"];
                    string file = Convert.ToString(DateTime.Now.ToFileTime());
                    FileStream writer = new FileStream(file, FileMode.CreateNew, FileAccess.Write);
                    writer.Write(image, 0, image.Length);
                    writer.Flush();
                    writer.Close();
                    //pictureBox1.Image = Image.FromFile(file);
                }
            }
        }
    }
}