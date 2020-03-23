﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Model4You.Data.Models.Enums;

namespace Model4You.Services.Data.ModelService
{
    using System.Collections.Generic;

    using Model4You.Data.Models;

    public interface IModelService
    {
        Task<IEnumerable<T>> TakeSixModels<T>();

        Task<IEnumerable<T>> TakeAllModels<T>();

        Task<T> GetModelById<T>(string modelId);

        Task<string> ChangeUserFirstName(ApplicationUser user, string name);

        Task<string> ChangeUserLastName(ApplicationUser user, string name);

        Task<string> ChangeUserAge(ApplicationUser user, int age);

        Task<string> ChangeUserGender(ApplicationUser user, Gender gender);

        Task<string> ChangeUserEthnicity(ApplicationUser user, Ethnicity ethnicity);

        Task<string> ChangeUserValues(ApplicationUser user, double value, string property);

        Task<string> ChangeUserStringValues(ApplicationUser user, string value, string property);
    }
}