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
                INSERT INTO cliente (id_cliente, nome, email, senha)
                VALUES (@id_cliente, @nome, @email, @senha);
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
                            cmd.Parameters.AddWithValue("senha", senhaCriptografada ?? string.Empty);
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

            return Error();
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

            return Error();
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
                SELECT id, nome, email, telefone, senha
                FROM cliente
                WHERE email = @email
                LIMIT 1;
                ";

                    using (var connection = new NpgsqlConnection(connectionString))
                    {
                        connection.Open();

                        using (var cmd = new NpgsqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("email", model.Email ?? string.Empty);

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string senhaHash = reader["senha"].ToString();

                                    bool senhaCorreta = BCrypt.Net.BCrypt.Verify(model.Senha, senhaHash);

                                    if (senhaCorreta)
                                    {
                                        return View("~/Views/Home/Index.cshtml");
                                    }

                                    else
                                    {
                                        ModelState.AddModelError(string.Empty, "senha incorreta.");
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
            return Error();
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

        [HttpGet]
        public IActionResult Agendar()
        {
            return View("~/Views/Agendamento/Cadastro.cshtml");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult Agendar(AgendarViewModel model)
        {
            return View(model);
        }

    }
}
