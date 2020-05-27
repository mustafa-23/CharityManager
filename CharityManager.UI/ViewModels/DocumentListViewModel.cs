using System;
using DevExpress.Mvvm.DataAnnotations;
using DevExpress.Mvvm;
using CharityManager.UI.Models;
using System.Collections.ObjectModel;
using CharityManager.UI.CharityService;
using Araneo.Common;
using System.Threading.Tasks;
using System.Linq;

namespace CharityManager.UI.ViewModels
{
    [POCOViewModel]
    public class DocumentListViewModel : ISupportParameter, DocumentInputViewModel.IDocumentInputListener
    {
        public ObservableCollection<DocumentModel> DocumentList { get; set; } = new ObservableCollection<DocumentModel>();

        public void Add() => SliderHelper.Open(AppModules.DocumentInput, this);
        public void OnDocumentConfirm(DocumentModel document)
        {
            document.PatronID = (int)Parameter;

            var request = new DocumentRequest { DTO = Mapper.Map(document, new DocumentDTO()) };
            var response = Helper.Call(s => s.DocumentSet(request));

            Task.Run(() => RefreshDocumentList());
        }

        private void RefreshDocumentList()
        {
            AppUIManager.Application.Dispatcher.Invoke(() => DocumentList.Clear());
            if (Parameter is int patronId && patronId > 0)
            {
                var request = new DocumentRequest { Filter = new DocumentFilter { PatronID = patronId } };
                var response = Helper.Call(s => s.DocumentGetList(request));
                if (response?.Success ?? false)
                    AppUIManager.Application.Dispatcher.Invoke(() =>
                   DocumentList.AddRange(response.ResultList.Select(dto => Mapper.SmartMap(dto, new DocumentModel()))));
            }
        }

        #region ISupportParameter
        public virtual object Parameter { get; set; }
        protected void OnParameterChanged() => Task.Run(() => RefreshDocumentList());
        #endregion
    }
}