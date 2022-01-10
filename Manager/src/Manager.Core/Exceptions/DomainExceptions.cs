

namespace Manager.Core.Exceptions
{
    public class DomainExceptions : Exception {

        internal List<string> _errors;
        public IReadOnlyCollection<string> Erros => _errors;

        public DomainExceptions ()
        {}

        public DomainExceptions(string message, List<string> erros) : base(message) {
            _errors = erros;
        }

        public DomainExceptions(string message)  : base(message)
        {}

        public DomainExceptions(string message, Exception innerException) : base(message, innerException) 
        {}

    }
}