using App.Application.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Interfaces
{
    public partial interface ILocalizedModelFactory
    {
        Task<IList<T>> PrepareLocalizedModels<T>(Action<T, int> configure = null) where T : ILocalizedLocaleModel;
    }
}
