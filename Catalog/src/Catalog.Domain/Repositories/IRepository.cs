using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Domain.Repositories
{
    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
