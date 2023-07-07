using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Narrative {
    public partial class SettingsForm : Form {

        private Dictionary<String, Object> settings = new Dictionary<String, Object>();

        public void Set ( String key, Object value ) {
            settings[key] = value;
        }
        public void Ensure ( String key, Object value ) {
            if ( !Has(key) )
                Set(key, value);
        }
        public T Get<T> ( String key ) {
            return (T) settings[key];
        }
        public T Get<T> ( String key, T defaultValue ) {
            if ( !Has(key) )
                return defaultValue;
            return Get<T>(key);
        }
        public Boolean Has ( String key ) {
            return settings.ContainsKey(key);
        }

        public void AddBooleanSetting ( String key, String text ) {
            Ensure(key, false);
            CheckBox checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Checked = Get<Boolean>(key);
            checkBox.CheckedChanged += ( sender, e ) => {
              Set(key, checkBox.Checked);
              ConsoleWidget.TemporaryLine(() => $"Set {key} to {checkBox.Checked}", 2000);
            };
            Controls.Add(checkBox);
        }
    }
}
