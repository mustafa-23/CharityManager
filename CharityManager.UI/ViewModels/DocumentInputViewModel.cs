using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using System.IO;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class DocumentInputViewModel:ISupportParameter
    {
        public virtual DocumentModel Model { get; set; }
        public DocumentInputViewModel()
        {
            Model = new DocumentModel();
        }

        #region Commands
        public void Confirm()
        {
            if (Parameter is IDocumentInputListener listener)
            {
                listener.OnDocumentConfirm(Model);
                SliderHelper.Close();
            }
        }
        public void Browse()
        {
            DialogHelper.OpenFileDialog.Filter = "Word Document|*.doc;*.docx|PDF File|*.pdf";
            if (DialogHelper.OpenFileDialog.ShowDialog())
            {
                Model.Path = DialogHelper.OpenFileDialog.File.GetFullName();
                Model.Title = Path.GetFileNameWithoutExtension(Model.Path);
            }
        }
        #endregion

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        #endregion

        #region Interfaces
        public interface IDocumentInputListener
        {
            void OnDocumentConfirm(DocumentModel asset);
        }
        #endregion
    }
}