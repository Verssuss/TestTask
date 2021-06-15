using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;

namespace TestTask.Controls
{
    public class ValidateEntry : Xamarin.Forms.Entry
    {
        public ValidateEntry()
        {
            TextChanged += ValidateEntry_TextChanged;
        }

        private void ValidateEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            var rgxPattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";
            var entry = sender as Xamarin.Forms.Entry;
            if (Regex.IsMatch(e.NewTextValue, rgxPattern))
            {
                IsValidate = true;
                entry.BackgroundColor = Color.Transparent;
            }
            else
            {
                IsValidate = false;
                entry.BackgroundColor = Color.FromRgba(243, 151, 143, 182);
            }
        }
        public static BindableProperty IsValidateProperty = BindableProperty.Create(nameof(IsValidate), typeof(bool), typeof(ValidateEntry), default(bool), defaultBindingMode: BindingMode.TwoWay);

        public bool IsValidate
        {
            get => (bool)GetValue(IsValidateProperty);
            set => SetValue(IsValidateProperty, value);
        }

    }
}
