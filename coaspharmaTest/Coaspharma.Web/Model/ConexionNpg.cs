using coaspharma.Dal.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace Coaspharma.Web.Model
{
    public class ConexionNpg
    {
        NpgsqlConnection con = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["BDLocal"].ToString());
        public void conectar()
        {
            con.Open();

        }

        public void Desconectar()
        {
            con.Close();
        }

       
        public string QueryLogin(string user, string pss)
        {
            var query = "select * from simaeusu where usuario = '" + user + "' and clave = '" + pss + "'";
            NpgsqlCommand conexion = new NpgsqlCommand(query, con);
            NpgsqlDataReader data = conexion.ExecuteReader();
            var Name = "";
            //data.Fill(table);
            while (data.Read())
            {
                Name = data[2].ToString();

            }
            return Name;
        }

        public bool UpdateUser(nomaeempModel ObjnomaeempModel)
        {
            var flag = false;
            try
            {
                conectar();
                var query = "UPDATE nomaeemp SET cedula = '" + ObjnomaeempModel.cedula + "', nombres = '" + ObjnomaeempModel.nombre + "', apellidos = '" + ObjnomaeempModel.apellido + "', nombre_completo = '" + ObjnomaeempModel.nombrecompleto + "', dep_codi = '" + ObjnomaeempModel.dep_codi + "', mun_codi = '" + ObjnomaeempModel.mun_codi + "'WHERE codcia = '" + ObjnomaeempModel.codcia + "' ";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                // NpgsqlDataAdapter data = new NpgsqlDataAdapter(conexion);
                NpgsqlDataReader data = conexion.ExecuteReader();

                flag = true;
                Desconectar();
                return flag;
            }
            catch (Exception e)
            {
                flag = false;
                Desconectar();
                return flag;
            }

        }

        public bool ValidateUser(string cc)
        {
            var flag = false;
            try
            {
                conectar();
                var query = "select cedula from nomaeemp where cedula = '" + cc + "'";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                NpgsqlDataReader data = conexion.ExecuteReader();
                var Name = "";
                //data.Fill(table);
                while (data.Read())
                {
                    Name = data[0].ToString();

                }
                if (!String.IsNullOrEmpty(Name))
                    flag = true;

            }            
            catch (Exception ex)
            {
                flag = false;
            }
            Desconectar();
            return flag;

        }


        public bool InsertUser(nomaeempModel ObjnomaeempModel)
        {
            var flag = false;
            try
            {
                conectar();
                var query = "INSERT INTO nomaeemp (codcia,cedula, nombres, apellidos,nombre_completo,fecha_ingreso,dep_codi,mun_codi)VALUES('" + ObjnomaeempModel.codcia + "', '" + ObjnomaeempModel.cedula + "', '" + ObjnomaeempModel.nombre + "', '" + ObjnomaeempModel.apellido + "', '" + ObjnomaeempModel.nombrecompleto + "', '" + ObjnomaeempModel.fecha_ingreso + "', '" + ObjnomaeempModel.dep_codi + "','" + ObjnomaeempModel.mun_codi + "'); ";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                // NpgsqlDataAdapter data = new NpgsqlDataAdapter(conexion);
                NpgsqlDataReader data = conexion.ExecuteReader();

                flag = true;
                Desconectar();


                string sQL = "UPDATE nomaeemp SET foto=@Image WHERE cedula='" + ObjnomaeempModel.cedula + "'";
                using (var command = new NpgsqlCommand(sQL, con))
                {
                    NpgsqlParameter param = command.CreateParameter();
                    param.ParameterName = "@Image";
                    param.NpgsqlDbType = NpgsqlTypes.NpgsqlDbType.Bytea;
                    param.Value = ObjnomaeempModel.foto;
                    command.Parameters.Add(param);

                    conectar();
                    command.ExecuteNonQuery();
                    Desconectar();
                }

                return flag;
            }
            catch (Exception e)
            {
                flag = false;
                Desconectar();
                return flag;
            }

        }


        public bool DeleteUser(string IdUser)
        {
            var flag = false;
            try
            {
                conectar();
                var query = "DELETE FROM nomaeemp WHERE cedula = '" + IdUser + "' ";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                // NpgsqlDataAdapter data = new NpgsqlDataAdapter(conexion);
                NpgsqlDataReader data = conexion.ExecuteReader();

                flag = true;
                Desconectar();
                return flag;
            }
            catch (Exception e)
            {
                flag = false;
                Desconectar();
                return flag;
            }

        }

        public nomaeempModel GetAllDataById(int Id)
        {
            var nomaeemp = new nomaeempModel();
            try
            {
                conectar();

                var query = "select codcia,cedula,nombres,apellidos,nombre_completo,fecha_ingreso, d.dep_nomb, m.mun_nomb,foto from nomaeemp n LEFT OUTER JOIN gnmaedep d on n.dep_codi = d.dep_codi LEFT OUTER JOIN gnmaemun m on n.mun_codi = m.mun_codi where cedula='" + Id + "'";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                NpgsqlDataReader data = conexion.ExecuteReader();
                while (data.Read())
                {
                    nomaeemp.codcia = data[0].ToString();
                    nomaeemp.cedula = data[1].ToString();
                    nomaeemp.nombre = data[2].ToString().ToUpper();
                    nomaeemp.apellido = data[3].ToString().ToUpper();
                    nomaeemp.foto = (byte[])data[8];
                }
                data.Close();

            }
            catch (Exception ex)
            {

            }
            Desconectar();

            return nomaeemp;
        }

        public List<nomaeempModel> GetAllData()
        {
            var nomaeemp = new List<nomaeempModel>();
            //var query = "select * from nomaeemp";
            var query = "select codcia,cedula,nombres,apellidos,fecha_ingreso, d.dep_nomb, m.mun_nomb from nomaeemp n LEFT OUTER JOIN gnmaedep d on n.dep_codi = d.dep_codi LEFT OUTER JOIN gnmaemun m on n.mun_codi = m.mun_codi";
            NpgsqlCommand conexion = new NpgsqlCommand(query, con);
            NpgsqlDataReader data = conexion.ExecuteReader();

            while (data.Read())
            {

                nomaeemp.Add(new nomaeempModel
                {
                    codcia = data[0].ToString(),
                    cedula = data[1].ToString(),
                    nombre = data[2].ToString().ToUpper(),
                    apellido = data[3].ToString().ToUpper(),
                    fecha_ingreso = data[4].ToString(),
                    dep_codi = data[5].ToString(),
                    mun_codi = data[6].ToString()

                });
            }
            return nomaeemp;
        }

        public List<gnmaedepModel> GetDep()
        {
            conectar();
            var deps = new List<gnmaedepModel>();
            try
            {
                var query = "select dep_codi, dep_nomb from gnmaedep ";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                NpgsqlDataReader data = conexion.ExecuteReader();

                while (data.Read())
                {
                    deps.Add(new gnmaedepModel
                    {
                        dep_codi = Convert.ToInt32(data[0]),
                        dep_nomb = data[1].ToString()


                    });
                }
            }
            catch (NpgsqlException e)
            {
                return null;
            }
            Desconectar();

            return deps;
        }

        public List<gnempre> GetEmp()
        {
            conectar();
            var emps = new List<gnempre>();
            try
            {
                var query = "select emp_codi, emp_nomb from gn_empre ";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                NpgsqlDataReader data = conexion.ExecuteReader();

                while (data.Read())
                {
                    emps.Add(new gnempre
                    {
                        emp_codi = Convert.ToInt32(data[0]),
                        emp_nomb = data[1].ToString()


                    });
                }
            }
            catch (NpgsqlException e)
            {
                return null;
            }
            Desconectar();

            return emps;
        }


        public List<gnmaemunModel> GetMunName()
        {
            conectar();
            var Muns = new List<gnmaemunModel>();
            try
            {
                var query = "select mun_codi, mun_nomb from gnmaemun";
                NpgsqlCommand conexion = new NpgsqlCommand(query, con);
                NpgsqlDataReader data = conexion.ExecuteReader();

                while (data.Read())
                {
                    Muns.Add(new gnmaemunModel
                    {
                        mun_codi = Convert.ToInt32(data[0]),
                        mun_nomb = data[1].ToString()
                    });
                }
            }
            catch (NpgsqlException e)
            {
                return null;
            }
            Desconectar();

            return Muns;
        }
    }
}