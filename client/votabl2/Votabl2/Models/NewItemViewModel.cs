using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Votabl2.Common;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace Votabl2.Models
{
    public class NewItemViewModel : ViewModel
    {
        private Action _onDone;

        public NewItemViewModel(Action onDone)
        {
            _onDone = onDone;
            _createCommand = new RelayCommand(Create);
        }

        public async void Create()
        {
            var imgPicked = await ChooseImage();

            if (!imgPicked)
            {
                return;
            }

            _onDone();
        }

        private async Task<bool> ChooseImage()
        {
            FileOpenPicker fop = new FileOpenPicker();
            fop.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fop.FileTypeFilter.Add(".png");
            fop.FileTypeFilter.Add(".jpg");
            fop.FileTypeFilter.Add(".jpeg");
            var file = await fop.PickSingleFileAsync();
            if (file == null)
            {
                return false;
            }
            ImageFile = file;
            return true;
        }

        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                SetValue(ref _name, value, "Name");
            }
        }

        private StorageFile _imageFile;

        public StorageFile ImageFile
        {
            get { return _imageFile; }
            set
            {
                SetValue(ref _imageFile, value, "ImageFile");
            }
        }

        private RelayCommand _createCommand;

        public RelayCommand CreateCommand
        {
            get { return _createCommand; }
            set
            {
                SetValue(ref _createCommand, value, "CreateCommand");
            }
        }
    }
}
