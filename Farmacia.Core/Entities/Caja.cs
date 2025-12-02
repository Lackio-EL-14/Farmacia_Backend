

namespace Farmacia.Core.Entities
{
    public class Caja : BaseEntities
    {
     
        public string Nombre { get; set; } = null!;
        public bool Activa { get; set; } = true;

        public ICollection<ArqueoCaja> Arqueos { get; set; } = new List<ArqueoCaja>();
    }
}
