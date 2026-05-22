using Aviva.Multilabs.Peticiones.Demonio.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aviva.Multilabs.Peticiones.Demonio.Models
{
    public class DataMultilabs
    {
        public List<Lista_Peticiones_Multilabs> listaPeticionesMultilabs()
        {
            var lista = new List<Lista_Peticiones_Multilabs>();

            try
            {
                string cadenaConexion =
                    "Data Source=10.19.67.65;" +
                    "User ID=prim;" +
                    "Password=prim;" +
                    "Initial Catalog=XHIS_PRD;" +
                    "MultipleActiveResultSets=True;";

                using (SqlConnection conexion = new SqlConnection(cadenaConexion))
                using (SqlCommand comando = new SqlCommand("[dbo].[aviva_getMonitorLaboratorio_Multilabs]", conexion))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 500;

                    // Parámetros tipados
                    comando.Parameters.Add("@COD_CENTRO", SqlDbType.Int).Value = 0;
                    comando.Parameters.Add("@HORAS_FILTRO", SqlDbType.Int).Value = 24;

                    conexion.Open();

                    using (SqlDataReader dr = comando.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            var item = new Lista_Peticiones_Multilabs
                            {
                                Numero_de_orden = dr["Numero_de_orden"] as string ?? string.Empty,
                                Estado = dr["Estado"] as string ?? string.Empty,
                                encuentro = dr["encuentro"] != DBNull.Value ? Convert.ToInt32(dr["encuentro"]) : 0,
                                codigo_cliente = dr["codigo_cliente"] != DBNull.Value ? Convert.ToInt32(dr["codigo_cliente"]) : 0,
                                Historia_clinica = dr["Historia_clinica"] as string ?? string.Empty,
                                Paciente = dr["Paciente"] as string ?? string.Empty,
                                DNI = dr["DNI"] as string ?? string.Empty,
                                Sexo = dr["Sexo"] as string ?? string.Empty,
                                Edad = dr["Edad"] != DBNull.Value ? Convert.ToInt32(dr["Edad"]) : 0,
                                Procedimiento = dr["Procedimiento"] as string ?? string.Empty,
                                Servicio = dr["Servicio"] as string ?? string.Empty,
                                Ubicacion = dr["Ubicacion"] as string ?? string.Empty,
                                Prioridad = dr["Prioridad"] as string ?? string.Empty,
                                Fecha = dr["Fecha"] as string ?? string.Empty,
                                Hora = dr["Hora"] as string ?? string.Empty,
                                Tiempo_de_Espera = dr["Tiempo_de_Espera"] as string ?? string.Empty,
                                Peticion = dr["Peticion"] != DBNull.Value ? Convert.ToInt32(dr["Peticion"]) : 0,
                                Catalogo = dr["Catalogo"] != DBNull.Value ? Convert.ToInt32(dr["Catalogo"]) : 0,
                                Estado_Muestra = dr["Estado_Muestra"] as string ?? string.Empty,
                                Comentarios = dr["Comentarios"] as string ?? string.Empty,
                                SedeId = dr["SedeId"] != DBNull.Value ? Convert.ToInt32(dr["SedeId"]) : 0,
                            };

                            lista.Add(item);
                        }
                    }
                }

                return lista;
            }
            catch (Exception ex)
            {
                // Puedes registrar el error aquí (log)
                throw new Exception("Error al obtener monitor de laboratorio.", ex);
            }
        }

        public void grabarPeticionMultilabs(Lista_Peticiones_Multilabs peticion, string estadoCod, int estadoPk)
        {
            try
            {
                string[] partes = peticion.Paciente.Split(' ');

                string nombre1 = partes.Length > 0 ? partes[0] : "";
                string nombre2 = partes.Length > 1 ? partes[1] : "";
                string apellido1 = partes.Length > 2 ? partes[2] : "";
                string apellido2 = partes.Length > 3 ? partes[3] : "";

                string fecha = peticion.Fecha; // 21/05/2026
                string fechaFormateada = DateTime
                    .ParseExact(fecha, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                    .ToString("yyyy-MM-dd");

                string cadenaConexion =
                    "Data Source=10.19.67.65;" +
                    "User ID=prim;" +
                    "Password=prim;" +
                    "Initial Catalog=XHIS_PRD;" +
                    "MultipleActiveResultSets=True;";
                SqlConnection conexion = new SqlConnection(cadenaConexion);
                SqlCommand comando = new SqlCommand("[dbo].[aviva_insertarPeticiones_Unilabs] @epis_pk , @apellido1, @apellido2, @nombres, @gpe_peticion, @origen, @codCentro, @ubicacion, @numDocum, @estadoPdf, @idEstadoPdf, @fechaPeticion, @userPeticion", conexion);
                //comando.CommandType = CommandType.StoredProcedure;
                comando.Parameters.AddWithValue("@epis_pk", peticion.encuentro);
                comando.Parameters.AddWithValue("@apellido1", apellido1);
                comando.Parameters.AddWithValue("@apellido2", apellido2);
                comando.Parameters.AddWithValue("@nombres", nombre1 + ' ' + nombre2);
                comando.Parameters.AddWithValue("@gpe_peticion", peticion.Numero_de_orden);
                comando.Parameters.AddWithValue("@origen", peticion.Servicio);
                comando.Parameters.AddWithValue("@codCentro", peticion.SedeId);
                comando.Parameters.AddWithValue("@ubicacion", peticion.Ubicacion);
                comando.Parameters.AddWithValue("@numDocum", peticion.DNI);
                comando.Parameters.AddWithValue("@estadoPdf", estadoCod);
                comando.Parameters.AddWithValue("@idEstadoPdf", estadoPk);
                comando.Parameters.AddWithValue("@fechaPeticion", fechaFormateada);
                comando.Parameters.AddWithValue("@userPeticion", 1);
                conexion.Open();
                comando.ExecuteNonQuery();

                //Cerrando conexion
                if (conexion.State == ConnectionState.Open)
                {
                    conexion.Close();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
