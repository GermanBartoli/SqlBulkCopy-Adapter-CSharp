using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Using_SqlBulkCopy_CSharp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Cadena de conexión a tu base de datos (reemplázala con la tuya)
            string connectionString = "Data Source=German;Initial Catalog=bulkcopy;User ID=sa;Password=1234";

            // Datos de muestra para insertar
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("valor", typeof(int));
            dataTable.Columns.Add("string", typeof(string));

            // Agregar   filas a la DataTable
            for (int i = 0; i < 80000; i++)
            {
                DataRow row = dataTable.NewRow();
                row["valor"] = i;
                row["string"] = $"Valor {i}";
                dataTable.Rows.Add(row);
            }

            // Crear una nueva instancia de Stopwatch.
            Stopwatch stopwatch = new Stopwatch();

            // Comenzar a medir el tiempo.
            stopwatch.Start();

            // Método 1: SqlDataAdapter
            Console.WriteLine("Insertando datos con SqlDataAdapter...");
            InsertarConDataAdapter(connectionString, dataTable);

            Console.WriteLine("Datos insertados correctamente.");
            // Detener la medición del tiempo.
            stopwatch.Stop();

            // Obtener el tiempo transcurrido como un objeto TimeSpan.
            TimeSpan ts = stopwatch.Elapsed;

            // Mostrar el tiempo transcurrido en la consola.
            Console.WriteLine("Tiempo transcurrido: {0:hh\\:mm\\:ss\\.ff}", ts);

            // Crear una nueva instancia de Stopwatch.
            stopwatch = new Stopwatch();

            // Comenzar a medir el tiempo.
            stopwatch.Start();

            // Método 2: SqlBulkCopy
            Console.WriteLine("Insertando datos con SqlBulkCopy...");
            InsertarConBulkCopy(connectionString, dataTable);

            stopwatch.Stop();

            // Obtener el tiempo transcurrido como un objeto TimeSpan.
            ts = stopwatch.Elapsed;

            // Mostrar el tiempo transcurrido en la consola.
            Console.WriteLine("Tiempo transcurrido: {0:hh\\:mm\\:ss\\.ff}", ts);
        }

        private static void InsertarConDataAdapter(string connectionString, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter())
                {
                    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
                    adapter.SelectCommand = new SqlCommand("SELECT * FROM bulkcopytable", connection);

                    adapter.InsertCommand = commandBuilder.GetInsertCommand();
                    //adapter.InsertCommand = new SqlCommandBuilder(adapter).GetInsertCommand();

                    connection.Open();
                    int rowsNumber = adapter.Update(dataTable);
                    //adapter.Fill(dataTable);
                    connection.Close();
                }
            }
        }

        private static void InsertarConBulkCopy(string connectionString, DataTable dataTable)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "bulkcopytable";
                    connection.Open();
                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();
                }
            }
        }
    }
}