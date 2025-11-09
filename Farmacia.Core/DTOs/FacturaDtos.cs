
public class FacturaDtos
{
    public int Id { get; set; }
    public int VentaId { get; set; }
    public string? Numero { get; set; }
    public DateTime FechaEmision { get; set; }
    public decimal ImporteTotal { get; set; }
}
