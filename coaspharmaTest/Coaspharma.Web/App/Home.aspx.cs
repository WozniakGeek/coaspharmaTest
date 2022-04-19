using coaspharma.Dal.Model;
using Coaspharma.Web.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Coaspharma.Web.App
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                uploadData();
            }

        }

        protected void uploadData()
        {
            try
            {
                ConexionNpg conexion = new ConexionNpg();
                conexion.conectar();
                var getData = conexion.GetAllData();
                if (getData.Count > 0)
                {
                    tablaValores.DataSource = getData.ToList();
                    tabla_ppal.DataSource = getData.ToList();
                }
                tablaValores.DataBind();
                tabla_ppal.DataSource = getData.ToList();
                tabla_ppal.DataBind();
                conexion.Desconectar();

            }
            catch (Exception e)
            {

            }
        }

        protected void Descargar(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                app.Visible = true;
                //worksheet = workbook.Sheets["Sheet1"];
                worksheet = workbook.ActiveSheet;
                worksheet.Name = "Exported from gridview";
                for (int i = 1; i < tabla_ppal.Columns.Count + 1; i++)
                {
                    worksheet.Cells[1, i] = tabla_ppal.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < tabla_ppal.Rows.Count; i++)
                {
                    for (int j = 0; j < tabla_ppal.Columns.Count; j++)
                    {
                        worksheet.Cells[i + 2, j + 1] = tabla_ppal.Rows[i].Cells[j].Text;
                    }
                }

                workbook.SaveAs("c:\\Usuarios", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

                app.Quit();

            }
            catch (Exception ex)
            {

            }
        }


        protected void ShowPopup(object sender, EventArgs e)
        {
            try
            {
                //imageurl.ImageUrl = "";
                ConexionNpg conexion = new ConexionNpg();
                int id;
                string[] parametros = ((LinkButton)sender).CommandArgument.Split(';');
                if (!String.IsNullOrEmpty(parametros[0]))
                {
                    id = Convert.ToInt32(parametros[0]);
                    string title = ((LinkButton)sender).SkinID;
                    if (title == "Editar")
                    {
                        
                        var GetUser = conexion.GetAllDataById(id);                       
                        CedulaModal.ReadOnly = true;
                        CedulaModal.Text = GetUser.cedula;
                        NombreModal.Text = GetUser.nombre;
                        ApellidoModal.Text = GetUser.apellido;
                        if (GetUser.foto != null && GetUser.foto.Length > 0)
                        {
                            imageurl.ImageUrl = "data:image;base64," + Convert.ToBase64String(GetUser.foto);
                        }

                        var getEmps = conexion.GetEmp();
                        if (getEmps.Count > 0)
                        {
                            ddlcodModal.Items.Clear();
                            //ddlDepaModal.Items.Add(new ListItem("Seleccione...", "0"));
                            foreach (var item in getEmps)
                            {
                                ddlcodModal.Items.Add(new ListItem(item.emp_nomb, item.emp_codi.ToString()));
                            }

                        }

                        var getDeps = conexion.GetDep();
                        if (getDeps.Count > 0)
                        {
                            ddlDepaModal.Items.Clear();
                            foreach (var item in getDeps)
                            {
                                ddlDepaModal.Items.Add(new ListItem(item.dep_nomb, item.dep_codi.ToString()));
                            }

                        }
                        var getMuns = conexion.GetMunName();
                        if (getMuns.Count > 0)
                        {
                            ddlMuniModal.Items.Clear();
                            //ddlMuniModal.Items.Add(new ListItem("Seleccione...", "0"));
                            foreach (var item in getMuns)
                            {
                                ddlMuniModal.Items.Add(new ListItem(item.mun_nomb, item.mun_codi.ToString()));
                            }

                        }

                        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal('" + title + "');", true);
                    }
                    else if (title == "Eliminar")
                    {
                        Session["IdDelete"] = id;
                        lbl_error.Text = "¿Esta Seguro de eliminar este Usuario?";
                        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModalD('" + title + "');", true);                        
                    }
                }

                else
                {

                    var title = "Insertar";
                    clear_formulario();
                    var getEmps = conexion.GetEmp();
                    if (getEmps.Count > 0)
                    {
                        ddlcodModal.Items.Clear();
                        ddlcodModal.Items.Add(new ListItem("Seleccione...", "0"));
                        foreach (var item in getEmps)
                        {
                            ddlcodModal.Items.Add(new ListItem(item.emp_nomb, item.emp_codi.ToString()));
                        }

                    }

                    var getDeps = conexion.GetDep();
                    if (getDeps.Count > 0)
                    {
                        ddlDepaModal.Items.Clear();
                        ddlDepaModal.Items.Add(new ListItem("Seleccione...", "0"));
                        foreach (var item in getDeps)
                        {
                            ddlDepaModal.Items.Add(new ListItem(item.dep_nomb, item.dep_codi.ToString()));
                        }

                    }
                    var getMuns = conexion.GetMunName();
                    if (getMuns.Count > 0)
                    {
                        ddlMuniModal.Items.Clear();
                        ddlMuniModal.Items.Add(new ListItem("Seleccione...", "0"));
                        foreach (var item in getMuns)
                        {
                            ddlMuniModal.Items.Add(new ListItem(item.mun_nomb, item.mun_codi.ToString()));
                        }

                    }
                    ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal('" + title + "');", true);
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void UpdateModal(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                if (Uploader.HasFile)
                {
                    try
                    {
                        sb.AppendFormat(" Uploading file: {0}", Uploader.FileName);
                        string savePath = "c:\\temp\\Download\\";
                        //saving the file
                        Uploader.SaveAs(savePath + Uploader.FileName);

                        //Showing the file information
                        sb.AppendFormat("<br/> Save As: {0}", Uploader.PostedFile.FileName);
                        sb.AppendFormat("<br/> File type: {0}", Uploader.PostedFile.ContentType);
                        sb.AppendFormat("<br/> File length: {0}", Uploader.PostedFile.ContentLength);
                        sb.AppendFormat("<br/> File name: {0}", Uploader.PostedFile.FileName);

                    }
                    catch (Exception ex)
                    {
                        sb.Append("<br/> Error <br/>");
                        sb.AppendFormat("Unable to save file <br/> {0}", ex.Message);
                    }
                }

                else
                {
                    //lblmessage.Text = sb.ToString();
                }
                //Photo
                var size = UplodadPhoto.PostedFile.ContentLength;
                byte[] ImageOriginal = new byte[size];
                if (size > 0)
                {
                    UplodadPhoto.PostedFile.InputStream.Read(ImageOriginal, 0, size);
                    Bitmap bitmap = new Bitmap(UplodadPhoto.PostedFile.InputStream);
                    var ImageUrl64 = "data:image/png;base64," + Convert.ToBase64String(ImageOriginal);
                    imageurl.ImageUrl = ImageUrl64;
                }


                ConexionNpg conexion = new ConexionNpg();
                var nomaeemp = new nomaeempModel();
                var request = ((LinkButton)sender).SkinID;
                var Validation = Validar_formulario();
                if (Validation)
                {
                    nomaeemp.codcia = ddlcodModal.SelectedValue;
                    nomaeemp.cedula = CedulaModal.Text;
                    nomaeemp.nombre = NombreModal.Text;
                    nomaeemp.apellido = ApellidoModal.Text;
                    nomaeemp.nombrecompleto = NombreModal.Text + " " + ApellidoModal.Text;
                    nomaeemp.dep_codi = ddlDepaModal.SelectedValue;
                    nomaeemp.mun_codi = ddlMuniModal.SelectedValue;
                    nomaeemp.foto = ImageOriginal;
                    if (request == "Update")
                    {
                        var isUpdate = conexion.UpdateUser(nomaeemp);
                        if (isUpdate)
                            Response.Redirect("~/App/Home.aspx", false);
                    }
                    else
                    {
                        var isExist = conexion.ValidateUser(nomaeemp.cedula);
                        if (isExist)
                        {
                            lbl_mensaje_error.Text = "El usuario ya esta creado";
                            ClientScript.RegisterStartupScript(this.GetType(), "ramdom", "AlertModal();", true);
                        }
                        else
                        {
                            nomaeemp.fecha_ingreso = DateTime.Now.ToString("dd-MM-yyyy h:mm");
                            var isInsert = conexion.InsertUser(nomaeemp);
                            if (isInsert)
                            {
                                //var isSenEmail = send_Email();
                                Response.Redirect("~/App/Home.aspx", false);

                            }
                        }


                    }


                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected void DeleteModal(object sender, EventArgs e)
        {
            try
            {
                var conexion = new ConexionNpg();
                var codUser = Session["IdDelete"].ToString();
                var resultDelete = conexion.DeleteUser(codUser);
                if (resultDelete)
                {

                    Response.Redirect("~/App/Home.aspx", false);

                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        protected bool Validar_formulario()
        {
            if (!String.IsNullOrEmpty(CedulaModal.Text)
                && !String.IsNullOrEmpty(NombreModal.Text)
                && !String.IsNullOrEmpty(ApellidoModal.Text))

            {
                return true;
            }
            else
            {
                lbl_mensaje_error.Text = "Faltan campos por ser completados";
                ClientScript.RegisterStartupScript(this.GetType(), "ramdom", "AlertModal();", true);
                return false;
            }
        }

        protected void clear_formulario()
        {
            imageurl.ImageUrl = "";
            CedulaModal.ReadOnly = false;
            lbtnUpdate.Visible = false;
            lbtnInsert.Visible = true;
            CedulaModal.Text = "";
            NombreModal.Text = "";
            ApellidoModal.Text = "";
        }

        protected bool send_Email()
        {
            try
            {
                var fromAddress = new MailAddress("ruben.gaona16@gmail.com", "From Name");
                var toAddress = new MailAddress("ruben.gaona@cun.edu.co", "To Name");
                const string fromPassword = "";
                const string subject = "Subject";
                const string body = "Body";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                    Timeout = 20000
                };
                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }

        }


    }
}