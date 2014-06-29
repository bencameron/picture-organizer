using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PictureOrganizer
{
    public class FileManager
    {
        #region Public Methods

        public void OrganizeFiles(string sourceFolder, string destinationBaseFolder)
        {
            List<MediaFile> files = this.GetFiles(sourceFolder);
            this.MoveFiles(files, destinationBaseFolder);
        }

        #endregion

        #region Private Methods

        private List<MediaFile> GetFiles(string sourceFolder)
        {
            List<MediaFile> files = new List<MediaFile>();

            foreach (var sourcePath in Directory.EnumerateFiles(sourceFolder))
            {
                files.Add(new MediaFile(sourcePath));
            }

            return files;
        }

        private void MoveFiles(List<MediaFile> files, string destinationBaseFolder)
        {
            foreach (var sourceFile in files)
            {
                string subFolder = this.GetFileSubfolder(sourceFile);
                string destinationFolder = Path.Combine(destinationBaseFolder, subFolder);
                string newPath = Path.Combine(destinationFolder, sourceFile.SourceFileName);

                if (!Directory.Exists(destinationFolder))
                {
                    if (sourceFile.AllowCreateDestinationFolder)
                    {
                        Directory.CreateDirectory(destinationFolder);
                    }
                    else
                    {
                        throw new Exception(string.Format("Destination folder {0} does not exist", destinationFolder));
                    }
                }

                File.Move(sourceFile.SourcePath, newPath);
            }
        }

        private string GetFileSubfolder(MediaFile sourceFile)
        {            
            string year = sourceFile.DateTaken.Year.ToString();
            string monthNumber = sourceFile.DateTaken.ToString("MM");
            string monthName = sourceFile.DateTaken.ToString("MMM");

            return string.Format(@"{0}s\{1}\{2} - {3}", sourceFile.FileType.ToString(), year, monthNumber, monthName);
        }

        #endregion
    }
}
