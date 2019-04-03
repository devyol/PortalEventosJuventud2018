using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EVE01.UI.Clases
{
    public static class NumeroLetras
    {

        public static string NumeroALetras(string num)
        {

            string res, dec = "";

            Int64 entero;

            int decimales;

            double nro;



            try
            {

                nro = Convert.ToDouble(num);

            }

            catch
            {

                return "";

            }



            entero = Convert.ToInt64(Math.Truncate(nro));

            decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));



            if (decimales > 0)
            {
                if (decimales < 9)
                    dec = "quetzales con 0" + decimales + "/100";
                else
                    dec = "quetzales con " + decimales + "/100";

            }
            else { dec = "quetzales con " + "00/100"; }


            res = NumeroLetras.NumeroALetras(Convert.ToDouble(entero)) + "  " + dec;

            return res;

        }

        private static string NumeroALetras(double value)
        {

            string Num2Text = "";

            value = Math.Truncate(value);



            if (value == 0) Num2Text = "cero";

            else if (value == 1) Num2Text = "uno";

            else if (value == 2) Num2Text = "dos";

            else if (value == 3) Num2Text = "tres";

            else if (value == 4) Num2Text = "cuatro";

            else if (value == 5) Num2Text = "cinco";

            else if (value == 6) Num2Text = "seis";

            else if (value == 7) Num2Text = "siete";

            else if (value == 8) Num2Text = "ocho";

            else if (value == 9) Num2Text = "nueve";

            else if (value == 10) Num2Text = "diez";

            else if (value == 11) Num2Text = "once";

            else if (value == 12) Num2Text = "doce";

            else if (value == 13) Num2Text = "trece";

            else if (value == 14) Num2Text = "catorce";

            else if (value == 15) Num2Text = "quince";

            else if (value < 20) Num2Text = "dieci" + NumeroALetras(value - 10);

            else if (value == 20) Num2Text = "veinte";

            else if (value < 30) Num2Text = "veinti" + NumeroALetras(value - 20);

            else if (value == 30) Num2Text = "treinta";

            else if (value == 40) Num2Text = "cuarenta";

            else if (value == 50) Num2Text = "cincuenta";

            else if (value == 60) Num2Text = "sesenta";

            else if (value == 70) Num2Text = "setenta";

            else if (value == 80) Num2Text = "ochenta";

            else if (value == 90) Num2Text = "noventa";



            else if (value < 100) Num2Text = NumeroALetras(Math.Truncate(value / 10) * 10) + " y " + NumeroALetras(value % 10);

            else if (value == 100) Num2Text = "cien";

            else if (value < 200) Num2Text = "ciento " + NumeroALetras(value - 100);

            else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = NumeroALetras(Math.Truncate(value / 100)) + " cientos ";



            else if (value == 500) Num2Text = "quinientos";

            else if (value == 700) Num2Text = "setecientos";

            else if (value == 900) Num2Text = "novecientos";

            else if (value < 1000) Num2Text = NumeroALetras(Math.Truncate(value / 100) * 100) + " " + NumeroALetras(value % 100);

            else if (value == 1000) Num2Text = "mil";

            else if (value < 2000) Num2Text = "mil " + NumeroALetras(value % 1000);

            else if (value < 1000000)
            {

                Num2Text = NumeroALetras(Math.Truncate(value / 1000)) + " mil";

                if ((value % 1000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value % 1000);

            }



            else if (value == 1000000) Num2Text = "un millon";

            else if (value < 2000000) Num2Text = "un millon " + NumeroALetras(value % 1000000);

            else if (value < 1000000000000)
            {

                Num2Text = NumeroALetras(Math.Truncate(value / 1000000)) + " millones ";

                if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000) * 1000000);

            }

            else if (value == 1000000000000) Num2Text = "un billon";

            else if (value < 2000000000000) Num2Text = "un billon " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            else
            {

                Num2Text = NumeroALetras(Math.Truncate(value / 1000000000000)) + " billones";

                if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + NumeroALetras(value - Math.Truncate(value / 1000000000000) * 1000000000000);

            }



            return Num2Text;

        }


    }
}