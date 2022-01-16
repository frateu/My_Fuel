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
            int alcoolBit = 0;
            if (abastecimento.Alcool)
            {
                alcoolBit = 1;
            }

            string sqlFormattedDate = abastecimento.Data.ToString("yyyy-MM-dd");

            sqlAbastecimento = db.commandTxt("INSERT INTO Abastecimentos (data, km_rodado, km_total, litros, valor, alcool) " +
                "VALUES ('" + sqlFormattedDate + "', " + double.Parse(abastecimento.KmRodado, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.KmTotal, CultureInfo.InvariantCulture) + ", " + double.Parse(abastecimento.Litros, CultureInfo.InvariantCulture) +
                ", " + double.Parse(abastecimento.Valor, CultureInfo.InvariantCulture) + ", " + alcoolBit + ")");
            return Redirect("/Cadastrar");
        }
    }
}
