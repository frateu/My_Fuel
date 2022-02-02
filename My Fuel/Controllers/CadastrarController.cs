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

            //SQL
            sqlAbastecimento = db.commandTxt("INSERT INTO Abastecimentos (km_rodado, km_total, litros, valor, alcool, media, valor_litro, dia, mes, ano) " +
                "VALUES (" + double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture) + ", " + double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) + ", " + alcoolInt + ", " + abastecimento.Media + ", " + abastecimento.ValorLitro +
                ", " + abastecimento.Data.Day + ", " + abastecimento.Data.Month + ", " + abastecimento.Data.Year + ")");

            //Informações para a nova pagina
            ViewBag.KmRodado = Math.Round(double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture), 2);
            ViewBag.KmTotal = Math.Round(double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture), 2);
            ViewBag.Litros = Math.Round(double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture), 2);
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
