using Microsoft.AspNetCore.Mvc;
using My_Fuel.Models;
using System.Collections;
using System.Data.SqlClient;
using System.Text.Json;

namespace My_Fuel.Controllers
{
    public class VisualizarController : Controller
    {
        DBConnect db = new DBConnect();
        SqlDataReader sqlAbastecimento;
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Abastecimentos([FromQuery] PesquisaModel pesquisa)
        {
            double mediaCalculadaTotal = 0;
            double valorCalculadoTotal = 0;
            double litroCalculadoTotal = 0;
            double kmRodadoCalculadoTotal = 0;
            double contDias = 0;

            ViewBag.abastecimentos = ArrayAbastecimento(pesquisa);
            ViewBag.mes = pesquisa.Mes;
            ViewBag.ano = pesquisa.Ano;

            foreach (var abast in ViewBag.abastecimentos)
            {
                mediaCalculadaTotal += Convert.ToDouble(abast[5]);
                valorCalculadoTotal += Convert.ToDouble(abast[3]);
                litroCalculadoTotal += Convert.ToDouble(abast[2]);
                kmRodadoCalculadoTotal += Convert.ToDouble(abast[0]);
                contDias++;
            }

            mediaCalculadaTotal = mediaCalculadaTotal / contDias;

            ViewBag.mediaTotal = Math.Round(mediaCalculadaTotal, 2);
            ViewBag.valorTotal = Math.Round(valorCalculadoTotal, 2);
            ViewBag.litrosTotal = Math.Round(litroCalculadoTotal, 2);
            ViewBag.kmRodadoTotal = Math.Round(kmRodadoCalculadoTotal, 2);

            return View();
        }
        private ArrayList ArrayAbastecimento(PesquisaModel pesquisa)
        {
            if (pesquisa.Mes.Equals("Todos") && pesquisa.Ano.Equals("Todos"))
            {
                sqlAbastecimento = db.commandTxt("SELECT * FROM Abastecimentos ORDER BY ano, mes, dia");
            }
            else
            {
                if (pesquisa.Mes.Equals("Todos"))
                {
                    sqlAbastecimento = db.commandTxt("SELECT * FROM Abastecimentos WHERE " +
                        "ano = " + Convert.ToInt32(pesquisa.Ano) + " ORDER BY ano, mes, dia");
                }else if (pesquisa.Ano.Equals("Todos"))
                {
                    sqlAbastecimento = db.commandTxt("SELECT * FROM Abastecimentos WHERE " +
                        "mes = " + Convert.ToInt32(pesquisa.Mes) + " ORDER BY ano, mes, dia");
                }
                else
                {
                    sqlAbastecimento = db.commandTxt("SELECT * FROM Abastecimentos WHERE " +
                        "mes = " + Convert.ToInt32(pesquisa.Mes) + "AND ano = " + Convert.ToInt32(pesquisa.Ano) + " ORDER BY ano, mes, dia");
                }
            }
            ArrayList allAbastecimentos = new ArrayList();

            while (sqlAbastecimento.Read())
            {
                string[] listAbastecimentos = new string[10];
                listAbastecimentos[0] = sqlAbastecimento["km_rodado"].ToString();
                listAbastecimentos[1] = sqlAbastecimento["km_total"].ToString();
                listAbastecimentos[2] = sqlAbastecimento["litros"].ToString();
                listAbastecimentos[3] = sqlAbastecimento["valor"].ToString();
                listAbastecimentos[4] = sqlAbastecimento["alcool"].ToString();
                listAbastecimentos[5] = sqlAbastecimento["media"].ToString();
                listAbastecimentos[6] = sqlAbastecimento["valor_litro"].ToString();
                listAbastecimentos[7] = sqlAbastecimento["dia"].ToString();
                listAbastecimentos[8] = sqlAbastecimento["mes"].ToString();
                listAbastecimentos[9] = sqlAbastecimento["ano"].ToString();

                allAbastecimentos.Add(listAbastecimentos);
            }
            
            return allAbastecimentos;
        }
    }
}
