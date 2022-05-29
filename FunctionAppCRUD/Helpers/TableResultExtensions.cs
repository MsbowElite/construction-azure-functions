using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FunctionAppCRUD.Helpers
{
    internal static class TableResultExtensions
    {
        internal static void EnsureSuccessStatusCode(
            this TableResult tableResult)
        {
            switch (tableResult.HttpStatusCode)
            {
                case (int)HttpStatusCode.Created:
                case (int)HttpStatusCode.OK:
                case (int)HttpStatusCode.NoContent:
                    break;
                default:
                    throw new HttpRequestException(
                        $"Something went wrong in table operation, a {tableResult.HttpStatusCode} status code was returned.");
            }
        }
    }
}
