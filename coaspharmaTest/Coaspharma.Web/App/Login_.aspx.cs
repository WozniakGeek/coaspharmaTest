
using Coaspharma.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Coaspharma.Web.App
{
    public partial class Login_ : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ingresar(object sender, EventArgs e)
        {

            try
            {
                string error = "";
                if (!string.IsNullOrEmpty(user.Text) && !string.IsNullOrEmpty(pass.Text))
                {
                    ConexionNpg conexion = new ConexionNpg();
                    conexion.conectar();
                    var userLogin = conexion.QueryLogin(user.Text, pass.Text);
                    if (!string.IsNullOrEmpty(userLogin))
                    {
                        Session["Name"] = userLogin;
                        Response.Redirect("~/App/Home.aspx", false);
                        conexion.Desconectar();
                    }
                    else
                    {
                        error = error + "<p>Usuario no se encuentra registrado</p>";
                        lbl_mensaje_error.Text = error;
                        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "AlertModal('" + error + "');", true);
                    }


                }
                else
                {
                    var Title = "Error";
                    if (string.IsNullOrEmpty(user.Text))
                    {
                        error = error + "<p>Debe de escribir un nombre de Usuario.</p>";
                    }
                    if (string.IsNullOrEmpty(pass.Text))
                    {
                        error = error + "<p>Debe de escribir una Contraseña.</p>";
                    }
                    lbl_mensaje_error.Text = error;
                    ClientScript.RegisterStartupScript(this.GetType(), "ramdom", "AlertModal('" + Title + "');", true);
                }
            }
            catch (Exception ex)
            {
                string error = "InnerException: " + ex.InnerException + " StackTrace: " + ex.StackTrace;
            }
        }
    }
}