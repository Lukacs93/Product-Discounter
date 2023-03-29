﻿using ProductDiscounter.Model.Enums;
using ProductDiscounter.Model.Products;

namespace ProductDiscounter.Model.Discounts;

public record MonthlyDiscount(string Name, int Rate, IEnumerable<Month> Months) : IDiscount
{
    public bool Accepts(Product product, DateTime date)
    {
        foreach (var month in Months)
        {
            if ((int)month == date.Month)
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, {nameof(Rate)}: {Rate}, {nameof(Months)}: {string.Join(", ", Months)}";
    }
}
