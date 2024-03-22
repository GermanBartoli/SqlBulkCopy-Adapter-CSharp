using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BulkInsertExample
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Data Source=German;Initial Catalog=SqlBulkCopy;User ID=sa;Password=1234";

            DataTable dataTable = CrearDataTable(); // Método para crear DataTable con 80,000 registros

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "Table_1"; // Nombre de la tabla en la base de datos
                    try
                    {
                        // Crear una nueva instancia de Stopwatch.
                        Stopwatch stopwatch = new Stopwatch();

                        // Comenzar a medir el tiempo.
                        stopwatch.Start();

                        bulkCopy.WriteToServer(dataTable);
                        Console.WriteLine("Datos insertados correctamente.");
                        // Detener la medición del tiempo.
                        stopwatch.Stop();

                        // Obtener el tiempo transcurrido como un objeto TimeSpan.
                        TimeSpan ts = stopwatch.Elapsed;

                        // Mostrar el tiempo transcurrido en la consola.
                        Console.WriteLine("Tiempo transcurrido: {0:hh\\:mm\\:ss\\.ff}", ts);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error al insertar datos: " + ex.Message);
                    }
                }
            }
        }

        static DataTable CrearDataTable()
        {
            DataTable dataTable = new DataTable();
            // Agregar columnas a tu DataTable según tu esquema de base de datos
            // Por ejemplo:
            dataTable.Columns.Add("Columna1", typeof(int));
            dataTable.Columns.Add("Columna2", typeof(string));
            // ...
            // Agregar 80,000 filas con datos ficticios
            for (int i = 0; i < 80000; i++)
            {
                dataTable.Rows.Add(i, "Dato " + i.ToString());
            }
            return dataTable;
        }
    }
}