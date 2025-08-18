using Licoreria.Domain;
using Xunit;

public class InvoiceTests
{
    [Fact]
    public void RecalcTotals_CalculaCorrecto()
    {
        var inv = new Invoice();
        inv.Lines.Add(new InvoiceLine { Quantity = 2, UnitPrice = 10m });
        inv.Lines.Add(new InvoiceLine { Quantity = 1, UnitPrice = 20m });
        inv.RecalcTotals(0.19m);
        Assert.Equal(40m, inv.Subtotal);
        Assert.Equal(7.60m, inv.Tax);
        Assert.Equal(47.60m, inv.Total);
    }
}
