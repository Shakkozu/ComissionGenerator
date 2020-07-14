using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ClassLibrary
{
    public class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel() : base("settingsViewModel")
        {
           
        }

        private string _templateFilePath = "";

        public string TemplateFilePath
        {
            get
            {
                return _templateFilePath;
            }
            set
            {
                if(File.Exists(value) && value.Contains(".docx"))
                {
                    _templateFilePath = value;
                    OnPropertyChanged();
                }
            }
        }

        protected override void LoadProperties(object viewModel)
        {
            if(viewModel is SettingsViewModel settingsViewModel)
            {
                if (settingsViewModel.TemplateFilePath != null)
                {
                    TemplateFilePath = settingsViewModel.TemplateFilePath;
                }
            }
        }

        /// <summary>
        /// Save current properties to file in xml format
        /// </summary>
        /// <returns></returns>
        public bool SaveXml()
        {
            return Save(this, ExtensionType.Xml);
        }


        /// <summary>
        /// Load CompanyViewModel from File in xml format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        public bool LoadXml()
        {
            return Load(this, ExtensionType.Xml);
        }

        /// <summary>
        /// Load CompanyViewModel from File in json format
        /// </summary>
        /// <returns>true if loading is succesful, false otheriwse</returns>
        public bool LoadJson()
        {
            return Load(this, ExtensionType.Json);
        }

        /// <summary>
        /// Save current properties to file in json format
        /// </summary>
        /// <returns></returns>
        public bool SaveJson()
        {
            return Save(this, ExtensionType.Json);
        }


    }
}
