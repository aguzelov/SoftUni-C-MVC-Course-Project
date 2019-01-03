using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dialog.Common.Mapping;
using Dialog.Data.Common.Repositories;
using Dialog.Data.Models;
using Dialog.Services.Contracts;

namespace Dialog.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IDeletableEntityRepository<Setting> _settingsRepository;

        public SettingsService(IDeletableEntityRepository<Setting> settingsRepository)
        {
            this._settingsRepository = settingsRepository;
        }

        public string Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            var setting = this._settingsRepository.All().FirstOrDefault(s => s.Name == name);

            if (setting == null)
            {
                return string.Empty;
            }

            return setting.Value;
        }

        public async Task Change(string name, string value)
        {
            if (string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(value))
            {
                return;
            }

            var setting = this._settingsRepository.All().FirstOrDefault(s => s.Name == name);

            if (setting != null)
            {
                setting.Value = value;
            }

            await this._settingsRepository.SaveChangesAsync();
        }

        public ICollection<T> All<T>()
        {
            var settings = this._settingsRepository.All()
                .OrderBy(s => s.Name)
                .To<T>()
                .ToList();

            return settings;
        }
    }
}