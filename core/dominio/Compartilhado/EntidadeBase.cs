public abstract class EntidadeBase<T>
{
    public Guid Id { get; set; }
    public Guid EmpresaId { get; set; }

    public abstract void AtualizarRegistro(T registroEditado);
}