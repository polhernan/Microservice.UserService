namespace UserService.Domain.Common
{
    public class AuditableEntity : BaseEntity
    {


        public EnumEntityState State { get; private set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime? DeletedOn { get; set; }


        public AuditableEntity() 
        {
            CreatedOn = DateTime.Now;
            UpdatedOn = DateTime.Now;
            DeletedOn = null;
            State = EnumEntityState.Pendiente;
        }


        public void Suspender()
        {
            State = EnumEntityState.Suspendido;
        }


        public void Activar()
        {
            State = EnumEntityState.Activo;
        }


        public void UpdateEntity()
        {
            UpdatedOn = DateTime.Now;
        }


        public void DeleteEntity()
        {
            DeletedOn = DateTime.Now;
            State = EnumEntityState.Eliminado;
        }
    }
}
