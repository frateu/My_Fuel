using Microsoft.AspNetCore.Mvc;
using My_Fuel.Models;
using System.Data.SqlClient;
using System.Globalization;

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
            string sqlFormattedDate = abastecimento.Data.ToString("yyyy-MM-dd");

            abastecimento.Media = double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) / double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture);
            abastecimento.ValorLitro = double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) / double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture);

            sqlAbastecimento = db.commandTxt("INSERT INTO Abastecimentos (data, km_rodado, km_total, litros, valor, alcool, media, valor_litro) " +
                "VALUES ('" + sqlFormattedDate + "', " + double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture) + ", " + double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) + ", " + alcoolInt + ", " + abastecimento.Media + ", " + abastecimento.ValorLitro + ")");

            ViewBag.KmRodado = double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture).ToString("0.##");
            ViewBag.KmTotal = double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture);
            ViewBag.Litros = double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture);
            ViewBag.Valor = double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture).ToString("0.##");
            ViewBag.Data = abastecimento.Data;
            ViewBag.Alcool = alcoolStr;
            ViewBag.Media = abastecimento.Media.ToString("0.##");
            ViewBag.ValorLitro = abastecimento.ValorLitro.ToString("0.##");

            return View();
        }
    }
}
