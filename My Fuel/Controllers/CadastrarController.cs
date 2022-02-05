using Microsoft.AspNetCore.Mvc;
using My_Fuel.Models;
using System.Data.SqlClient;
using System.Globalization;
using System.Text.RegularExpressions;

namespace My_Fuel.Controllers
{
    public class CadastrarController : Controller
    {
        DBConnect db = new DBConnect();
        SqlDataReader sqlAbastecimento;
        SqlDataReader sqlModificar;
        SqlDataReader sqlUltimoID;
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Abastecimento(AbastecimentoModel abastecimento)
        {
            CultureInfo nonInvariantCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentCulture = nonInvariantCulture;
            int alcoolInt = 0;
            string alcoolStr = "Gasolina";
            int ultimoID = 0;

            if (abastecimento.Alcool)
            {
                alcoolInt = 1;
                alcoolStr = "Álcool";
            }

            //Tratamento de erros de inserção
            ErrorInsert(abastecimento);

            //Calculos da media e do valolr do litro
            abastecimento.Media = Math.Round(double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) / double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture), 2);
            abastecimento.ValorLitro = Math.Round(double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) / double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture), 2);

            //SQL pegar o ultimo ID
            sqlUltimoID = db.commandTxt("SELECT * FROM Abastecimentos ORDER BY ano, mes, dia");
            while (sqlUltimoID.Read())
            {
                ultimoID = Convert.ToInt32(sqlUltimoID["id"].ToString());
            }

            Console.WriteLine(ultimoID);

            //SQL Inserir
            sqlAbastecimento = db.commandTxt("INSERT INTO Abastecimentos (km_rodado, km_total, litros, valor, alcool, media, valor_litro, dia, mes, ano) " +
                "VALUES (" + 0 + ", " + double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture) + ", " + double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) + ", " + alcoolInt + ", " + 0 + ", " + abastecimento.ValorLitro +
                ", " + abastecimento.Data.Day + ", " + abastecimento.Data.Month + ", " + abastecimento.Data.Year + ")");

            //SQL atualizar a ultimo abastecimento
            sqlModificar = db.commandTxt("UPDATE Abastecimentos SET km_rodado = " + double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) + ", media = " + abastecimento.Media + " WHERE id = " + ultimoID);

            //Informações para a nova pagina
            ViewBag.KmRodado = Math.Round(double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture), 2);
            ViewBag.KmTotal = Math.Round(double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture), 2);
            ViewBag.Litros = Math.Round(double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture), 3);
            ViewBag.Valor = Math.Round(double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture), 2);
            ViewBag.Data = abastecimento.Data;
            ViewBag.Alcool = alcoolStr;
            ViewBag.Media = Math.Round(abastecimento.Media, 2);
            ViewBag.ValorLitro = Math.Round(abastecimento.ValorLitro, 2);

            return View();
        }
        private void ErrorInsert(AbastecimentoModel abastecimento)
        {
            if (Regex.Matches(abastecimento.KmTotal, @"[a-zA-Z]").Count > 0 || Regex.Matches(abastecimento.KmRodado, @"[a-zA-Z]").Count > 0 || Regex.Matches(abastecimento.Litros, @"[a-zA-Z]").Count > 0 || Regex.Matches(abastecimento.Valor, @"[a-zA-Z]").Count > 0)
            {
                RedirectToAction("Index");
            }

            abastecimento.KmTotal = abastecimento.KmTotal.Replace(",",".");
            abastecimento.KmRodado = abastecimento.KmRodado.Replace(",", ".");
            abastecimento.Litros = abastecimento.Litros.Replace(",", ".");
            abastecimento.Valor = abastecimento.Valor.Replace(",", ".");
        }
    }
}
