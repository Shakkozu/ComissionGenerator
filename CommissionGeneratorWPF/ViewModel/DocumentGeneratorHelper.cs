using ClassLibrary;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Text;
using Xceed.Words.NET;

namespace CommissionGeneratorWPF.ViewModel
{
    class DocumentGeneratorHelper
    {
        private readonly CommissionViewModel _commissionViewModel;
        private readonly ClientViewModel _clientViewModel;
        private readonly CompanyViewModel _companyViewModel;

        DocX DocumentTemplate { get; set; }

        public DocumentGeneratorHelper(CompanyViewModel companyViewModel, ClientViewModel clientViewModel, CommissionViewModel commissionViewModel)
        {
            _commissionViewModel = commissionViewModel;
            _companyViewModel = companyViewModel;
            _clientViewModel = clientViewModel;

            string templateFileLocation = Environment.SpecialFolder.ApplicationData.ToString() + @"\template.docx";
            if (File.Exists(templateFileLocation))
            {
                DocumentTemplate = DocX.Load(templateFileLocation);
            }

        }

        /// <summary>
        /// Function generates new document using data from viewModels
        /// </summary>
        public void GenerateDocument()
        {
            if(DocumentTemplate != null)
            {
                EditTemplateDocument();
            }
            else
            {
                CreateNewDocument();
            }
        }

        private void EditTemplateDocument()
        {
            
        }

        private void CreateNewDocument()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Function changes document template
        /// </summary>
        /// <param name="path"></param>
        public void ChangeTemplate(string path)
        {
            if (File.Exists(path))
            {
                try
                {
                    DocX.Load(path);
                }

                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
            File.Copy(path, Environment.SpecialFolder.ApplicationData.ToString() + @"\template.docx",true);
        }

       
    }
}
