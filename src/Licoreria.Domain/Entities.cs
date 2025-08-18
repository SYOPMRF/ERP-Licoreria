using System;
using System.Collections.Generic;

namespace Licoreria.Domain;

public enum InventoryTxnType { Ingreso = 1, Salida = 2, Ajuste = 3 }

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public bool Active { get; set; } = true;
}

public class Supplier
{
    public int Id { get; set; }
    public string Name { get; set; } = default!;
    public string? ContactEmail { get; set; }
    public string? Phone { get; set; }
    public bool Active { get; set; } = true;
}

public class Product
{
    public int Id { get; set; }
    public string SKU { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? Brand { get; set; }
    public string? Barcode { get; set; }
    public decimal AlcoholVolumePct { get; set; } // % alcohol
    public int VolumeMl { get; set; }             // mililitros por unidad
    public decimal Cost { get; set; }
    public decimal Price { get; set; }
    public bool Active { get; set; } = true;

    // getters "seguros"
    public decimal GetIva(decimal ivaRate) => Math.Round(Price * ivaRate, 2);
}

public class InventoryTransaction
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public InventoryTxnType Type { get; set; }
    public int Quantity { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string? Reason { get; set; } // compra, venta, ajuste, merma, etc.
}

public class Customer
{
    public int Id { get; set; }
    public string Document { get; set; } = default!; // NIT/CC
    public string Name { get; set; } = default!;
    public string? Email { get; set; }
    public string? Phone { get; set; }
}

public class Invoice
{
    public int Id { get; set; }
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;
    public int? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public string? CashierUserId { get; set; } // Identity User
    public decimal Subtotal { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
    public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();

    public void RecalcTotals(decimal taxRate)
    {
        Subtotal = 0;
        foreach (var l in Lines) Subtotal += l.UnitPrice * l.Quantity;
        Tax = Math.Round(Subtotal * taxRate, 2);
        Total = Subtotal + Tax;
    }
}

public class InvoiceLine
{
    public int Id { get; set; }
    public int InvoiceId { get; set; }
    public Invoice? Invoice { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
