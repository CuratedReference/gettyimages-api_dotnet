﻿using GettyImages.Api.Search.Entity;

namespace GettyImages.Api
{
    public class DownloadImage : AssetDownload
    {
        private const string FileTypeKey = "file_type";
        private const string HeightKey = "height";
        private DownloadImage(Credentials credentials, string baseUrl)
            : base(credentials, baseUrl)
        {
            AssetType = "images";
        }

        internal static DownloadImage GetInstance(Credentials credentials, string baseUrl)
        {
            return new DownloadImage(credentials, baseUrl);
        }

        protected override sealed string AssetType { get; set; }

        public DownloadImage WithId(string value)
        {
            AssetId = value;
            return this;
        }

        public DownloadImage WithFileType(FileType value)
        {
            if (QueryParameters.ContainsKey(FileTypeKey))
            {
                QueryParameters[FileTypeKey] = value == FileType.None
                    ? value
                    : (FileType)QueryParameters[FileTypeKey] | value;
            }
            else
            {
                QueryParameters.Add(FileTypeKey, value);
            }
            return this;
        }

        public DownloadImage WithHeight(int height)
        {
            QueryParameters.Add(HeightKey, height);
            return this;
        }
    }
}