using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dialog.Services.Contracts
{
    public interface ISettingsService
    {
        string Get(string name);

        Task Change(string name, string value);

        ICollection<T> All<T>();
    }
}