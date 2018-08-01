using System;
using System.Collections.Generic;
using System.Reflection;

namespace Unicasa.Domain.Helper
{
    public class Components
    {
        public Components()
        {

        }

        public List<GenericDropdown> GetDrowdown<T>(List<T> list, string text, string value)
        {
            List<GenericDropdown> dropdown = new List<GenericDropdown>();
            foreach (var item in list)
            {
                var sItem = new GenericDropdown();
                sItem.Text = item.GetType().GetProperty(text).GetValue(item, null) as string;
                sItem.Value = item.GetType().GetProperty(value).GetValue(item, null) as string;

                dropdown.Add(sItem);

            }

            return dropdown;
        }
        public List<GenericDropdown> GetDrowdown<T>()
        {
            List<GenericDropdown> dropdown = new List<GenericDropdown>();
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var sItem = new GenericDropdown();
                sItem.Text = Enum.GetName(typeof(T), item);
                sItem.Value = ((int)item).ToString();

                dropdown.Add(sItem);
            }

            return dropdown;
        }

        public List<GenericCheckBoxListItem> GetCheckBoxList<T>()
        {
            List<GenericCheckBoxListItem> checkbox = new List<GenericCheckBoxListItem>();

            foreach (var item in Enum.GetValues(typeof(T)))
            {
                var sItem = new GenericCheckBoxListItem();
                sItem.Display = Enum.GetName(typeof(T), item);
                sItem.Id = ((int)item).ToString();
                sItem.IsChecked = ((bool)item);

                checkbox.Add(sItem);
            }

            return checkbox;
        }
        public List<GenericCheckBoxListItem> GetCheckBoxList<T>(List<T> list, string text, string value, bool ischeck = false)
        {
            List<GenericCheckBoxListItem> checkbox = new List<GenericCheckBoxListItem>();

            foreach (var item in list)
            {

                var sItem = new GenericCheckBoxListItem();
                sItem.Display = item.GetType().GetProperty(text).GetValue(item, null) as string;
                sItem.Id = item.GetType().GetProperty(value).GetValue(item, null) as string;

                foreach (PropertyInfo propertyInfo in item.GetType().GetProperties())
                {
                    if (propertyInfo.PropertyType != typeof(string) && propertyInfo.GetValue(ischeck) != null)
                        sItem.IsChecked = true;
                    else
                        sItem.IsChecked = false;
                }

                checkbox.Add(sItem);

            }

            return checkbox;
        }
    }

    public class GenericCheckBoxListItem
    {
        public string Display { get; set; }
        public string Id { get; set; }
        public bool IsChecked { get; set; }
    }
    public class GenericDropdown
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}
