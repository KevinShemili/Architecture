namespace Application.Behavior.ResultPattern.ErrorModels
{
    public sealed record Error(int Code, string? Message = null)
    {
        public static readonly Error None = new(int.MinValue, string.Empty);
    }
}
