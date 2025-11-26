using System.Diagnostics;
using BarbeariaSilva.Models;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto.Generators;
using BCrypt.Net;
using Npgsql;

namespace BarbeariaSilva.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Agendar()
        {
            return View("~/Views/Agendamento/Cadastro.cshtml");
        }

        [HttpPost]

        public IActionResult agendamento(AgendarViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {

                    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

                    const string query = @"
                INSERT INTO agendamento (id_agendamento, data, id_barbeiro, id_cliente, id_servico, status, nome)
                VALUES (@id_agendamento, @data, @id_barbeiro, @id_cliente, @id_servico, @status, @nome);
                    ";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(id_agendamento), 0) + 1 FROM agendamento", connection))
                        {
                            model.id_agendamento = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("id_agendamento", model.id_agendamento);
                            cmd.Parameters.AddWithValue("data", model.data);
                            cmd.Parameters.AddWithValue("id_barbeiro", model.id_barbeiro );
                            cmd.Parameters.AddWithValue("id_cliente", 0);
                            cmd.Parameters.AddWithValue("id_servico", model.id_servico);
                            cmd.Parameters.AddWithValue("status", model.status ?? string.Empty);
                            cmd.Parameters.AddWithValue("nome", model.nome ?? string.Empty);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return View("~/Views/Home/Home.cshtml");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao Agendar o Horario: {ex.Message}");

                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao Agendar o Horario. Tente Novamente.");
                }
            }


            return View("~/Views/Home/Home.cshtml");
        }


        [HttpGet]
        public IActionResult CadastroCliente()
        {
            return View("~/Views/Cliente/Cadastro.cshtml");
        }

        [HttpPost]

        public IActionResult CadastroCliente(ClienteViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {


                    string senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(model.Senha);

                    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

                    const string query = @"
                INSERT INTO cliente (id_cliente, nome, email, telefone, senha, data_cadastro)
                VALUES (@id_cliente, @nome, @email, @telefone, @senha, @data_cadastro);
                    ";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(id_cliente), 0) + 1 FROM cliente", connection))
                        {
                            model.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("id_cliente", model.Id);
                            cmd.Parameters.AddWithValue("nome", model.Nome ?? string.Empty);
                            cmd.Parameters.AddWithValue("email", model.Email ?? string.Empty);
                            cmd.Parameters.AddWithValue("telefone", model.Telefone ?? string.Empty);
                            cmd.Parameters.AddWithValue("senha", senhaCriptografada ?? string.Empty);
                            cmd.Parameters.AddWithValue("data_cadastro", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return View("~/Views/Home/Login.cshtml");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao cadastrar o Cliente: {ex.Message}");

                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao cadastrar Cliente. Tente Novamente.");
                }
            }

            return View("~/Views/Cliente/Cadastro.cshtml");
        }

        [HttpGet]
        public IActionResult CadastroBarbeiro()
        {
            return View("~/Views/Barbeiro/Cadastro.cshtml");
        }

        [HttpPost]

        public IActionResult CadastroBarbeiro(BarbeiroViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    string senhaCriptografada = BCrypt.Net.BCrypt.HashPassword(model.Senha);

                    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

                    const string query = @"
                INSERT INTO barbeiro (id_barbeiro, nome, email, senha)
                VALUES (@id_barbeiro, @nome, @email, @senha);
                    ";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();


                        using (var cmd = new NpgsqlCommand("SELECT COALESCE(MAX(id_barbeiro), 0) + 1 FROM barbeiro", connection))
                        {
                            model.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("id_barbeiro", model.Id);
                            cmd.Parameters.AddWithValue("nome", model.Nome ?? string.Empty);
                            cmd.Parameters.AddWithValue("email", model.Email ?? string.Empty);
                            cmd.Parameters.AddWithValue("senha", senhaCriptografada ?? string.Empty);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    return View("~/Views/Home/Login.cshtml");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao registrar o Barbeiro: {ex.Message}");

                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao registraro Barbeiro. Tente Novamente.");
                }
            }

            return View("~/Views/Home/Login.cshtml");
        }

        [HttpPost]

        public IActionResult Login(ClienteViewModel model)
        {
            if (!ModelState.IsValid)
            {

                try
                {
                    var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

                    const string query = @"
                SELECT id_cliente, nome, email, telefone, senha, data_cadastro
                FROM cliente
                WHERE nome = @nome
                LIMIT 1;
                ";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("nome", model.Nome ?? string.Empty);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    if (model.Senha != null)
                                    {
                                        string senhaHash = reader["senha"].ToString();
                                        bool senhaCorreta = BCrypt.Net.BCrypt.Verify(model.Senha, senhaHash);

                                        if (senhaCorreta)
                                        {
                                            return View("~/Views/Home/Home.cshtml");
                                        }

                                        else
                                        {
                                            ModelState.AddModelError(string.Empty, "senha incorreta.");
                                        }
                                    }

                                }
                                else
                                {
                                    ModelState.AddModelError(string.Empty, "Usuário não encontrado.");
                                }
                            }
                        }
                    }

                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao fazer login: {ex.Message}");
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao fazer login. Tente novamente.");
                }
            }
            return View("~/Views/Home/Login.cshtml");

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Privacy()
        {
            return View();
        }
            
             
    }
}
