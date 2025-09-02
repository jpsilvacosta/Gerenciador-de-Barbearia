namespace BarberBoss.Exception.ExceptionsBase
{
    public abstract class BarberBossException : System.Exception
    {
        protected BarberBossException(string message) : base(message)
        {

        }

        public abstract int StatusCode { get; }
        public abstract List<string> GetErrors();
        }
}
