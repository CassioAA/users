using System.Collections.Generic;

namespace Manager.Domain.Entities {

    public abstract class Base {

        public Long Id { get; set; }

        internal List<string> _erros;

        public IReadOnlyCollection<string> Erros => _erros;

        public abstract bool Validate();

    }

}