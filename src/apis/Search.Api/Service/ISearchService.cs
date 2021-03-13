﻿using Search.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Search.Api.Service
{
    public interface ISearchService
    {
        Task<ArticleResult> RunQueryAsync(SearchParameter parameters);
    }
}