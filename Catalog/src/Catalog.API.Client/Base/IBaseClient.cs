﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Catalog.API.Client.Base
{
    public interface IBaseClient
    {
        Task<T> GetAsync<T>(Uri uri, CancellationToken cancellationToken);
        Uri BuildUri(string format);
    }
}
