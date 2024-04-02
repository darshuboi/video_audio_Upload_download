using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

namespace video_audio_Upload_download
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string s = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=F:\video_audio_Upload_download\video_audio_Upload_download\video_audio_Upload_download\App_Data\file_db.mdf;Integrated Security=True;";

        SqlConnection con;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataForFileId();
            }
        }

        public void getcon()
        {
            con = new SqlConnection(s);
            con.Open();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (FileUpload1.HasFile)
            {
                string fileName = Path.GetFileName(FileUpload1.FileName);
                fileName = fileName.Replace(" ", "");
                FileUpload1.SaveAs(Server.MapPath("~/Upload/") + fileName);
                string filePath = "Upload/" + fileName;

                string videoTag = "<video width='450' controls><source src='" + filePath + "' type='video/mp4'></video>";
                Literal1.Text = videoTag;

                getcon();

                SqlCommand cmd = new SqlCommand("INSERT INTO file_tbl (name, contenttype) VALUES (@name, @path)", con);
                cmd.Parameters.AddWithValue("@name", fileName);
                cmd.Parameters.AddWithValue("@path", filePath);
                cmd.ExecuteNonQuery();
                GetDataForFileId(); 

                con.Close(); 
            }
        }

        public void GetDataForFileId()
        {
            getcon();

            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM file_tbl", con);
            DataSet ds = new DataSet();
            da.Fill(ds);
            GridView1.DataSource = ds;
            GridView1.DataBind();

            con.Close();
        }
    }
}
