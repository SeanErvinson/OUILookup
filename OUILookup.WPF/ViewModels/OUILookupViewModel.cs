using OUILookup.Data.Models;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using OUILookup.WPF.ViewModels.BaseModel;
using System.Diagnostics;

namespace OUILookup.WPF.ViewModels
{
    class OUILookupViewModel : BaseViewModel
    {
        #region Private Variables
        private List<OUI> _OUIs;
        #endregion
        #region Private Properties
        private string _macAddress;
        private string _lookUpResult;

        #endregion

        #region Public Properties
        public string MacAddress
        {
            get { return _macAddress; }
            set
            {
                SetField(ref _macAddress, value, nameof(MacAddress));
                LookUpResult = GenerateResult();
            }
        }

        public string LookUpResult
        {
            get { return _lookUpResult; }
            set { SetField(ref _lookUpResult, value, nameof(LookUpResult)); }
        }
        #endregion

        #region Private Command
        private ICommand _updateOUICommand;
        private ICommand _openSourceICommand;
        private ICommand _openVendorListCommand;
        #endregion

        #region Public Command
        public ICommand UpdateOUICommand
        {
            get { return _updateOUICommand ?? (_updateOUICommand = new RelayCommand(param => Services.Update())); }
        }
        public ICommand OpenSourceCommand
        {
            get { return _openSourceICommand ?? (_openSourceICommand = new RelayCommand(param => ExecuteQuery(Services.SourceURL))); }
        }
        public ICommand OpenVendorListCommand
        {
            get { return _openVendorListCommand ?? (_openVendorListCommand = new RelayCommand(param => ExecuteQuery(Services.Location))); }
        }
        #endregion

        #region Helper Method
        private string FormatOUI(string unformattedAddress)
        {
            string pattern = @"[.: -]+";
            string result = Regex.Replace(unformattedAddress, pattern, string.Empty).ToUpper();
            pattern = @"^[\dA-F]+$";
            if (result.Length < 6 || !Regex.IsMatch(result, pattern))
                return null;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                if(i % 2 == 0 && i != 0)
                    stringBuilder.Append(':');
                stringBuilder.Append(result[i]);
            }
            return stringBuilder.ToString().Substring(0, 8);
        }

        private void ExecuteQuery(string query)
        {
            Process.Start(query);
        }

        private string GenerateResult()
        {
            string inputAddress;
            string result = string.Empty;
            inputAddress = FormatOUI(_macAddress);
            if (!string.IsNullOrEmpty(inputAddress))
            {
                foreach (var oui in _OUIs)
                {
                    if (oui.OUIAddress == inputAddress)
                    { 
                        result = oui.CompanyName;
                        break;
                    }
                    else
                        result = "No Vendor";
                }
            }
            return result;
        }

        private async void Setup()
        {
            _OUIs = await Services.RetrieveOUIs();
        }
        #endregion

        #region Default Constructor
        public OUILookupViewModel()
        {
            Setup();
        }
        #endregion
    }
}
