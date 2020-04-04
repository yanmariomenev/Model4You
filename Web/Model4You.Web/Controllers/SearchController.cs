﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model4You.Data.Models.Enums;
using Model4You.Services.Data.SearchService;
using Model4You.Web.ViewModels.Search;

namespace Model4You.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchService searchService;

        public SearchController(ISearchService searchService)
        {
            this.searchService = searchService;
        }

        public async Task<IActionResult> Search(SearchInputModel input)
        {
            if (!ModelState.IsValid)
            {
                return this.Redirect("/");
            }

            var search = await
                this.searchService.SearchResult<SearchViewModel>(
                    input.Country,
                    input.City,
                    input.Gender,
                    input.Age,
                    input.To);
            var status = string.Empty;
            if (search.Count == 0)
            {
                status = "Models NOT FOUND";
            }

            var viewModel = new SearchResultViewModel
            {
                SearchViewModels = search,
                EmptyResult = status,
                SearchInputModel = new SearchInputModel
                {
                    Country = input.Country,
                    City = input.City,
                    Gender = input.Gender,
                    Age = input.Age,
                    To = input.To,
                },
            };
            return this.View(viewModel);
        }
    }
}