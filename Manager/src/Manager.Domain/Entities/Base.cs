using System.Collections.Generic;

namespace Manager.Domain.Entities {

    public abstract class Base {

        public long Id { get; set; }

        internal List<string> _errors;

        public IReadOnlyCollection<string> Erros => _errors;

        public abstract bool Validate();

    }

}