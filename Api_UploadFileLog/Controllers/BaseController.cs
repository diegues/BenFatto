﻿using Api_UploadFileLog.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Net;

namespace Api_UploadFileLog.Controllers
{
    public class BaseController : Controller
    {
        protected int? IntTryParseNullable(string val)
        {
            int outValue;
            return int.TryParse(val, out outValue) ? (int?)outValue : null;
        }

        protected bool ipAddressValido(string ip)
        {
            IPAddress outValue;
            return IPAddress.TryParse(ip, out outValue);
        }

        protected DateTime ConvertDateTime(string date)
        {
            try
            {
                int day = Convert.ToInt32(date.Substring(0, 2));
                int month = DateTime.ParseExact(date.Substring(3, 3), "MMM", CultureInfo.InvariantCulture).Month;
                int year = Convert.ToInt32(date.Substring(7, 4));

                int hour = Convert.ToInt32(Convert.ToInt32(date.Substring(12, 2)));
                int minute = Convert.ToInt32(Convert.ToInt32(date.Substring(15, 2)));
                int second = Convert.ToInt32(Convert.ToInt32(date.Substring(18, 2)));

                DateTime dateConverted = new DateTime(year, month, day, hour, minute, second);

                return dateConverted;
            }
            catch (Exception)
            {
                throw new ArgumentException("Data inválida!");
            }
        }

        protected string ConvertTimeZone(string date)
        {
            return date.Substring(21, 5);
        }

        protected string findData(int start, string caracter, ref string linha)
        {
            string value = "";

            if (linha.Trim().Length > 0)
            {
                int index = linha.Length;
                int cont = 0;

                if (start > 0)
                    linha = linha.Substring(start);

                if (linha.IndexOf(caracter) > 0)
                {
                    index = linha.IndexOf(caracter);
                    cont = 1;
                }


                value = linha.Substring(0, index);
                linha = linha.Substring(value.Length + cont).Trim();
            }
            return value.Trim().Equals("-") ? null : value.Trim();
        }
    }
}
