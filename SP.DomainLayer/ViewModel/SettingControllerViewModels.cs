
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using SP.DomainLayer.Models;
using SP.Infrastructure.Constants;
using SP.Infrastructure.Enumerations;

namespace SP.DomainLayer.ViewModel
{
    public class SettingViewModel
    {
        [Display(Name = "عنوان")]
        public string title { set; get; }

        [Display(Name = "تست")]
        public string test { set; get; }
    }

    public class SettingCollection : ICollection
    {
        private List<Setting> Settings;

        public SettingCollection()
        {
            this.Settings = new List<Setting>();
        }

        public SettingCollection(params Setting[] settings)
        {
            this.Settings = settings.ToList();
        }

        public SettingCollection(List<Setting> Settings)
        {
            this.Settings = Settings;
        }

        public int Count => Settings.Count();

        public void Add(Tuple<SettingType, string> setting)
        {
            Settings.Add(new Setting() { SettingType = setting.Item1, Value = setting.Item2 });
        }

        public void Add(Setting setting)
        {
            Settings.Add(setting);
        }

        public void Add(SettingType settingType, string value)
        {
            Settings.Add(new Setting() { SettingType = settingType, Value = value });
        }

        public Setting this[SettingType index]
        {
            get
            {
                return Settings.Find(f => f.SettingType == index);
            }

            set
            {
                Settings[Settings.FindIndex(f => f.SettingType == index)] = value;
            }
        }

        public T[] Cast<T>(Func<Setting, T> func)
        {
            return Settings.Select(func).ToArray();
        }


        object ICollection.SyncRoot => this;

        bool ICollection.IsSynchronized => false;

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(Settings);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            foreach (var i in Settings)
            {
                array.SetValue(i, index);
                index = index + 1;
            }
        }
    }


    public class Enumerator : IEnumerator
    {
        private List<Setting> Settings;
        private int Cursor;

        public Enumerator(List<Setting> Settings)
        {
            this.Settings = Settings;
            Cursor = -1;
        }

        void IEnumerator.Reset()
        {
            Cursor = -1;
        }
        bool IEnumerator.MoveNext()
        {
            if (Cursor < Settings.Count)
                Cursor++;

            return (!(Cursor == Settings.Count));
        }

        object IEnumerator.Current
        {
            get
            {
                if ((Cursor < 0) || (Cursor == Settings.Count))
                    throw new InvalidOperationException();
                return Settings[Cursor];
            }
        }
    }
}