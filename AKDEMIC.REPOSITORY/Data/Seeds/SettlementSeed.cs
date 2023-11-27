using System;
using System.Collections.Generic;
using AKDEMIC.ENTITIES.Models.EconomicManagement;

namespace AKDEMIC.REPOSITORY.Data.Seeds
{
    public class SettlementSeed
    {
        public static Settlement[] Seed(AkdemicContext _context)
        {
            var result = new List<Settlement>()
            {
                new Settlement {CustomerDni = "75923651", CustomerName = "Karina Alva", CurrencyCode = "PEN", TotalAmount = 35.00M,QuantityOfDocs = 2, CardNumber = "7848151418471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "47184718", CustomerName = "Yvonne Taboada", CurrencyCode = "PEN", TotalAmount = 85.50M,QuantityOfDocs = 4, CardNumber = "7848151445871819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "74758418", CustomerName = "Isable Pando", CurrencyCode = "PEN", TotalAmount = 45.10M,QuantityOfDocs = 3, CardNumber = "7848154988471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "71842514", CustomerName = "Fiorella Figueroa", CurrencyCode = "PEN", TotalAmount = 65.80M,QuantityOfDocs = 3, CardNumber = "7844561418471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "76184849", CustomerName = "Belsy Castillo", CurrencyCode = "PEN", TotalAmount = 56.15M,QuantityOfDocs = 3, CardNumber = "7848132218471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "73154847", CustomerName = "Melisa Mejía", CurrencyCode = "PEN", TotalAmount = 80.00M,QuantityOfDocs = 4, CardNumber = "7481561818471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "75144878", CustomerName = "Ximena Rodriguez", CurrencyCode = "PEN", TotalAmount = 53.80M,QuantityOfDocs = 3, CardNumber = "7848744418471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "75585984", CustomerName = "Anyeli Principe", CurrencyCode = "PEN", TotalAmount = 30.00M,QuantityOfDocs = 2, CardNumber = "7848485918471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "75941845", CustomerName = "Ingrid Carranza", CurrencyCode = "PEN", TotalAmount = 26.00M,QuantityOfDocs = 1, CardNumber = "7848116518471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow},
                new Settlement {CustomerDni = "73194825", CustomerName = "Monica Santa María", CurrencyCode = "PEN", TotalAmount = 38.00M,QuantityOfDocs = 2, CardNumber = "7816771418471819", PaymentDate = DateTime.UtcNow, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow}
            };
            return result.ToArray();
        }
    }
}
